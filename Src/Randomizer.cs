using ArchipelagoMod.Src.Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace ArchipelagoMod.Src
{
    class Randomizer : MonoBehaviour
    {
        public static Random rnd = new Random();

        // Gets a randomly picked break reason for a ride, except:
        // NONE,
        // CRASHED = 2,
		// STRUCK_BY_LIGHTNING = 4,
		public static Attraction.BreakReason GetRandomBreakReason()
		{
            int index = Randomizer.rnd.Next(Constants.Attraction.BreakReason.Length);
            string value = Constants.Attraction.BreakReason[index];

            return (Attraction.BreakReason)Enum.Parse(typeof(Attraction.BreakReason), value);
        }

        // Gets Randomly guests by percentage or specific amount
        public static List<Guest> GetRandomGuests(float percentage = 20f, int amount = 0)
        {
            percentage = percentage <= 1f ? percentage : percentage / 100f;
            List<Guest> guests = GameController.Instance.park.getGuests().ToList();

            if (percentage == 1f)
            {
                return guests.ToList();
            }

            if (guests.Count < 20)
            {
                percentage = .5f;
            }

            int guestCount = amount > 0 ? amount : Mathf.CeilToInt(guests.Count * percentage);

            return guests
                .OrderBy(g => Randomizer.rnd.Next())
                .Take(guestCount)
                .ToList();
        }

        // Gets a randomly picked ProductShop
        public static List<ProductShop> GetRandomProductShopsFromPark(float percentage = 10f, int amount = 0)
        {
            percentage = percentage <= 1f ? percentage : percentage / 100f;

            ReadOnlyCollection<Shop> shops = GameController.Instance.park.getShops();
            List<ProductShop> productShops = shops.OfType<ProductShop>().ToList();

            if (percentage == 1f)
            {
                return productShops.ToList();
            }

            if (productShops.Count <= 0)
            {
                return new List<ProductShop>();
            }

            int productShopCount = amount > 0 ? amount : Mathf.CeilToInt(productShops.Count * percentage);

            return productShops
                .Where(s => s.opened)
                .OrderBy(s => Randomizer.rnd.Next())
                .Take(productShopCount)
                .ToList();
        }
        public static Shop GetRandomProductShopFromPark(Prefabs shopType)
        {
            List<Shop> productShops = GameController.Instance.park.getShops().ToList();

            return productShops
                .Where(s => s.getPrefabType() == shopType)
                .OrderBy(s => Randomizer.rnd.Next())
                .FirstOrDefault();
        }

        public static List<Shop> GetRandomShopsFromParkForResearch(ParkitectController controller)
        {
            int min = 1;
            int max = Constants.Research.MaxShops;

            List<Shop> shops = controller.GetAllAvailableShops();

            if (shops.Count <= 0)
            {
                return new List<Shop>();
            }

            return shops
                .OrderBy(s => Randomizer.rnd.Next())
                .Take(Randomizer.GetRandomInt(min, max))
                .ToList();
        }

        // Returns random Attractions if found
        public static List<Attraction> GetRandomAttractionFromPark(float percentage = 10f, int amount = 0)
        {
            percentage = percentage <= 1f ? percentage : percentage / 100f;
            ReadOnlyCollection<Attraction> attractions = GameController.Instance.park.getAttractions();

            if (percentage == 1f)
            {
                return attractions.ToList();
            }

            if (attractions.Count <= 0)
            {
                return new List<Attraction>();
            }

            int attractionsCount = amount > 0 ? amount : Mathf.CeilToInt(attractions.Count * percentage);

            return attractions
                .Where(a => !a.isBroken())
                .OrderBy(a => Randomizer.rnd.Next())
                .Take(attractionsCount)
                .ToList();
        }

        public static List<Attraction> GetRandomAttractionFromParkForResearch(ParkitectController controller)
        {
            int min = 1;
            int max = Constants.Research.MaxAttractions;

            List<Attraction> attractions = controller.GetAllAvailableAttractions();

            if (attractions.Count <= 0)
            {
                return new List<Attraction>();
            }

            return attractions
                .OrderBy(a => Randomizer.rnd.Next())
                .Take(Randomizer.GetRandomInt(min, max))
                .ToList();
        }
        public static List<string> GetRandomDecorationPropsFromParkForResearch(string themeTag)
        {
            if (themeTag == null)
            {
                return new List<string>();
            }

            return Constants.Decorations.ThemeTagMapToProps[themeTag];
        }

        public static string GetRandomDecorationThemeTagFromParkForResearch(ParkitectController controller)
        {
            int max = 1;

            return controller.SaveData.GetDecorationThemes()
                .OrderBy(d => Randomizer.rnd.Next())
                .Take(max)
                .FirstOrDefault();
        }

        public static Prefabs GetRandomEmployee()
        {
            int number = Randomizer.GetRandomInt(0, Constants.Employee.Options.Length - 1);
            return Constants.Employee.Options[number];
        }

        public static float GetRandomOption(float[] options)
        {
            int index = Randomizer.GetRandomInt(options);
            return options[index];
        }

        public static int GetRandomOption(int[] options)
        {
            int index = Randomizer.GetRandomInt(options);
            return options[index];
        }
        public static string GetRandomOption(string[] options)
        {
            int index = Randomizer.GetRandomInt(0, options.Length - 1);
            return options[index];
        }

        // Random Integer between min and max
        public static int GetRandomInt(int[] options)
        {
            return Randomizer.rnd.Next(0, options.Length);
        }
        public static int GetRandomInt(float[] options)
        {
            return Randomizer.rnd.Next(0, options.Length);
        }
        public static int GetRandomInt(int min = 1, int max = 10)
        {
            return Randomizer.rnd.Next(min, max + 1);
        }
        public static int GetRandomInt((int Start, int End) range)
        {
            return Randomizer.rnd.Next(range.Start, range.End + 1);
        }

        // Random Float between min and max
        public static float GetRandomFloat(int min = 0, int max = 100)
        {
            return Randomizer.GetRandomInt(min, max) / 1f;
        }
        public static float GetRandomFloat((int Start, int End) range)
        {
            return Randomizer.GetRandomInt(range) / 1f; // checks if ok
        }

        public static string GetRandomResearchType()
        {
            return Randomizer.GetRandomOption(Constants.Research.Types);
        }
    }
}
