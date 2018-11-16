namespace System
{
    public static class TypeExtension
    {
        public static bool IsNullable(this Type type)
        {
            return type.IsNullable(out Type type2);
        }

        public static bool IsNullable(this Type type, out Type unType)
        {
            unType = Nullable.GetUnderlyingType(type);
            return unType != null;
        }

        public static Type GetUnderlyingType(this Type type)
        {
            if (!type.IsNullable(out Type result))
            {
                result = type;
            }
            return result;
        }
    }
}