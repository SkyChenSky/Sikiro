using Chloe.Infrastructure;
using Chloe.MySql;

namespace Sikiro.Chloe.Cap.SamplesB.Db
{
    /// <summary>
    /// 企业平台
    /// </summary>
    public class BusinessPlatformContext : MySqlContext
    {
        public BusinessPlatformContext(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }
    }
}
