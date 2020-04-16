using GS.Tookits.Interfaces;
using Sikiro.Entity.Customer.DBContext;

namespace Sikiro.Service.Customer
{
    /// <summary>
    /// 基础服务类
    /// </summary>
    public class BaseService : IDepend
    {
        protected readonly PersonPlatformContext Db;

        public BaseService(PersonPlatformContext db)
        {
            Db = db;
        }
    }
}
