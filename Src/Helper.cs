using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace ArchipelagoMod.Src
{
    class Helper
    {
        private static readonly object fileLock = new object();
        private static readonly object fileLock2 = new object();

        public static bool IsRange(List<(int Start, int End)> ranges, (int Start, int End) range)
        {
            return ranges.Any(v => v.Start == range.Start && v.End == range.End);
        }

        public static Prefabs GetPrefabsFromString(string prefab)
        {
            return (Prefabs)System.Enum.Parse(typeof(Prefabs), prefab);
        }

        public static void Debug(string content, string filename = "debug.log.txt", bool append = true)
        {
            if (!Helper.LogsEnabled())
            {
                return;
            }

            string filePath = Constants.ModPath + filename;

            if (append)
            {
                lock (fileLock)
                {
                    File.AppendAllText(filePath, content + "\n");
                }
                return;
            }

            lock (fileLock)
            {
                File.WriteAllText(filePath, content + "\n");
            }
        }

        public static void LogSlotData(string content, string path)
        {
            if (!Helper.LogsEnabled())
            {
                return;
            }

            lock (fileLock2)
            {
                File.WriteAllText(path, content + "\n");
            }
        }

        public static float SafeFloat(float? value)
        {
            return value ?? 0f;
        }

        public static int SafeInt(int? value)
        {
            return value ?? 0;
        }

        public static string SerializeText(string[] items)
        {
            if (items.Length == 0)
            {
                return string.Empty;
            }

            if (items.Length == 1)
            {
                return items[0];
            }

            if (items.Length == 2)
            {
                return string.Join(" and ", items);
            }

            return string.Join(", ", items, 0, items.Length - 1) + " and " + items[items.Length - 1];
        }

        public static string MakeJsonData(object data, bool ignoreNullAndReference = false, Formatting format = Formatting.Indented)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
            };

            if (ignoreNullAndReference)
            {
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }

            return JsonConvert.SerializeObject(data, format, settings);
        }

        public static bool LogsEnabled()
        {
            return Constants.Debug && Constants.ModPath != null && Constants.ModPath.Contains("Archipelago");
        }

        public static List<List<T>> Chunk<T>(List<T> source, int size = 50)
        {
            var result = new List<List<T>>();

            for (int i = 0; i < source.Count; i += size)
            {
                result.Add(source.GetRange(i, Math.Min(size, source.Count - i)));
            }

            return result;
        }

        public static void PrintObject(object obj, int indent = 0)
        {
            Helper.Debug($"[Helper::PrintObject]");
            string spaces = new string(' ', indent);

            if (obj == null)
            {
                Helper.Debug(spaces + "null");
                return;
            }

            // Dictionary<string, object>
            var dict = obj as Dictionary<string, object>;
            if (dict != null)
            {
                foreach (var kvp in dict)
                {
                    Helper.Debug($"{spaces}{kvp.Key}:");
                    Helper.PrintObject(kvp.Value, indent + 2);
                }
                return;
            }

            // Any other IDictionary
            var idict = obj as IDictionary;
            if (idict != null)
            {
                foreach (DictionaryEntry entry in idict)
                {
                    Helper.Debug($"{spaces}{entry.Key}:");
                    Helper.PrintObject(entry.Value, indent + 2);
                }
                return;
            }

            // Lists/arrays (but not strings)
            if (obj is IEnumerable && !(obj is string))
            {
                foreach (var item in (IEnumerable)obj)
                {
                    Helper.PrintObject(item, indent + 2);
                }
                return;
            }

            // Primitive value
            Helper.Debug(spaces + obj);
        }

        public static BigInteger GetTaxedMoneyForEnergyLink(float amount, float fee, bool onTop = false)
        {
            return (BigInteger)(GetTaxedMoney(amount, fee, onTop) * Constants.EnergyLink.Divider);
        }

        public static BigInteger GetMoneyForEnergyLink(float amount)
        {
            return (BigInteger)(amount * Constants.EnergyLink.Divider);
        }

        public static float GetTaxedMoney(float amount, float fee, bool onTop = false)
        {
            return amount * (onTop ? (1f - fee / 100f) : (1f + fee / 100f));
        }

        public static int GetTax(float amount, float fee)
        {
            return (int)(amount - (amount * (1f - fee / 100f)));
        }
    }
}
