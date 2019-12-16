
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{
    public class SectionBarService : ISectionBarService
    {
        private readonly IFreeSql freeSql;

        public SectionBarService(IFreeSql freeSql)
        {
            this.freeSql = freeSql;
        }
        public async Task<IList<SectionBar>> GetList()
        {
           return await this.freeSql.Select<SectionBar>().Page(1, 100).ToListAsync();
        }
    }
}
