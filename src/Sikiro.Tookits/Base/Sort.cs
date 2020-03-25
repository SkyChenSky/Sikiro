using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sikiro.Tookits.Base.Enum;

namespace Sikiro.Tookits.Base
{
    /// <summary>
    /// 排序容器类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Sort<T>
    {
        private readonly List<SortItem> _list = new List<SortItem>();

        /// <summary>
        /// 倒叙
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Sort<T> Desc<TField>(Expression<Func<T, TField>> expression)
        {
            _list.Add(new SortItem(expression.Body, ESort.Desc));
            return this;
        }

        /// <summary>
        /// 顺序
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Sort<T> Asc<TField>(Expression<Func<T, TField>> expression)
        {
            _list.Add(new SortItem(expression.Body, ESort.Asc));
            return this;
        }

        /// <summary>
        /// 重载自定义类
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator List<SortItem>(Sort<T> value)
        {
            return value._list;
        }
    }

    /// <summary>
    /// 排序项
    /// </summary>
    public class SortItem
    {
        public SortItem(Expression expressionBody, ESort sortType)
        {
            ExpressionBody = expressionBody;
            SortType = sortType;
        }
        public Expression ExpressionBody { get; }

        public string FieldName => (ExpressionBody as MemberExpression)?.Member.Name;

        public ESort SortType { get; }
    }
}
