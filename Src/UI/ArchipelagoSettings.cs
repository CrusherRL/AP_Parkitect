using ArchipelagoMod.Src.Config;
using System;

namespace ArchipelagoMod.Src.UI
{
    class ArchipelagoSettings: ScriptableSingleton<ArchipelagoSettings>
    {
        public ParkitectAPConfig ParkitectAPConfig = new ParkitectAPConfig();
        public event Action UpdatedParkitectAPConfig;

        public void Save()
        {
            this.ParkitectAPConfig.Save();
            this.UpdatedParkitectAPConfig?.Invoke();
        }

        public void Load()
        {
            if (!ParkitectAPConfig.HasConfigFile())
            {
                ParkitectAPConfig.CreateConfig();
            }

            this.ParkitectAPConfig = ParkitectAPConfig.Load();
        }
    }
}
