using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class PageData<T>
    {
        /// <summary>
        /// 记录数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 页码数据
        /// </summary>
        public IEnumerable<T> data { get; set; } = new List<T>();
    }
}
