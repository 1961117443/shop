using Shop.EntityModel;
using System.Collections.Generic;

namespace Shop.IService
{
    public interface IBaseStockService<T> where T:IBaseStockEntity<T>
    {
        /// <summary>
        /// 更新库存
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        IEnumerable<T> UpdateStock(IEnumerable<T> entities);
    }
}
