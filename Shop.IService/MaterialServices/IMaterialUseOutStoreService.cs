using Shop.Common.Data;
using Shop.EntityModel;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService.MaterialServices
{
    /// <summary>
    /// 材料领用出库服务
    /// </summary>
    public interface IMaterialUseOutStoreService: IBaseBillService<MaterialUseOutStore, MaterialUseOutStoreDetail>
    {
    }

    public interface IMaterialUseOutStoreReturnService : IBaseBillService<MaterialUseOutStoreReturn, MaterialUseOutStoreReturnDetail>
    {
        /// <summary>
        /// 获取单个实体对象包含外键
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        [Cache]
        Task<MaterialUseOutStoreReturn> GetEntityAsync(Guid uid);
        
    }
}
