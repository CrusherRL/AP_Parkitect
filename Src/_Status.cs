using UnityEngine;

namespace ArchipelagoMod.Src
{
    class _Status
    {
        public static Color Red = Colors.ConvertFromHex("#E50808");
        public static Color Green = Colors.ConvertFromHex("#00E916");
        public static Color Orange = Colors.ConvertFromHex("#D75B27");

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
            else if (state == States.CONNECTED)
            {
                return _Status.Green;
            }

            return _Status.Orange;
        }
    }
}
