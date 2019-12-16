using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IService
{
    public interface IBaseService<T> where T:class
    {
        Task<IList<T>> GetListAsync();

        Task<IList<T>> GetListAsync(Expression<Func<T,bool>> where);
    }
}
