using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.EntityModel;

namespace Shop.IService
{
    public interface ISectionBarService:IBaseService<SectionBar>
    {
        Task<IList<SectionBar>> GetList();
    }
}