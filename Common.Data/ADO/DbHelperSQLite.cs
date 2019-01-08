using System.Collections;
using System.Data.SQLite;

namespace System.Data.ADO
{
    /// <summary>
    /// 数据访问基础类(基于SQLite)
    /// </summary>
    public abstract class DbHelperSQLite
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString { get; set; } = DBInfo.ConnectionString;

        /// <summary>
        ///
        /// </summary>
        private DbHelperSQLite()
        {
            throw new NotSupportedException();
        }

        #region 公用方法

        /// <summary>
        ///
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int GetMaxID(string fieldName, string tableName)
        {
            var strsql = "select max(" + fieldName + ")+1 from " + tableName;
            var obj = GetSingle(strsql);
            return obj == null ? 1 : int.Parse(obj.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static bool Exists(string strSql)
        {
            var obj = GetSingle(strSql);
            var cmdresult = Equals(obj, null) || Equals(obj, DBNull.Value) ? 0 : int.Parse(obj.ToString());
            return cmdresult != 0 ? true : false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string strSql, params SQLiteParameter[] cmdParms)
        {
            var obj = GetSingle(strSql, cmdParms);
            var cmdresult = Equals(obj, null) || Equals(obj, DBNull.Value) ? 0 : int.Parse(obj.ToString());
            return cmdresult != 0 ? true : false;
        }

        #endregion 公用方法

        #region 执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sqlString)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var cmd = new SQLiteCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        var rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SQLiteException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">多条SQL语句</param>
        public static void ExecuteSqlTran(ArrayList sqlStringList)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand
                {
                    Connection = conn
                };
                var tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (var n = 0; n < sqlStringList.Count; n++)
                    {
                        var strsql = sqlStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (SQLiteException e)
                {
                    tx.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sqlString, string content)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var cmd = new SQLiteCommand(sqlString, connection);
                var myParameter = new SQLiteParameter("@content", DbType.String)
                {
                    Value = content
                };
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    var rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var cmd = new SQLiteCommand(strSQL, connection);
                var myParameter = new SQLiteParameter("@fs", DbType.Binary)
                {
                    Value = fs
                };
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    var rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string sqlString)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var cmd = new SQLiteCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        var obj = cmd.ExecuteScalar();
                        return Equals(obj, null) || Equals(obj, DBNull.Value) ? null : obj;
                    }
                    catch (SQLiteException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public static SQLiteDataReader ExecuteReader(string strSQL)
        {
            var connection = new SQLiteConnection(ConnectionString);
            var cmd = new SQLiteCommand(strSQL, connection);
            try
            {
                connection.Open();
                var myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (SQLiteException e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sqlString)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SQLiteDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
                return ds;
            }
        }

        #endregion 执行简单SQL语句

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sqlString, params SQLiteParameter[] cmdParms)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var cmd = new SQLiteCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        var rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (SQLiteException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的SQLiteParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable sqlStringList)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    var cmd = new SQLiteCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in sqlStringList)
                        {
                            var cmdText = myDE.Key.ToString();
                            var cmdParms = (SQLiteParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            var val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string sqlString, params SQLiteParameter[] cmdParms)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var cmd = new SQLiteCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        var obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        return Equals(obj, null) || Equals(obj, DBNull.Value) ? null : obj;
                    }
                    catch (SQLiteException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public static SQLiteDataReader ExecuteReader(string sqlString, params SQLiteParameter[] cmdParms)
        {
            var connection = new SQLiteConnection(ConnectionString);
            var cmd = new SQLiteCommand();
            try
            {
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                var myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (SQLiteException e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sqlString, params SQLiteParameter[] cmdParms)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var cmd = new SQLiteCommand();
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                using (var da = new SQLiteDataAdapter(cmd))
                {
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (SQLiteException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (var parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }

        #endregion 执行带参数的SQL语句
    }
}