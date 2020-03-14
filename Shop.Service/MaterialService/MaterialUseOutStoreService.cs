using FreeSql;
using Microsoft.Extensions.Logging;
using Shop.EntityModel;
using Shop.IService;
using Shop.IService.MaterialServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Service.MaterialService
{
    /// <summary>
    /// 领用出库服务类
    /// </summary>
    public class MaterialUseOutStoreService : BaseBillService<MaterialUseOutStore, MaterialUseOutStoreDetail>, IMaterialUseOutStoreService // IBaseBillService<MaterialUseOutStore, MaterialUseOutStoreDetail>
    {
        private readonly ILogger<MaterialUseOutStore> logger;

        public MaterialUseOutStoreService(IFreeSql freeSql,ILogger<MaterialUseOutStore> logger,IUnitOfWork unitOfWork) : base(freeSql,unitOfWork)
        {
            this.logger = logger;
        }

        public override ISelect<MaterialUseOutStoreDetail> GetDetailModelQuery()
        {
            return BaseFreeSql.Select<MaterialUseOutStoreDetail>()
                .Include(w=>w.Product.ProductCategory)
                .Include(w=>w.MaterialWarehouse);
        }

        public override ISelect<MaterialUseOutStore> GetMasterModelQuery()
        {
            return BaseFreeSql.Select<MaterialUseOutStore>().Include(w => w.MaterialDepname);
        }
    }
}
