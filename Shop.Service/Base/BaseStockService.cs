using Shop.EntityModel;
using Shop.IService;
using Shop.IService.MaterialServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Shop.Service.Base
{
    public abstract class BaseStockService<T> : IBaseStockService<T> where T :class, IBaseStockEntity<T>, new()
    {
        private readonly IFreeSql freeSql;

        public BaseStockService(IFreeSql freeSql)
        {
            this.freeSql = freeSql;
        }
        public IEnumerable<T> UpdateStock(IEnumerable<T> entities)
        {
            Expression<Func<T, bool>> where = a => 1 == 2;
            foreach (var stock in entities)
            {
                where=where.Or(stock.GetWhereSql());
            }
            var stocks = freeSql.Select<T>().Where(where).ToList();
            var updater = freeSql.Update<T>().SetSource(stocks);
            var insert = freeSql.Insert<T>();
            IList<T> over = new List<T>();
            IList<T> news = new List<T>();
            foreach (var entity in entities)
            {
                var stock = stocks.FirstOrDefault(w => w.Equals(entity));
                if (stock == null)
                {
                    stock = new T();
                    insert.AppendData(stock);
                    stocks.Add(stock);
                }
                //// 分析:
                //// 1.有库存记录 直接加减目标对象 记录结果小于0 提示超库存
                //// 2.没有库存记录 如果目标对象小于0 提示超库存
                //stock.Quantity += entity.Quantity;
                //if (stock.Quantity < 0)
                //{
                //    over.Add(entity);
                //}
                if (!UpdateStockCore(stock,entity))
                {
                    over.Add(entity);
                }
            }
            // 没有超库存的记录 更新库存
            if (over.Count == 0)
            {
                var a = insert.ExecuteAffrows();
                var b = updater.ExecuteAffrows();
            }

            return over;
        }

        public virtual bool UpdateStockCore(T stock,T target)
        {
            // 分析:
            // 1.有库存记录 直接加减目标对象 记录结果小于0 提示超库存
            // 2.没有库存记录 如果目标对象小于0 提示超库存
            //stock.Quantity += entity.Quantity;
            //if (stock.Quantity < 0)
            //{
            //    over.Add(entity);
            //}
            return false;
        }
    }
}
