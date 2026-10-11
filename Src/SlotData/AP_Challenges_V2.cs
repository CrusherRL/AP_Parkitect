using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArchipelagoMod.Src.SlotData
{
    public class AP_Challenges_V2
    {
        public Dictionary<string, List<AP_Challenge_V2>> challenges { get; set; }

        // Init method to convert dynamic array into AP_Challenges_V2
        public static AP_Challenges_V2 Init(object challengesData)
        {
            // Return an empty AP_Challenges_V2 if no data is provided
            if (challengesData == null)
            {
                return new AP_Challenges_V2 { challenges = new Dictionary<string, List<AP_Challenge_V2>>() };
            }

            // Convert the incoming object (could be a JSON array) to a JSON string
            string arrayJson = JsonConvert.SerializeObject(challengesData);

            // Wrap it into an object with "challenges" property
            string wrappedJson = "{ \"challenges\": " + arrayJson + " }";

            // Deserialize to AP_Challenges_V2
            return JsonConvert.DeserializeObject<AP_Challenges_V2>(wrappedJson);
        }

    }

    public class AP_Challenge_V2
    {
        [JsonProperty("location_id")]
        public int LocationId { get; set; }

        [JsonProperty("challenge_index")]
        public int ChallengeIndex { get; set; }

        public ChallengeItem item { get; set; }
    }
}
