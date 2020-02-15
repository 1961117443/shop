using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class MaterialPurchaseDetailViewModel:BaseViewModel
    {
        public string ID { get; set; }
        public string ProductID { get; set; }
        public string ProductID_ProductCode { get; set; }
        public string ProductID_ProductName { get; set; }
        public string ProductID_ProductSpec { get; set; }
        public string ProductID_ProductCategory_Name { get; set; }
        public string Unit { get; set; }
        public int TotalQuantity { get; set; }
        public string MaterialWareHouseID { get; set; }
        public string MaterialWareHouseID_Name { get; set; }
        public decimal NoTaxAmount { get; set; }
        public decimal NoTaxPrice { get; set; }
        public decimal DTaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxPrice { get; set; }
        public decimal AmountTax { get; set; }
        public string BatNo { get; set; }
        public string ItRemark { get; set; }
    }
}
