﻿using System;
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
        Task<IList<T>> GetPageListAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> where, Expression<Func<T, object>> order=null);
    }
}
