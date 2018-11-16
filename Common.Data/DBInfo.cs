namespace System.Data
{
  public static class DBInfo
  {
    public static string ConnectionString { get; set; }
    public static string OracleConnectionString { get; set; }
    public static string DbType { get; set; }
    public static int LogLevel { get; set; }
  }
}