namespace Archipelago.Src
{
    class ParkitectGuests
    {
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
    }
}
