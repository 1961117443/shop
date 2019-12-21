using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shop.Common.Utils;
using Shop.EntityModel;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    [Route("api/orders/detail")]
    [ApiController]
    public class OrderDetailController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IBaseService<SalesOrderDetail> detailService;
        private readonly CustomExpressionHelper expressionHelper;

        public OrderDetailController(IMapper mapper, IBaseService<SalesOrderDetail> detailService, CustomExpressionHelper expressionHelper)
        {
            this.mapper = mapper;
            this.detailService = detailService;
            this.expressionHelper = expressionHelper;
        }
        [HttpPost("{id}/{key}")]
        public async Task<IActionResult> Put(int id, JObject data)
        {
            var key = RouteData.Values["key"]?.ToString();
            var detail = await detailService.GetAsync(w => w.AutoID == id);
            bool res = false;
            if (key != null)
            {
                OrderDetailViewModel dto = data.ToObject<OrderDetailViewModel>();

                var exp = expressionHelper.ViewToEntity<OrderDetailViewModel,SalesOrderDetail>(dto, key);
                res = await detailService.UpdateAsync(detail, exp);
            } 
            return Ok(new { message = "操作成功" });
        }


    }
}