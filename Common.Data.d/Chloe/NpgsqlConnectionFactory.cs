using Chloe.Infrastructure;

using Npgsql;

namespace System.Data.Chloe
{
    public class NpgsqlConnectionFactory : IDbConnectionFactory
    {
        private string _connString = null;

        public NpgsqlConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            var conn = new NpgsqlConnection(this._connString);
            return conn;
        }
    }
}