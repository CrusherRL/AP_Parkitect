namespace ArchipelagoMod.Src
{
    public class Scenario
    {
        public string Name = "";
        public string ScenarioGUID = "";
        public string CampaignGUID = null;

        public Scenario(string name, string scenarioGUID, string campaignGUID = null)
        {
            this.Name = name;
            this.ScenarioGUID = scenarioGUID;
            this.CampaignGUID = campaignGUID;
        }
    }
}
