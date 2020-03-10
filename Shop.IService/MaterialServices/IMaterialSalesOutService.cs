using Shop.EntityModel;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService.MaterialServices
{
    public interface IMaterialSalesOutService
    {
        /// <summary>
        /// 获取从表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IList<MaterialSalesOutDetail>> GetDetailFromMainIdAsync(Guid id,bool all=true);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="total"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [Obsolete("instead of GetPageList")]
        Task<AjaxResultModelList<MaterialSalesOut>> GetPageListAsync(int page, int limit, Expression<Func<MaterialSalesOut, bool>> where = null, Expression<Func<MaterialSalesOut, object>> order = null);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="total"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        IList<MaterialSalesOut> GetPageList(int page, int limit, out int total, Expression<Func<MaterialSalesOut, bool>> where = null,Expression<Func<MaterialSalesOut,object>> order = null);

        /// <summary>
        /// 获取单个实体对象不包含主键
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<MaterialSalesOut> GetAsync(Guid key);

        /// <summary>
        /// 获取单个实体对象包含外键
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<MaterialSalesOut> GetEntityAsync(Expression<Func<MaterialSalesOut, bool>> where);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="func">局部更新表达式</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(MaterialSalesOut entity, Expression<Func<MaterialSalesOut, MaterialSalesOut>> func = null, Expression<Func<MaterialSalesOut, bool>> where = null);
        /// <summary>
        /// 保存入库单数据，主从表一起保存，事物处理
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        Task<bool> PostAsync(MaterialSalesOutPostModel postModel);

        /// <summary>
        /// 删除入库单，主从表一起删除
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid uid);
    }
}
