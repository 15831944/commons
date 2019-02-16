using Chloe.Infrastructure;

using System.Data.SQLite;

namespace System.Data.Chloe
{
    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        private string _connString = null;

        public SQLiteConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            var conn = new SQLiteConnection(this._connString);
            return conn;
        }
    }
}