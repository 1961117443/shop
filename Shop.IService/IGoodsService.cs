using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.EntityModel;

namespace Shop.IService
{
    public interface IGoodsService
    {
        Task<IList<SectionBarCategory>> GetCateoryListAsync(Guid id);
    }
}