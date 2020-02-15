using Shop.EntityModel;
using Shop.IService.MaterialServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Service.MaterialService
{
    public class MaterialWarehouseService: BaseService<MaterialWarehouse>, IMaterialWarehouseService
    {
        public MaterialWarehouseService(IFreeSql freeSql):base(freeSql)
        {
            base.Instance = freeSql;
        }
    }
}
