using FreeSql;
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Service
{
    public class ProductService: BaseService<Product>, IProductService
    {
        private readonly IFreeSql freeSql;

        public ProductService(IFreeSql freeSql):base(freeSql)
        {
            this.freeSql = freeSql;
            base.Instance = freeSql;
        }

        protected override ISelect<Product> SelectEntity()
        {
            return this.Instance.Select<Product>().Include(a => a.ProductCategory);
        }
    }

    public class ProductCategoryService : BaseService<ProductCategory>, IProductCategoryService
    { 

        public ProductCategoryService(IFreeSql freeSql) : base(freeSql)
        { 
        } 
    }
}
