using Shop.EntityModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.IService
{
    public interface IBaseStockService<T> where T:IBaseStockEntity<T>
    {
        //
        // 摘要:
        //     更新库存信息.
        //
        // 参数:
        //   entities:
        //     库存实体集合.
        //
        // 异常:
        //   T:OverStockExcpetion:
        //     超库存了.
        IEnumerable<T> UpdateStock(IEnumerable<T> entities);
    }
}
