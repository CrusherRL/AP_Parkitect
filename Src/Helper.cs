using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ArchipelagoMod.Src
{
    class Helper
    {
        private static readonly object fileLock = new object();
    
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
            if (!Constants.Debug) {
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
    }
}
