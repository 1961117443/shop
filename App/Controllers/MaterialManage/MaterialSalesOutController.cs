﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Common;
using Shop.Common.Extensions;
using Shop.Entity;
using Shop.EntityModel;
using Shop.IService.MaterialServices;
using Shop.ViewModel;

namespace App.Controllers.MaterialManage
{
    /// <summary>
    /// 销售出库api
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialSalesOutController : BaseController
    {
        private readonly IMaterialSalesOutService materialSalesOutService;
        private readonly IMapper mapper;
        private readonly IUser user;

        /// <summary>
        /// 构造函数注入服务
        /// </summary>
        /// <param name="materialSalesOutService"></param>
        /// <param name="mapper"></param>
        /// <param name="user"></param>
        public MaterialSalesOutController(IMaterialSalesOutService materialSalesOutService,IMapper mapper,IUser user)
        {
            this.materialSalesOutService = materialSalesOutService;
            this.mapper = mapper;
            this.user = user;
        }

        /// <summary>
        /// 销售出库列表 默认取前20条
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            AjaxResultPageModel<MaterialSalesOutViewModel> ajaxResult = new AjaxResultPageModel<MaterialSalesOutViewModel>(); 
            //var data = this.materialSalesOutService.GetPageList(this.Page.Index, Page.Size, out total);
            var res = await this.materialSalesOutService.GetPageListAsync(this.Page.Index, Page.Size);
            ajaxResult.data.total = res.code;
            ajaxResult.data.data = mapper.MapList<MaterialSalesOutViewModel>(res.data);
            return Ok(ajaxResult);
        }

        /// <summary>
        /// 获取销售出库单主表信息
        /// </summary>
        /// <param name="id">主表id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AjaxResultModel<MaterialSalesOutViewModel> ajaxResult = new AjaxResultModel<MaterialSalesOutViewModel>();
            var res = await materialSalesOutService.GetEntityAsync(w => w.ID.Equals(id.ToGuid()));
            ajaxResult.data = this.mapper.Map<MaterialSalesOutViewModel>(res);
            return Ok(ajaxResult);
        }

        /// <summary>
        /// 获取出库单从表信息
        /// </summary>
        /// <param name="id">主表的id</param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetail(string id)
        {
            var data = await this.materialSalesOutService.GetDetailFromMainIdAsync(id.ToGuid());
            AjaxResultModelList<MaterialSalesOutDetailViewModel> ajaxResult = new AjaxResultModelList<MaterialSalesOutDetailViewModel>();
            ajaxResult.data = mapper.MapList<MaterialSalesOutDetailViewModel>(data);
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
            var postModel = data.ToObject<MaterialSalesOutPostModel>();

            if (postModel.ID.ToGuid().IsEmpty())
            {
                postModel.Maker = this.user.Name;
                postModel.MakeDate = DateTime.Now.ToShortDateString();
            }
            var res = await this.materialSalesOutService.PostAsync(postModel);

            if (res)
            {
                ajaxResult.data = "保存成功！";
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
            var flag = await this.materialSalesOutService.DeleteAsync(id.ToGuid());
            if (!flag)
            {
                ajaxResult.code = HttpResponseCode.ResourceNotFound;
                ajaxResult.data = "出库单不存在。";
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
            var entity = await this.materialSalesOutService.GetAsync(id.ToGuid());
            if (entity == null)
            {
                ajaxResult.code = HttpResponseCode.ResourceNotFound;
                ajaxResult.data = "入库单不存在。";
            }
            else if (entity.AuditDate.HasValue)
            {
                ajaxResult.code = HttpResponseCode.ResourceNotFound;
                ajaxResult.data = "入库单已审核。";
            }
            else
            {
                var obj = new
                {
                    Audit = user.Name,
                    AuditDate = DateTime.Now
                };
                var flag = await this.materialSalesOutService.UpdateAsync(entity, e => new MaterialSalesOut { Audit = user.Name, AuditDate = DateTime.Now });
                if (flag)
                {
                    var res = await this.materialSalesOutService.GetEntityAsync(w => w.ID.Equals(id.ToGuid()));
                    ajaxResult.data = this.mapper.Map<MaterialSalesOutViewModel>(res);
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
            var entity = await this.materialSalesOutService.GetAsync(id.ToGuid());
            if (entity == null)
            {
                ajaxResult.code = HttpResponseCode.ResourceNotFound;
                ajaxResult.data = "入库单不存在。";
            }
            else if (!entity.AuditDate.HasValue)
            {
                ajaxResult.code = HttpResponseCode.ResourceNotFound;
                ajaxResult.data = "入库单未审核。";
            }
            else
            {
                var flag = await this.materialSalesOutService.UpdateAsync(entity, e => new MaterialSalesOut() { Audit = "", AuditDate = null });
                if (flag)
                {
                    var res = await this.materialSalesOutService.GetEntityAsync(w => w.ID.Equals(id.ToGuid()));
                    ajaxResult.data = this.mapper.Map<MaterialSalesOutViewModel>(res);
                }
            }
            return Ok(ajaxResult);
        }
    }
}