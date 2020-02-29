using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    public class MaterialUseOutStoreController : BaseController , IBillActionController // BaseBillActionController
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
        public MaterialUseOutStoreController(IMaterialUseOutStoreService materialUseOutStoreService,IMapper mapper,IUser user)
        {
            this.service = materialUseOutStoreService;
            this.mapper = mapper;
            this.user = user;
        }
        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("audit/{id}")]
        public async Task<IActionResult> Audit(string id)
        {
            AjaxResultModel<object> ajaxResult = new AjaxResultModel<object>();
            var entity = await this.service.GetAsync(id.ToGuid());
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
                var flag = await this.service.UpdateAsync(entity, e => new MaterialUseOutStore { Audit = user.Name, AuditDate = DateTime.Now });
                if (flag)
                {
                    var res = await this.service.GetEntityAsync(w => w.ID.Equals(id.ToGuid()));
                    //ajaxResult.data = this.mapper.Map<MaterialUseOutStoreViewModel>(res);
                }
            }
            return Ok(ajaxResult);
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
                ajaxResult.code = HttpResponseCode.ResourceNotFound;
                ajaxResult.data = "出库单不存在。";
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
            ajaxResult.data = this.mapper.Map<MaterialUseOutStoreViewModel>(res);
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
            var res = await this.service.GetPageListAsync(this.Page.Index, Page.Size);
            ajaxResult.data.total = res.code;
            ajaxResult.data.data = mapper.MapList<MaterialUseOutStoreViewModel>(res.data);
            return Ok(ajaxResult);
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
            AjaxResultModelList<MaterialUseOutStoreDetailViewModel> ajaxResult = new AjaxResultModelList<MaterialUseOutStoreDetailViewModel>();
            ajaxResult.data = mapper.MapList<MaterialUseOutStoreDetailViewModel>(data);
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

            var master = await this.service.GetAsync(postModel.ID.ToGuid());
            var detail = await this.service.GetDetailFromMainIdAsync(postModel.ID.ToGuid());
            if (master!=null)
            {
                master.Details = detail;
                await this.service.PostAsync(master);
            }
            //if (postModel.ID.ToGuid().IsEmpty())
            //{
            //    postModel.Maker = this.user.Name;
            //    postModel.MakeDate = DateTime.Now.ToShortDateString();
            //}
            //var res = await this.service.PostAsync(postModel);

            //if (res)
            //{
            //    ajaxResult.data = "保存成功！";
            //}

            return Ok(ajaxResult);
        }
        /// <summary>
        /// 反审单据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("unaudit/{id}")]
        public async Task<IActionResult> UnAudit(string id)
        {
            AjaxResultModel<object> ajaxResult = new AjaxResultModel<object>();
            var entity = await this.service.GetAsync(id.ToGuid());
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
                var flag = await this.service.UpdateAsync(entity, e => new MaterialUseOutStore() { Audit = "", AuditDate = null });
                if (flag)
                {
                    var res = await this.service.GetEntityAsync(w => w.ID.Equals(id.ToGuid()));
                    ajaxResult.data = this.mapper.Map<MaterialUseOutStoreViewModel>(res);
                }
            }
            return Ok(ajaxResult);
        }
    }
}