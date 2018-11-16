namespace System.Data
{
    public class DataParam
    {
        private string _name;

        private object _value;

        private Type _type;

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
                if (value != null)
                {
                    this._type = value.GetType();
                }
            }
        }

        public byte? Precision
        {
            get;
            set;
        }

        public byte? Scale
        {
            get;
            set;
        }

        public int? Size
        {
            get;
            set;
        }

        public Type Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public DataParam()
        {
        }

        public DataParam(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        public DataParam(string name, object value, Type type)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
        }

        public static DataParam Create<T>(string name, T value)
        {
            DataParam dataParam = new DataParam(name, value);
            if (value == null)
            {
                dataParam.Type = typeof(T);
            }
            return dataParam;
        }

        public static DataParam Create(string name, object value)
        {
            return new DataParam(name, value);
        }

        public static DataParam Create(string name, object value, Type type)
        {
            return new DataParam(name, value, type);
        }
    }
}