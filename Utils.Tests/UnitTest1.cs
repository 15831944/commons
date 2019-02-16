using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

namespace Utils.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var s = "asdad";
            var ss = s.Split(new char[] { ',' });
            var list = new List<string>();
            
        }
    }
}