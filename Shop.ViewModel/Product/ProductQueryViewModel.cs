using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class ProductViewModel:BaseViewModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductSpec { get; set; }
        public string ProductType { get; set; }
        public string Unit { get; set; }
        public string IUint { get; set; }
        public string HelpCode { get; set; }
        public string ProductCategoryID { get; set; }
        public string ProductCategoryID_Name { get; set; }
    }

    public class ProductCategoryViewModel:BaseViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
