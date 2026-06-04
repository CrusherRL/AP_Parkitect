using System;

namespace ArchipelagoMod.Src.Challenges
{
    class DecoRating
    {
        public string Label { get; set; } = "Decoration";
        public string Color { get; set; } = "#4B78C9"; // Light Blue

        public string Value { get; set; } = null;

        public DecoRating(string value) 
        {
            this.Value = value;
        }

        public string SubText()
        {
            return $"<color={ this.Color }> {this.Label} at least {this.Value}</color>";
        }

        public bool Check(float rating)
        {
            string ratingValue = TextUtility.getSceneryRatingTier(rating);
            return this.IsValueOrHigher(ratingValue);
        }

        private bool IsValueOrHigher(string ratingValue)
        {
            int ratingValueIndex = Array.IndexOf(Constants.Attraction.DecoRatings, ratingValue); // Attraction
            int valueIndex = Array.IndexOf(Constants.Attraction.DecoRatings, this.Value); // Challenge

            return ratingValueIndex <= valueIndex;
        }
    }
}
