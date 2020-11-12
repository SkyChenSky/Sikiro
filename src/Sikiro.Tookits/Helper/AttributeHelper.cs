using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Sikiro.Tookits.Helper
{
    /// <summary>
    /// 特性扩展类
    /// </summary>
    public static class AttributeHelper<TAttribute> where TAttribute : class
    {
        private static readonly ConcurrentDictionary<Type, TAttribute> AttributeDic =
            new ConcurrentDictionary<Type, TAttribute>();

        /// <summary>
        /// 获取实体特性信息
        /// </summary>
        /// <typeparam name="TAttribute">所需要的Attribute</typeparam>
        /// <param name="objectOfType">某值、对象</param>
        /// <returns></returns>
        public static TAttribute GetAttribute(Type objectOfType)
        {
            return AttributeDic.GetOrAdd(objectOfType, item =>
             {
                 var customAttribute = objectOfType.GetCustomAttribute(typeof(TAttribute));

                 return customAttribute as TAttribute;
             });

        }
    }
}
