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

    public partial class SalesOrderDetail : BaseEntity
    {
        public virtual SectionBar SectionBar { get; set; }
        public virtual Packing Packing { get; set; }
        public virtual Surface Surface { get; set; }
        public virtual Texture Texture { get; set; }
    }
}
