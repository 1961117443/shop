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
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService CustomerService;
        private readonly IMapper mapper;

        public CustomerController(ICustomerService customerService,IMapper mapper)
        {
            this.CustomerService = customerService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Expression<Func<Customer, bool>> where = w => w.ID != Guid.Empty;
            if (Request.Query.ContainsKey("q"))
            {
                string val = Request.Query["q"];
                if (!val.IsEmpty())
                {
                    where = where.And(w => w.Code.Contains(val) || w.Name.Contains(val));
                }
            }
            var list = await this.CustomerService.GetPageListAsync(Page.Index, Page.Size, where);
            AjaxResultModelList<CustomerQueryViewModel> ajaxResult = new AjaxResultModelList<CustomerQueryViewModel>();
            ajaxResult.data = mapper.MapList<CustomerQueryViewModel>(list);
            return Ok(ajaxResult);
        }

        /// <summary>
        /// 根据主键id获取供应商数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AjaxResultModel<CustomerQueryViewModel> ajaxResult = new AjaxResultModel<CustomerQueryViewModel>();
            var uid = id.ToGuid();
            if (!uid.IsEmpty())
            {
                var data = await this.CustomerService.GetAsync(uid);
                ajaxResult.data = mapper.Map<CustomerQueryViewModel>(data);
            }
            return Ok(ajaxResult);
        }
    }
}