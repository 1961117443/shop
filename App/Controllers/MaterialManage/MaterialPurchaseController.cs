using App.Filters;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Common.Extensions;
using Shop.Common;
using Shop.Common.Utils;
using Shop.EntityModel;
using Shop.IService.MaterialServices;
using Shop.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Controllers.MaterialManage
{
    /// <summary>
    /// 材料采购入库
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialPurchaseController : BaseController
    {
        private readonly IMaterialPurchaseService purchaseService;
        private readonly IMaterialPurchaseDetailService detailService;
        private readonly IMapper mapper;
        private readonly IUser user;

        public MaterialPurchaseController(IMaterialPurchaseService purchaseService, IMaterialPurchaseDetailService detailService, IMapper mapper, IUser user)
        {
            this.purchaseService = purchaseService;
            this.detailService = detailService;
            this.mapper = mapper;
            this.user = user;
        }

        /// <summary>
        /// 获取入库单从表信息
        /// </summary>
        /// <param name="id">主表的id</param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetail(string id)
        {
            var data = await this.detailService.GetDetailFromMainIdAsync(id.ToGuid());
            AjaxResultModelList<MaterialPurchaseDetailViewModel> ajaxResult = new AjaxResultModelList<MaterialPurchaseDetailViewModel>
            {
                Data = mapper.MapList<MaterialPurchaseDetailViewModel>(data)
            };
            return Ok(ajaxResult);
        }

        /// <summary>
        /// 采购入库列表 默认取前20条
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            AjaxResultPageModel<MaterialPurchaseViewModel> ajaxResult = new AjaxResultPageModel<MaterialPurchaseViewModel>();
            ajaxResult.Data.total = await this.purchaseService.Count();
            var data = await this.purchaseService.GetPageListAsync(this.Page.Index, Page.Size);
            ajaxResult.Data.data = mapper.MapList<MaterialPurchaseViewModel>(data);
            return Ok(ajaxResult);
        }

        /// <summary>
        /// 获取采购入库单主表信息
        /// </summary>
        /// <param name="id">主表id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AjaxResultModel<MaterialPurchaseViewModel> ajaxResult = new AjaxResultModel<MaterialPurchaseViewModel>();
            var res = await this.purchaseService.GetAsync(w => w.ID.Equals(id.ToGuid()));
            ajaxResult.Data = this.mapper.Map<MaterialPurchaseViewModel>(res);
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
            var postModel = data.ToObject<MaterialPurchasePostModel>();

            if (postModel.ID.ToGuid().IsEmpty())
            {
                postModel.ID = Guid.Empty.ToString();
                postModel.Maker = this.user.Name;
                postModel.MakeDate = DateTime.Now.ToShortDateString();
            }

            var res = await this.purchaseService.PostAsync(postModel);

            if (res)
            {
                ajaxResult.Data = "保存成功！";
            }

            return Ok(ajaxResult);
        }

        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            AjaxResultModel<string> ajaxResult = new AjaxResultModel<string>();
            var flag = await this.purchaseService.DeleteAsync(id.ToGuid());
            if (flag == 0)
            {
                ajaxResult.Code = HttpResponseCode.ResourceNotFound;
                ajaxResult.Data = "入库单不存在。";
            }
            return Ok(ajaxResult);
        }

        /// <summary>
        /// 审核入库单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("audit/{id}")]
        public async Task<IActionResult> Audit(string id)
        {
            AjaxResultModel<object> ajaxResult = new AjaxResultModel<object>();
            var entity = await this.purchaseService.GetAsync(id.ToGuid());
            if (entity == null)
            {
                ajaxResult.Code = HttpResponseCode.ResourceNotFound;
                ajaxResult.Data = "入库单不存在。";
            }
            else if (entity.AuditDate.HasValue)
            {
                ajaxResult.Code = HttpResponseCode.ResourceNotFound;
                ajaxResult.Data = "入库单已审核。";
            }
            else
            {
                var flag=await this.purchaseService.UpdateAsync(entity, e => new MaterialPurchase() { Audit = user.Name, AuditDate = DateTime.Now });
                if (flag)
                {
                    var res = await this.purchaseService.GetAsync(w => w.ID.Equals(id.ToGuid()));
                    ajaxResult.Data = this.mapper.Map<MaterialPurchaseViewModel>(res);
                }
            }
            return Ok(ajaxResult);
        }

        /// <summary>
        /// 反审入库单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("unaudit/{id}")]
        public async Task<IActionResult> UnAudit(string id)
        {
            AjaxResultModel<object> ajaxResult = new AjaxResultModel<object>();
            var entity = await this.purchaseService.GetAsync(id.ToGuid());
            if (entity == null)
            {
                ajaxResult.Code = HttpResponseCode.ResourceNotFound;
                ajaxResult.Data = "入库单不存在。";
            }
            else if (!entity.AuditDate.HasValue)
            {
                ajaxResult.Code = HttpResponseCode.ResourceNotFound;
                ajaxResult.Data = "入库单未审核。";
            }
            else
            {
                var flag = await this.purchaseService.UpdateAsync(entity, e => new MaterialPurchase() { Audit = "", AuditDate = null });
                if (flag)
                {
                    var res = await this.purchaseService.GetAsync(w => w.ID.Equals(id.ToGuid()));
                    ajaxResult.Data = this.mapper.Map<MaterialPurchaseViewModel>(res);
                }
            }
            return Ok(ajaxResult);
        }

    }
}