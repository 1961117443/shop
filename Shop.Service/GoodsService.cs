using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    public class GoodsService : IGoodsService
    {
        private readonly IFreeSql freeSql;

        public GoodsService(IFreeSql freeSql)
        {
            this.freeSql = freeSql;
        }

        public async Task<IList<SectionBarCategory>> GetCateoryListAsync(Guid id)
        {
            var select = this.freeSql.Select<SectionBarCategory>();
            select=select.Where(w => !w.IsStop.Value && w.ParentID.Equals(id));

            return await select.ToListAsync();
        }
    }
}
