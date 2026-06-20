using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using ArchipelagoMod.Src.Config;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArchipelagoMod.Src.Connector
{
    class ArchipelagoConnector : ScriptableSingleton<ArchipelagoConnector>
    {
        private string Game;
        private ParkitectAPConfig ParkitectAPConfig;
        private readonly string[] _protocols = new[] { "wss://", "", "ws://" };

        private readonly SemaphoreSlim _sessionLock = new SemaphoreSlim(1, 1);
        private volatile bool _stopRetries;

        // CancellationTokenSource for cleanly cancelling active connection attempts
        private CancellationTokenSource _cts;

        public int Retry = 10 * 1000;
        public int maxRetries { get; private set; } = 12;

        public ArchipelagoSession Session { get; private set; }

        public bool IsConnected
        {
            get
            {
                var session = this.Session;
                return session != null && session.Socket.Connected;
            }
        }

        // Events
        public event Action<string> OnReceivedPacket;
        public event Action<string, string, long> OnItemReceived;
        public event Action OnDisconnected;
        public event Action OnReconnect;
        public event Action OnStopped;
        public event Action<LoginSuccessful> OnConnected;
        public event Action OnConnectionFailed;
        public event Action<string> OnLoginFailed;
        public event Action<string, string> OnTrapReceived;

        protected Task CurrentTask = null;

        enum Links
        {
            TrapLink,
            DeathLink,
            EnergyLink,
        }
        public bool JoinedTrapLink = false;
        public bool JoinedDeathLink = false;
        public bool JoinedEnergyLink = false;

        enum TrapLinkKeys
        {
            time,
            source,
            trap_link,
        }

        public void Init(ParkitectAPConfig parkitectAPConfig, string game)
        {
            this.ParkitectAPConfig = parkitectAPConfig;
            this.Game = game;
        }

        public void ConnectAsync()
        {
            // Cancel the previous connection task via the token if it exists
            if (this._cts != null)
            {
                this._cts.Cancel();
                this._cts.Dispose();
            }

            // Create a new TokenSource for this specific call
            this._cts = new CancellationTokenSource();
            CancellationToken token = this._cts.Token;

            this._stopRetries = false;
            this.CurrentTask = this.TryConnectWithRetries(token);
        }

        public void JoinTrapLink()
        {
            this.JoinedTrapLink = true;
            List<string> tags = this.GetCurrentTags();
            tags.Add(Links.TrapLink.ToString());
            this.Session.ConnectionInfo.UpdateConnectionOptions(tags.ToArray());
        }

        public void LeaveTrapLink()
        {
            this.JoinedTrapLink = false;
            List<string> tags = this.GetCurrentTags();
            tags.Remove(Links.TrapLink.ToString());
            this.Session.ConnectionInfo.UpdateConnectionOptions(tags.ToArray());
        }

        public bool ToggleTrapLink()
        {
            if (!this.JoinedTrapLink)
            {
                this.JoinTrapLink();
            }
            else
            {
                this.LeaveTrapLink();
            }

            return this.JoinedTrapLink;
        }

        public async Task DisconnectAsync()
        {
            this._stopRetries = true;

            // Stop any active connection attempt
            if (this._cts != null)
            {
                this._cts.Cancel();
            }

            await this._sessionLock.WaitAsync();
            try
            {
                var session = this.Session;
                if (session != null && session.Socket.Connected)
                {
                    await session.Socket.DisconnectAsync();
                }
                this.Session = null;
            }
            finally
            {
                this._sessionLock.Release();
            }
        }

        public void ForwardSayPacket(string message)
        {
            Helper.Debug($"[ArchipelagoConnector::ForwardSayPacket]");
            if (!this.IsConnected)
            {
                return;
            }

            SayPacket packet = new SayPacket();
            packet.Text = message;

            Helper.Debug($"[ArchipelagoConnector::ForwardSayPacket] SendPacket");
            this.Session.Socket.SendPacket(packet);
        }

        public bool ForwardTrapLink(string trap)
        {
            if (!this.HasTrapLinkEnabled())
            {
                return false;
            }

            try
            {
                BouncePacket bouncePacket = new BouncePacket()
                {
                    Tags = new List<string>()
                    {
                        Links.TrapLink.ToString()
                    },
                    Data = new Dictionary<string, JToken>()
                    {
                        { TrapLinkKeys.time.ToString(), new JValue(DateTimeOffset.UtcNow.ToUnixTimeSeconds()) },
                        { TrapLinkKeys.source.ToString(), new JValue(this.ParkitectAPConfig.Playername) },
                        { TrapLinkKeys.trap_link.ToString(), new JValue(trap) }
                    }
                };

                this.Session.Socket.SendPacket(bouncePacket);
            }
            catch
            {
                Helper.Debug($"[ArchipelagoConnector::ForwardTrapLink] TrapLink {trap} was not send to Server. Check Logs");
            }
            
            return true;
        }

        public bool HasTrapLinkEnabled()
        {
            return this.Session.ConnectionInfo.Tags.Contains(Links.TrapLink.ToString());
        }

        public void GoalComplete()
        {
            StatusUpdatePacket statusUpdate = new StatusUpdatePacket();
            statusUpdate.Status = ArchipelagoClientState.ClientGoal;
            this.Session.Socket.SendPacket(statusUpdate);
        }

        private async Task TryConnectWithRetries(CancellationToken token)
        {
            int attempt = 0;

            Helper.Debug($"[ArchipelagoConnector::TryConnectWithRetries]");

            while (!this._stopRetries)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                attempt++;

                foreach (string protocol in this._protocols)
                {
                    if (this._stopRetries || token.IsCancellationRequested)
                    {
                        continue;
                    }

                    string fullHost = string.IsNullOrEmpty(this.ParkitectAPConfig.Address)
                        ? string.Empty
                        : protocol + this.ParkitectAPConfig.Address;

                    Helper.Debug($"[ArchipelagoConnector::TryConnectWithRetries] fullHost {fullHost}");

                    try
                    {
                        // The token cancels waiting for the semaphore if a restart occurs
                        await this._sessionLock.WaitAsync(token);

                        if (this._stopRetries || token.IsCancellationRequested)
                        {
                            continue;
                        }

                        ArchipelagoSession newSession = ArchipelagoSessionFactory.CreateSession(fullHost, this.ParkitectAPConfig.Port);
                        this.HookSessionEvents(newSession);

                        await newSession.ConnectAsync();
                        var result = await newSession.LoginAsync(this.Game, this.ParkitectAPConfig.Playername, ItemsHandlingFlags.AllItems, password: this.ParkitectAPConfig.Password);

                        Helper.Debug($"[ArchipelagoConnector::TryConnectWithRetries] LoginResult - {result.Successful}");

                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        if (!result.Successful)
                        {
                            this.OnLoginFailed?.Invoke("Login unsuccessful");
                            continue;
                        }

                        if (result is LoginSuccessful success)
                        {
                            Helper.Debug("[ArchipelagoConnector::TryConnectWithRetries] LoginSuccessful");
                            this.Session = newSession;
                            this._stopRetries = true;
                            this.OnConnected?.Invoke(success);
                            return;
                        }
                    }
                    catch (OperationCanceledException) when (token.IsCancellationRequested)
                    {
                        // ONLY exit the method if OUR token requested the cancellation!
                        Helper.Debug("[ArchipelagoConnector::TryConnectWithRetries] Current connection attempt cancelled by user or script.");
                        this.UnhookSessionEvents();
                        return;
                    }
                    catch (Exception e)
                    {
                        // Internal connection errors (like Connection Refused) are caught here now
                        Helper.Debug($"[ArchipelagoConnector::TryConnectWithRetries] OnConnectionFailed - (attempt {attempt}): {e.Message}");
                        Helper.Debug(e.StackTrace);
                        try
                        {
                            this.OnConnectionFailed?.Invoke();
                        }
                        catch (Exception eventEx)
                        {
                            Helper.Debug($"[ArchipelagoConnector::TryConnectWithRetries] OnConnectionFailed - {eventEx.Source}");
                        }
                    }
                    finally
                    {
                        // Only release the semaphore if we actually entered it
                        try
                        {
                            this._sessionLock.Release();
                        }
                        catch (SemaphoreFullException)
                        {
                        }
                    }
                }

                if (attempt >= this.maxRetries)
                {
                    this._stopRetries = true;
                    this.OnStopped?.Invoke();
                }

                if (!this._stopRetries)
                {
                    try
                    {
                        // IMPORTANT: The token immediately aborts the wait time (Retry-Delay)!
                        this.OnReconnect?.Invoke();
                        await Task.Delay(this.Retry, token);
                        Helper.Debug("[ArchipelagoConnector::TryConnectWithRetries] -> OnReconnected");
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                }
            }
        }

        private void HookSessionEvents(ArchipelagoSession session)
        {
            session.Socket.SocketClosed += this.OnSocketClosed;
            session.Socket.ErrorReceived += this.OnErrorReceived;
            session.Socket.PacketReceived += this.OnPacketReceived;
            session.Items.ItemReceived += this.OnReceivingItem;
        }

        private void UnhookSessionEvents()
        {
            this.Session.Socket.SocketClosed -= this.OnSocketClosed;
            this.Session.Socket.ErrorReceived -= this.OnErrorReceived;
            this.Session.Socket.PacketReceived -= this.OnPacketReceived;
            this.Session.Items.ItemReceived -= this.OnReceivingItem;
            this.Session = null;
        }

        private void OnSocketClosed(string reason)
        {
            this.OnDisconnected?.Invoke();

            if (!this._stopRetries)
            {
                Helper.Debug("[ArchipelagoConnector::OnSocketClosed]");
                //_ = this.TryConnectWithRetries(); // optional auto-retry
            }

            _ = this._sessionLock.WaitAsync();
            try
            {
                this.Session = null;
            }
            finally
            {
                this._sessionLock.Release();
            }
        }

        private void OnErrorReceived(Exception e, string message)
        {
            if (e.Message.Contains("Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy"))
            {
                return;
            }

            Helper.Debug("[ArchipelagoConnector::OnErrorReceived] -> : " + e.ToString());

            if (e.Message.Contains("closed the WebSocket connection"))
            {
                this.UnhookSessionEvents();
                _ = this.DisconnectAsync();
                this.OnDisconnected?.Invoke();
                this.ConnectAsync();
            }
        }

        private void OnReceivingItem(IReceivedItemsHelper helper)
        {
            int index = helper.Index - 1;
            ItemInfo item = helper.DequeueItem();
            string itemName = helper.GetItemName(item.ItemId, this.Game);

            long locationId = item.LocationId;
            string locationName = this.Session?.Locations?.GetLocationNameFromId(locationId);
            Helper.Debug($"[ArchipelagoConnector::OnReceivingItem] Got item -> '{itemName}' (ID {item.ItemId}) from location '{locationName}' (ID {locationId})");
            this.OnItemReceived?.Invoke(itemName, item.Player.Name, locationId);
        }

        private void OnPacketReceived(ArchipelagoPacketBase packet)
        {
            Helper.Debug($"[ArchipelagoConnector::OnPacketReceived] " + packet.GetType().Name);
            if (packet is ChatPrintJsonPacket chatPrint)
            {
                Helper.Debug($"[ArchipelagoConnector::OnPacketReceived] is ChatPrintJsonPacket");
                this.OnReceivedPacket?.Invoke(string.Join("", chatPrint.Data.Select(p => p.Text)));
            }
            else if (packet is PrintJsonPacket print)
            {
                Helper.Debug($"[ArchipelagoConnector::OnPacketReceived] is PrintJsonPacket");
                this.OnReceivedPacket?.Invoke(string.Join("", print.Data.Select(p => p.Text)));
            }
            else if (packet is BouncedPacket bouncedPacket)
            {
                if (this.IsTrapLink(bouncedPacket))
                {
                    Helper.Debug($"[ArchipelagoConnector::OnPacketReceived] is TrapLink");
                    string player = this.GetSource(bouncedPacket);
                    this.OnTrapReceived?.Invoke(this.GetTrapLinkValue(bouncedPacket), player);
                }
            }
        }

        private bool IsTrapLink(BouncedPacket bouncedPacket)
        {
            if (this.IsOwnSource(bouncedPacket))
            {
                return false;
            }

            List<string> keys = bouncedPacket.Data.Keys.ToList();
            return keys.Contains(TrapLinkKeys.trap_link.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        private string GetTrapLinkKey(BouncedPacket bouncedPacket)
        {
            List<string> keys = bouncedPacket.Data.Keys.ToList();
            return keys.FirstOrDefault(key => string.Equals(key, TrapLinkKeys.trap_link.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        private string GetTrapLinkValue(BouncedPacket bouncedPacket)
        {
            return bouncedPacket.Data[this.GetTrapLinkKey(bouncedPacket)].ToString();
        }

        private string GetSource(BouncedPacket bouncedPacket)
        {
            KeyValuePair<string, JToken> entry = bouncedPacket.Data.FirstOrDefault(x =>
                string.Equals(
                    x.Key,
                    TrapLinkKeys.source.ToString(),
                    StringComparison.OrdinalIgnoreCase));

            return entry.Value?.ToString();
        }

        private bool IsOwnSource(BouncedPacket bouncedPacket)
        {
            return this.GetSource(bouncedPacket) == this.ParkitectAPConfig.Playername;
        }

        private List<string> GetCurrentTags()
        {
            return this.Session.ConnectionInfo.Tags.ToList();
        }
    }
}