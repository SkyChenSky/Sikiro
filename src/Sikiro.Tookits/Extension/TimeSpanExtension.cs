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

        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        public static long DateTimeToTimestampOfMillisecond(this DateTime dateTime)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dateTime.Ticks - dt.Ticks) / 10000;
        }

        /// <summary>
        /// 获取1970-01-01至dateTime的微秒数
        /// </summary>
        public static long DateTimeToTimestampOfMicrosecond(this DateTime dateTime)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dateTime.Ticks - dt.Ticks);
        }

        /// <summary>
        /// 根据时间戳timestamp（单位毫秒）计算日期
        /// </summary>
        public static DateTime TimestampOfMillisecondToDateTime(this long timestamp)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var tt = dt.Ticks + timestamp * 10000;
            return new DateTime(tt);
        }
    }
}