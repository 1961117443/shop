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
    public class VendorController : BaseController
    {
        private readonly IVendorService vendorService;
        private readonly IMapper mapper;

        public VendorController(IVendorService vendorService,IMapper mapper)
        {
            this.vendorService = vendorService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Expression<Func<Vendor, bool>> where = w => w.ID != Guid.Empty;
            if (Request.Query.ContainsKey("q"))
            {
                string val = Request.Query["q"];
                if (!val.IsEmpty())
                {
                    where = where.And(w => w.Code.Contains(val) || w.Name.Contains(val));
                }
            }
            var list = await this.vendorService.GetPageListAsync(Page.Index, Page.Size, where);
            AjaxResultModelList<VendorQueryViewModel> ajaxResult = new AjaxResultModelList<VendorQueryViewModel>();
            ajaxResult.data = mapper.MapList<VendorQueryViewModel>(list);
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
            AjaxResultModel<VendorQueryViewModel> ajaxResult = new AjaxResultModel<VendorQueryViewModel>();
            var uid = id.ToGuid();
            if (!uid.IsEmpty())
            {
                var data = await this.vendorService.GetAsync(uid);
                ajaxResult.data = mapper.Map<VendorQueryViewModel>(data);
            }
            return Ok(ajaxResult);
        }
    }
}