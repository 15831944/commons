using Chloe;
using Chloe.Infrastructure.Interception;
using Chloe.MySql;
using Chloe.Oracle;
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

      if (DbType == "sqlite")
      {
        dbContext = CreateSQLiteContext(connString);
      }
      else if (DbType == "sqlserver" || DbType == "mssql")
      {
        dbContext = CreateSqlServerContext(connString);
      }
      else if (DbType == "mysql")
      {
        dbContext = CreateMySqlContext(connString);
      }
      else if (DbType == "oracle")
      {
        dbContext = CreateOracleContext(connString);
      }
      else
      {
        dbContext = CreateSqlServerContext(connString);
      }

      return dbContext;
    }

    private static IDbContext CreateSqlServerContext(string connString)
    {
      var dbContext = new MsSqlContext(new MsSqlConnectionFactory(connString))
      {
        PagingMode = PagingMode.ROW_NUMBER,
      };
      return dbContext;
    }

    private static IDbContext CreateSqlServerContext2012(string connString)
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
  }
}