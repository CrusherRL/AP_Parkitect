using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ArchipelagoMod.Src.Config
{
    class ParkitectAPConfig
    {
        [JsonProperty] public string Address = "";
        [JsonProperty] public int Port = 0;
        [JsonProperty] public string Playername = "";
        [JsonProperty] public string Password = "";

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

                Helper.Debug($"[ParkitectAPConfig::ParkitectAPConfig] Using server={self.Address}:{self.Port} player={self.Playername}");
                return self;
            }
            catch (Exception ex)
            {
                Helper.Debug($"[ParkitectAPConfig::ParkitectAPConfig] Manual parse failed: {ex.Message}");
                Helper.Debug($"[ParkitectAPConfig::ParkitectAPConfig] JSON: {jsonData}");
                return null;
            }
        }

        public static ParkitectAPConfig LoadLocal ()
        {
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
            return File.Exists(ParkitectAPConfig.GetConfigFilePath());
        }

        public void Save()
        {
            File.WriteAllText(ParkitectAPConfig.GetConfigFilePath(), Helper.MakeJsonData(this));
        }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.Address) && this.Port > 0 && !String.IsNullOrEmpty(this.Playername);
        }

        public string GetInvalidMessage()
        {
            List<string> messages = new List<string>() { "Archipelago Config is invalid:" };

            if (this.IsValid())
            {
                return "";
            }

            if (String.IsNullOrEmpty(this.Address))
            {
                messages.Add("- Address is empty");
            }

            if (this.Port <= 0)
            {
                messages.Add("- Port must be higher than 0");
            }

            if (String.IsNullOrEmpty(this.Playername))
            {
                messages.Add("- Playername is empty");
            }

            return String.Join("\n", messages);
        }
    }
}
