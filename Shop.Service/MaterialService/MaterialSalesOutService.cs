using AutoMapper;
using FreeSql;
using Shop.Common.Extensions;
using Shop.Entity;
using Shop.EntityModel;
using Shop.IService.MaterialServices;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service.MaterialService
{
    public class MaterialSalesOutService : BaseBillService<MaterialSalesOut,MaterialSalesOutDetail>, IMaterialSalesOutService
    {
        private readonly IFreeSql freeSql;
        private readonly IMapper mapper;
        private readonly IMaterialStockService stockService;  
        
        public MaterialSalesOutService(IFreeSql freeSql,IMapper mapper, IMaterialStockService stockService,IUnitOfWork unitOfWork) : base(freeSql,unitOfWork)
        {
            this.freeSql = freeSql;
            this.mapper = mapper;
            this.stockService = stockService;
            base.BaseFreeSql = freeSql;
        }
        public override async Task<bool> DeleteAsync(Guid uid)
        {
            bool flag = false;
            using (var uow = this.freeSql.CreateUnitOfWork())
            {
                var tran = uow.GetOrBeginTransaction();
                 
                var master = uow.GetGuidRepository<MaterialSalesOut>();
                var detail = uow.GetGuidRepository<MaterialSalesOutDetail>();

                int a = await master.DeleteAsync(uid);
                int b = await detail.DeleteAsync(w => w.MainID == uid);
                //stockService.UseTransaction(tran);
                //var res= stockService.InsertOrUpdate(null);
                flag = a + b > 0;
                uow.Commit();
            }
            return flag;
        }

        

        public override async Task<IList<MaterialSalesOutDetail>> GetDetailFromMainIdAsync(Guid id)
        {
            var query = this.freeSql.Select<MaterialSalesOutDetail>()
                .Include(a => a.Product)
                .Include(a => a.Product.ProductCategory)
                .Include(a => a.MaterialWarehouse)
                .Where(w => w.MainID.Equals(id))
                .OrderBy(w => w.RowNo);
            var list = await query.ToListAsync();
            return list;
        }

        public override async Task<MaterialSalesOut> GetAsync(Guid id)
        {
            return await this.freeSql.GetGuidRepository<MaterialSalesOut>().GetAsync(id);
        }

        public override async Task<MaterialSalesOut> GetEntityAsync(Expression<Func<MaterialSalesOut, bool>> where)
        {
            if (where==null)
            {
                where = w => 1 == 2;
            }
            var query = this.freeSql.Select<MaterialSalesOut>()
                .Include(a=>a.Customer)
                .Where(where);

            return await query.ToOneAsync();
        }

        public override IList<MaterialSalesOut> GetPageList(int page, int limit, out int total, Expression<Func<MaterialSalesOut, bool>> where = null, Expression<Func<MaterialSalesOut, object>> order = null)
        {
            var query = this.GetMasterModelQuery().WhereIf(where != null, where);
            if (order!=null)
            {
                query = query.OrderBy(order);
            }
            total = (int)query.Count();
            var list = query.Page(page, limit).ToList();
            return list;
        }

        public override async Task<AjaxResultModelList<MaterialSalesOut>> GetPageListAsync(int page, int limit, Expression<Func<MaterialSalesOut, bool>> where = null, Expression<Func<MaterialSalesOut, object>> order = null)
        {
            AjaxResultModelList<MaterialSalesOut> ajaxResult = new AjaxResultModelList<MaterialSalesOut>();
            var query = this.GetMasterModelQuery().WhereIf(where != null, where);
            if (order != null)
            {
                query = query.OrderBy(order);
            }
            ajaxResult.code = (int)await query.CountAsync();
            ajaxResult.data = await query.Page(page, limit).ToListAsync();
            return ajaxResult;
        }

        public async Task<bool> PostAsync(MaterialSalesOutPostModel postModel)
        {
            bool flag = false;
            using (var uow = this.freeSql.CreateUnitOfWork())
            {
                var masterRepository = uow.GetGuidRepository<MaterialSalesOut>();
                var detailRepository = uow.GetGuidRepository<MaterialSalesOutDetail>();

                var id = postModel.ID.ToGuid();
                MaterialSalesOut master = null;
                List<MaterialSalesOutDetail> detail = null;
                // 新增单据
                if (id.IsEmpty())
                {
                    master = new MaterialSalesOut();
                    detail = new List<MaterialSalesOutDetail>();
                }
                else
                {
                    // 获取主从表最新记录
                    master = await masterRepository.GetAsync(id);
                    detail = await detailRepository.Where(w => w.MainID.Equals(id)).ToListAsync();

                    if (master == null)
                    {
                        throw new Exception("出库单不存在！");
                    }
                }
                mapper.Map(postModel, master);

                // 保存主表数据
                master = await masterRepository.InsertOrUpdateAsync(master);

                IList<MaterialSalesOutDetail> updateItems = new List<MaterialSalesOutDetail>();
                IList<MaterialSalesOutDetail> insertItems = new List<MaterialSalesOutDetail>();
                // 循环最新的记录
                foreach (var view in postModel.Detail)
                {
                    // 1、存在则修改
                    // 2、不存在新增
                    var item = detail.FirstOrDefault(w => w.ID.Equals(view.ID.ToGuid()));
                    if (item == null)
                    {
                        //view.ID = Guid.NewGuid().ToString();
                        item = mapper.Map<MaterialSalesOutDetail>(view);
                        insertItems.Add(item);
                    }
                    else
                    {
                        mapper.Map(view, item);
                        updateItems.Add(item);
                    }
                    item.MainID = master.ID;
                    detail.Remove(item);
                }
                // detail剩下的是已经删除的记录
                int a = await detailRepository.DeleteAsync(detail);
                int b = await detailRepository.UpdateAsync(updateItems);
                var c = await detailRepository.InsertAsync(insertItems);
                flag = a + b + c.Count > 0;
                uow.Commit();
            }
            return flag;
        }

        public override async Task<bool> UpdateAsync(MaterialSalesOut entity, Expression<Func<MaterialSalesOut, MaterialSalesOut>> func = null, Expression<Func<MaterialSalesOut, bool>> where = null)
        {
            var updater = this.freeSql.Update<MaterialSalesOut>(entity);
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
            var tran = UnitOfWork?.GetOrBeginTransaction(false);
            if (tran!=null)
            {
                updater = updater.WithTransaction(tran);
            }
            int res = await updater.ExecuteAffrowsAsync();
            return res > 0;
        }

        public override ISelect<MaterialSalesOutDetail> GetDetailModelQuery()
        {
            return this.freeSql.Select<MaterialSalesOutDetail>()
                 .Include(a => a.Product)
                 .Include(a => a.Product.ProductCategory)
                 .Include(a => a.MaterialWarehouse);
        }

        public override ISelect<MaterialSalesOut> GetMasterModelQuery()
        {
            return this.freeSql.Select<MaterialSalesOut>()
                .Include(a => a.Customer);
        }
    }
}
