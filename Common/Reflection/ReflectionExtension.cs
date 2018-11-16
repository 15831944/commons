namespace System.Reflection
{
    public static class ReflectionExtension
    {
        public static object GetPropertyValue(this object instance, string propertyName)
        {
            CheckNull(instance, "");
            CheckNullOrEmpty(propertyName, "");
            Type type = instance.GetType();
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException(string.Format("The type of '{0}' do not define property '{1}'", type.FullName, propertyName));
            }
            return property.GetValue(instance);
        }

        public static object GetFieldValue(this object instance, string fieldName)
        {
            CheckNull(instance, "");
            CheckNullOrEmpty(fieldName, "");
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldName);
            if (field == null)
            {
                throw new ArgumentException(string.Format("The type of '{0}' do not define field '{1}'", type.FullName, fieldName));
            }
            return field.GetValue(instance);
        }

        public static T GetPropertyValue<T>(this object instance, string propertyName)
        {
            object propertyValue = instance.GetPropertyValue(propertyName);
            return (T)((object)propertyValue);
        }

        public static T GetFieldValue<T>(this object instance, string fieldName)
        {
            object fieldValue = instance.GetFieldValue(fieldName);
            return (T)((object)fieldValue);
        }

        public static void SetPropertyValue(this object instance, string propertyName, object value)
        {
            CheckNull(instance, "");
            CheckNullOrEmpty(propertyName, "");
            Type type = instance.GetType();
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException(string.Format("The type of '{0}' do not define property '{1}'", type.FullName, propertyName));
            }
            property.SetValue(instance, value);
        }

        public static void SetFieldValue(this object instance, string fieldName, object value)
        {
            CheckNull(instance, "");
            CheckNullOrEmpty(fieldName, "");
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldName);
            if (field == null)
            {
                throw new ArgumentException(string.Format("The type of '{0}' do not define field '{1}'", type.FullName, fieldName));
            }
            field.SetValue(instance, value);
        }

        private static void CheckNull(object obj, string paramName = "")
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        private static void CheckNullOrEmpty(object obj, string paramName = "")
        {
            if (obj is string && string.IsNullOrEmpty((string)obj))
            {
                throw new ArgumentException(paramName);
            }
            if (obj == null)
            {
                throw new ArgumentException(paramName);
            }
        }
    }
}