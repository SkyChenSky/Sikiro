using System;
using System.Linq.Expressions;

namespace Sikiro.Nosql.Mongo.Extension
{
    internal static class LambdaExpressExtension
    {
        public static MemberExpression GetRootMember(this MemberExpression e)
        {
            if (e.Expression == null || e.Expression.NodeType == ExpressionType.Constant)
                return e;

            return e.Expression.NodeType == ExpressionType.MemberAccess
                ? ((MemberExpression)e.Expression).GetRootMember()
                : null;
        }

        public static object MemberToValue(this MemberExpression memberExpression)
        {
            var topMember = GetRootMember(memberExpression);
            if (topMember == null)
                throw new InvalidOperationException("需计算的条件表达式只支持由 MemberExpression 和 ConstantExpression 组成的表达式");

            return memberExpression.MemberToValue(topMember);
        }

        public static object MemberToValue(this MemberExpression memberExpression, MemberExpression topMember)
        {
            if (topMember.Expression == null)
            {
                var aquire = GetStaticProperty(memberExpression);
                return aquire(null, null);
            }
            else
            {
                var aquire = GetInstanceProperty(memberExpression, topMember);
                return aquire((topMember.Expression as ConstantExpression).Value, null);
            }
        }

        private static Func<object, object[], object> GetInstanceProperty(Expression e, MemberExpression topMember)
        {
            var parameter = Expression.Parameter(typeof(object), "local");
            var parameters = Expression.Parameter(typeof(object[]), "args");
            var castExpression = Expression.Convert(parameter, topMember.Member.DeclaringType);
            var localExpression = topMember.Update(castExpression);
            var replaceExpression = ExpressionModifier.Replace(e, topMember, localExpression);
            replaceExpression = Expression.Convert(replaceExpression, typeof(object));
            var compileExpression = Expression.Lambda<Func<object, object[], object>>(replaceExpression, parameter, parameters);
            return compileExpression.Compile();
        }

        private static Func<object, object[], object> GetStaticProperty(Expression e)
        {
            var parameter = Expression.Parameter(typeof(object), "local");
            var parameters = Expression.Parameter(typeof(object[]), "args");
            var convertExpression = Expression.Convert(e, typeof(object));
            var compileExpression = Expression.Lambda<Func<object, object[], object>>(convertExpression, parameter, parameters);
            return compileExpression.Compile();
        }
    }

    internal class ExpressionModifier : ExpressionVisitor
    {
        public ExpressionModifier(Expression newExpression, Expression oldExpression)
        {
            _newExpression = newExpression;
            _oldExpression = oldExpression;
        }

        private readonly Expression _newExpression;
        private readonly Expression _oldExpression;

        public Expression Replace(Expression node)
        {
            return Visit(node == _oldExpression ? _newExpression : node);
        }

        public static Expression Replace(Expression node, Expression oldExpression, Expression newExpression)
        {
            return new ExpressionModifier(newExpression, oldExpression).Replace(node);
        }
    }
}
