using Newtonsoft.Json;

namespace ArchipelagoMod.Src.SlotData
{
    class AP_Goals
    {
        public Goal park_tickets { get; set; }
        public Goal guests { get; set; }
        public Goal money { get; set; }
        public Goal coaster_rides { get; set; }
        public Goal ride_profit { get; set; }
        public Goal shop_profit { get; set; }
        public Goal shops { get; set; }
        
        public static AP_Goals Init(object goalsObject)
        {
            string json = JsonConvert.SerializeObject(goalsObject);
            return JsonConvert.DeserializeObject<AP_Goals>(json);
        }
    }

    class Goal
    {
        public bool enabled { get; set; }

        // for simple goals
        public int value { get; set; }

        // for coaster_rides (object values)
        public GoalValues values { get; set; }
    }

    class GoalValues
    {
        public int excitement { get; set; }
        public int intensity { get; set; }
    }
}
