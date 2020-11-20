using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Sikiro.Tookits.Extension
{
    public static class EnumExtension
    {
        private static ConcurrentDictionary<string, string> Cache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 获取DescriptionAttribute
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum input)
        {
            var name = input.ToString();

            var value = Cache.GetOrAdd(name, a =>
             {
                 var memInfo = input.GetType().GetMember(input.ToString());
                 var attribute = memInfo[0].GetCustomAttribute<DescriptionAttribute>();
                 return attribute?.Description;
             });

            return value;
        }
    }
}
