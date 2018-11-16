using Chloe.Infrastructure;
using System.Data.SqlClient;

namespace System.Data.Chloe
{
    internal class MsSqlConnectionFactory : IDbConnectionFactory
    {
        private string _connString = null;

        public MsSqlConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            var conn = new SqlConnection(this._connString);
            return conn;
        }
    }
}