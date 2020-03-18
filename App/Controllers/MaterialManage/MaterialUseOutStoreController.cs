using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FreeSql;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Common;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService.MaterialServices;
using Shop.ViewModel;

namespace App.Controllers.MaterialManage
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialUseOutStoreController : BaseController //, IBillActionController // BaseBillActionController
    {
        private readonly IMaterialUseOutStoreService service;
        private readonly IMapper mapper;
        private readonly IUser user;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="materialUseOutStoreService"></param>
        /// <param name="mapper"></param>
        /// <param name="user"></param>
        public MaterialUseOutStoreController(IMaterialUseOutStoreService materialUseOutStoreService, IMapper mapper, IUser user)
        {
            this.service = materialUseOutStoreService;
            this.mapper = mapper;
            this.user = user;
        }
        
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            AjaxResultModel<string> ajaxResult = new AjaxResultModel<string>();
            var flag = await this.service.DeleteAsync(id.ToGuid());
            if (!flag)
            {
                ajaxResult.Code = HttpResponseCode.ResourceNotFound;
                ajaxResult.Data = "出库单不存在。";
            }
            return Ok(ajaxResult);
        }
        /// <summary>
        /// 获取单条主表记录
        /// </summary>
        /// <param name="id">主表id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AjaxResultModel<MaterialUseOutStoreViewModel> ajaxResult = new AjaxResultModel<MaterialUseOutStoreViewModel>();
            var res = await service.GetEntityAsync(w => w.ID.Equals(id.ToGuid()));
            //mapper.Map<MaterialUseOutStore,MaterialUseOutStoreViewModel>(res,v1)
            //var v0 = this.mapper.Map(res, v1,typeof(MaterialUseOutStore),typeof(MaterialUseOutStoreViewModel));
            ajaxResult.Data = this.mapper.Map<MaterialUseOutStoreViewModel>(res);
            return Ok(ajaxResult);
        }
        /// <summary>
        /// 获取主表列表 默认取前20条
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            AjaxResultPageModel<MaterialUseOutStoreViewModel> ajaxResult = new AjaxResultPageModel<MaterialUseOutStoreViewModel>();
            var res = this.service.GetPageList(this.Page.Index, Page.Size, out int total);
            ajaxResult.Data.total = 0;
            ajaxResult.Data.data = mapper.MapList<MaterialUseOutStoreViewModel>(res);
            return await Task.FromResult(Ok(ajaxResult));
        }
        /// <summary>
        /// 获取从表信息
        /// </summary>
        /// <param name="id">主表的id</param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetail(string id)
        {
            var data = await this.service.GetDetailFromMainIdAsync(id.ToGuid());
            AjaxResultModelList<MaterialUseOutStoreDetailViewModel> ajaxResult = new AjaxResultModelList<MaterialUseOutStoreDetailViewModel>
            {
                Data = mapper.MapList<MaterialUseOutStoreDetailViewModel>(data)
            };
            return Ok(ajaxResult);
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(JObject data)
        {
            AjaxResultModel<string> ajaxResult = new AjaxResultModel<string>();
            var postModel = data.ToObject<MaterialUseOutStoreViewModel>();

            var detail = data["detail"]?.ToObject<IEnumerable<MaterialUseOutStoreDetailViewModel>>();
            var uid = postModel.ID.ToGuid();
            bool res = await service.PostAsync(uid, entity =>
             {
                 mapper.Map(postModel, entity);
                 var details = mapper.MapList<MaterialUseOutStoreDetail>(detail).ToList();
                 entity.Details = details;
                 if (uid.IsEmpty())
                 {
                     entity.MakeDate = DateTime.Now;
                     entity.Maker = user.Name;
                 }
             });

            if (res)
            {
                ajaxResult.Data = "保存成功！";
            }

            return Ok(ajaxResult);
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stockService"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        [HttpPost("audit/{id}")]
        public async Task<IActionResult> Audit(string id, [FromServices] IMaterialStockService stockService,
            [FromServices] IUnitOfWork unitOfWork)
        {
            AjaxResultModel ajaxResult = null;
            var entity = await this.service.GetAsync(id.ToGuid());
            if (entity == null)
            {
                ajaxResult = new AjaxResultModel(HttpResponseCode.ResponseErrorMsg, "出库单不存在。");
                return Ok(ajaxResult);
            }
            else if (entity.AuditDate.HasValue)
            {
                ajaxResult = new AjaxResultModel(HttpResponseCode.ResponseErrorMsg, "出库单已审核。");
                return Ok(ajaxResult);
            }
            var details = await service.GetDetailFromMainIdAsync(id.ToGuid(), false);
            var tempStock = mapper.MapList<MaterialStock>(details);
            foreach (var item in tempStock)
            {
                item.Quantity = item.Quantity * -1;
            }
            var uow = unitOfWork.GetOrBeginTransaction();
            try
            {
                var list = stockService.UpdateStock(tempStock);
                var flag = await service.UpdateAsync(entity, e => new MaterialUseOutStore { Audit = user.Name, AuditDate = DateTime.Now });
                if (!flag)
                {
                    throw new Exception("更新失败！");
                }
                uow.Commit();
                var res = await service.GetEntityAsync(w => w.ID.Equals(id.ToGuid()));
                ajaxResult = new AjaxResultModel<MaterialUseOutStoreViewModel>()
                {
                    Data = this.mapper.Map<MaterialUseOutStoreViewModel>(res),
                    Msg = "出库单审核成功！"
                };
            }
            catch (Exception ex)
            {
                uow.Rollback();
                if (ex is StockOverExcpetion<MaterialStock>)
                {
                    ajaxResult = new AjaxResultModelList<MaterialStock>((ex as StockOverExcpetion<MaterialStock>).OverData)
                    {
                        Code = HttpResponseCode.ResponseErrorMsg,
                        Msg = "超库存，不能审核"
                    };
                    return Ok(ajaxResult);
                }
            }

            return Ok(ajaxResult);
        }


        /// <summary>
        /// 反审单据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("unaudit/{id}")]
        public async Task<IActionResult> UnAudit(string id, [FromServices] IMaterialStockService stockService,
            [FromServices] IUnitOfWork unitOfWork)
        {
            //AjaxResultModel<MaterialUseOutStoreViewModel> ajaxResult = new AjaxResultModel<MaterialUseOutStoreViewModel>();
            //var entity = await this.service.GetAsync(id.ToGuid());
            //if (entity == null)
            //{
            //    ajaxResult.Code = HttpResponseCode.ResourceNotFound;
            //    ajaxResult.Msg = "出库单不存在。";
            //}
            //else if (!entity.AuditDate.HasValue)
            //{
            //    ajaxResult.Code = HttpResponseCode.ResourceNotFound;
            //    ajaxResult.Msg = "出库单未审核。";
            //}
            //else
            //{
            //    var flag = await this.service.UpdateAsync(entity, e => new MaterialUseOutStore() { Audit = "", AuditDate = null });
            //    if (flag)
            //    {
            //        var res = await this.service.GetEntityAsync(w => w.ID.Equals(id.ToGuid()));
            //        ajaxResult.Data = this.mapper.Map<MaterialUseOutStoreViewModel>(res);
            //        ajaxResult.Msg = "出库单反审成功！";
            //    }
            //}
            // return Ok(ajaxResult);


            AjaxResultModel ajaxResult = null;
            var entity = await this.service.GetAsync(id.ToGuid());
            if (entity == null)
            {
                ajaxResult = new AjaxResultModel(HttpResponseCode.ResponseErrorMsg, "出库单不存在。");
                return Ok(ajaxResult);
            }
            else if (!entity.AuditDate.HasValue)
            {
                ajaxResult = new AjaxResultModel(HttpResponseCode.ResponseErrorMsg, "出库单未审核。");
                return Ok(ajaxResult);
            }
            var details = await service.GetDetailFromMainIdAsync(id.ToGuid(), false);
            var tempStock = mapper.MapList<MaterialStock>(details);
            var uow = unitOfWork.GetOrBeginTransaction();
            try
            {
                var list = stockService.UpdateStock(tempStock);
                var flag = await service.UpdateAsync(entity, e => new MaterialUseOutStore { Audit = "", AuditDate = null });
                if (!flag)
                {
                    throw new Exception("更新失败！");
                }
                uow.Commit();
                var res = await service.GetEntityAsync(w => w.ID.Equals(id.ToGuid()));
                ajaxResult = new AjaxResultModel<MaterialUseOutStoreViewModel>()
                {
                    Data = this.mapper.Map<MaterialUseOutStoreViewModel>(res),
                    Msg = "出库单反审成功！"
                };
            }
            catch (Exception ex)
            {
                uow.Rollback();
                if (ex is StockOverExcpetion<MaterialStock>)
                {
                    ajaxResult = new AjaxResultModelList<MaterialStock>((ex as StockOverExcpetion<MaterialStock>).OverData)
                    {
                        Code = HttpResponseCode.ResponseErrorMsg,
                        Msg = "超库存，不能审核"
                    };
                    return Ok(ajaxResult);
                }
            }

            return Ok(ajaxResult);
        }
    }
}