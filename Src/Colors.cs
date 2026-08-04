using UnityEngine;

namespace ArchipelagoMod.Src
{
    public static class Colors
    {
        public static string Red = "#E50808";
        public static string LightRed = "#ff6363";
        public static string Orange = "#D75B27";
        public static string LightOrange = "#B46A08";
        public static string BloodOrange = "#D75031";
        public static string Green = "#00E916";
        public static string LightGreen = "#66B508";
        public static string DarkGreen = "#6A814D";
        public static string LightBlue = "#4B78C9";
        public static string Grey = "#A1A1A1";
        public static string Cyan = "#07B0BA";
        public static string Gold = "#8C9327";
        public static string Purple = "#AC62C3";
        public static string Pink = "#DD55B2";

        public static UnityEngine.Color ConvertFromHex(string hex)
        {
            UnityEngine.Color c;
            ColorUtility.TryParseHtmlString(hex, out c);
            return c;
        }
    }
}
