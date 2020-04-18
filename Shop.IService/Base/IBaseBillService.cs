using Shop.Common.Data;
using Shop.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService
{
    public interface IBaseBillService<TMaster,TDetail> where TMaster : class, IBaseMasterEntity<TDetail>, new() where TDetail: class, IBaseDetailEntity, new()
    {
        /// <summary>
        /// 根据主表id获取从表数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        Task<IList<TDetail>> GetDetailFromMainIdAsync(Guid id,bool all = true);

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
        Task<AjaxResultModelList<TMaster>> GetPageListAsync(int page, int limit, Expression<Func<TMaster, bool>> where = null, Expression<Func<TMaster, object>> order = null);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="total"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        IList<TMaster> GetPageList(int page, int limit, out int total, Expression<Func<TMaster, bool>> where = null, Expression<Func<TMaster, object>> order = null);

        /// <summary>
        /// 获取单个实体对象不包含主键
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<TMaster> GetAsync(Guid key);

        /// <summary>
        /// 获取单个实体对象包含外键
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<TMaster> GetEntityAsync(Expression<Func<TMaster, bool>> where);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="func">局部更新表达式</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TMaster entity, Expression<Func<TMaster, TMaster>> func = null, Expression<Func<TMaster, bool>> where = null);
        /// <summary>
        /// 保存入库单数据，主从表一起保存，事物处理
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [Operation(Description = "保存")]
        [Obsolete("instead of PostAsync(uid,beforePost)")]
        Task<bool> PostAsync(TMaster master,IEnumerable<TDetail> details);

        /// <summary>
        /// 删除入库单，主从表一起删除
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Operation(Description ="删除")]
        Task<bool> DeleteAsync(Guid uid);

        //Task<TMaster> GetAttchAsync(Guid uid);
        /// <summary>
        /// 保存单据
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="beforePost"></param>
        /// <returns></returns>
        Task<TMaster> PostAsync(Guid uid, Action<TMaster> beforePost);
    }
}
