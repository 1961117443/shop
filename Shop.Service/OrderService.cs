using FreeSql;
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

        public OrderService(IFreeSql freeSql):base(freeSql)
        {
            this.freeSql = freeSql;
        } 

        internal override IFreeSql CreateFreeSql()
        {
            return this.freeSql;
        }

        protected override ISelect<SalesOrder> SelectEntity()
        {
            return this.freeSql.Select<SalesOrder>()
                   .IncludeMany(o => o.Detail.Where(a => a.MainID == o.ID),
                                   then => then.Include(a => a.SectionBar)
                                   .Include(a => a.Surface)
                                   .Include(a => a.Packing)
                                   .Include(a => a.Texture)
                               );

        }

        public async Task<SalesOrder> GetEntityAsync(Expression<Func<SalesOrder, bool>> where)
        {
            return await this.SelectEntity().WhereIf(where != null, where).FirstAsync();
        }

        public async Task<IList<SalesOrderDetail>> GetDetailAsync(Guid key)
        {
            return await this.freeSql.Select<SalesOrderDetail>().Where(w => w.MainID == key).ToListAsync();
        }

        //public async Task<bool> UpdateAsync(SalesOrderDetail entity, Expression<Func<SalesOrderDetail>> func)
        //{
        //    var updater = this.freeSqlInstance.Update<SalesOrderDetail>(entity);
        //    if (func != null)
        //    {
        //        updater = updater.Set<SalesOrderDetail>((a) => new SalesOrderDetail() { TotalQuantity=a.TotalQuantity});
        //        //updater = updater.Set(func);
        //    }
        //  //  updater.Set(a => a.ModifyDate, DateTime.Now);
        //    int res = await updater.ExecuteAffrowsAsync();
        //    return res > 0;
        //}
    }
}
