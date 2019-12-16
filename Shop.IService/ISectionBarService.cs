using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.EntityModel;

namespace Shop.IService
{
    public interface ISectionBarService
    {
        Task<IList<SectionBar>> GetList();
    }
}