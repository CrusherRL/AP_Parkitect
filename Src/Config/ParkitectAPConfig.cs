using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEngine;

namespace ArchipelagoMod.Src.Config
{
    class ParkitectAPConfig
    {
        [JsonProperty] public string Address = null;
        [JsonProperty] public int Port = 0;
        [JsonProperty] public string Playername = null;
        [JsonProperty] public string Password = null;

        public static void CreateConfig ()
        {
            string folder = System.IO.Path.Combine(Application.persistentDataPath, Constants.ParkitectAPFolder);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = System.IO.Path.Combine(folder, Constants.ParkitectAPFilename);
            ParkitectAPConfig self = new ParkitectAPConfig();

            self.Address = "archipelago.gg";
            self.Port = 0;
            self.Playername = "";
            self.Password = "";

            string json = JsonConvert.SerializeObject(self, Formatting.Indented);
            File.WriteAllText(path, json);
            Helper.Debug("[ParkitectAPConfig::CreateConfig] -> ParkitectAPConfig Created new config");
        }

        public static ParkitectAPConfig Load ()
        {
            string jsonData = File.ReadAllText(ParkitectAPConfig.GetConfigFilePath());

            try
            {
                // Manual parse only – avoids any global converter noise.
                JObject json = JObject.Parse(jsonData);
                ParkitectAPConfig self = new ParkitectAPConfig();

                self.Address = (string)json["Address"];
                self.Port = (int)json["Port"];
                self.Playername = (string)json["Playername"];
                self.Password = (string)json["Password"];

                if (string.IsNullOrWhiteSpace(self.Address))
                {
                    Helper.Debug("[ParkitectAPConfig::ParkitectAPConfig] -> Invalid address given.");
                    return null;
                }

                Helper.Debug($"[ParkitectAPConfig::ParkitectAPConfig] -> Using server={self.Address}:{self.Port} player={self.Playername}");
                return self;
            }
            catch (Exception ex)
            {
                Helper.Debug($"[ParkitectAPConfig::ParkitectAPConfig] -> Manual parse failed: {ex.Message}");
                Helper.Debug($"[ParkitectAPConfig::ParkitectAPConfig] -> JSON: {jsonData}");
                return null;
            }
        }

        public static ParkitectAPConfig Load (bool debug = false)
        {
            string jsonData = File.ReadAllText(ParkitectAPConfig.GetConfigFilePath());
            // Manual parse only – avoids any global converter noise.
            ParkitectAPConfig self = new ParkitectAPConfig();

            self.Address = "localhost";
            self.Port = 38281;
            self.Playername = "CrusherParkitect";
            self.Password = "";

            return self;
        }

        public static string GetConfigFilePath ()
        {
            string folder = System.IO.Path.Combine(Application.persistentDataPath, Constants.ParkitectAPFolder);
            return System.IO.Path.Combine(folder, Constants.ParkitectAPFilename);
        }

        public static bool HasConfigFile ()
        {
            return true;
            //return File.Exists(ParkitectAPConfig.GetConfigFilePath());
        }
    }
}
