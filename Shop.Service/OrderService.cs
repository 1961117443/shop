using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    public class OrderService : BaseService<SalesOrder>, IOrderService
    {
        private readonly IFreeSql freeSql;

        public OrderService(IFreeSql freeSql)
        {
            this.freeSql = freeSql;
        }

        public async Task<IList<SalesOrder>> GetPageListAsync(int pageIndex, int pageSize , Expression<Func<SalesOrder, bool>> where)
        {
            var q = freeSql.Select<SalesOrder>() 
                .IncludeMany(o => o.Detail.Where(a=>a.MainID==o.ID),then=> then.Include(a=>a.SectionBar).Include(a=>a.Surface).Include(a=>a.Packing) )
                .WhereIf(where != null, where)
                .Page(pageIndex, pageSize)
                .OrderBy(a => a.AutoID);
             
            return await q.ToListAsync();
        }

        internal override IFreeSql CreateFreeSql()
        {
            return this.freeSql;
        }
    }
}
