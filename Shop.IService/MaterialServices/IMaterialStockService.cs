using Shop.EntityModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace Shop.IService.MaterialServices
{
    public interface IMaterialStockService
    {
        void UseTransaction(DbTransaction transaction); 
        bool InsertOrUpdate(IList<MaterialStock> stocks);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="total"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        IList<MaterialStock> GetPageList(int page, int limit, out int total, Expression<Func<MaterialStock, bool>> where = null, Expression<Func<MaterialStock, object>> order = null);

        /// <summary>
        /// 更新库存
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IList<MaterialStock> UpdateStock(IList<MaterialStock> entities);
    }
}
