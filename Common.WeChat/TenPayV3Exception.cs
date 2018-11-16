using System;
using System.Collections.Generic;
using System.Web;

namespace System.WeChat
{
    public class TenPayV3Exception : Exception
    {
        public TenPayV3Exception(string msg) : base(msg)
        {
        }
    }
}