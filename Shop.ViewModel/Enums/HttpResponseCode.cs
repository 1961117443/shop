using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace System
{
    public class HttpResponseCode
    {
        /// <summary>
        /// 访问成功
        /// </summary> 
        [Description("请求成功")]
        public static readonly int OK = 20000;
        [Description("资源不存在")]
        public static int ResourceNotFound = 200404;
    }
}
