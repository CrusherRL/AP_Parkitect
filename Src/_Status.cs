using UnityEngine;

namespace ArchipelagoMod.Src
{
    class _Status
    {
        public static Color Red = Helper.ConvertFromHex("#E50808");
        public static Color Green = Helper.ConvertFromHex("#00E916");
        public static Color Orange = Helper.ConvertFromHex("#D75B27");

        public enum States
        {
            DISCONNECTED,   // Red
            CONNECTED,      // Green
            CONNECTING      // Orange
        };

        public static Color GetColor (States state)
        {
            if (state == States.DISCONNECTED)
            {
                return _Status.Red;
            }
            if (state == States.CONNECTED)
            {
                return _Status.Green;
            }

            return _Status.Orange;
        }
    }
}
