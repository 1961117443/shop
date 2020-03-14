using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class MaterialUseOutStoreViewModel : BaseViewModel, IMasterViewModel
    {
        #region IMasterViewModel
        public string BillCode { get; set; }
        public string Maker { get; set; }
        public string MakeDate { get; set; }
        public string Audit { get; set; }
        public string AuditDate { get; set; }
        #endregion
        public string OutStoreDate { get; set; }
        public string MaterialDepnameID { get; set; }
        public string MaterialDepnameID_depname { get; set; }
        public string LeadingPerson { get; set; }
        public string MaterialUseTeam { get; set; }
        public string MaterialUseTeamHead { get; set; }
        public string Remark { get; set; }
    }

    //public class MaterialSalesOutPostModel : MaterialSalesOutViewModel, IMasterDetailViewModel<MaterialSalesOutDetailViewModel>
    //{
    //    public IList<MaterialSalesOutDetailViewModel> Detail { get; set; }
    //}
}
