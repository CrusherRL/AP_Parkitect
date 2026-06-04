using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Controller;
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

        public bool enabled_decorations = false;

        public bool enabled_utility_buildings = false;

        public bool enabled_statistics = false;

        public int available_skips = 5;

        public int max_speedup = -1; // -1 is no progressive speedup. 3 or more meant to be max speedup with progressive speedup

        public List<int> current_challenges = new List<int>(); // our 3 current challenges

        public string seed = string.Empty;
       
        public List<string> unlocked_items { get; set; } = new List<string>(); // Prefab name of the Attraction/Shop we received
        
        public List<long> unlocked_locations = new List<long>(); // Location Id's we received from the server
        
        public List<long> pending_locations = new List<long>(); // Challenge Locations that we were not able to send it to the server
    }

    class SaveData : MonoBehaviour
    {
        private SaveDataExport SaveDataExport = null;
        private ParkitectController ParkitectController = null;

        private bool Loaded = false;

        public void Init(string seed)
        {
            if (this.Loaded || Constants.ScenarioName == null)
            {
                return;
            }

            if (ParkitectController == null)
            {
                this.ParkitectController = GetComponent<ParkitectController>();
            }

            Helper.Debug($"[SaveData::Init]");

            this.CreateSavegameFolder(seed);
            this.SaveDataExport = SaveData.Load(seed);

            if (this.SaveDataExport == null)
            {
                this.SaveDataExport = new SaveDataExport();
                this.SetSeed(seed);
                this.Save();
                return;
            }

            this.Loaded = true;
        }

        public void LoadItems()
        {
            Helper.Debug($"[SaveData::LoadItems] Load");
            this.ParkitectController.PlayerRemoveAllRides();
            this.ParkitectController.PlayerRemoveAllStalls();

            if (this.GetEnabledDecorations())
            {
                this.ParkitectController.PlayerRemoveAllDecorations();
            }

            if (this.GetEnabledUtilityBuildings())
            {
                this.ParkitectController.PlayerRemoveAllUtilityBuildings();
            }

            if (this.GetEnabledStatistics())
            {
                this.ParkitectController.PlayerRemoveStatistics();
            }

            List<List<string>> chunks = Helper.Chunk(this.SaveDataExport.unlocked_items);

            foreach (List<string> chunk in chunks)
            {
                foreach (string item in chunk)
                {
                    if (Constants.Mods.All.Contains(item))
                    {
                        if (Constants.Mods.Stalls.Contains(item))
                        {
                            this.ParkitectController.PlayerAddStall(item);
                        }
                        else
                        {
                            this.ParkitectController.PlayerAddAttraction(item);
                        }
                    }
                    else if (Constants.Attraction.All.Contains(item))
                    {
                        this.ParkitectController.PlayerAddAttraction(item);
                    }
                    else if (Constants.Stall.All.Contains(item))
                    {
                        this.ParkitectController.PlayerAddStall(item);
                    }
                    else if (Constants.UtilityBuilding.All.Contains(item))
                    {
                        this.ParkitectController.PlayerAddUtilityBuilding(item);
                    }
                    else if (Constants.Decorations.All.Contains(item))
                    {
                        this.ParkitectController.PlayerAddDecorations(item);
                    }
                    else if (Constants.Research.Rules.Statistics.Contains(item) && !this.ParkitectController.HasUnlockedResearchRule(item))
                    {
                        this.ParkitectController.PlayerAddStatistics(item);
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
            this._help();
            return this.SaveDataExport.current_challenges;
        }

        protected void SetSeed(string seed)
        {
            this._help();

            if (this.GetSeed() != string.Empty)
            {
                return;
            }

            this.SaveDataExport.seed = seed;
        }

        public string GetSeed()
        {
            return this.SaveDataExport.seed;
        }

        public bool IsSameSeed(string seed)
        {
            if (string.IsNullOrEmpty(seed))
            {
                return false;
            }

            return this.GetSeed() == seed;
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
            if (this.HasUnlockedItem(name))
            {
                return;
            }

            this.SaveDataExport.unlocked_items.Add(name);
            this.Save();
        }

        public bool GetEnabledUtilityBuildings()
        {
            return this.SaveDataExport.enabled_utility_buildings;
        }

        public void SetEnabledUtilityBuildings(bool enabled)
        {
            this._help();
            this.SaveDataExport.enabled_utility_buildings = enabled;
            this.Save();
        }

        public bool GetEnabledDecorations()
        {
            return this.SaveDataExport.enabled_decorations;
        }

        public void SetEnabledDecorations(bool enabled)
        {
            this._help();
            this.SaveDataExport.enabled_decorations = enabled;
            this.Save();
        }

        public bool GetEnabledStatistics()
        {
            return this.SaveDataExport.enabled_statistics;
        }

        public void SetEnabledStatistics(bool enabled)
        {
            this._help();
            this.SaveDataExport.enabled_statistics = enabled;
            this.Save();
        }

        public List<string> GetDecorationThemes()
        {
            List<string> result = new List<string>();
            foreach (string tc in Constants.Decorations.ThemeTags)
            {
                if (this.HasUnlockedItem(tc)) {
                    result.Add(tc);
                }
            }
            return result;
        }

        public bool HasUnlockedAPLocation(long id)
        {
            this._help();
            return this.SaveDataExport.unlocked_locations.Contains(id);
        }

        public void SetUnlockedAPLocation(long id)
        {
            this._help();
            if (this.HasUnlockedAPLocation(id))
            {
                return;
            }

            this.SaveDataExport.unlocked_locations.Add(id);
            this.Save();
        }

        public void AddPendingLocation(long id)
        {
            this._help();
            if (this.HasUnlockedAPLocation(id))
            {
                return;
            }

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

            if (Constants.Player.SpeedupOptions.Last<int>() > this.SaveDataExport.max_speedup)
            {
                this.SaveDataExport.max_speedup += 1;
            }

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
            File.WriteAllText(SaveData.GetFilePath(this.GetSeed()), this.MakeJsonData());
        }

        public void Backup()
        {
            if (this.SaveDataExport == null) {
                return;
            }

            Helper.Debug("[SaveData::Backup]");
            File.WriteAllText(SaveData.GetFilePath(this.GetSeed()) + ".backup", this.MakeJsonData());
        }

        private void CreateSavegameFolder(string seed)
        {
            Directory.CreateDirectory(SaveData.GetSaveGamePath(seed));
        }

        public static SaveDataExport Load(string seed)
        {
            Helper.Debug("[SaveData::Load]");
            try
            {
                string json = File.ReadAllText(SaveData.GetFilePath(seed));
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<SaveDataExport>(json);
            } catch {
                return null;
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

        public static string GetFilePath(string seed)
        {
            return System.IO.Path.Combine(SaveData.GetSaveGamePath(seed), "park.data");
        }

        public static string GetSaveGamePath(string seed)
        {
            return System.IO.Path.Combine(Constants.SaveGamesPath, seed);
        }

        private string MakeJsonData()
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return Helper.MakeJsonData(this.SaveDataExport, jsonSettings);
        }
    }
}
