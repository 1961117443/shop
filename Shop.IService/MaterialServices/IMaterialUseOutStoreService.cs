using Shop.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.IService.MaterialServices
{
    /// <summary>
    /// 材料领用出库服务
    /// </summary>
    public interface IMaterialUseOutStoreService: IBaseBillService<MaterialUseOutStore, MaterialUseOutStoreDetail>
    {
    }
}
