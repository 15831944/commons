using Chloe.Infrastructure.Interception;

using System.Diagnostics;
using System.Text;

namespace System.Data.Chloe
{
    internal class DbCommandInterceptor : IDbCommandInterceptor
    {
        public void ReaderExecuting(IDbCommand command, DbCommandInterceptionContext<IDataReader> interceptionContext)
        {
            interceptionContext.DataBag.Add("startTime", DateTime.Now);
            Debug.WriteLine(AppendDbCommandInfo(command));
            Console.WriteLine(command.CommandText);
            DBLog.Info(command.CommandText);
        }

        public void ReaderExecuted(IDbCommand command, DbCommandInterceptionContext<IDataReader> interceptionContext)
        {
            var startTime = (DateTime)interceptionContext.DataBag["startTime"];
            Console.WriteLine(DateTime.Now.Subtract(startTime).TotalMilliseconds);
            if (interceptionContext.Exception == null)
            {
                Console.WriteLine(interceptionContext.Result.FieldCount);
            }
        }

        public void NonQueryExecuting(IDbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Debug.WriteLine(AppendDbCommandInfo(command));
            Console.WriteLine(command.CommandText);
        }

        public void NonQueryExecuted(IDbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            if (interceptionContext.Exception == null)
            {
                Console.WriteLine(interceptionContext.Result);
            }
        }

        public void ScalarExecuting(IDbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            interceptionContext.DataBag.Add("startTime", DateTime.Now);
            Debug.WriteLine(AppendDbCommandInfo(command));
            Console.WriteLine(command.CommandText);
        }

        public void ScalarExecuted(IDbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            var startTime = (DateTime)interceptionContext.DataBag["startTime"];
            Console.WriteLine(DateTime.Now.Subtract(startTime).TotalMilliseconds);
            if (interceptionContext.Exception == null)
            {
                Console.WriteLine(interceptionContext.Result);
            }
        }

        public static string AppendDbCommandInfo(IDbCommand command)
        {
            var sb = new StringBuilder();

            foreach (IDbDataParameter param in command.Parameters)
            {
                if (param == null)
                {
                    continue;
                }

                object value = null;
                if (param.Value == null || param.Value == DBNull.Value)
                {
                    value = "NULL";
                }
                else
                {
                    value = param.Value;

                    if (param.DbType == DbType.String || param.DbType == DbType.AnsiString || param.DbType == DbType.DateTime)
                    {
                        value = "'" + value + "'";
                    }
                }

                sb.AppendFormat("{3} {0} {1} = {2};", Enum.GetName(typeof(DbType), param.DbType), param.ParameterName, value, Enum.GetName(typeof(ParameterDirection), param.Direction));
                sb.AppendLine();
            }

            sb.AppendLine(command.CommandText);

            return sb.ToString();
        }
    }
}