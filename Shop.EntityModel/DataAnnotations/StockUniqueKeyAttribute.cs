using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    /// <summary>
    /// 库存联合主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class StockUniqueKeyAttribute:Attribute
    {

    }
}
