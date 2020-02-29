using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Shop.ViewModel
{
    public class MaterialUseOutStoreDetailViewModel : IBaseViewModel
    {
        public string ID { get; set; }
        public string ProductID { get; set; }
        
        [Description("货品编号")]
        public string ProductID_ProductCode { get; set; }
        [Description("货品名称")]
        public string ProductID_ProductName { get; set; }
        [Description("货品类别")]
        public string ProductID_ProductCategoryID_Name { get; set; }
        [Description("货品规格")]
        public string ProductID_ProductSpec { get; set; }
        [Description("基本单位")]
        public string ProductID_Unit { get; set; }
        [Description("批号")]
        public string BatNo { get; set; }
        public string MaterialWareHouseID { get; set; }
        [Description("库位")]
        public string MaterialWareHouseID_Name { get; set; }
        [Description("出库数量")]
        public decimal TotalQuantity { get; set; }
        [Description("单价")]
        public decimal Price { get; set; }
        [Description("金额")]
        public decimal Amount { get; set; }
    }
}
