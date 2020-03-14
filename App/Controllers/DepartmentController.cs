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
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService DepartmentService;
        private readonly IMapper mapper;

        public DepartmentController(IDepartmentService DepartmentService,IMapper mapper)
        {
            this.DepartmentService = DepartmentService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Expression<Func<Department, bool>> where = w => w.ID != Guid.Empty;
            if (Request.Query.ContainsKey("q"))
            {
                string val = Request.Query["q"];
                if (!val.IsEmpty())
                {
                    where = where.And(w => w.depcode.Contains(val) || w.depname.Contains(val));
                }
            }
            var list = await this.DepartmentService.GetPageListAsync(Page.Index, Page.Size, where);
            AjaxResultModelList<DepartmentQueryViewModel> ajaxResult = new AjaxResultModelList<DepartmentQueryViewModel>();
            ajaxResult.data = mapper.MapList<DepartmentQueryViewModel>(list);
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
            AjaxResultModel<DepartmentQueryViewModel> ajaxResult = new AjaxResultModel<DepartmentQueryViewModel>();
            var uid = id.ToGuid();
            if (!uid.IsEmpty())
            {
                var data = await this.DepartmentService.GetAsync(uid);
                ajaxResult.data = mapper.Map<DepartmentQueryViewModel>(data);
            }
            return Ok(ajaxResult);
        }
    }
}