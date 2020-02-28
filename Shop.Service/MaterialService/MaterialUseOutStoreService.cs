using FreeSql;
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Service.MaterialService
{
    /// <summary>
    /// 领用出库服务类
    /// </summary>
    public class MaterialUseOutStoreService : BaseBillService<MaterialUseOutStore, MaterialUseOutStoreDetail>, IBaseBillService<MaterialUseOutStore, MaterialUseOutStoreDetail>
    {
        public MaterialUseOutStoreService(IFreeSql freeSql) : base(freeSql)
        {
        }

        public override ISelect<MaterialUseOutStoreDetail> GetDetailModelQuery()
        {
            return BaseFreeSql.Select<MaterialUseOutStoreDetail>();
        }

        public override ISelect<MaterialUseOutStore> GetMasterModelQuery()
        {
            return BaseFreeSql.Select<MaterialUseOutStore>();
        }
    }
}
