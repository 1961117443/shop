using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Common.Data;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService.MaterialServices;
using Shop.ViewModel;
using Shop.ViewModel.Common;

namespace App.Controllers.MaterialManage
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialStockController : BaseController
    {
        private readonly IMaterialStockService stockService;
        private readonly IMapper mapper;

        public MaterialStockController(IMaterialStockService stockService, IMapper mapper)
        {
            this.stockService = stockService;
            this.mapper = mapper;
        }
        /// <summary>
        /// 材料库存列表 默认取前20条
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            AjaxResultPageModel<MaterialStockViewModel> ajaxResult = new AjaxResultPageModel<MaterialStockViewModel>();
            Expression<Func<MaterialStock, bool>> where = w => w.Quantity != 0;
            var exp = mapper.MapParamList<MaterialStock, MaterialStockViewModel>(QueryParamList).QueryParamToExpression();
            where = where.And(exp);
            var data = this.stockService.GetPageList(this.Page.Index, Page.Size, out int total, where, w => w.ID);
            ajaxResult.data.total = total;
            ajaxResult.data.data = mapper.MapList<MaterialStockViewModel>(data);
            return await Task.FromResult(Ok(ajaxResult));
        }
    }
}