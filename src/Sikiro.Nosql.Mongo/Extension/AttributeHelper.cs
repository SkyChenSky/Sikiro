using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Sikiro.Nosql.Mongo.Extension
{
    /// <summary>
    /// 特性扩展类
    /// </summary>
    internal static class AttributeHelper<T> where T : class
    {
        private static readonly ConcurrentDictionary<Type, T> AttributeDic =
            new ConcurrentDictionary<Type, T>();

        /// <summary>
        /// 获取实体特性信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetAttribute(Type type)
        {
            return AttributeDic.GetOrAdd(type, item =>
             {
                 var customAttribute = type.GetCustomAttribute(typeof(T));

                 return customAttribute as T;
             });

        }
    }
}
