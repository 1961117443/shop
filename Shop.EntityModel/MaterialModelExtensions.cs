﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    /// <summary>
    /// 采购入库单主表
    /// </summary>
    public partial class MaterialPurchase
    {
        public virtual Vendor Vendor { get; set; }
    }
    /// <summary>
    /// 采购入库单从表
    /// </summary>
    public partial class MaterialPurchaseDetail
    {
        public virtual Product Product { get; set; }
        public virtual MaterialWarehouse MaterialWarehouse { get; set; }
    }

    /// <summary>
    /// 销售出库主表
    /// </summary>
    public partial class MaterialSalesOut
    {
        public virtual Customer Customer { get; set; }
    }

    /// <summary>
    /// 销售出库从表
    /// </summary>
    public partial class MaterialSalesOutDetail
    {
        public virtual Product Product { get; set; }
        public virtual MaterialWarehouse MaterialWarehouse { get; set; }
    }

    /// <summary>
    /// 材料库存
    /// </summary>
    public partial class MaterialStock
    {
        public virtual Product Product { get; set; }
        public virtual MaterialWarehouse MaterialWarehouse { get; set; }
    }
}
