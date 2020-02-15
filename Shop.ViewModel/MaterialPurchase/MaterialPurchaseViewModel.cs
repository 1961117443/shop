using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class MaterialPurchaseViewModel
    {
        public string ID { get; set; }
        public string BillCode { get; set; }
        public string InStoreDate { get; set; }
        public string VendorID { get; set; }
        public string VendorID_Name { get; set; }
        public string Buyer { get; set; }
        public string DeliveryNum { get; set; }
        public decimal TaxRate { get; set; }
        public string Remark { get; set; }
        public string Maker { get; set; }
        public string MakeDate { get; set; }
        public string Audit { get; set; }
        public string AuditDate { get; set; }

        public int Status { get; set; }
    }

    public class MaterialPurchasePostModel : MaterialPurchaseViewModel , IMasterDetailViewModel<MaterialPurchaseDetailViewModel>
    {
        public IList<MaterialPurchaseDetailViewModel> Detail { get; set; }
    }
}
