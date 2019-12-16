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

        public OrderService(IFreeSql freeSql)
        {
            this.freeSql = freeSql;
        } 

        internal override IFreeSql CreateFreeSql()
        {
            return this.freeSql;
        }

        protected override ISelect<SalesOrder> SelectEntity()
        {
            return this.Select
                .IncludeMany(o => o.Detail.Where(a => a.MainID == o.ID),
                                then => then.Include(a => a.SectionBar)
                                .Include(a => a.Surface)
                                .Include(a => a.Packing)
                                .Include(a => a.Texture)
                            );
        }
    }
}
