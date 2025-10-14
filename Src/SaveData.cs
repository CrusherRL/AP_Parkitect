using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Controller;
using MiniJSON;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ArchipelagoMod.Src
{
    class SaveDataExport
    {
        public bool finished = false;

        public List<int> current_challenges = new List<int>(); // our 3 current challenges
       
        public List<string> unlocked_items { get; set; } = new List<string>(); // Prefab name of the Attraction/Shop we received
        
        public List<long> unlocked_ap_items = new List<long>(); // AP Item that we already received
        
        public List<long> pending_locations = new List<long>(); // Challenge Locations that we were not able to send it to the server
    }

    class SaveData : MonoBehaviour
    {
        public GameObject gameObject;
        public MonoBehaviour component;

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

            this.ParkitectController.PlayerRemoveAllRides();
            this.ParkitectController.PlayerRemoveAllStalls();

            Helper.Debug($"[SaveData::Update]");
            this.SaveDataExport = new SaveDataExport();

            this.SaveDataExport = SaveData.Load();

            if (this.SaveDataExport != null)
            {
                foreach (string thing in this.SaveDataExport.unlocked_items)
                {
                    if (Constants.Attraction.All.Contains(thing))
                    {
                        this.ParkitectController.PlayerAddAttraction(thing);
                    }
                    else if (Constants.Stall.All.Contains(thing))
                    {
                        this.ParkitectController.PlayerAddStall(thing);
                    }
                }
            }
        }
        
        public SaveDataExport GetExport()
        {
            return this.SaveDataExport;
        }

        public List<int> GetChallenges()
        {
            if (this.SaveDataExport == null)
            {
                return new List<int>();
            }
            return this.SaveDataExport.current_challenges;
        }

        public void SetChallenges(List<Challenge> challenges)
        {
            this.SetChallenges(challenges.Select(c => c.LocationId).ToList());
        }

        public void SetChallenges(List<int> challenges)
        {
            if (this.SaveDataExport == null)
            {
                this.SaveDataExport = new SaveDataExport();
            }

            this.SaveDataExport.current_challenges = challenges;
            this.Save();
        }

        public bool HasUnlockedItem(Prefabs PrefabName)
        {
            return this.HasUnlockedItem(PrefabName.ToString());
        }

        public bool HasUnlockedItem(string PrefabName)
        {
            if (this.SaveDataExport == null)
            {
                return false;
            }
            return this.SaveDataExport.unlocked_items.Contains(PrefabName);
        }

        public void AddUnlockedItem(Prefabs name)
        {
            this.AddUnlockedItem(name.ToString());
        }

        public void AddUnlockedItem(string name)
        {
            if (this.SaveDataExport.unlocked_items.Contains(name))
            {
                return;
            }
            this.SaveDataExport.unlocked_items.Add(name);
            this.Save();
        }
        
        public bool HasUnlockedAPItem(long id)
        {
            if (this.SaveDataExport == null)
            {
                return false;
            }
            return this.SaveDataExport.unlocked_ap_items.Contains(id);
        }

        public void SetUnlockedAPItem(long id)
        {
            this.SaveDataExport.unlocked_ap_items.Add(id);
            this.Save();
        }

        public void AddPendingLocation(long id)
        {
            this.SaveDataExport.pending_locations.Add(id);
            this.Save();
        }

        public List<long> GetPendingLocations()
        {
            if (this.SaveDataExport == null)
            {
                return new List<long>();
            }
            return this.SaveDataExport.pending_locations;
        }

        public void DeleteAllPendingLocations()
        {
            this.SaveDataExport.pending_locations = new List<long>();
        }

        public void Save()
        {
            Helper.Debug("[SaveData::Save]");
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            string json = JsonConvert.SerializeObject(SaveDataExport, Formatting.None, jsonSettings);
            string filePath = Constants.ModPath + Constants.ScenarioName + ".data";

            File.WriteAllText(filePath, json);
            this.Backup(filePath, json);
        }

        public void Backup(string filePath, string json)
        {
            Helper.Debug("[SaveData::Backup]");
            File.WriteAllText(filePath + ".backup", json);
        }

        public static SaveDataExport Load()
        {
            Helper.Debug("[SaveData::Load]");
            string filePath = Constants.ModPath + Constants.ScenarioName + ".data";
            string json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<SaveDataExport>(json);
        }

        public bool HasFinished ()
        {
            if (this.SaveDataExport == null)
            {
                return false;
            }
            return this.SaveDataExport.finished;
        }
        public void Finish()
        {
            this.SaveDataExport.finished = true;
            this.Save();
        }
    }
}
