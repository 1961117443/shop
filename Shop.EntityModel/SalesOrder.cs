using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    public partial class SalesOrder
    {
        public virtual IList<SalesOrderDetail> Detail { get; set; }
    }
}
