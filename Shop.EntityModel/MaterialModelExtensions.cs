using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public partial class MaterialSalesOut: BaseMasterEntity<MaterialSalesOutDetail>, IBaseMasterEntity<MaterialSalesOutDetail>
    {
        public virtual Customer Customer { get; set; }
    }

    /// <summary>
    /// 销售出库从表
    /// </summary>
    public partial class MaterialSalesOutDetail:BaseDetailEntity, IBaseDetailEntity
    {
        public virtual Product Product { get; set; }
        public virtual MaterialWarehouse MaterialWarehouse { get; set; }
    }

    /// <summary>
    /// 材料库存
    /// </summary>
    public partial class MaterialStock: BaseStockEntity<MaterialStock>
    {
        public virtual Product Product { get; set; }
        public virtual MaterialWarehouse MaterialWarehouse { get; set; }

        public Expression<Func<MaterialStock,bool>> EqualExpression { get; set; }
        public override string[] KeyFields
        {
            get
            {
                return new string[] { "ProductID", "MaterialWareHouseID" };
            }
        }

        public override bool Equals(MaterialStock entity)
        {
            return entity.ProductID == this.ProductID && entity.MaterialWareHouseID == this.MaterialWareHouseID;
        }
    }

    /// <summary>
    /// 领用出库主表
    /// </summary>
    public partial class MaterialUseOutStore :  IBaseMasterEntity<MaterialUseOutStoreDetail>
    {
        public virtual Department MaterialDepname { get; set; }
        
        public virtual IList<MaterialUseOutStoreDetail> Details { get; set; }
    }

    /// <summary>
    /// 领用出库从表
    /// </summary>
    public partial class MaterialUseOutStoreDetail : IBaseDetailEntity
    {
        public virtual Product Product { get; set; }
        public virtual MaterialWarehouse MaterialWarehouse { get; set; }
    }
}
