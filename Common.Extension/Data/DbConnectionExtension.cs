namespace System.Data
{
    public static class DbConnectionExtension
    {
        public static object ExecuteScalar(this IDbConnection conn, string cmdText, params DataParam[] dbParams)
        {
            return conn.ExecuteScalar(cmdText, CommandType.Text, dbParams);
        }

        public static object ExecuteScalar(this IDbConnection conn, string cmdText, CommandType cmdType, params DataParam[] dbParams)
        {
            return conn.ExecuteScalar(cmdText, cmdType, null, dbParams);
        }

        public static object ExecuteScalar(this IDbConnection conn, string cmdText, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteScalar(cmdText, CommandType.Text, tran, dbParams);
        }

        public static object ExecuteScalar(this IDbConnection conn, string cmdText, CommandType cmdType, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteScalar(cmdText, cmdType, null, tran, dbParams);
        }

        public static object ExecuteScalar(this IDbConnection conn, string cmdText, CommandType cmdType, int? cmdTimeout, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.Execute(cmdText, cmdType, cmdTimeout, tran, dbParams, (IDbCommand cmd) => cmd.ExecuteScalar());
        }

        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, params DataParam[] dbParams)
        {
            return conn.ExecuteNonQuery(cmdText, CommandType.Text, dbParams);
        }

        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, CommandType cmdType, params DataParam[] dbParams)
        {
            return conn.ExecuteNonQuery(cmdText, cmdType, null, dbParams);
        }

        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteNonQuery(cmdText, CommandType.Text, tran, dbParams);
        }

        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, CommandType cmdType, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteNonQuery(cmdText, cmdType, null, tran, dbParams);
        }

        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, CommandType cmdType, int? cmdTimeout, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.Execute(cmdText, cmdType, cmdTimeout, tran, dbParams, (IDbCommand cmd) => cmd.ExecuteNonQuery());
        }

        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, params DataParam[] dbParams)
        {
            return conn.ExecuteReader(cmdText, CommandType.Text, dbParams);
        }

        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, CommandType cmdType, params DataParam[] dbParams)
        {
            return conn.ExecuteReader(cmdText, cmdType, null, dbParams);
        }

        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteReader(cmdText, CommandType.Text, tran, dbParams);
        }

        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, CommandType cmdType, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteReader(cmdText, cmdType, null, tran, dbParams);
        }

        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, CommandType cmdType, int? cmdTimeout, IDbTransaction tran, params DataParam[] dbParams)
        {
            if (conn.State != ConnectionState.Open)
            {
                throw new Exception("调用 ExecuteReader 请先确保 conn 保持 Open 状态");
            }
            return conn.Execute(cmdText, cmdType, cmdTimeout, tran, dbParams, (IDbCommand cmd) => cmd.ExecuteReader());
        }

        private static T Execute<T>(this IDbConnection conn, string cmdText, CommandType cmdType, int? cmdTimeout, IDbTransaction tran, DataParam[] dbParams, Func<IDbCommand, T> action)
        {
            bool flag = false;
            T result;
            try
            {
                using (IDbCommand dbCommand = conn.CreateCommand())
                {
                    DbConnectionExtension.SetupCommand(dbCommand, cmdText, cmdType, cmdTimeout, dbParams, tran);
                    if (conn.State != ConnectionState.Open)
                    {
                        flag = true;
                        conn.Open();
                    }
                    result = action(dbCommand);
                }
            }
            finally
            {
                if (flag && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result;
        }

        private static void SetupCommand(IDbCommand cmd, string cmdText, CommandType cmdType, int? cmdTimeout, DataParam[] dbParams, IDbTransaction tran)
        {
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdTimeout.HasValue)
            {
                cmd.CommandTimeout = cmdTimeout.Value;
            }
            if (tran != null)
            {
                cmd.Transaction = tran;
            }
            if (dbParams != null)
            {
                for (int i = 0; i < dbParams.Length; i++)
                {
                    DataParam dataParam = dbParams[i];
                    IDbDataParameter dbDataParameter = cmd.CreateParameter();
                    dbDataParameter.Value = dataParam.Value;
                    cmd.Parameters.Add(dbDataParameter);
                }
            }
        }
    }
}