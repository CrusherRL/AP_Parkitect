using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Controller;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ArchipelagoMod.Src.Challenges.Challenge;

namespace ArchipelagoMod.Src
{
    class SaveDataExport
    {
        public List<string> unlocked { get; set; } = new List<string>();
    
        public List<Challenge.ChallengeExport> challenges { get; set; } = null;
     
        public List<long> PendingLocations = new List<long>();
    }

    class SaveData : MonoBehaviour
    {
        [JsonIgnore] public GameObject gameObject;
        [JsonIgnore] public MonoBehaviour component;

        private SaveDataExport SaveDataExport = null;
        private ParkitectController ParkitectController = null;

        public void Update ()
        {
            if (this.SaveDataExport != null)
            {
                return;
            }

            if (Constants.ScenarioName == null)
            {
                return;
            }

            if (ParkitectController == null)
            {
                this.ParkitectController = GetComponent<ParkitectController>();
            }

            Helper.Debug($"[SaveData::Update] set new SaveDataExport");
            this.SaveDataExport = new SaveDataExport();

            this.SaveDataExport= Helper.GetChallengeFile();
            Helper.Debug($"[SaveData::Update] - " + this.SaveDataExport.ToString());

            this.ParkitectController.PlayerRemoveAllRides();
            this.ParkitectController.PlayerRemoveAllStalls();

            if (this.SaveDataExport != null)
            {
                foreach (string p in this.SaveDataExport.unlocked)
                {
                    if (Constants.Attraction.All.Contains(p))
                    {
                        this.ParkitectController.PlayerAddAttraction(p);
                    }
                    else if (Constants.Stall.All.Contains(p))
                    {
                        this.ParkitectController.PlayerAddStall(p);
                    }
                }
            }
        }

        public List<Challenge> GetChallenges()
        {
            return (this.SaveDataExport.challenges ?? new List<ChallengeExport>()).Select(e => Challenge.FromExport(e, this.ParkitectController)).ToList();
        }

        public void SetChallenges(List<Challenge> challenges)
        {
            if (this.SaveDataExport == null)
            {
                this.SaveDataExport = new SaveDataExport();
            }

            this.SaveDataExport.challenges = challenges.Select(c => c.GetExport()).ToList();
        }

        public SaveDataExport GetExport()
        {
            return this.SaveDataExport;
        }

        public void SetExport(SaveDataExport SaveDataExport)
        {
            this.SaveDataExport = SaveDataExport;
        }

        public bool HasUnlocked(Prefabs name)
        {
            return this.HasUnlocked(name.ToString());
        }

        public bool HasUnlocked(string name)
        {
            if (this.SaveDataExport == null)
            {
                return false;
            }
            return this.SaveDataExport.unlocked.Contains(name);
        }

        public void AddUnlocked(Prefabs name)
        {
            this.AddUnlocked(name.ToString());
        }

        public void AddUnlocked(string name)
        {
            if (this.SaveDataExport.unlocked.Contains(name))
            {
                return;
            }
            this.SaveDataExport.unlocked.Add(name);
        }

        public void AddLocation(long id)
        {
            this.SaveDataExport.PendingLocations.Add(id);
        }

        public List<long> GetLocations()
        {
            if (this.SaveDataExport == null)
            {
                return new List<long>();
            }
            return this.SaveDataExport.PendingLocations;
        }

        public void DeleteAllLocations()
        {
            this.SaveDataExport.PendingLocations = new List<long>();
        }
    }
}
