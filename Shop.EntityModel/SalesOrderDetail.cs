using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    public partial class SalesOrderDetail
    {
        public virtual SectionBar SectionBar { get; set; }
        public virtual Packing Packing { get; set; } 
        public virtual Surface Surface { get; set; }
    }
}
