using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    public partial class Product
    {
        [FreeSql.DataAnnotations.Navigate("ProductCategoryID")]
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
