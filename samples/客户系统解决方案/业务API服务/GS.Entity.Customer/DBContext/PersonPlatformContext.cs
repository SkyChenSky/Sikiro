using Chloe.Infrastructure;
using Chloe.MySql;

namespace GS.Entity.Customer.DBContext
{
    public class PersonPlatformContext : MySqlContext
    {
        public PersonPlatformContext(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }
    }
}
