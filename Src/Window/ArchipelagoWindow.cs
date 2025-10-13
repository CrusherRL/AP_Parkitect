using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Controller;
using System.Collections.Generic;
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
        private bool finished = false;

        public float nextCheckTime = Time.time;
        public SaveData SaveData = null;

        public override string BundleFilename { get; set; } = "archipelagowindow";

        public override KeyCode KeyCode { get; set; } = KeyCode.Z;
        
        public override void OnAwake ()
        {
            Helper.Debug($"[ArchipelagoWindow::OnAwake]");
            this.ParkitectController = GetComponent<ParkitectController>();
            this.SetStatus(this.State, true);
            this.SetSpeedupButtons();
            this.ToggleActiveState();

            Helper.Debug($"[ArchipelagoWindow::OnAwake] -> Booted");
        }

        // -----------------------------
        // Doing things lol
        // -----------------------------
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
            this.GetChild("Frame/Body/Footer/Version").GetComponent<TextMeshProUGUI>().text = "v" + version;
        }

        public void SetChallenge(Challenge challenge)
        {
            if (this.finished)
            {
                return;
            }

            Helper.Debug($"[ArchipelagoWindow::SetChallenge] -> {challenge.SerializedPanelId} - {challenge.PanelId}");
            if (this.CurrentChallenges.Count >= 3)
            {
                Helper.Debug($"[ArchipelagoWindow::SetChallenge] -> Can not add more Challenges!");
                return;
            }

            Helper.Debug($"[ArchipelagoWindow::SetChallenge] -> {challenge.SerializedPanelId} - {challenge.PanelId}");

            this.CurrentChallenges.Add(challenge);
            this.SetChallengeListener(challenge);

            this.GetPanelChild(challenge.SerializedPanelId, "/Button/Text").GetComponent<TextMeshProUGUI>().text = $"({challenge.Index}) {challenge.Text()}";
            this.GetPanelChild(challenge.SerializedPanelId, "/Button/SubText").GetComponent<TextMeshProUGUI>().text = challenge.SubText();
        }
     
        public void SetChallenges(List<Challenge> challenges)
        {
            if (challenges.Count > 3)
            {
                Helper.Debug($"[ArchipelagoWindow::SetChallenges] -> Can not add more then 3 Challenges!");
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
                Helper.Debug($"[ArchipelagoWindow::RemoveChallenge] -> Challenge can not be deleted - not found");
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
            foreach (Challenge challenge in challenges)
            {
                this.RemoveChallenge(challenge);
            }
        }

        public void Finish ()
        {
            this.RemoveChallenges(this.CurrentChallenges);
        }

        public void NextChallenge (int finishedLocationId)
        {
            Challenge challenge = this.all_challenges.Where(c => c.LocationId == finishedLocationId + 3).FirstOrDefault();

            if (challenge == null)
            {
                //this.ParkitectController.SendMessage("Finished all Challenges in that row!");
                return;
            }

            this.SetChallenge(challenge);
        }
       
        public void HandOver (List<Challenge> Challenges)
        {
            Helper.Debug($"[ArchipelagoWindow::HandOver]");
            this.all_challenges = Challenges;

            this.SaveData = GetComponent<SaveData>();

            if (this.SaveData == null)
            {
                this.ParkitectController.SendMessage("Something failed on the Handover from Archipelago");
                return;
            }

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
                Helper.Debug($"[ArchipelagoWindow::HandOver] -> {locationId}");
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
            this.GetPanelChild(challenge.SerializedPanelId, "/Button").GetComponent<Button>().onClick.AddListener(() => { this.OnChallengeClicked(challenge); });
        }
      
        private void RemoveChallengeListener(Challenge challenge)
        {
            this.GetPanelChild(challenge.SerializedPanelId, "/Button").GetComponent<Button>().onClick.RemoveAllListeners();
        }

        private void OnChallengeClicked(Challenge challenge)
        {
            if (this.nextCheckTime < Time.time && challenge.Check())
            {
                this.nextCheckTime = Time.time + .5f;
                this.FinishChallenge(challenge);
                return;
            }
        }

        public void FinishChallenge(Challenge challenge)
        {
            Helper.Debug("[ArchipelagoWindow::FinishChallenge]");
            if (this.ArchipelagoController == null)
            {
                this.ArchipelagoController = GetComponent<ArchipelagoController>();
            }

            this.RemoveChallenge(challenge);
            this.NextChallenge(challenge.LocationId);
            this.SaveData.SetChallenges(this.CurrentChallenges);
            this.ArchipelagoController.CompleteLocation(challenge.LocationId, this.SaveData);
        }

        public Transform GetPanelChild(string serializedPanelId, string hierarchy = null)
        {
            return this.GetChild($"Frame/Body/List/{serializedPanelId}{hierarchy}");
        }

        private void SetSpeedupButtons ()
        {
            string buttonList = "Frame/Body/Footer/Speedup List/Button List";
            this.GetChild($"{ buttonList }/Speed 5").GetComponent<Button>().onClick.AddListener(() => { this.ParkitectController.PlayerRaiseSpeed(5); });
            this.GetChild($"{ buttonList }/Speed 7").GetComponent<Button>().onClick.AddListener(() => { this.ParkitectController.PlayerRaiseSpeed(7); });
            this.GetChild($"{ buttonList }/Speed 9").GetComponent<Button>().onClick.AddListener(() => { this.ParkitectController.PlayerRaiseSpeed(9); });
        }
    }
}