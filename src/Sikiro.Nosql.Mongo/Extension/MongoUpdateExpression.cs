using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Nosql.Mongo.Extension
{
    #region Mongo更新字段表达式解析

    /// <inheritdoc />
    /// <summary>
    /// Mongo更新字段表达式解析
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class MongoUpdateExpression<T> : ExpressionVisitor
    {
        #region 成员变量

        /// <summary>
        /// 更新列表
        /// </summary>
        internal List<UpdateDefinition<T>> UpdateDefinitionList = new List<UpdateDefinition<T>>();

        private string _fileName;

        #endregion

        #region 获取更新列表

        /// <summary>
        /// 获取更新列表
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static List<UpdateDefinition<T>> GetUpdateDefinition(Expression<Func<T, T>> expression)
        {
            var mongoDb = new MongoUpdateExpression<T>();

            mongoDb.Resolve(expression);
            return mongoDb.UpdateDefinitionList;
        }

        #endregion

        #region 解析表达式

        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="expression"></param>
        private void Resolve(Expression<Func<T, T>> expression)
        {
            Visit(expression);
        }

        #endregion

        #region 访问对象初始化表达式

        /// <inheritdoc />
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
                var memberAssignment = (MemberAssignment) item;
                _fileName = item.Member.Name;

                if (memberAssignment.Expression.NodeType == ExpressionType.MemberInit)
                {
                    var lambda =
                        Expression.Lambda<Func<object>>(Expression.Convert(memberAssignment.Expression, Types.Object));
                    var value = lambda.Compile().Invoke();
                    UpdateDefinitionList.Add(Builders<T>.Update.Set(_fileName, value));
                }
                else if (memberAssignment.Expression.NodeType == ExpressionType.Call)
                {
                    ArrayOperate((MethodCallExpression) memberAssignment.Expression);
                }
                else
                {
                    Visit(memberAssignment.Expression);
                }
            }

            return node;
        }

        /// <summary>
        /// 数组原子操作
        /// </summary>
        /// <param name="expression"></param>
        private void ArrayOperate(MethodCallExpression expression)
        {
            var ex = expression.Arguments[1];

            var value = GetValue(ex);
            if (value == null)
                return;

            switch (expression.Method.Name)
            {
                case "Push":
                {
                    var updateDefinition = Builders<T>.Update.Push(_fileName, value);
                    UpdateDefinitionList.Add(updateDefinition);
                }
                    break;
                case "Pull":
                {
                    var updateDefinition = Builders<T>.Update.Pull(_fileName, value);
                    UpdateDefinitionList.Add(updateDefinition);
                }
                    break;
                case "AddToSet":
                {
                    var updateDefinition = Builders<T>.Update.AddToSet(_fileName, value);
                    UpdateDefinitionList.Add(updateDefinition);
                }
                    break;
            }

        }

        private object GetValue(Expression ex)
        {
            if (ex.NodeType == ExpressionType.MemberAccess)
            {
                return ((MemberExpression) ex).MemberToValue();
            }

            if (ex.NodeType == ExpressionType.Constant)
            {
                return ((ConstantExpression) ex).Value;
            }

            throw new Exception("未知类型无法解析");
        }

        #endregion

        #region 访问二元表达式

        /// <summary>
        /// 访问二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var value = ((ConstantExpression) node.Right).Value;

            if (node.NodeType == ExpressionType.Decrement)
            {
                if (node.Type == Types.Int)
                {
                    value = -(int) value;
                }
                else if (node.Type == Types.Long)
                {
                    value = -(long) value;
                }
                else if (node.Type == Types.Double)
                {
                    value = -(double) value;
                }
                else if (node.Type == Types.Decimal)
                {
                    value = -(decimal) value;
                }
                else if (node.Type == Types.Float)
                {
                    value = -(float) value;
                }
                else
                {
                    throw new Exception(_fileName + "不支持该类型操作");
                }
            }

            var updateDefinition = Builders<T>.Update.Inc(_fileName, value);

            UpdateDefinitionList.Add(updateDefinition);

            return node;
        }

        #endregion

        #region 访问数组表达式

        /// <inheritdoc />
        /// <summary>
        /// 访问数组表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            SetList(node);

            return node;
        }

        /// <inheritdoc />
        /// <summary>
        /// 访问集合表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitListInit(ListInitExpression node)
        {
            SetList(node);

            return node;
        }

        private void SetList(Expression node)
        {
            var lambda = Expression.Lambda(node);
            var value = lambda.Compile().DynamicInvoke();
            UpdateDefinitionList.Add(Builders<T>.Update.Set(_fileName, (IList)value));

        }

        #endregion

        #region 访问常量表达式

        /// <inheritdoc />
        /// <summary>
        /// 访问常量表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            UpdateDefinitionList.Add(Builders<T>.Update.Set(_fileName, node.Value));

            return node;
        }

        #endregion

        #region 访问成员表达式

        /// <inheritdoc />
        /// <summary>
        /// 访问成员表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Type.GetInterfaces().Any(a => a == typeof(IList)))
            {
                SetList(node);
            }
            else
            {
                var lambda = Expression.Lambda<Func<object>>(Expression.Convert(node, Types.Object));
                var value = lambda.Compile().Invoke();

                if (node.Type.IsEnum)
                    value = (int) value;

                UpdateDefinitionList.Add(Builders<T>.Update.Set(_fileName, value));
            }

            return node;
        }

        #endregion
    }

    #endregion
}
