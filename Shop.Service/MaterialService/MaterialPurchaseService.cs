using AutoMapper;
using FreeSql;
using Shop.Common.Extensions;
using Shop.Common.Utils;
using Shop.EntityModel;
using Shop.IService;
using Shop.IService.MaterialServices;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service.MaterialService
{
    /// <summary>
    /// 采购入库单
    /// </summary>
    public class MaterialPurchaseService:BaseService<MaterialPurchase>, IMaterialPurchaseService
    {
        //private readonly IFreeSql freeSqlInstance;
        private readonly IMapper mapper;

        public MaterialPurchaseService(IFreeSql freeSql, IMapper mapper):base(freeSql)
        {
            this.mapper = mapper;
        }

        public async Task<int> DeleteAsync(Guid uid)
        {
            int flag = 0;
            using (var uow = this.Instance.CreateUnitOfWork())
            {
                var masterRepository = uow.GetGuidRepository<MaterialPurchase>();
                var detailRepository = uow.GetGuidRepository<MaterialPurchaseDetail>();

                int a = await masterRepository.DeleteAsync(uid);
                int b = await detailRepository.DeleteAsync(w => w.MainID == uid);
                flag = a + b;
                uow.Commit();
            }
            return flag;
        }

        /// <summary>
        /// 保存数据
        /// postModel 主从表姐沟
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public async Task<bool> PostAsync(MaterialPurchasePostModel postModel)
        {
            bool flag = false;
            using (var uow = this.Instance.CreateUnitOfWork())
            {
                var masterRepository = uow.GetGuidRepository<MaterialPurchase>();
                var detailRepository = uow.GetGuidRepository<MaterialPurchaseDetail>();

                var id = postModel.ID.ToGuid();
                MaterialPurchase master = null;
                List<MaterialPurchaseDetail> detail = null;
                // 新增单据
                if (id.IsEmpty())
                {
                    master = new MaterialPurchase();
                    detail = new List<MaterialPurchaseDetail>();
                }
                else
                {
                    // 获取主从表最新记录
                    master = await masterRepository.GetAsync(id);
                    detail = await detailRepository.Where(w => w.MainID.Equals(id)).ToListAsync();

                    if (master == null)
                    {
                        throw new Exception("入库单不存在！");
                    }
                }
                mapper.Map(postModel, master);

                // 保存主表数据
                master = await masterRepository.InsertOrUpdateAsync(master);
                
                IList<MaterialPurchaseDetail> updateItems = new List<MaterialPurchaseDetail>();
                IList<MaterialPurchaseDetail> insertItems = new List<MaterialPurchaseDetail>();
                // 循环最新的记录
                foreach (var view in postModel.Detail)
                {
                    // 1、存在则修改
                    // 2、不存在新增
                    var item = detail.FirstOrDefault(w => w.ID.Equals(view.ID.ToGuid()));
                    if (item==null)
                    {
                        //view.ID = Guid.NewGuid().ToString();
                        item = mapper.Map<MaterialPurchaseDetail>(view);
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
                int a=await detailRepository.DeleteAsync(detail);
                int b=await detailRepository.UpdateAsync(updateItems);
                var c=await detailRepository.InsertAsync(insertItems);
                flag = a + b + c.Count > 0;
                uow.Commit();
            }
            return flag;
        } 

        protected override ISelect<MaterialPurchase> SelectEntity()
        {
            return this.Instance.Select<MaterialPurchase>().Include(a => a.Vendor);
        }
    }

    public class MaterialPurchaseDetailService : BaseService<MaterialPurchaseDetail>, IMaterialPurchaseDetailService
    {
        public MaterialPurchaseDetailService(IFreeSql freeSqlInstance):base(freeSqlInstance)
        {
            this.Instance = freeSqlInstance;
            base.Instance = freeSqlInstance;
        }

        public async Task<IList<MaterialPurchaseDetail>> GetDetailFromMainIdAsync(Guid mId)
        {
            var query = this.Instance.Select<MaterialPurchaseDetail>()
                .Include(a => a.Product.ProductCategory)
                .Include(a => a.MaterialWarehouse)
                .Where(w => w.MainID.Equals(mId));
            var list = await query.ToListAsync();
            return list;
        }
    }
}
