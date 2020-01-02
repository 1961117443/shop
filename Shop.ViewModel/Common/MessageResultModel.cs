using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel.Common
{
    public class MessageResultModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string message { get; set; }
    }

    public class MessageResultModel<T> : MessageResultModel
    {
        public T data { get; set; }
    }
}
