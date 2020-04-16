using System;
using Chloe;
using Chloe.MySql;
using DotNetCore.CAP;

namespace Sikiro.Chloe.Cap
{
    /// <summary>
    /// 分布式事务扩展
    /// </summary>
    public static class TransactionExtension
    {
        /// <summary>
        /// 使用分布式事务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="publisher"></param>
        /// <param name="action"></param>
        /// <param name="catchAction"></param>
        public static void UseTransactionEx(this IDbContext dbContext, ICapPublisher publisher, Action action, Action<Exception> catchAction = null)
        {
            action.CheckNull();
            ExecuteAction(dbContext, publisher, action);
        }

        /// <summary>
        /// 使用分布式事务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="publisher"></param>
        /// <param name="action"></param>
        /// <param name="catchAction"></param>
        public static void UseTransactionEx(this MySqlContext dbContext, ICapPublisher publisher, Action action, Action<Exception> catchAction = null)
        {
            action.CheckNull();
            ExecuteAction(dbContext, publisher, action);
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
        /// <param name="publisher"></param>
        /// <param name="action"></param>
        /// <param name="catchAction"></param>
        private static void ExecuteAction(this IDbContext dbContext, ICapPublisher publisher, Action action, Action<Exception> catchAction = null)
        {
            dbContext.Session.BeginTransaction();
            var capTransaction = dbContext.Session.CurrentConnection.BeginCapTransaction(dbContext.Session.CurrentTransaction, publisher);
            try
            {
                action();
                capTransaction.Commit();
            }
            catch (Exception ex)
            {
                if (dbContext.Session.IsInTransaction)
                    capTransaction.Rollback();

                if (catchAction == null)
                    throw;

                catchAction(ex);
            }
        }
    }
}