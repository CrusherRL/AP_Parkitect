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

        public int available_skips = 5;

        public int max_speedup = -1; // -1 is always false -> no progressive speedup. 3 or more meant to be max speedup with progressive speedup

        public List<int> current_challenges = new List<int>(); // our 3 current challenges
       
        public List<string> unlocked_items { get; set; } = new List<string>(); // Prefab name of the Attraction/Shop we received
        
        public List<long> unlocked_locations = new List<long>(); // Location Id's we received from the server
        
        public List<long> pending_locations = new List<long>(); // Challenge Locations that we were not able to send it to the server
    }

    class SaveData : MonoBehaviour
    {
        public GameObject gameObject;
        public MonoBehaviour component;

        private SaveDataExport SaveDataExport = null;
        private ParkitectController ParkitectController = null;

        private bool Loaded = false;

        public void Update ()
        {
            if (this.Loaded || Constants.ScenarioName == null)
            {
                return;
            }

            if (ParkitectController == null)
            {
                this.ParkitectController = GetComponent<ParkitectController>();
            }

            this.Loaded = true;

            this.ParkitectController.PlayerRemoveAllRides();
            this.ParkitectController.PlayerRemoveAllStalls();

            Helper.Debug($"[SaveData::Update]");
            this.SaveDataExport = SaveData.Load();

            if (this.SaveDataExport == null)
            {
                this.SaveDataExport = new SaveDataExport();
                this.Save();
                return;
            }

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
        
        public SaveDataExport GetExport()
        {
            return this.SaveDataExport;
        }

        public List<int> GetChallenges()
        {
            this._help();
            return this.SaveDataExport.current_challenges;
        }

        public void SetChallenges(List<Challenge> challenges)
        {
            this.SetChallenges(challenges.Select(c => c.LocationId).ToList());
        }

        public void SetChallenges(List<int> challenges)
        {
            this._help();
            this.SaveDataExport.current_challenges = challenges;
            this.Save();
        }

        public bool HasUnlockedItem(Prefabs PrefabName)
        {
            return this.HasUnlockedItem(PrefabName.ToString());
        }

        public bool HasUnlockedItem(string PrefabName)
        {
            this._help();
            return this.SaveDataExport.unlocked_items.Contains(PrefabName);
        }

        public void AddUnlockedItem(Prefabs name)
        {
            this.AddUnlockedItem(name.ToString());
        }

        public void AddUnlockedItem(string name)
        {
            this._help();
            this.SaveDataExport.unlocked_items.Add(name);
            this.Save();
        }
        
        public bool HasUnlockedAPLocation(long id)
        {
            this._help();
            return this.SaveDataExport.unlocked_locations.Contains(id);
        }

        public void SetUnlockedAPLocation(long id)
        {
            this.SaveDataExport.unlocked_locations.Add(id);
            this.Save();
        }

        public void AddPendingLocation(long id)
        {
            this.SaveDataExport.pending_locations.Add(id);
            this.Save();
        }

        public List<long> GetPendingLocations()
        {
            this._help();
            return this.SaveDataExport.pending_locations;
        }

        public void DeleteAllPendingLocations()
        {
            this.SaveDataExport.pending_locations = new List<long>();
        }

        public void IncreaseSkip()
        {
            this._help();
            this.SaveDataExport.available_skips += 1;
            this.Save();
        }
      
        public bool HasSkipsLeft()
        {
            return this.GetSkipCount() > 0;
        }
       
        public void DecreaseSkip()
        {
            this.SaveDataExport.available_skips -= 1;
            this.Save();
        }

        public int GetSkipCount()
        {
            this._help();
            return this.SaveDataExport.available_skips;
        }

        public void IncreaseMaxSpeedup()
        {
            this._help();
            this.SaveDataExport.max_speedup += 1;
            this.Save();
        }
        public int GetMaxSpeedup()
        {
            this._help();
            return this.SaveDataExport.max_speedup;
        }

        public void InitMaxSpeedup()
        {
            // this method only gets called when progressive speedup option is used
            // -1 is means no progressive speedup
            if (this.GetMaxSpeedup() >= 3)
            {
                return;
            }

            this.SaveDataExport.max_speedup = 3;
        }

        private void _help()
        {
            if (this.SaveDataExport == null)
            {
                this.SaveDataExport = new SaveDataExport();
            }
        }

        public void Save()
        {
            Helper.Debug("[SaveData::Save]");
            File.WriteAllText(SaveData.GetFilePath(), this.MakeJsonData());
        }

        public void Backup()
        {
            if (this.SaveDataExport == null) {
                return;
            }
            Helper.Debug("[SaveData::Backup]");
            File.WriteAllText(SaveData.GetFilePath() + ".backup", this.MakeJsonData());
        }

        public static SaveDataExport Load()
        {
            Helper.Debug("[SaveData::Load]");
            try
            {
                string json = File.ReadAllText(SaveData.GetFilePath());
                if (string.IsNullOrEmpty(json))
                {
                    return new SaveDataExport();
                }

                return JsonConvert.DeserializeObject<SaveDataExport>(json);
            } catch {
                return new SaveDataExport();
            }
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

        public static string GetFilePath()
        {
            return Constants.ModPath + Constants.ScenarioName + ".data";
        }

        private string MakeJsonData()
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.SerializeObject(this.SaveDataExport, Formatting.None, jsonSettings);
        }
    }
}
