using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    public interface IBaseStockEntity<T> :IEquatable<T> where T:IBaseStockEntity<T>
    {
        //string[] KeyFields { get; set; }
    }

    public abstract class BaseStockEntity<T> : IBaseStockEntity<T> where T : IBaseStockEntity<T>
    {
        public abstract string[] KeyFields { get;  }

        /// <summary>
        /// 关联条件 匹配条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract bool Equals(T entity);
    }
}
