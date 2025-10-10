using Newtonsoft.Json;
using System;

namespace ArchipelagoMod.Src.SlotData
{
    class AP_Scenario
    {
        public string name { get; set; }

        public static AP_Scenario Init(object scenarioData)
        {
            // Convert object to JSON string
            string json = JsonConvert.SerializeObject(scenarioData);

            // Try parse as int
            if (Int32.TryParse(json, out int scenarioId))
            {
                return new AP_Scenario { name = Constants.Scenario.Maps[scenarioId] };
            }

            // fallback if parsing fails
            return new AP_Scenario { name = "unknown" };
        }
    }
}
