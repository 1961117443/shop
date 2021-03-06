﻿using FreeSql;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service
{

    public abstract class BaseBillService<TMaster, TDetail> : IBaseBillService<TMaster, TDetail> where TMaster : class, IBaseMasterEntity<TDetail>, new() where TDetail : class, IBaseDetailEntity,new ()
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected IFreeSql BaseFreeSql { get; set; }
        public BaseBillService(IFreeSql freeSql, IUnitOfWork unitOfWork)
        {
            this.BaseFreeSql = freeSql;
            this.UnitOfWork = unitOfWork;

            this.MasterRepository = BaseFreeSql.GetGuidRepository<TMaster>();
            this.DetailRepository = BaseFreeSql.GetGuidRepository<TDetail>();
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
        public virtual async Task<IList<TDetail>> GetDetailFromMainIdAsync(Guid id, bool all = true)
        {
            var query = all ? GetDetailModelQuery() : BaseFreeSql.Select<TDetail>();
            query = query.Where(w => w.MainID.Equals(id))
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
            ajaxResult.Code = (int)await query.CountAsync();
            ajaxResult.Data = await query.Page(page, limit).ToListAsync();
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
            var tran = UnitOfWork.GetOrBeginTransaction(false);
            if (tran!=null)
            {
                updater = updater.WithTransaction(tran);
            }
            int res = await updater.ExecuteAffrowsAsync();
            return res > 0;
        }

        /// <summary>
        /// 如果是新纪录需要保持 id=empty
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public async virtual Task<bool> PostAsync(TMaster entity, IEnumerable<TDetail> details)
        {
            bool flag = false;
            //details = entity.Details;
            using (var uow = this.BaseFreeSql.CreateUnitOfWork())
            {
                var masterRepository =  uow.GetGuidRepository<TMaster>();
                var detailRepository =  uow.GetGuidRepository<TDetail>();

                //// 保存主表数据
                bool _new = false;
                if (entity.ID.IsEmpty())
                {
                    _new = true;
                    entity.ID = Guid.NewGuid();
                    entity = await masterRepository.InsertAsync(entity);
                }
                else
                {
                    // 附加主表数据
                    masterRepository.Attach(new TMaster() { ID = entity.ID });
                    await masterRepository.UpdateAsync(entity);
                }

                // 附加从表数据
                //detailRepository.Attach(details.Where(w=>!w.ID.IsEmpty()).Select(w => new TDetail() { ID = w.ID }).ToArray()); 

                IList<TDetail> updateItems = new List<TDetail>();
                IList<TDetail> insertItems = new List<TDetail>();
                // 循环最新的记录
                int i = 1;
                foreach (var detail in details)
                {
                    if (detail.ID.IsEmpty())
                    {
                        detail.ID = Guid.NewGuid();
                        insertItems.Add(detail);
                    }
                    else
                    {
                        detailRepository.Attach(detail);
                        updateItems.Add(detail);
                    }
                    if (detail.MainID!= entity.ID)
                    {
                        detail.MainID = entity.ID;
                    }
                    if (detail.MainID != entity.ID)
                    {
                        detail.RowNo = i;
                    }
                    i = i + 1; 
                }  
                Expression<Func<TDetail, bool>> delExp = w => w.MainID == entity.ID;
                if (details != null && details.Count() > 0)
                {
                    var arr = details.Select(w => w.ID).ToArray();
                    delExp = delExp.And(w => !arr.Contains(w.ID));
                }
                // 删除
                var del = await detailRepository.DeleteAsync(delExp);
                // 更新
                if (updateItems.Count>0)
                {
                    int upd = await detailRepository.UpdateAsync(updateItems);
                }
                // 插入
                if (insertItems.Count>0)
                {
                    var add = await detailRepository.InsertAsync(insertItems);
                }                 
                flag = true;
                uow.Commit();
            }
            return flag;
        }


        /// <summary>
        /// Action<TMaster> 的参数master 不能重新new
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="beforePost"></param>
        /// <returns></returns>
        public async virtual Task<TMaster> PostAsync(Guid uid,Action<TMaster> beforePost)
        {
            bool flag = false;
            //details = entity.Details;
            TMaster master = null;
            using (var uow = this.BaseFreeSql.CreateUnitOfWork())
            {
                var masterRepository = uow.GetGuidRepository<TMaster>();
                var detailRepository = uow.GetGuidRepository<TDetail>();

                
                //IEnumerable<TDetail> details = null;
                //// 保存主表数据
                bool _new = false;
                if (uid.IsEmpty())
                {
                    _new = true;
                    master = new TMaster()
                    {
                        Details = new List<TDetail>()
                    };
                }
                else
                {
                    master =await masterRepository.GetAsync(uid); 
                    var details = await detailRepository.Where(w => w.MainID == uid).ToListAsync();
                    master.Details = details == null ? new List<TDetail>() : details.ToList();
                }

                // 原单据从表所有的id 用于判断从表记录是新增还是修改还是删除
                var dbDetialIds = master.Details.Select(w => w.ID).ToList();


                beforePost?.Invoke(master);

                if (_new)
                {
                    if (master.ID.IsEmpty())
                    {
                        master.ID = Guid.NewGuid();
                    }
                }
                IList<TDetail> updateItems = new List<TDetail>();
                IList<TDetail> insertItems = new List<TDetail>();
                // 循环最新的记录
                int i = 1;
                foreach (var detail in master.Details)
                {
                    #region new recored
                    if (detail.ID.IsEmpty())
                    {
                        detail.ID = Guid.NewGuid();
                    }

                    if (!dbDetialIds.Contains(detail.ID))
                    { 
                        insertItems.Add(detail);
                    }
                    else
                    {
                        updateItems.Add(detail);
                        dbDetialIds.Remove(detail.ID);
                    }
                    #endregion



                    if (detail.MainID != master.ID)
                    {
                        detail.MainID = master.ID;
                    }
                    if (detail.RowNo != i)
                    {
                        detail.RowNo = i;
                    }
                    i = i + 1;
                }
                // 更新主表
                if (_new)
                {
                    var entity = await masterRepository.InsertAsync(master);
                }
                else
                {
                    int res1 = await masterRepository.UpdateAsync(master);
                }

                List<Task> tasks = new List<Task>();
                // 删除
                if (dbDetialIds.Count>0)
                {
                    tasks.Add(detailRepository.DeleteAsync(w => w.MainID == master.ID && dbDetialIds.Contains(w.ID)));
                    //var del = await detailRepository.DeleteAsync(w => w.MainID == master.ID && dbDetialIds.Contains(w.ID));
                }                
                // 更新
                if (updateItems.Count > 0)
                {
                    tasks.Add(detailRepository.UpdateAsync(updateItems));
                    //int upd = await detailRepository.UpdateAsync(updateItems);
                }
                // 插入
                if (insertItems.Count > 0)
                {
                    tasks.Add(detailRepository.InsertAsync(insertItems));
                    //var add = await detailRepository.InsertAsync(insertItems);
                }
                Task.WaitAll(tasks.ToArray()); 
                flag = true;
                uow.Commit();
            }
            if (flag)
            {
                return await GetEntityAsync(w => w.ID == master.ID);
            }
            return null;
        }
        //public async virtual Task<TMaster> GetAttchAsync(Guid uid)
        //{
        //    //var masterRepository = BaseFreeSql.GetGuidRepository<TMaster>(); 

        //    //var detailRepository = BaseFreeSql.GetGuidRepository<TDetail>();
        //    var master = await MasterRepository.GetAsync(uid);
        //    var details = await DetailRepository.Where(w => w.MainID == uid).ToListAsync();
        //    master.Details = details;
        //    //masterRepository.Attach(master); 
        //    //detailRepository.Attach(details); 
        //    return master;
        //}


        private GuidRepository<TMaster> MasterRepository { get; }
        private GuidRepository<TDetail> DetailRepository { get; }
    }
}
