using System;

namespace MultiClass
{
    /// <summary>
    /// 
    /// </summary>
    public class Class1
    {
#if NET40
        int foo = 0;
#elif NET472
        readonly int bar = 0;
#elif NETSTANDARD1_6
        int foo = 0;
#elif NETCOREAPP21
        int foo = 0;
#endif
    }
}
