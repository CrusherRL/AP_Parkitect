using ArchipelagoMod.Src;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace Archipelago.Src.EnergyLink
{
    class EnergyLinkHistory
    {
        public List<EnergyLinkItem> History = new List<EnergyLinkItem>();
        private SaveData SaveData;

        public EnergyLinkHistory(SaveData saveData)
        {
            this.SaveData = saveData;
            this.Load();
        }

        public void AddItem(EnergyLinkItem EnergyLinkItem)
        {
            this.History.Add(EnergyLinkItem);
            this.Save();
        }

        public string GetFilePath()
        {
            string seed = this.SaveData.GetSeed();
            return System.IO.Path.Combine(SaveData.GetSaveGamePath(seed), "park.energylink");
        }

        public void Save()
        {
            File.WriteAllText(this.GetFilePath(), Helper.MakeJsonData(this.History, true));
        }

        public void Load()
        {
            Helper.Debug("[EnergyLinkHistory::Load]");
            try
            {
                string json = File.ReadAllText(this.GetFilePath());
                if (!string.IsNullOrEmpty(json))
                {
                   this.History = JsonConvert.DeserializeObject<List<EnergyLinkItem>>(json);
                }
            }
            catch {}
        }
    }
}
