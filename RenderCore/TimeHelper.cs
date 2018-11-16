using System;

namespace WPRenderer
{
    public static class TimeHelper
    {
        static long tick1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;
        static long tickStart = DateTime.UtcNow.Ticks;

        public static double GetSystemTime()
        {
            return (DateTime.UtcNow.Ticks - tick1970) / 10000000.0;
        }

        public static float GetTimeSinceStartup()
        {
            return (float)((DateTime.UtcNow.Ticks - tickStart) / 10000000.0);
        }
    }
}
