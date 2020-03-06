using FreeSql;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService.MaterialServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Shop.Service.MaterialService
{
    public class MaterialStockService : IMaterialStockService
    {
        private readonly IFreeSql freeSql;
        private DbTransaction _DbTransaction;
        protected DbTransaction CurrentTransaction
        {
            get
            {
                return _DbTransaction;
            }
        }

        public MaterialStockService(IFreeSql freeSql)
        {
            this.freeSql = freeSql;
        }

        public bool InsertOrUpdate(IList<MaterialStock> stocks)
        {
            Expression<Func<MaterialStock, bool>> where = a => 1 == 2;
            foreach (var stock in stocks)
            {
                where.Or(a => a.ProductID == stock.ProductID && a.MaterialWareHouseID == stock.MaterialWareHouseID);
            }
            var _stocks = this.freeSql.Select<MaterialStock>().Where(where).ToList();
            var updater = this.freeSql.Update<MaterialStock>().SetSource(_stocks);
            foreach (var _stock in _stocks)
            {
                var item = stocks.FirstOrDefault(a => a.ProductID == _stock.ProductID && a.MaterialWareHouseID == _stock.MaterialWareHouseID);
                if (item!=null)
                {
                    _stock.Quantity += item.Quantity;
                    stocks.Remove(item);
                }
            }
            _stocks.AddRange(stocks);
            if (CurrentTransaction!=null)
            {
                updater = updater.WithTransaction(CurrentTransaction);
            }
            int flag = updater.ExecuteAffrows();
            return true;
        }

        public void UseTransaction(DbTransaction transaction)
        {
            _DbTransaction = transaction;
        }

        /// <summary>
        /// 复杂的查询语句，带上外键一起查询
        /// </summary>
        /// <returns></returns>
        protected ISelect<MaterialStock> GetEntitySelect()
        {
            return this.freeSql.Select<MaterialStock>()
                .Include(a => a.Product.ProductCategory)
                .Include(a=>a.MaterialWarehouse);
        }
        public IList<MaterialStock> GetPageList(int page, int limit, out int total, Expression<Func<MaterialStock, bool>> where = null, Expression<Func<MaterialStock, object>> order = null)
        {
            var query = this.GetEntitySelect().WhereIf(where != null, where);
            if (order != null)
            {
                query = query.OrderBy(order);
            }
            total = (int)query.Count();
            var list = query.Page(page, limit).ToList();
            return list;
        }

        public IList<MaterialStock> UpdateStock(IList<MaterialStock> entities)
        {
            Expression<Func<MaterialStock, bool>> where = a => 1 == 2;
            foreach (var stock in entities)
            {
                where.Or(a => a.Equals(stock));
            }
            var stocks = this.freeSql.Select<MaterialStock>().Where(where).ToList();
            var updater = this.freeSql.Update<MaterialStock>().SetSource(stocks);
            var insert = this.freeSql.Insert<MaterialStock>();
            IList<MaterialStock> over = new List<MaterialStock>();
            IList<MaterialStock> news = new List<MaterialStock>();
            foreach (var entity in entities)
            {
                var stock = stocks.FirstOrDefault(w => w.Equals(entity));
                if (stock==null)
                {
                    stock = new MaterialStock();
                    insert.AppendData(stock);
                    stocks.Add(stock);
                }
                // 分析:
                // 1.有库存记录 直接加减目标对象 记录结果小于0 提示超库存
                // 2.没有库存记录 如果目标对象小于0 提示超库存
                stock.Quantity += entity.Quantity;
                if (stock.Quantity < 0)
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
    }
}
