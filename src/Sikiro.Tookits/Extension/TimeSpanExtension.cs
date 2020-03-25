using System;

namespace Sikiro.Tookits.Extension
{
    public static class TimeSpanExtension
    {
        /// <summary>
        /// 秒转时间搓
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static TimeSpan SecondsToTimeSpan(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }
    }
}
