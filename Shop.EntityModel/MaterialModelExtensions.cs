using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    public partial class MaterialPurchase
    {
        public virtual Vendor Vendor { get; set; }
    }
    public partial class MaterialPurchaseDetail
    {
        public virtual Product Product { get; set; }
        public virtual MaterialWarehouse MaterialWarehouse { get; set; }
    }


  
}
