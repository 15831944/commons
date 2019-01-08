using MySql.Data.MySqlClient;

using System.Collections;
using System.Collections.Generic;

namespace System.Data.ADO
{
    /// <summary>
    /// 数据访问抽象基础类MySQL
    /// </summary>
    public abstract class DbHelperMySQL
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString { get; set; } = DBInfo.ConnectionString;

        /// <summary>
        /// Oracle混合事务 Oracle连接字符串
        /// </summary>
        public static string OracleConnectionString { get; set; } = DBInfo.OracleConnectionString;

        /// <summary>
        /// 阻止实例化
        /// </summary>
        private DbHelperMySQL()
        {
            //阻止实例化
            throw new NotSupportedException();
        }

        #region 公用方法

        /// <summary>
        /// 得到最大值
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
        /// 是否存在
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
        /// 是否存在（基于MySqlParameter）
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string strSql, params MySqlParameter[] cmdParms)
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
            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var cmd = new MySqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        var rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static int ExecuteSqlByTime(string sqlString, int times)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var cmd = new MySqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = times;
                        var rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行MySql和Oracle滴混合事务
        /// </summary>
        /// <param name="list">SQL命令行列表</param>
        /// <param name="oracleCmdSqlList">Oracle命令行列表</param>
        /// <returns>执行结果 0-由于SQL造成事务失败 -1 由于Oracle造成事务失败 1-整体事务执行成功</returns>
        public static int ExecuteSqlTran(List<CommandInfo> list, List<CommandInfo> oracleCmdSqlList)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand
                {
                    Connection = conn
                };
                var tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (var myDE in list)
                    {
                        var cmdText = myDE.CommandText;
                        var cmdParms = (MySqlParameter[])myDE.Parameters;
                        PrepareCommand(cmd, conn, tx, cmdText, cmdParms);
                        if (myDE.EffentNextType == EffentNextType.SolicitationEvent)
                        {
                            if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                tx.Rollback();
                                throw new Exception("违背要求" + myDE.CommandText + "必须符合select count(..的格式");
                                //return 0;
                            }

                            var obj = cmd.ExecuteScalar();
                            var isHave = false;
                            if (obj == null && obj == DBNull.Value)
                            {
                                isHave = false;
                            }
                            isHave = Convert.ToInt32(obj) > 0;
                            if (isHave)
                            {
                                //引发事件
                                myDE.OnSolicitationEvent();
                            }
                        }
                        if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                        {
                            if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "必须符合select count(..的格式");
                                //return 0;
                            }

                            var obj = cmd.ExecuteScalar();
                            var isHave = false;
                            if (obj == null && obj == DBNull.Value)
                            {
                                isHave = false;
                            }
                            isHave = Convert.ToInt32(obj) > 0;

                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须大于0");
                                //return 0;
                            }
                            if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                            {
                                tx.Rollback();
                                throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须等于0");
                                //return 0;
                            }
                            continue;
                        }
                        var val = cmd.ExecuteNonQuery();
                        if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                        {
                            tx.Rollback();
                            throw new Exception("SQL:违背要求" + myDE.CommandText + "必须有影响行");
                            //return 0;
                        }
                        cmd.Parameters.Clear();
                    }
                    var res = OracleHelper.ExecuteSqlTran(OracleConnectionString, oracleCmdSqlList);
                    if (!res)
                    {
                        tx.Rollback();
                        throw new Exception("执行失败");
                        // return -1;
                    }
                    tx.Commit();
                    return 1;
                }
                catch (MySqlException e)
                {
                    tx.Rollback();
                    throw e;
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">多条SQL语句</param>
        public static int ExecuteSqlTran(List<string> sqlStringList)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand
                {
                    Connection = conn
                };
                var tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    var count = 0;
                    for (var n = 0; n < sqlStringList.Count; n++)
                    {
                        var strsql = sqlStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
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
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var cmd = new MySqlCommand(sqlString, connection);
                var myParameter = new MySqlParameter("@content", SqlDbType.NText)
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
                catch (MySqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static object ExecuteSqlGet(string sqlString, string content)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var cmd = new MySqlCommand(sqlString, connection);
                var myParameter = new MySqlParameter("@content", SqlDbType.NText)
                {
                    Value = content
                };
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    var obj = cmd.ExecuteScalar();
                    return Equals(obj, null) || Equals(obj, DBNull.Value) ? null : obj;
                }
                catch (MySqlException e)
                {
                    throw e;
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
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var cmd = new MySqlCommand(strSQL, connection);
                var myParameter = new MySqlParameter("@fs", SqlDbType.Image)
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
                catch (MySqlException e)
                {
                    throw e;
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
            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var cmd = new MySqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        var obj = cmd.ExecuteScalar();
                        return Equals(obj, null) || Equals(obj, DBNull.Value) ? null : obj;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static object GetSingle(string sqlString, int times)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var cmd = new MySqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = times;
                        var obj = cmd.ExecuteScalar();
                        return Equals(obj, null) || Equals(obj, DBNull.Value) ? null : obj;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(string strSQL)
        {
            var connection = new MySqlConnection(ConnectionString);
            var cmd = new MySqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                var myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (MySqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sqlString)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new MySqlDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        public static DataSet Query(string sqlString, int times)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new MySqlDataAdapter(sqlString, connection);
                    command.SelectCommand.CommandTimeout = times;
                    command.Fill(ds, "ds");
                }
                catch (MySqlException ex)
                {
                    throw new Exception(ex.Message);
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
        public static int ExecuteSql(string sqlString, params MySqlParameter[] cmdParms)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        var rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable sqlStringList)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    var cmd = new MySqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in sqlStringList)
                        {
                            var cmdText = myDE.Key.ToString();
                            var cmdParms = (MySqlParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            var val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
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
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        public static int ExecuteSqlTran(List<CommandInfo> cmdList)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    var cmd = new MySqlCommand();
                    try
                    {
                        var count = 0;
                        //循环
                        foreach (var myDE in cmdList)
                        {
                            var cmdText = myDE.CommandText;
                            var cmdParms = (MySqlParameter[])myDE.Parameters;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);

                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                                {
                                    trans.Rollback();
                                    return 0;
                                }

                                var obj = cmd.ExecuteScalar();
                                var isHave = false;
                                if (obj == null && obj == DBNull.Value)
                                {
                                    isHave = false;
                                }
                                isHave = Convert.ToInt32(obj) > 0;

                                if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                continue;
                            }
                            var val = cmd.ExecuteNonQuery();
                            count += val;
                            if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                            {
                                trans.Rollback();
                                return 0;
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return count;
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
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(List<CommandInfo> sqlStringList)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    var cmd = new MySqlCommand();
                    try
                    {
                        var indentity = 0;
                        //循环
                        foreach (var myDE in sqlStringList)
                        {
                            var cmdText = myDE.CommandText;
                            var cmdParms = (MySqlParameter[])myDE.Parameters;
                            foreach (var q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            var val = cmd.ExecuteNonQuery();
                            foreach (var q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
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
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(Hashtable sqlStringList)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    var cmd = new MySqlCommand();
                    try
                    {
                        var indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in sqlStringList)
                        {
                            var cmdText = myDE.Key.ToString();
                            var cmdParms = (MySqlParameter[])myDE.Value;
                            foreach (var q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            var val = cmd.ExecuteNonQuery();
                            foreach (var q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
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
        public static object GetSingle(string sqlString, params MySqlParameter[] cmdParms)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        var obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        return Equals(obj, null) || Equals(obj, DBNull.Value) ? null : obj;
                    }
                    catch (MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(string sqlString, params MySqlParameter[] cmdParms)
        {
            var connection = new MySqlConnection(ConnectionString);
            var cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                var myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (MySqlException e)
            {
                throw e;
            }
            //finally
            //{
            //    cmd.Dispose();
            //    connection.Close();
            //}
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sqlString, params MySqlParameter[] cmdParms)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                using (var da = new MySqlDataAdapter(cmd))
                {
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySqlException ex)
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
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
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
                foreach (var parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion 执行带参数的SQL语句
    }
}