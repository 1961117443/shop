using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Data
{
    public class OperationAttribute: Attribute
    {
        /// <summary>
        /// 操作描述
        /// </summary>
        public string Description { get; set; }
    }
}
