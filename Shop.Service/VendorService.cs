using FreeSql;
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Service
{
    public class VendorService: BaseService<Vendor>, IVendorService
    {

        public VendorService(IFreeSql freeSql):base(freeSql)
        { 
        }
    }
}
