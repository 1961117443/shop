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
        Task<IList<SalesOrder>> GetPageListAsync(int pageIndex, int  pageSize, Expression<Func<SalesOrder, bool>> where);
    }
}
