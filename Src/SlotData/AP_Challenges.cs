using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArchipelagoMod.Src.SlotData
{
    public class AP_Challenges
    {
        public List<AP_Challenge> challenges { get; set; }

        // Init method to convert dynamic array into AP_Challenges
        public static AP_Challenges Init(object challengesData)
        {
            // Return an empty AP_Challenges if no data is provided
            if (challengesData == null)
            {
                return new AP_Challenges { challenges = new List<AP_Challenge>() };
            }

            // Convert the incoming object (could be a JSON array) to a JSON string
            string arrayJson = JsonConvert.SerializeObject(challengesData);

            // Wrap it into an object with "challenges" property
            string wrappedJson = "{ \"challenges\": " + arrayJson + " }";

            // Deserialize to AP_Challenges
            return JsonConvert.DeserializeObject<AP_Challenges>(wrappedJson);
        }

    }

    public class AP_Challenge
    {
        [JsonProperty("location_id")]
        public int LocationId { get; set; }

        public ChallengeItem item { get; set; }
    }

    public class ChallengeItem
    {
        public string name { get; set; }
        public int amount { get; set; }
        public int profit { get; set; }
        public int customers { get; set; }
        public string type { get; set; }

        // Optional fields (some challenges like coasters)
        public float? excitement { get; set; }
        public float? intensity { get; set; }
        public float? nausea { get; set; }
        public float? satisfaction { get; set; }
    }
}
