using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using Sikiro.Tookits.Base;

namespace Sikiro.Nosql.Mongo.Extension
{
    internal class MongoSortExpression<T> : ExpressionVisitor
    {
        #region 成员变量
        /// <summary>
        /// 更新列表
        /// </summary>
        internal List<SortDefinition<T>> SortDefinitionList = new List<SortDefinition<T>>();

        #endregion

        #region 获取排序列表
        /// <summary>
        /// 获取更新列表
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static List<SortDefinition<T>> GetSortDefinition(Expression<Func<Sort<T>, Sort<T>>> expression)
        {
            var mongoDb = new MongoSortExpression<T>();

            mongoDb.Resolve(expression);
            return mongoDb.SortDefinitionList;
        }
        #endregion

        #region 解析表达式
        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="expression"></param>
        private void Resolve(Expression<Func<Sort<T>, Sort<T>>> expression)
        {
            Visit(expression);
        }
        #endregion

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            SortDefinitionList.Add(new SortDefinitionBuilder<T>().Ascending(a => a));
            return node;
        }

        #region 访问对象初始化表达式

        /// <summary>
        /// 访问对象初始化表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var bingdings = node.Bindings;

            foreach (var item in bingdings)
            {
                var memberAssignment = (MemberAssignment)item;

                if (memberAssignment.Expression.NodeType == ExpressionType.MemberInit)
                {
                    SortDefinitionList.Add(new SortDefinitionBuilder<T>().Ascending(a => a));
                }
                else
                {
                    Visit(memberAssignment.Expression);
                }
            }
            return node;
        }

        #endregion
    }
}
