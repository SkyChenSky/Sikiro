using System;
using Chloe;
using Chloe.MySql;

namespace Sikiro.Chloe.Extension
{
    /// <summary>
    /// 事务扩展
    /// </summary>
    public static class TransactionExtension
    {
        /// <summary>
        /// 启动事务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="action"></param>
        /// <param name="catchAction"></param>
        public static void UseTransactionEx(this IDbContext dbContext, Action action, Action<Exception> catchAction = null)
        {
            action.CheckNull();
            ExecuteAction(dbContext, action);
        }

        /// <summary>
        /// 启动事务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="action"></param>
        /// <param name="catchAction"></param>
        public static void UseTransactionEx(this MySqlContext dbContext, Action action, Action<Exception> catchAction = null)
        {
            action.CheckNull();
            ExecuteAction(dbContext, action);
        }

        /// <summary>
        /// 判断是否空
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paramName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private static void CheckNull(this object obj, string paramName = null)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="action"></param>
        /// <param name="catchAction"></param>
        private static void ExecuteAction(IDbContext dbContext, Action action, Action<Exception> catchAction = null)
        {
            dbContext.Session.BeginTransaction();
            try
            {
                action();
                dbContext.Session.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (dbContext.Session.IsInTransaction)
                    dbContext.Session.RollbackTransaction();

                if (catchAction == null)
                    throw;

                catchAction(ex);
            }
        }
    }
}