using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class CustomerQueryViewModel
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class DepartmentQueryViewModel
    {
        public string ID { get; set; }
        public string depcode { get; set; }
        public string depname { get; set; }
    }
}
