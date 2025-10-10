using ArchipelagoMod.Src.Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace ArchipelagoMod.Src
{
    class Randomizer
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
            IList<Guest> guests = GameController.Instance.park.getGuests();

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

            ReadOnlyCollection<Shop> employees = GameController.Instance.park.getShops();
            IList<ProductShop> productShops = employees.OfType<ProductShop>().ToList();

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

        public static string GetRandomAttraction(ParkitectController controller)
        {
            int length = Constants.Attraction.All.Length;
            string attraction = Constants.Attraction.All[Randomizer.GetRandomInt(1, length)];

            if (attraction != null && controller.HasResearchItem(attraction))
            {
                return attraction;
            }

            return null;
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
            return Randomizer.GetRandomInt(min, max) / 100f;
        }
    }
}
