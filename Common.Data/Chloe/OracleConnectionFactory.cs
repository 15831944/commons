using Chloe.Infrastructure;
using Chloe.Oracle;

using Oracle.ManagedDataAccess.Client;

namespace System.Data.Chloe
{
    public class OracleConnectionFactory : IDbConnectionFactory
    {
        private string _connString = null;

        public OracleConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            var oracleConnection = new OracleConnection(this._connString);
            var connd = new OracleConnectionDecorator(oracleConnection);
            var conn = new ChloeOracleConnection(connd);
            return conn;
        }
    }

    internal class OracleConnectionDecorator : IDbConnection, IDisposable
    {
        private OracleConnection _oracleConnection;

        public OracleConnectionDecorator(OracleConnection oracleConnection)
        {
            this._oracleConnection = oracleConnection ?? throw new Exception("Please call 911.");
        }

        public string ConnectionString
        {
            get { return this._oracleConnection.ConnectionString; }
            set { this._oracleConnection.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return this._oracleConnection.ConnectionTimeout; }
        }

        public string Database
        {
            get { return this._oracleConnection.Database; }
        }

        public ConnectionState State
        {
            get { return this._oracleConnection.State; }
        }

        public IDbTransaction BeginTransaction()
        {
            return this._oracleConnection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return this._oracleConnection.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            this._oracleConnection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            this._oracleConnection.Close();
        }

        public IDbCommand CreateCommand()
        {
            var cmd = this._oracleConnection.CreateCommand();
            cmd.BindByName = true;
            return cmd;
        }

        public void Open()
        {
            this._oracleConnection.Open();
        }

        public void Dispose()
        {
            this._oracleConnection.Dispose();
        }
    }
}