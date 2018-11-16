namespace System.Reflection
{
    public static class MemberInfoExtension
    {
        public static Type GetPropertyOrFieldType(this MemberInfo propertyOrField)
        {
            if (propertyOrField.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)propertyOrField).PropertyType;
            }
            if (propertyOrField.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)propertyOrField).FieldType;
            }
            throw new NotSupportedException();
        }

        public static void SetPropertyOrFieldValue(this MemberInfo propertyOrField, object obj, object value)
        {
            if (propertyOrField.MemberType == MemberTypes.Property)
            {
                ((PropertyInfo)propertyOrField).SetValue(obj, value, null);
            }
            else if (propertyOrField.MemberType == MemberTypes.Field)
            {
                ((FieldInfo)propertyOrField).SetValue(obj, value);
            }
            throw new ArgumentException();
        }

        public static object GetPropertyOrFieldValue(this MemberInfo propertyOrField, object obj)
        {
            if (propertyOrField.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)propertyOrField).GetValue(obj, null);
            }
            if (propertyOrField.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)propertyOrField).GetValue(obj);
            }
            throw new ArgumentException();
        }
    }
}