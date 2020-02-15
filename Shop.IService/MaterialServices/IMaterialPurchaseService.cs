using Shop.EntityModel;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService.MaterialServices
{
    public interface IMaterialPurchaseService : IBaseService<MaterialPurchase>
    {
        /// <summary>
        /// 保存入库单数据，主从表一起保存，事物处理
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        Task<bool> PostAsync(MaterialPurchasePostModel postModel);
        /// <summary>
        /// 删除入库单，主从表一起删除
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Guid uid);
    }
    public interface IMaterialPurchaseDetailService:IBaseService<MaterialPurchaseDetail>
    {
        /// <summary>
        /// 根据关联主表的主键获取从表明细
        /// </summary>
        /// <param name="mId"></param>
        /// <returns></returns>
        Task<IList<MaterialPurchaseDetail>> GetDetailFromMainIdAsync(Guid mId);
    }
}
