using System;
using System.Linq.Expressions;
using Chloe;
using GS.Tookits.Base;

namespace Sikiro.Chloe.Extension
{
    /// <summary>
    /// 查询扩展
    /// </summary>
    public static class QueryExtension
    {
        public static PageList<T> PageList<T>(this IQuery<T> query, Expression<Func<T, bool>> predicate, int pageIndex, int pageSize)
        {
            var count = query.Where(predicate).Count();

            var items = query.Where(predicate).TakePage(pageIndex, pageSize).ToList();

            return new PageList<T>(pageIndex, pageSize, count, items);
        }

        public static PageList<TResult> PageList<T, TResult>(this IQuery<T> query, Expression<Func<T, bool>> predicate, int pageIndex, int pageSize, Expression<Func<T, TResult>> selector)
        {
            var count = query.Where(predicate).Count();

            var items = query.Where(predicate).Select(selector).TakePage(pageIndex, pageSize).ToList();

            return new PageList<TResult>(pageIndex, pageSize, count, items);
        }

        public static PageList<T> PageList<T>(this IQuery<T> query, int pageIndex, int pageSize)
        {
            var count = query.Count();

            var items = query.TakePage(pageIndex, pageSize).ToList();

            return new PageList<T>(pageIndex, pageSize, count, items);
        }

        public static PageList<TResult> PageList<T, TResult>(this IQuery<T> query, int pageIndex, int pageSize, Expression<Func<T, TResult>> selector)
        {
            var count = query.Count();

            var items = query.Select(selector).TakePage(pageIndex, pageSize).ToList();

            return new PageList<TResult>(pageIndex, pageSize, count, items);
        }
    }
}
