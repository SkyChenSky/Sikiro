using Chloe.Infrastructure;
using Chloe.MySql;

namespace Sikiro.Entity.Customer.DBContext
{
    public class PersonPlatformContext : MySqlContext
    {
        public PersonPlatformContext(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }
    }
}
