using System.Collections.Generic;

namespace Sikiro.Nosql.Mongo.Extension
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// mongodb数组原子添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> Push<T>(this IEnumerable<T> list, T t)
        {
            return null;
        }

        /// <summary>
        /// mongodb数组原子删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> Pull<T>(this IEnumerable<T> list, T t)
        {
            return null;
        }

        /// <summary>
        /// mongodb数组原子添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> AddToSet<T>(this IEnumerable<T> list, T t)
        {
            return null;
        }
    }
}
