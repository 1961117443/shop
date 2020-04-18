using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService.MaterialServices;
using Shop.ViewModel;

namespace App.Controllers.MaterialManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialUseOutStoreReturnController : BaseController
    {
        private readonly IMaterialUseOutStoreReturnService service;
        private readonly IMapper mapper;

        public MaterialUseOutStoreReturnController(IMaterialUseOutStoreReturnService _service, IMapper _mapper)
        {
            this.service = _service;
            this.mapper = _mapper;
        }
        /// <summary>
        /// 获取主表列表 默认取前20条
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            AjaxResultPageModel<MaterialUseOutStoreReturnViewModel> ajaxResult = new AjaxResultPageModel<MaterialUseOutStoreReturnViewModel>();
            var res = this.service.GetPageList(this.Page.Index, Page.Size, out int total);
            ajaxResult.Data.total = total;
            ajaxResult.Data.data = mapper.MapList<MaterialUseOutStoreReturnViewModel>(res);
            return await Task.FromResult(Ok(ajaxResult));
        }

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="id">主表id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AjaxResultModel<MaterialUseOutStoreReturnViewModel> ajaxResult = new AjaxResultModel<MaterialUseOutStoreReturnViewModel>();
            var uid = id.ToGuid();
            if (uid.IsNotEmpty())
            {
                var main = await service.GetEntityAsync(uid);
                var detail = await service.GetDetailFromMainIdAsync(uid);
                ajaxResult.Data = mapper.Map<MaterialUseOutStoreReturnViewModel>(main);
                if (ajaxResult.Data != null)
                {
                    ajaxResult.Data.Details = mapper.MapList<MaterialUseOutStoreReturnDetailViewModel>(detail);
                }
                else
                {
                    ajaxResult.Code = HttpResponseCode.ResponseErrorMsg;
                    ajaxResult.Msg = "单据不存在";
                }
            }
            else
            {
                ajaxResult.Code = HttpResponseCode.ResponseErrorMsg;
                ajaxResult.Msg = "单据不存在";
            }
            return Ok(ajaxResult);
        } 

        [HttpPost]
        public async Task<IActionResult> Post(JObject data)
        {
            AjaxResultModel ajaxResult = new AjaxResultModel();
            var dto = data.ToObject<MaterialUseOutStoreReturnViewModel>();
            var res = await service.PostAsync(dto.ID.ToGuid(), entity => mapper.Map(dto, entity));
            if (res!=null)
            {
                ajaxResult = new AjaxResultModel<MaterialUseOutStoreReturnViewModel>()
                {                    
                    Data = mapper.Map<MaterialUseOutStoreReturnViewModel>(res),
                    Msg = "保存成功！"
                };
            }
            else
            {
                ajaxResult.Code = HttpResponseCode.ResponseErrorMsg;
                ajaxResult.Msg = "保存失败!";
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
                ajaxResult.Code = HttpResponseCode.ResponseErrorMsg;
                ajaxResult.Data = "删除失败。";
            }
            return Ok(ajaxResult);
        }
    }
}