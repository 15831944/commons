using Chloe;
using Chloe.Infrastructure.Interception;
using Chloe.MySql;
using Chloe.Oracle;
using Chloe.PostgreSQL;
using Chloe.SQLite;
using Chloe.SqlServer;

namespace System.Data.Chloe
{
    public class DbContextFactory
    {
        public static string ConnectionString { get; private set; }
        public static string DbType { get; private set; }

        static DbContextFactory()
        {
            ConnectionString = DBInfo.ConnectionString;

            var dbType = DBInfo.DbType;
            if (string.IsNullOrEmpty(dbType) == false)
            {
                DbType = dbType.Trim().ToLower();
            }

#if DEBUG
            IDbCommandInterceptor interceptor = new DbCommandInterceptor();
            DbInterception.Add(interceptor);
#endif
        }

        public static IDbContext CreateContext()
        {
            var dbContext = CreateContext(ConnectionString);
            return dbContext;
        }

        public static IDbContext CreateContext(string connString)
        {
            IDbContext dbContext = null;
            switch (DbType)
            {
                case "sqlite":
                    dbContext = CreateSQLiteContext(connString);
                    break;

                case "sqlserver":
                case "mssql":
                    dbContext = CreateMsSqlContext(connString);
                    break;

                case "sqlserver2018":
                case "mssql2018":
                    dbContext = CreateSqlServerContext(connString);
                    break;

                case "mysql":
                    dbContext = CreateMySqlContext(connString);
                    break;

                case "oracle":
                    dbContext = CreateOracleContext(connString);
                    break;

                case "pgsql":
                case "npgsql":
                case "postgres":
                case "postgresql":
                    dbContext = CreatePostgreSQLContext(connString);
                    break;

                default:
                    dbContext = CreateMsSqlContext(connString);
                    break;
            }

            return dbContext;
        }

        private static IDbContext CreateMsSqlContext(string connString)
        {
            var dbContext = new MsSqlContext(new MsSqlConnectionFactory(connString))
            {
                PagingMode = PagingMode.ROW_NUMBER,
            };
            return dbContext;
        }

        private static IDbContext CreateSqlServerContext(string connString)
        {
            var dbContext = new MsSqlContext(new MsSqlConnectionFactory(connString))
            {
                PagingMode = PagingMode.OFFSET_FETCH,
            };
            return dbContext;
        }

        private static IDbContext CreateMySqlContext(string connString)
        {
            var dbContext = new MySqlContext(new MySqlConnectionFactory(connString));
            return dbContext;
        }

        private static IDbContext CreateOracleContext(string connString)
        {
            var dbContext = new OracleContext(new OracleConnectionFactory(connString));
            return dbContext;
        }

        private static IDbContext CreateSQLiteContext(string connString)
        {
            var dbContext = new SQLiteContext(new SQLiteConnectionFactory(connString));
            return dbContext;
        }

        private static IDbContext CreatePostgreSQLContext(string connString)
        {
            var dbContext = new PostgreSQLContext(new NpgsqlConnectionFactory(connString));
            return dbContext;
        }
    }
}