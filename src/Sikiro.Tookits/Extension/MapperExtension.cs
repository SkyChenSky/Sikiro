using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mapster;
using MapsterMapper;

namespace Sikiro.Tookits.Extension
{
    public static class MapperExtension
    {
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TResult MapTo<TFrom, TResult>(this TFrom obj)
        {
            return new Mapper().Map<TFrom, TResult>(obj);
        }

        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TResult MapTo<TResult>(this object obj)
        {
            return new Mapper().Map<TResult>(obj);
        }

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            var type = typeof(T);

            var properties = type.GetProperties().ToList();

            var newDt = new DataTable(type.Name);

            properties.ForEach(propertie =>
            {
                Type columnType;
                if (propertie.PropertyType.IsGenericType && propertie.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = propertie.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    columnType = propertie.PropertyType;
                }

                newDt.Columns.Add(propertie.Name, columnType);
            });

            foreach (var item in list)
            {
                var newRow = newDt.NewRow();

                properties.ForEach(propertie =>
                {
                    newRow[propertie.Name] = propertie.GetValue(item, null) ?? DBNull.Value;
                });

                newDt.Rows.Add(newRow);
            }

            return newDt;
        }
    }
}
