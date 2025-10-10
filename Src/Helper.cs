using ArchipelagoMod.Src.Challenges;
using ArchipelagoMod.Src.Controller;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ArchipelagoMod.Src
{
    class Helper
    {
        public static bool IsRange(List<(int Start, int End)> ranges, (int Start, int End) range)
        {
            return ranges.Any(v => v.Start == range.Start && v.End == range.End);
        }

        public static Prefabs GetPrefabsFromString(string prefab)
        {
            return (Prefabs)System.Enum.Parse(typeof(Prefabs), prefab);
        }

        public static void Debug(string content, string filename = "debug.log.txt")
        {
            if (!Constants.Debug) {
                return;
            }

            string filePath = Constants.ModPath + filename;

            File.AppendAllText(filePath, content + "\n");
        }

        public static Color ConvertFromHex(string hex)
        {
            Color c;
            ColorUtility.TryParseHtmlString(hex, out c);
            return c;
        }

        public static float SafeFloat(float? value)
        {
            return value ?? 0f;
        }

        public static string SerializeText (string[] items)
        {
            string result;

            if (items.Length == 0)
            {
                result = string.Empty;
            }
            else if (items.Length == 1)
            {
                result = items[0];
            }
            else if (items.Length == 2)
            {
                result = string.Join(" and ", items);
            }
            else
            {
                result = string.Join(", ", items, 0, items.Length - 1) + " and " + items[items.Length - 1];
            }

            return result;
        }

        public static void UpdateChallengeJson(List<Challenge> challenges)
        {
            Helper.Debug("[Helper::UpdateChallengeJson] Storing Challenges");

            List<Challenge.ChallengeExport> exportList = challenges.Select(c => c.GetExport()).ToList();
            string json = JsonConvert.SerializeObject(exportList, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            string filePath = Constants.ModPath + "challenges.json";

            File.WriteAllText(filePath, json);
        }
        public static List<Challenge> GetChallengeJson(ParkitectController controller)
        {
            string json = File.ReadAllText(Constants.ModPath + "challenges.json");

            // Deserialize into the export DTOs
            List<Challenge.ChallengeExport> exportList = JsonConvert.DeserializeObject<List<Challenge.ChallengeExport>>(json);

            return exportList
                .Select(e => Challenge.FromExport(e, controller))
                .ToList();
        }
    }
}
