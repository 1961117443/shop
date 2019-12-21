using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    public partial class SalesOrder: BaseEntity
    {
        public virtual IList<SalesOrderDetail> Detail { get; set; }
        public virtual Packing Packing2 { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
