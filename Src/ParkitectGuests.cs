namespace ArchipelagoMod.Src
{
    class ParkitectGuests
    {
        public static void CreateAll()
        {
            ParkitectGuests.CreateMe();
            ParkitectGuests.CreateFirstChatter();
            ParkitectGuests.CreateFirstBigAsyncer();
        }
        public static void CreateMe()
        {
            Guest guest = GameController.Instance.park.spawnGuestUnInitialized();
            guest.Initialize();
            guest.setName("Crusher", "(Owner)", "");
            guest.MinIntensity = 0f;
            guest.MaxIntensity = 1f;
            guest.Patience = .85f;
            guest.Grumpiness = .15f;
            guest.Tidiness = .33f;
            guest.Generosity = .8f;
            guest.InterestedInScenery = .75f;
            guest.Happiness = 1f;
            guest.Money += 100f;
            guest.setIsFavorite(true);

            Thought thought = new Thought("Archipelago is great, isn't it? :)", Thought.Emotion.HAPPY, Thought.Emotion.HAPPY);
            guest.think(thought);
            guest.addToInventory(new Sunglasses(), false);
        }

        public static void CreateFirstChatter()
        {
            Guest guest = GameController.Instance.park.spawnGuestUnInitialized();
            guest.Initialize();
            guest.setName("Jeff Wheatman", "", "");
            guest.setIsFavorite(true);

            Thought thought = new Thought("i like chex quest; i like rpg", Thought.Emotion.HAPPY, Thought.Emotion.HAPPY);
            guest.think(thought);
        }

        public static void CreateFirstBigAsyncer()
        {
            Guest guest = GameController.Instance.park.spawnGuestUnInitialized();
            guest.Initialize();
            guest.setName("Chakraa", "(Pls Ping when Replying) [AP], ", "");
            guest.Patience = .80f;
            guest.InterestedInScenery = .75f;
            guest.Happiness = .80f;
            guest.Money += 50;
            guest.setIsFavorite(true);

            Thought thought = new Thought("Hello! :ShibaHeart:\r\n\r\nMy Midnight is your ...", Thought.Emotion.HAPPY, Thought.Emotion.HAPPY);
            guest.think(thought);
        }
    }
}
