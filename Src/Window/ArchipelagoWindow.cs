using Archipelago.Src.EnergyLink;
using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Controller;
using ArchipelagoMod.Src.Window.Scripts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArchipelagoMod.Src.Window
{
    class ArchipelagoWindow : AbstractWindow
    {
        protected _Status.States State = _Status.States.DISCONNECTED;
        protected ParkitectController ParkitectController = null;
        protected ArchipelagoController ArchipelagoController = null;
        protected List<Challenge> CurrentChallenges = new List<Challenge>();
        private List<Challenge> all_challenges = new List<Challenge>();

        public float nextCheckTime = Time.time;
        public SaveData SaveData = null;
        public EnergyLinkHistory EnergyLinkHistory = null;

        public override string BundleFilename { get; set; } = "archipelagowindow";

        public override KeyCode KeyCode { get; set; } = KeyCode.Z;
        
        public override void OnAwake ()
        {
            Helper.Debug($"[ArchipelagoWindow::OnAwake]");
            this.ParkitectController = GetComponent<ParkitectController>();
            this.AddScripts();
            this.SetStatus(this.State, true);
            this.SetSpeedupButtons();
            this.SetCLI();
            this.SetBankListeners();
            this.ToggleActiveState();

            Helper.Debug($"[ArchipelagoWindow::OnAwake] Booted");
        }

        public void OnDestroy ()
        {
            TabController TabController = this.GetChild("Frame/Menu").gameObject.GetComponent<TabController>();
            TabController.OnSwitch -= this.OnTabSwitch;

            this.Close();
            this.SaveData.Backup();
        }

        public void AddScripts()
        {
            this.AddTabControllerScript();
            this.AddScrollListScript();
        }

        public void AddTabControllerScript()
        {
            this.GetChild("Frame/Menu").gameObject.AddComponent<TabController>();

            TabController TabController = this.GetChild("Frame/Menu").gameObject.GetComponent<TabController>();
            
            // Challenges
            TabController.AddPage(0, this.GetChild("Frame/Pages/Challenges").gameObject);
            TabController.AddMenuButton(0, this.GetChild("Frame/Menu/Challenges/Button").GetComponent<Button>());
            TabController.AddMenuButtonImage(0, this.GetChild("Frame/Menu/Challenges").GetComponent<Image>());

            // EnergyLink
            TabController.AddPage(1, this.GetChild("Frame/Pages/EnergyLink").gameObject);
            TabController.AddMenuButton(1, this.GetChild("Frame/Menu/EnergyLink/Button").GetComponent<Button>());
            TabController.AddMenuButtonImage(1, this.GetChild("Frame/Menu/EnergyLink").GetComponent<Image>());

            TabController.SwitchTab(0);

            TabController.OnSwitch += this.OnTabSwitch;
        }

        private void OnTabSwitch(int index)
        {
            Helper.Debug($"[ArchipelagoWindow::OnTabSwitch] {index}");
            // Refresh Bank account when switched to EnergyLink Tab
            if (index == 1)
            {
                this._help();
                this.ArchipelagoController.RefreshBankAccount();
            }
        }

        public void AddScrollListScript()
        {
            this.GetChild("Frame/Pages/EnergyLink/History/Scroll View").gameObject.AddComponent<ScrollList>();
        }

        public void SetStatus(_Status.States state, bool silent = false)
        {
            // Send a Parkitect Message on Connection
            if (!silent)
            {
                string msg = state.ToString();
                this.ParkitectController.SendMessage(char.ToUpper(msg[0]) + msg.Substring(1).ToLower());
            }

            this.State = state;
            this.GetChild("Frame/Header/Status").GetComponent<Image>().color = _Status.GetColor(state);
        }

        public void SetVersion(string version)
        {
            string v = "v" + version;
            this.GetChild("Frame/Pages/Challenges/Footer/Version").GetComponent<TextMeshProUGUI>().text = v;
            this.GetChild("Frame/Pages/EnergyLink/Footer/Version").GetComponent<TextMeshProUGUI>().text = v;
        }

        public void SetChallenge(Challenge challenge)
        {
            if (challenge.LocationId < 0)
            {
                Helper.Debug($"[ArchipelagoWindow::SetChallenge] Challenge row {challenge.SerializedPanelId} completed");
                this.RemoveSkipButton(challenge);
                return;
            }

            if (this.SaveData.HasFinished())
            {
                return;
            }

            Helper.Debug($"[ArchipelagoWindow::SetChallenge] {challenge.SerializedPanelId} - {challenge.PanelId}");
            if (this.CurrentChallenges.Count >= 3)
            {
                Helper.Debug($"[ArchipelagoWindow::SetChallenge] Can not add more Challenges!");
                return;
            }

            this.CurrentChallenges.Add(challenge);
            this.SetChallengeListener(challenge);

            this.GetPanelChild(challenge.SerializedPanelId, "/Button/Text").GetComponent<TextMeshProUGUI>().text = $"[{challenge.Index}] {challenge.Text()}";
            this.GetPanelChild(challenge.SerializedPanelId, "/Button/SubText").GetComponent<TextMeshProUGUI>().text = challenge.SubText();
        }
     
        public void SetChallenges(List<Challenge> challenges)
        {
            if (challenges.Count > 3)
            {
                Helper.Debug($"[ArchipelagoWindow::SetChallenges] Can not add more then 3 Challenges!");
                return;
            }

            foreach (Challenge challenge in challenges)
            {
                this.SetChallenge(challenge);
            }
        }

        public void RemoveChallenge(Challenge challenge)
        {
            if (!this.CurrentChallenges.Contains(challenge))
            {
                Helper.Debug($"[ArchipelagoWindow::RemoveChallenge] Challenge can not be deleted - not found");
                return;
            }

            int index = 0;

            if (challenge.LocationId < 3)
            {
                index = challenge.LocationId;
            } else
            {
                index = challenge.LocationId % 3;
            }

            this.GetPanelChild(challenge.SerializedPanelId, "/Button/Text").GetComponent<TextMeshProUGUI>().text = "";
            this.GetPanelChild(challenge.SerializedPanelId, "/Button/SubText").GetComponent<TextMeshProUGUI>().text = "";
            this.RemoveChallengeListener(challenge);
            this.CurrentChallenges.Remove(challenge);
            this.all_challenges.Remove(challenge);
        }
      
        public void RemoveChallenges(List<Challenge> challenges)
        {
            foreach (Challenge challenge in challenges.ToList())
            {
                Helper.Debug($"[ArchipelagoWindow::RemoveChallenges] {challenge.PanelId}");
                this.RemoveChallenge(challenge);
            }
        }

        public void Finish ()
        {
            Helper.Debug($"[ArchipelagoWindow::Finish]");

            if (this.CurrentChallenges.Count > 0)
            {
                this.RemoveChallenges(this.CurrentChallenges);
            }

            if (this.SaveData != null)
            {
                this.SaveData.Finish();
            }
        }

        public void NextChallenge (Challenge challenge)
        {
            if (this.SaveData.HasFinished())
            {
                return;
            }

            Challenge nextChallenge = this.all_challenges.Where(c => c.LocationId == challenge.LocationId + 3).FirstOrDefault();
          
            if (nextChallenge == null)
            {
                string stringId = challenge.SerializedPanelId.Split(' ')[1];
                Int32.TryParse(stringId, out int id);
                this.CurrentChallenges.Add(new Challenge(this.ParkitectController, id * -1));
                this.SaveData.SetChallenges(this.CurrentChallenges);
                this.RemoveSkipButton(challenge);

                bool AllChallengesDone = this.CurrentChallenges.All(c => c.LocationId < 0);
                Helper.Debug($"[ArchipelagoWindow::NextChallenge] AllChallengesDone -> {AllChallengesDone}");

                if (AllChallengesDone)
                {
                    this.ArchipelagoController.GoalAchieved();
                }
                return;
            }
            Helper.Debug($"[ArchipelagoWindow::NextChallenge] next PanelId -> {nextChallenge.PanelId}");

            this.SetChallenge(nextChallenge);
        }
       
        public void HandOver(List<Challenge> Challenges)
        {
            Helper.Debug($"[ArchipelagoWindow::HandOver]");
            this.SaveData = GetComponent<SaveData>();

            if (this.SaveData == null)
            {
                this.ParkitectController.SendMessage("Something failed on the Handover from Archipelago");
                return;
            }

            if (this.SaveData.HasFinished())
            {
                return;
            }

            this.UpdateSkipText();
            this.UpdateSpeedups();
            this.all_challenges = Challenges;

            this.EnergyLinkHistory = new EnergyLinkHistory(this.SaveData);
            List<int> locationIds = this.SaveData.GetChallenges();
            
            // No challenges found, so we start from the beginning :)
            if (locationIds == null || locationIds.Count <= 0)
            {
                Helper.Debug($"[ArchipelagoWindow::HandOver] no Challenges ever done");
                this.SetChallenges(Challenges.Take(3).ToList());
                this.SaveData.SetChallenges(CurrentChallenges);
                return;
            }

            List<int> deletable_location_ids = new List<int>();
            foreach (int locationId in locationIds)
            {
                // Challenge row is done! keep pseudo challenge
                if (locationId < 0)
                {
                    Challenge finishedChallenge = new Challenge(this.ParkitectController, locationId);
                    this.SetChallenge(finishedChallenge);
                    continue;
                }

                Helper.Debug($"[ArchipelagoWindow::HandOver] {locationId}");
                if (locationId >= 3)
                {
                    int rest = locationId % 3;
                    List<int> meehhh = Enumerable.Range(rest, locationId - 3).Reverse().Where(n => n % 3 == rest).ToList();
                    deletable_location_ids.AddRange(meehhh);
                }

                Challenge challenge = Challenges.Where(c => c.LocationId == locationId).First();
                this.SetChallenge(challenge);
            }

            this.all_challenges = Challenges.Where(c => !deletable_location_ids.Contains(c.LocationId)).ToList();
        }

        private void SetChallengeListener(Challenge challenge)
        {
            this.GetPanelChild(challenge.SerializedPanelId, "/Skip").GetComponent<Button>().onClick.AddListener(() => { this.OnChallengeSkip(challenge); });
            this.GetPanelChild(challenge.SerializedPanelId, "/Button").GetComponent<Button>().onClick.AddListener(() => { this.OnChallengeClicked(challenge); });
        }

        private void RemoveSkipButton(Challenge challenge)
        {
            Helper.Debug("[ArchipelagoWindow::RemoveSkipButton]");
            Button button = this.GetPanelChild(challenge.SerializedPanelId, "/Skip").GetComponent<Button>();
            
            if (button == null)
            {
                return;
            }
                
            button.gameObject.SetActive(false);
        }

        private void RemoveChallengeListener(Challenge challenge)
        {
            this.GetPanelChild(challenge.SerializedPanelId, "/Skip").GetComponent<Button>().onClick.RemoveAllListeners();
            this.GetPanelChild(challenge.SerializedPanelId, "/Button").GetComponent<Button>().onClick.RemoveAllListeners();
        }

        public void UpdateSkipText()
        {
            this.GetChild("Frame/Pages/Challenges/Footer/Skip List/Count").GetComponent<TextMeshProUGUI>().text = this.SaveData.GetSkipCount().ToString();
        }

        public void UpdateSpeedups()
        {
            int maxSpeed = this.SaveData.GetMaxSpeedup();

            foreach (int speed in Constants.Player.SpeedupOptions)
            {
                if (maxSpeed == -1 || maxSpeed >= speed)
                {
                    this.EnableSpeedupButton(speed);
                }
            }
        }

        public void EnableSpeedupButton(int id)
        {
            string buttonList = "Frame/Pages/Challenges/Footer/Speedup List/Button List";
            this.GetChild($"{buttonList}/Speed {id}").GetComponent<Button>().interactable = true;
        }

        private void OnChallengeSkip(Challenge challenge)
        {
            this.SkipChallenge(challenge);
        }

        private void OnChallengeClicked(Challenge challenge)
        {
            bool check = this.nextCheckTime < Time.time && challenge.Check();
            Helper.Debug("[ArchipelagoWindow::OnChallengeClicked] Check: " + check.ToString());
            if (check)
            {
                this.nextCheckTime = Time.time + Constants.NextCheckTimeDelay + .05f;
                this.FinishChallenge(challenge);
                return;
            }
        }

        public void FinishChallenge(Challenge challenge)
        {
            Helper.Debug("[ArchipelagoWindow::FinishChallenge]");
            this._help();

            this.RemoveChallenge(challenge);
            this.NextChallenge(challenge);
            this.SaveData.SetChallenges(this.CurrentChallenges);
            this.ArchipelagoController.CompleteLocation(challenge.LocationId, this.SaveData);
        }

        public void SkipChallenge(string challengeSerializedPanelId, bool force = false)
        {
            Challenge Challenge = this.CurrentChallenges.Where(c => c.SerializedPanelId == challengeSerializedPanelId).FirstOrDefault();
            this.SkipChallenge(Challenge, force);
        }

        public void SkipChallenge(Challenge Challenge, bool force = false)
        {
            if (!force && !this.SaveData.HasSkipsLeft())
            {
                this.ParkitectController.SendMessage("No skips left");
                return;
            }

            if (!force)
            {
                this.SaveData.DecreaseSkip();
            }

            this.UpdateSkipText();
            this.FinishChallenge(Challenge);
        }

        public Transform GetPanelChild(string serializedPanelId, string hierarchy = null)
        {
            return this.GetChild($"Frame/Pages/Challenges/List/{serializedPanelId}{hierarchy}");
        }

        private void SetSpeedupButtons()
        {
            string buttonList = "Frame/Pages/Challenges/Footer/Speedup List/Button List";

            foreach(int speed in Constants.Player.SpeedupOptions)
            {
                Button btn = this.GetChild($"{buttonList}/Speed {speed}").GetComponent<Button>();
                btn.interactable = false;
                btn.onClick.AddListener(() => { this.ParkitectController.PlayerRaiseSpeed(speed); });
            }
        }

        private void SetCLI()
        {
            TMP_InputField input = this.GetChild($"Frame/Pages/Challenges/CLI").GetComponent<TMP_InputField>();

            if (input == null)
            {
                Helper.Debug("[ArchipelagoWindow::SetCLI] no input field");
                return;
            }

            input.onSubmit.AddListener((string text) =>
            {
                this._help();

                if (text.Length > 0)
                {
                    this.ArchipelagoController.Speak(text);
                    input.text = string.Empty; // clear input field
                }
            });
        }

        public void UpdateBankAccount(float money)
        {
            string _money = money.ToString("N0", Constants.GermanCulture);
            this.GetChild("Frame/Pages/EnergyLink/Available").GetComponent<TextMeshProUGUI>().text = $"Bank Account: ${_money}";
        }

        public void SetFee(float fee)
        {
            this.GetChild("Frame/Pages/EnergyLink/Footer/Fee").GetComponent<TextMeshProUGUI>().text = $"Fee: {fee}%";
        }

        public void AddEnergyLinkMessage(EnergyLinkItem item, bool add = true)
        {
            if (add)
            {
                this.EnergyLinkHistory.AddItem(item);
            }

            this.GetChild("Frame/Pages/EnergyLink/History/Scroll View").gameObject.GetComponent<ScrollList>().AddItem(item.Message());
        }

        public float GetMoneyFromBankInput()
        {
            TMP_InputField input = this.GetChild("Frame/Pages/EnergyLink/Bank/Money").GetComponent<TMP_InputField>();
            return float.Parse(input.text, CultureInfo.InvariantCulture);
        }

        public void ClearMoneyFromBankInput()
        {
            this.GetChild("Frame/Pages/EnergyLink/Bank/Money").GetComponent<TMP_InputField>().text = string.Empty;
        }

        private void WithdrawMoneyFromBank(float money, bool hitMax = false)
        {
            if (money < 0)
            {
                this.ParkitectController.SendMessage($"[EnergyLink] Withdraw at least {Constants.EnergyLink.MinDepositMoney}");
                return;
            }

            Helper.Debug($"[ArchipelagoWindow::WithdrawMoneyFromBank] {money} {hitMax}");
            this._help();
            float taxedMoney = this.ArchipelagoController.WithdrawMoneyFromBank(money, hitMax);
            this.ParkitectController.PlayerAddMoney(taxedMoney, true);
        }

        private void DepositMoneyToBank(float money)
        {
            if (money < Constants.EnergyLink.MinDepositMoney)
            {
                this.ParkitectController.SendMessage($"[EnergyLink] Deposit at least {Constants.EnergyLink.MinDepositMoney}");
                return;
            }

            Helper.Debug($"[ArchipelagoWindow::DepositMoneyToBank] {money}");
            this._help();
            this.ArchipelagoController.DepositMoneyToBank(money);
            this.ParkitectController.PlayerRemoveMoney(money);
        }

        private void SetBankListeners()
        {
            this.GetChild("Frame/Pages/EnergyLink/Bank/Actions/Withdraw").GetComponent<Button>().onClick.AddListener(() =>
            {
                this._help();
                float maxMoney = this.ArchipelagoController.RefreshBankAccount();
                float money = this.GetMoneyFromBankInput();
                this.WithdrawMoneyFromBank(money, money >= maxMoney);
                this.ClearMoneyFromBankInput();
                this.ArchipelagoController.RefreshBankAccount();
            });

            this.GetChild("Frame/Pages/EnergyLink/Bank/Actions/Deposit").GetComponent<Button>().onClick.AddListener(() =>
            {
                this._help();
                this.ArchipelagoController.RefreshBankAccount();

                float money = this.GetMoneyFromBankInput();
                this.DepositMoneyToBank(money);
                this.ClearMoneyFromBankInput();
                this.ArchipelagoController.RefreshBankAccount();
            });

            this.GetChild("Frame/Pages/EnergyLink/Bank/Balance").GetComponent<Button>().onClick.AddListener(() =>
            {
                this._help();
                float money = this.ArchipelagoController.RefreshBankAccount();
                this.WithdrawMoneyFromBank(money, true);
                this.ClearMoneyFromBankInput();
                this.ArchipelagoController.RefreshBankAccount();
            });
        }

        private void _help()
        {
            if (this.ArchipelagoController == null)
            {
                this.ArchipelagoController = GetComponent<ArchipelagoController>();
            }
        }
    }
}