namespace System.Data
{
    public class DbValue
    {
        private object _value;

        public object Value => this._value;

        public DbValue()
        {
        }

        public DbValue(object value)
        {
            if (value == DBNull.Value)
            {
                this._value = null;
                return;
            }
            this._value = value;
        }

        public static implicit operator byte(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(byte))
            {
                return (byte)dbValue._value;
            }
            return Convert.ToByte(dbValue._value);
        }

        public static implicit operator byte? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            byte value = dbValue;
            return new byte?(value);
        }

        public static implicit operator short(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(short))
            {
                return (short)dbValue._value;
            }
            return Convert.ToInt16(dbValue._value);
        }

        public static implicit operator short? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            short value = dbValue;
            return new short?(value);
        }

        public static implicit operator int(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(int))
            {
                return (int)dbValue._value;
            }
            return Convert.ToInt32(dbValue._value);
        }

        public static implicit operator int? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            int value = dbValue;
            return new int?(value);
        }

        public static implicit operator long(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(long))
            {
                return (long)dbValue._value;
            }
            return Convert.ToInt64(dbValue._value);
        }

        public static implicit operator long? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            long value = dbValue;
            return new long?(value);
        }

        public static implicit operator float(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(float))
            {
                return (float)dbValue._value;
            }
            return Convert.ToSingle(dbValue._value);
        }

        public static implicit operator float? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            float value = dbValue;
            return new float?(value);
        }

        public static implicit operator double(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(double))
            {
                return (double)dbValue._value;
            }
            return Convert.ToDouble(dbValue._value);
        }

        public static implicit operator double? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            double value = dbValue;
            return new double?(value);
        }

        public static implicit operator decimal(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(decimal))
            {
                return (decimal)dbValue._value;
            }
            return Convert.ToDecimal(dbValue._value);
        }

        public static implicit operator decimal? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            decimal value = dbValue;
            return new decimal?(value);
        }

        public static implicit operator Guid(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            Type type = dbValue._value.GetType();
            if (type == typeof(Guid))
            {
                return (Guid)dbValue._value;
            }
            if (type == typeof(string))
            {
                return Guid.Parse((string)dbValue._value);
            }
            if (type == typeof(byte[]))
            {
                return new Guid((byte[])dbValue._value);
            }
            throw new InvalidCastException();
        }

        public static implicit operator Guid? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            Guid value = dbValue;
            return new Guid?(value);
        }

        public static implicit operator DateTime(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(DateTime))
            {
                return (DateTime)dbValue._value;
            }
            return Convert.ToDateTime(dbValue._value);
        }

        public static implicit operator DateTime? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            DateTime value = dbValue;
            return new DateTime?(value);
        }

        public static implicit operator bool(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(bool))
            {
                return (bool)dbValue._value;
            }
            return Convert.ToBoolean(dbValue._value);
        }

        public static implicit operator bool? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            bool value = dbValue;
            return new bool?(value);
        }

        public static implicit operator char(DbValue dbValue)
        {
            DbValue.EnsureNotNull(dbValue);
            if (dbValue._value.GetType() == typeof(char))
            {
                return (char)dbValue._value;
            }
            return Convert.ToChar(dbValue._value);
        }

        public static implicit operator char? (DbValue dbValue)
        {
            if (dbValue._value == null)
            {
                return null;
            }
            char value = dbValue;
            return new char?(value);
        }

        public static implicit operator string(DbValue dbValue)
        {
            return (string)dbValue._value;
        }

        private static void EnsureNotNull(DbValue dbValue)
        {
            if (dbValue._value == null || dbValue._value == DBNull.Value)
            {
                throw new InvalidCastException();
            }
        }
    }
}