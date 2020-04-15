using GS.Entity.Customer.DBContext;
using GS.Tookits.Interfaces;

namespace GS.Service.Customer
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
