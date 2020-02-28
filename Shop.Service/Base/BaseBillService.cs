using FreeSql;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{

    public abstract class BaseBillService<TMaster, TDetail> : IBaseBillService<TMaster, TDetail> where TMaster : BaseMasterEntity<TDetail> where TDetail : BaseDetailEntity
    {
        protected IFreeSql BaseFreeSql { get; set; }
        public BaseBillService(IFreeSql freeSql)
        {
            this.BaseFreeSql = freeSql;
        }

        public virtual async Task<bool> DeleteAsync(Guid uid)
        {
            bool flag = false;
            using (var uow = this.BaseFreeSql.CreateUnitOfWork())
            {
                var tran = uow.GetOrBeginTransaction();

                var master = uow.GetGuidRepository<TMaster>();
                var detail = uow.GetGuidRepository<TDetail>();

                int a = await master.DeleteAsync(uid);
                int b = await detail.DeleteAsync(w => w.MainID == uid);
                flag = a + b > 0;
                uow.Commit();
            }
            return flag;
        }

        public virtual async Task<TMaster> GetAsync(Guid key)
        {
            return await this.BaseFreeSql.GetGuidRepository<TMaster>().GetAsync(key);
        }

        /// <summary>
        /// 从表的复杂查询语句，带上外键一起查询
        /// </summary>
        /// <returns></returns>
        public abstract ISelect<TDetail> GetDetailModelQuery();
        /// <summary>
        /// 主表的复杂查询语句，带上外键一起查询
        /// </summary>
        /// <returns></returns>
        public abstract ISelect<TMaster> GetMasterModelQuery();
        public virtual async Task<IList<TDetail>> GetDetailFromMainIdAsync(Guid id)
        {
            var query = GetDetailModelQuery()
                .Where(w => w.MainID.Equals(id))
                .OrderBy(w => w.RowNo);
            var list = await query.ToListAsync();
            return list;
        }

        public virtual async Task<TMaster> GetEntityAsync(Expression<Func<TMaster, bool>> where)
        {
            if (where == null)
            {
                where = w => 1 == 2;
            }
            var query = GetMasterModelQuery().Where(where);

            return await query.ToOneAsync();
        }

        public virtual IList<TMaster> GetPageList(int page, int limit, out int total, Expression<Func<TMaster, bool>> where = null, Expression<Func<TMaster, object>> order = null)
        {
            var query = this.GetMasterModelQuery().WhereIf(where != null, where);
            if (order != null)
            {
                query = query.OrderBy(order);
            }
            total = (int)query.Count();
            var list = query.Page(page, limit).ToList();
            return list;
        }

        public virtual async Task<AjaxResultModelList<TMaster>> GetPageListAsync(int page, int limit, Expression<Func<TMaster, bool>> where = null, Expression<Func<TMaster, object>> order = null)
        {
            AjaxResultModelList<TMaster> ajaxResult = new AjaxResultModelList<TMaster>();
            var query = this.GetMasterModelQuery().WhereIf(where != null, where);
            if (order != null)
            {
                query = query.OrderBy(order);
            }
            ajaxResult.code = (int)await query.CountAsync();
            ajaxResult.data = await query.Page(page, limit).ToListAsync();
            return ajaxResult;
        }

        public virtual async Task<bool> UpdateAsync(TMaster entity, Expression<Func<TMaster, TMaster>> func = null, Expression<Func<TMaster, bool>> where = null)
        {
            var updater = this.BaseFreeSql.Update<TMaster>(entity);
            if (func != null)
            {
                updater = updater.Set(func);
            }
            else
            {
                updater = updater.SetSource(entity);
            }
            if (where != null)
            {
                updater = updater.Where(where);
            }
            int res = await updater.ExecuteAffrowsAsync();
            return res > 0;
        }

        public async virtual Task<bool> PostAsync(TMaster entity)
        {
            bool flag = false;
            using (var uow = this.BaseFreeSql.CreateUnitOfWork())
            {
                var masterRepository = uow.GetGuidRepository<TMaster>();
                var detailRepository = uow.GetGuidRepository<TDetail>();
                 
                 
                // 保存主表数据
                var master = await masterRepository.InsertOrUpdateAsync(entity);

                IList<MaterialSalesOutDetail> updateItems = new List<MaterialSalesOutDetail>();
                IList<MaterialSalesOutDetail> insertItems = new List<MaterialSalesOutDetail>();
                // 循环最新的记录
                for (int i = 0; i < entity.Details.Count; i++)
                {
                    TDetail detail = entity.Details[i];
                    if (!detail.MainID.Equal(master.ID))
                    {
                        detail.MainID = master.ID;
                    }
                    detail.RowNo = i + 1;
                }
                int res = await detailRepository.UpdateAsync(entity.Details);
                flag = res > 0;
                uow.Commit();
            }
            return flag;
        }
    }
}
