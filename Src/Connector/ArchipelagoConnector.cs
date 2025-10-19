using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using ArchipelagoMod.Src.Config;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArchipelagoMod.Src.Connector
{
    class ArchipelagoConnector
    {
        private readonly string Game;
        private readonly ParkitectAPConfig ParkitectAPConfig;
        private readonly string[] _protocols = new[] { "", "wss://", "ws://" };

        private readonly SemaphoreSlim _sessionLock = new SemaphoreSlim(1, 1);
        private volatile bool _stopRetries;

        public int Retry = 10 * 1000;
        private int maxRetries = 12;

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
        public event Action<string, string, long> OnItemReceived;
        public event Action OnDisconnected;
        public event Action OnReconnected;
        public event Action<LoginSuccessful> OnConnected;
        public event Action OnConnectionFailed;
        public event Action<string> OnLoginFailed;

        public ArchipelagoConnector(ParkitectAPConfig parkitectAPConfig, string game)
        {
            this.ParkitectAPConfig = parkitectAPConfig;
            this.Game = game;
        }

        public void ConnectAsync()
        {
            this._stopRetries = false;
            _ = this.TryConnectWithRetries();
        }

        private async Task TryConnectWithRetries()
        {
            int attempt = 0;

            Helper.Debug($"[ArchipelagoConnector::TryConnectWithRetries]");

            while (!this._stopRetries)
            {
                attempt++;

                foreach (string protocol in this._protocols)
                {
                    if (this._stopRetries)
                        break;

                    string fullHost = string.IsNullOrEmpty(protocol)
                        ? this.ParkitectAPConfig.Address
                        : protocol + this.ParkitectAPConfig.Address;

                    Helper.Debug($"[ArchipelagoConnector::TryConnectWithRetries] fullHost {fullHost}");

                    try
                    {
                        await this._sessionLock.WaitAsync();

                        if (this._stopRetries)
                        {
                            break;
                        }

                        ArchipelagoSession newSession = ArchipelagoSessionFactory.CreateSession(fullHost, this.ParkitectAPConfig.Port);
                        this.HookSessionEvents(newSession);

                        await newSession.ConnectAsync();
                        var result = await newSession.LoginAsync(this.Game, this.ParkitectAPConfig.Playername, ItemsHandlingFlags.AllItems, password: this.ParkitectAPConfig.Password);
                        Helper.Debug($"[ArchipelagoConnector::TryConnectWithRetries] LoginResult - {result.Successful}");

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
                    catch (Exception e)
                    {
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
                        this._sessionLock.Release();
                    }
                }

                if (attempt >= this.maxRetries)
                {
                    this._stopRetries = true;
                }

                if (!this._stopRetries)
                {
                    await Task.Delay(this.Retry);
                    this.OnReconnected?.Invoke();
                    Helper.Debug("[ArchipelagoConnector::TryConnectWithRetries] -> OnReconnected");
                }
            }
        }

        public async Task DisconnectAsync()
        {
            this._stopRetries = true;

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

        private void HookSessionEvents(ArchipelagoSession session)
        {
            session.Socket.SocketClosed += this.OnSocketClosed;
            session.Socket.ErrorReceived += this.OnErrorReceived;
            session.Items.ItemReceived += this.OnReceivingItem;
        }

        private void OnSocketClosed(string reason)
        {
            this.OnDisconnected?.Invoke();

            if (!this._stopRetries)
            {
                Helper.Debug("[ArchipelagoConnector::OnSocketClosed]");
                //_ = TryConnectWithRetries(); // optional auto-retry
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
            //this.OnDisconnected?.Invoke();
            //Helper.Debug("[ArchipelagoConnector::OnErrorReceived] -> : " + e.Message);

            //this.Session.Socket.ErrorReceived -= this.OnErrorReceived;
            //this.Session = null;
        }

        private void OnReceivingItem(IReceivedItemsHelper helper)
        {
            Helper.Debug("[ArchipelagoConnector::OnReceivingItem]");
            int index = helper.Index - 1;
            ItemInfo item = helper.DequeueItem();
            string itemName = helper.GetItemName(item.ItemId, this.Game);

            long locationId = item.LocationId;
            string locationName = this.Session?.Locations?.GetLocationNameFromId(locationId);
            Helper.Debug($"[ArchipelagoConnector::OnReceivingItem] Got item -> '{itemName}' (ID {item.ItemId}) from location '{locationName}' (ID {locationId})");
            this.OnItemReceived?.Invoke(itemName, item.Player.Name, locationId);
        }

        public void GoalComplete()
        {
            StatusUpdatePacket statusUpdate = new StatusUpdatePacket();
            statusUpdate.Status = ArchipelagoClientState.ClientGoal;
            this.Session.Socket.SendPacket(statusUpdate);
        }
    }
}