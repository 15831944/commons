using Chloe.Infrastructure;
using Chloe.MySql;
using MySql.Data.MySqlClient;

namespace System.Data.Chloe
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private string _connString = null;

        public MySqlConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            var mysqlconn = new MySqlConnection(this._connString);
            var conn = new ChloeMySqlConnection(mysqlconn);
            return conn;
        }
    }
}