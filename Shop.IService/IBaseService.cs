using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService
{
    public interface IBaseService<T> where T:class
    {
        /// <summary>
        /// 根据条件获取记录数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<int> Count(Expression<Func<T, bool>> where = null);
        /// <summary>
        /// 获取全部实体数据
        /// </summary>
        /// <returns></returns>
        Task<IList<T>> GetListAsync();
        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<IList<T>> GetListAsync(Expression<Func<T,bool>> where);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<IList<T>> GetPageListAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> where=null, Expression<Func<T, object>> order=null);


        /// <summary>
        /// 获取单个实体对象
        /// 根据SelectEntity()可能包含外键对象，默认不包含
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        /// <summary>
        /// 根据主键获取单个实体对象
        /// 不加载外键对象
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<T> GetAsync<TKey>(TKey id) where TKey : struct;

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="func">局部更新表达式</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity, Expression<Func<T, T>> func = null, Expression<Func<T, bool>> where = null);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(T entity);
        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(IList<T> items);
        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(IList<T> items);
        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Expression<Func<T,bool>> where);
    }
}
