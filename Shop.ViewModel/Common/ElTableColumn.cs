using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class ElTableColumn
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string field { get; set; }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 对齐方式 靠左 靠右 居中
        /// </summary>
        public string align { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int width { get; set; }
    }
}
