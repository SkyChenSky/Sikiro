using System.Data;
using Chloe.Infrastructure;
using Chloe.MySql;
using MySql.Data.MySqlClient;

namespace Sikiro.Chloe.Extension
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connString;
        public MySqlConnectionFactory(string connString)
        {
            _connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            IDbConnection conn = new MySqlConnection(_connString);
            conn = new ChloeMySqlConnection(conn);
            return conn;
        }
    }
}
