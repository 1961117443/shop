﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers.BasicData
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            this._service = productService;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            Expression<Func<Product, bool>> where = w => w.ID != Guid.Empty;
            if (Request.Query.ContainsKey("q"))
            {
                string val = Request.Query["q"];
                if (!val.IsEmpty())
                {
                    where = where.And(w => w.ProductCode.Contains(val) || w.ProductName.Contains(val));
                }
            }
            var list = this._service.GetPageList(Page.Index, Page.Size, out int total, where, o => o.AutoID, false);
            AjaxResultPageModel<ProductViewModel> ajaxResult = new AjaxResultPageModel<ProductViewModel>();
            ajaxResult.Data.total = total;
            ajaxResult.Data.data = _mapper.MapList<ProductViewModel>(list);
            return await Task.FromResult(Ok(ajaxResult));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AjaxResultModel<ProductViewModel> ajaxResult = new AjaxResultModel<ProductViewModel>();
            var uid = id.ToGuid();
            if (!uid.IsEmpty())
            {
                var data = await this._service.GetAsync(w => w.ID.Equals(uid));
                ajaxResult.Data= _mapper.Map<ProductViewModel>(data);
            }
            return Ok(ajaxResult);           
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductViewModel viewModel)
        {
            AjaxResultModel res = new AjaxResultModel();
            var entity = _mapper.Map<Product>(viewModel);
            if (entity != null)
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
                    flag = await _service.UpdateAsync(entity,
                        w => new Product()
                        {
                            ProductCode = entity.ProductCode,
                            ProductName = entity.ProductName,
                            ProductType = entity.ProductType,
                            ProductSpec = entity.ProductSpec,
                            IUint = entity.IUint,
                            ProductCategoryID = entity.ProductCategoryID
                        });
                }
                if (flag)
                {
                    viewModel= _mapper.Map<ProductViewModel>(await _service.GetAsync(w => w.ID == entity.ID));
                    res = new AjaxResultModel<ProductViewModel>(viewModel)
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