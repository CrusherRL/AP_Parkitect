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

        public override string BundleFilename { get; set; } = "archipelagowindow";

        public override KeyCode KeyCode { get; set; } = KeyCode.Z;
        
        public override void OnAwake ()
        {
            Helper.Debug($"[ArchipelagoWindow::OnAwake]");
            this.ParkitectController = GetComponent<ParkitectController>();
            this.SetStatus(this.State, true);
            this.SetSpeedupButtons();
            this.ToggleActiveState();
            
            List<Challenge> challenges = Helper.GetChallengeJson(this.ParkitectController);
            if (challenges.Count > 0)
            {
                Helper.Debug($"[ArchipelagoWindow::OnAwake] -> Challenges found in file. Recovering");
                this.all_challenges = challenges;
                this.SetChallenges();
            }

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

            if (this.CurrentChallenges.Count > 3)
            {
                Helper.Debug($"[ArchipelagoWindow::SetChallenge] -> Can not add more Challenges!");
                return;
            }

            Helper.Debug($"[ArchipelagoWindow::SetChallenge] -> {challenge.SerializedPanelId} - {challenge.PanelId}");

            this.CurrentChallenges.Add(challenge);
            this.SetChallengeListener(challenge);

            this.GetPanelChild(challenge.SerializedPanelId, "/Button/Text").GetComponent<TextMeshProUGUI>().text = challenge.Text();
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
      
        private void SetChallenges()
        {
            Helper.Debug($"[ArchipelagoWindow::SetChallenges]");
            for (int i = 1; i <= 3; i += 1)
            {
                Challenge challenge = this.all_challenges.Where(c => c.SerializedPanelId == $"Challenge {i}").FirstOrDefault();

                // on late stage, the challenge could be resolved
                if (challenge != null)
                {
                    this.SetChallenge(challenge);
                }
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
            if (this.all_challenges.Count == 0 && Challenges.Count > 0)
            {
                // rewrite this file based on the earliest state of location
                this.all_challenges = Challenges;
                Helper.UpdateChallengeJson(this.all_challenges);
            }

            if (this.CurrentChallenges.Count < 3)
            {
                this.SetChallenges(this.all_challenges
                    .Where(s => s.LocationId < 3 - this.CurrentChallenges.Count)
                    .ToList());
            }
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
            if (challenge.nextCheckTime < Time.time && challenge.Check())
            {
                challenge.nextCheckTime = 0f;
                this.FinishChallenge(challenge);
                return;
            }

            challenge.nextCheckTime = Time.time + 3f;
        }

        public void FinishChallenge(Challenge challenge)
        {
            Helper.Debug("[ArchipelagoWindow::FinishChallenge]");
            if (this.ArchipelagoController == null)
            {
                this.ArchipelagoController = GetComponent<ArchipelagoController>();
            }

            // Solve this Challenge
            if (this.ArchipelagoController.CompleteLocation(challenge.LocationId))
            {
                this.RemoveChallenge(challenge);
                Helper.UpdateChallengeJson(this.all_challenges);
                this.NextChallenge(challenge.LocationId);
            }
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