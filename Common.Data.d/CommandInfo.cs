using System.Data.Common;
using System.Data.SqlClient;

namespace System.Data
{
    public class CommandInfo
    {
        public object ShareObject = null;

        public object OriginalData = null;

        private event EventHandler _solicitationEvent;

        public event EventHandler SolicitationEvent
        {
            add
            {
                _solicitationEvent += value;
            }
            remove
            {
                _solicitationEvent -= value;
            }
        }

        public void OnSolicitationEvent()
        {
            _solicitationEvent?.Invoke(this, new EventArgs());
        }

        public string CommandText;

        public DbParameter[] Parameters;

        public EffentNextType EffentNextType = EffentNextType.None;

        public CommandInfo()
        {
        }

        public CommandInfo(string sqlText, SqlParameter[] para)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
        }

        public CommandInfo(string sqlText, SqlParameter[] para, EffentNextType type)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
            this.EffentNextType = type;
        }
    }
}