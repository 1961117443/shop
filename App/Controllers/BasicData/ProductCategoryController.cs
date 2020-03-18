using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers.BasicData
{
    [Route("api/ProductCategory")]
    [ApiController]
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _service;
        private readonly IMapper _mapper;

        public ProductCategoryController(IProductCategoryService service,IMapper mapper)
        {
            this._service = service;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Expression<Func<ProductCategory, bool>> where = w => w.ID != Guid.Empty;
            if (Request.Query.ContainsKey("q"))
            {
                string val = Request.Query["q"];
                if (!val.IsEmpty())
                {
                    where = where.And(w => w.Code.Contains(val) || w.Name.Contains(val));
                }
            }
            var list = await _service.GetPageListAsync(Page.Index, Page.Size, where);
            AjaxResultModel ajaxResult = new AjaxResultModelList<ProductCategoryViewModel>
            {
                Data = _mapper.MapList<ProductCategoryViewModel>(list)
            };
            return Ok(ajaxResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AjaxResultModel<ProductCategoryViewModel> ajaxResult = new AjaxResultModel<ProductCategoryViewModel>();
            var uid = id.ToGuid();
            if (!uid.IsEmpty())
            {
                var data = await this._service.GetAsync(w => w.ID.Equals(uid));
                ajaxResult.Data = _mapper.Map<ProductCategoryViewModel>(data);
            }
            return Ok(ajaxResult);
        }


        [HttpPost]
        public async Task<IActionResult> Post(ProductCategoryViewModel viewModel)
        {
            AjaxResultModel res = new AjaxResultModel();
            var entity = _mapper.Map<ProductCategory>(viewModel);
            if (entity!=null)
            {
                var flag = false;
                if (entity.ID.IsEmpty())
                {
                    entity.ID = Guid.NewGuid();
                    viewModel.ID = entity.ID.ToString();
                    flag = await _service.InsertAsync(entity);
                }
                else
                {
                    flag = await _service.UpdateAsync(entity, w => new ProductCategory() { Code = entity.Code, Name = entity.Name });
                } 
                if (flag)
                {
                    res = new AjaxResultModel<ProductCategoryViewModel>(viewModel)
                    {
                        Msg = viewModel.ID.IsEmpty() ? "新增成功！" : "保存成功！"
                    };
                   
                    return Ok(res);
                }
            }
            res.Code = HttpResponseCode.ResponseErrorMsg;
            res.Msg = "提交失败，请稍后再试！";
            return Ok(res);
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            AjaxResultModel ajaxResult = new AjaxResultModel();
            Guid uid = id.ToGuid();
            if (!uid.IsEmpty())
            {
                var flag = await this._service.DeleteAsync(w => w.ID == uid);
                if (flag)
                {
                    ajaxResult.Msg = "删除成功！";
                }
                else
                {
                    ajaxResult.Msg = "操作失败，请刷新列表后再尝试！";
                }
            }
            return Ok(ajaxResult);
        }

    }
}