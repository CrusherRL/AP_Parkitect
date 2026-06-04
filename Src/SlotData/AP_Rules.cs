using Newtonsoft.Json;

namespace ArchipelagoMod.Src.SlotData
{
    class AP_Rules
    {

        // option_adding = 0
        // option_removing = 1
        // option_mixed = 2
        public int guests_money_flux { get; set; } = 2;

        // option_no= 0
        // option_yes = 1
        public int progressive_speedups { get; set; } = 0;

        // easy = 0
        // medium = 1
        // hard = 2
        // extreme = 3
        public int difficulty { get; set; } = 0;

        public bool utility_buildings { get; set; } = false;

        public bool decorations { get; set; } = false;

        public bool statistics { get; set; } = false;

        public bool trap_link { get; set; } = false;

        public static AP_Rules Init(object rulesObject)
        {
            // rulesObject is an array with one element
            string json = JsonConvert.SerializeObject(rulesObject);
            return JsonConvert.DeserializeObject<AP_Rules>(json);
        }

        public string GetGuestsMoneyFluxSign ()
        {
            // mixed - so flip a coin :D
            if (this.guests_money_flux == 2)
            {
                if (Randomizer.GetRandomInt(0, 100) > 50)
                {
                    return "+";
                }

                return "-";
            }

            if (this.guests_money_flux == 0)
            {
                return "+";
            }

            return "-";
        }
    }
}
