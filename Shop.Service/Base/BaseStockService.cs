using FreeSql;
using Shop.EntityModel;
using Shop.IService;
using Shop.IService.MaterialServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service.Base
{
    public abstract class BaseStockService<T> : IBaseStockService<T> where T :class, IBaseStockEntity<T>, new()
    {
        private readonly IFreeSql freeSql;
        private readonly IUnitOfWork unitOfWork;

        public BaseStockService(IFreeSql freeSql,IUnitOfWork unitOfWork)
        {
            this.freeSql = freeSql;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<T> UpdateStock(IEnumerable<T> entities)
        {
            Dictionary<string, Expression<Func<T, bool>>> tempKeys = new Dictionary<string, Expression<Func<T, bool>>>();
            foreach (var stock in entities)
            {
                var key = stock.GetUniqueValues;
                if (!tempKeys.ContainsKey(key))
                {
                    tempKeys.Add(key, stock.GetUniqueSql());
                }                
            }

            List<T> stocks = new List<T>();
            int limit = 20;
            for (int i = 0; i <= tempKeys.Values.Count / limit; i++)
            {
                Expression<Func<T, bool>> where = null;
                foreach (var exp in tempKeys.Values.Skip(i * limit).Take(limit))
                {
                    where = where.Or(exp);
                }
                if (where!=null)
                {
                    stocks.AddRange(freeSql.Select<T>().Where(where).ToList());
                }
            }
            if (stocks.Count==0)
            {
                return entities;
            }
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
                var tran = unitOfWork.GetOrBeginTransaction(false);
                var a = insert.WithTransaction(tran).ExecuteAffrows();
                var b = updater.WithTransaction(tran).ExecuteAffrows();
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
