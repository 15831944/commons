using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Data.SqlClient
{
    public static class SqlConnectionExtension
    {
        private class SysColumn
        {
            public string Name
            {
                get;
                set;
            }

            public int ColOrder
            {
                get;
                set;
            }
        }

        public static void BulkCopy<TModel>(this SqlConnection conn, List<TModel> modelList, int batchSize, string destinationTableName = null, int? bulkCopyTimeout = null, SqlTransaction externalTransaction = null)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(destinationTableName))
            {
                destinationTableName = typeof(TModel).Name;
            }
            DataTable table = SqlConnectionExtension.ToSqlBulkCopyDataTable<TModel>(modelList, conn, destinationTableName);
            try
            {
                SqlBulkCopy sqlBulkCopy;
                if (externalTransaction != null)
                {
                    sqlBulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, externalTransaction);
                }
                else
                {
                    sqlBulkCopy = new SqlBulkCopy(conn);
                }
                using (sqlBulkCopy)
                {
                    sqlBulkCopy.BatchSize = batchSize;
                    sqlBulkCopy.DestinationTableName = destinationTableName;
                    if (bulkCopyTimeout.HasValue)
                    {
                        sqlBulkCopy.BulkCopyTimeout = bulkCopyTimeout.Value;
                    }
                    if (conn.State != ConnectionState.Open)
                    {
                        flag = true;
                        conn.Open();
                    }
                    sqlBulkCopy.WriteToServer(table);
                }
            }
            finally
            {
                if (flag && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public static DataTable ToSqlBulkCopyDataTable<TModel>(List<TModel> modelList, SqlConnection conn, string tableName)
        {
            DataTable dataTable = new DataTable();
            Type typeFromHandle = typeof(TModel);
            List<SqlConnectionExtension.SysColumn> tableColumns = SqlConnectionExtension.GetTableColumns(conn, tableName);
            List<PropertyInfo> list = new List<PropertyInfo>();
            PropertyInfo[] properties = typeFromHandle.GetProperties();
            for (int i = 0; i < tableColumns.Count; i++)
            {
                SqlConnectionExtension.SysColumn column = tableColumns[i];
                PropertyInfo propertyInfo = (from a in properties
                                             where a.Name == column.Name
                                             select a).FirstOrDefault<PropertyInfo>();
                if (propertyInfo == null)
                {
                    throw new Exception(string.Format("model 类型 '{0}'未定义与表 '{1}' 列名为 '{2}' 映射的属性", typeFromHandle.FullName, tableName, column.Name));
                }
                list.Add(propertyInfo);
                Type type = SqlConnectionExtension.GetUnderlyingType(propertyInfo.PropertyType);
                if (type.IsEnum)
                {
                    type = typeof(int);
                }
                dataTable.Columns.Add(new DataColumn(column.Name, type));
            }
            foreach (TModel current in modelList)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int j = 0; j < list.Count; j++)
                {
                    PropertyInfo propertyInfo2 = list[j];
                    object obj = propertyInfo2.GetValue(current);
                    if (SqlConnectionExtension.GetUnderlyingType(propertyInfo2.PropertyType).IsEnum && obj != null)
                    {
                        obj = (int)obj;
                    }
                    dataRow[j] = (obj ?? DBNull.Value);
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        private static List<SqlConnectionExtension.SysColumn> GetTableColumns(SqlConnection sourceConn, string tableName)
        {
            string cmdText = string.Format("select * from syscolumns inner join sysobjects on syscolumns.id=sysobjects.id where sysobjects.xtype='U' and sysobjects.name='{0}' order by syscolumns.colid asc", tableName);
            List<SqlConnectionExtension.SysColumn> list = new List<SqlConnectionExtension.SysColumn>();
            using (SqlConnection sqlConnection = (SqlConnection)((ICloneable)sourceConn).Clone())
            {
                sqlConnection.Open();
                using (IDataReader dataReader = sqlConnection.ExecuteReader(cmdText, new DataParam[0]))
                {
                    while (dataReader.Read())
                    {
                        list.Add(new SqlConnectionExtension.SysColumn
                        {
                            Name = dataReader.GetDbValue("name"),
                            ColOrder = dataReader.GetDbValue("colorder")
                        });
                    }
                }
            }
            return list;
        }

        private static Type GetUnderlyingType(Type type)
        {
            Type type2 = Nullable.GetUnderlyingType(type);
            if (type2 == null)
            {
                type2 = type;
            }
            return type2;
        }
    }
}