using System;

namespace Sikiro.Tookits.Helper
{
    public static class RandomHelper
    {
        /// <summary>
        /// 随机数
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static decimal RandomNext(int maxValue)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var result = rand.Next(maxValue);
            return result;
        }
    }
}
