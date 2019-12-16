
using FreeSql;
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    public class PackingService : BaseService<Packing>, IPackingService
    {
        private readonly IFreeSql freeSql;
         
        public PackingService(IFreeSql freeSql)
        {
            this.freeSql = freeSql;
        }

        internal override IFreeSql CreateFreeSql()
        {
            return this.freeSql;
        }
    }
}
