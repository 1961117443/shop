using FreeSql;
using Shop.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService
{
    public interface IOrderService : IBaseService<SalesOrder>
    {
        /// <summary>
        /// 配置查询语句
        /// </summary>
        /// <returns></returns>
        IOrderService Configure(IList<Expression<Func<SalesOrder, bool>>> exps = null);

        IOrderService LeftJoin(Expression<Func<SalesOrder, bool>> exp);

        IOrderService IncludeMany<TNavigate>(Expression<Func<SalesOrder, IEnumerable<TNavigate>>> navigateSelector, Action<ISelect<TNavigate>> then = null) where TNavigate : class;

        /// <summary>
        /// 获取实体对象，包含所有需要的外键，从表信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<SalesOrder> GetEntityAsync(Expression<Func<SalesOrder, bool>> where);

        Task<IList<SalesOrderDetail>> GetDetailAsync(Guid key); 

    }
}
