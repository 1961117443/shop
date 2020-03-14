using FreeSql;
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Service
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {

        public CustomerService(IFreeSql freeSql):base(freeSql)
        { 
        }
    }

    public class DepartmentService : BaseService<Department>, IDepartmentService
    {

        public DepartmentService(IFreeSql freeSql) : base(freeSql)
        {
        }
    }
    
}
