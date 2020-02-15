using System;
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
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
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
            var list = await this.productService.GetPageListAsync(Page.Index, Page.Size, where);
            AjaxResultModelList<ProductQueryViewModel> ajaxResult = new AjaxResultModelList<ProductQueryViewModel>();
            ajaxResult.data = mapper.MapList<ProductQueryViewModel>(list);
            return Ok(ajaxResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AjaxResultModel<ProductQueryViewModel> ajaxResult = new AjaxResultModel<ProductQueryViewModel>();
            var uid = id.ToGuid();
            if (!uid.IsEmpty())
            {
                var data = await this.productService.GetAsync(w => w.ID.Equals(uid));
                ajaxResult.data= mapper.Map<ProductQueryViewModel>(data);
            }
            return Ok(ajaxResult);           
        }
    }
}