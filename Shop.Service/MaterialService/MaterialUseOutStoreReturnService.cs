using FreeSql;
using Microsoft.Extensions.Logging;
using Shop.Common.Data;
using Shop.EntityModel;
using Shop.IService;
using Shop.IService.MaterialServices;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service.MaterialService
{
    /// <summary>
    /// 领用退回服务类
    /// </summary>
    public class MaterialUseOutStoreReturnService : BaseBillService<MaterialUseOutStoreReturn, MaterialUseOutStoreReturnDetail>,IMaterialUseOutStoreReturnService
    {
        private readonly ILogger<MaterialUseOutStoreReturn> logger;

        public MaterialUseOutStoreReturnService(IFreeSql freeSql,ILogger<MaterialUseOutStoreReturn> logger,IUnitOfWork unitOfWork) : base(freeSql,unitOfWork)
        {
            this.logger = logger;
        }

        public override ISelect<MaterialUseOutStoreReturnDetail> GetDetailModelQuery()
        {
            return BaseFreeSql.Select<MaterialUseOutStoreReturnDetail>()
                .Include(w=>w.Product.ProductCategory)
                .Include(w=>w.MaterialWarehouse);
        }

        public override ISelect<MaterialUseOutStoreReturn> GetMasterModelQuery()
        {
            return BaseFreeSql.Select<MaterialUseOutStoreReturn>().Include(w => w.MaterialDepname);
        }

        
        public override Task<MaterialUseOutStoreReturn> GetEntityAsync(Expression<Func<MaterialUseOutStoreReturn, bool>> where)
        {
            return base.GetEntityAsync(where);
        }

        public Task<MaterialUseOutStoreReturn> GetEntityAsync(Guid uid)
        {
            return base.GetEntityAsync(w => w.ID == uid);
        }
    }
}
