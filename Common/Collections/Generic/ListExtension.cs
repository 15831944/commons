namespace System.Collections.Generic
{
    public static class ListExtension
    {
        public static string JoinToString<T>(this List<T> l, string sp)
        {
            return string.Join(sp, l);
        }

        public static string JoinToString<T>(this List<T> l)
        {
            return string.Join(",", l);
        }
    }
}