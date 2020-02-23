using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class MaterialSalesOutViewModel : BaseViewModel, IMasterViewModel
    {
        #region IMasterViewModel
        public string ID { get; set; }
        public string BillCode { get; set; }
        public string Maker { get; set; }
        public string MakeDate { get; set; }
        public string Audit { get; set; }
        public string AuditDate { get; set; }
        #endregion
        public string OutStoreDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerID_Code { get; set; }
        public string CustomerID_Name { get; set; }
        public string CustomerPO { get; set; }
        public string Salesman { get; set; }
        public string Remark { get; set; }
    }

    public class MaterialSalesOutPostModel : MaterialSalesOutViewModel, IMasterDetailViewModel<MaterialSalesOutDetailViewModel>
    {
        public IList<MaterialSalesOutDetailViewModel> Detail { get; set; }
    }
}
