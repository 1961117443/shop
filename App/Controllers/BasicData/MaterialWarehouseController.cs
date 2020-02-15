using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Common.Extensions;
using Shop.IService.MaterialServices;
using Shop.ViewModel;

namespace App.Controllers.BasicData
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialWarehouseController : BaseController
    {
        private readonly IMaterialWarehouseService warehouseService;
        private readonly IMapper mapper;

        public MaterialWarehouseController(IMaterialWarehouseService warehouseService,IMapper mapper)
        {
            this.warehouseService = warehouseService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await this.warehouseService.GetListAsync();
            AjaxResultModelList<MaterialWarehouseQueryViewModel> ajaxResult = new AjaxResultModelList<MaterialWarehouseQueryViewModel>();  
            ajaxResult.data= mapper.Map<IList<MaterialWarehouseQueryViewModel>>(data);
            return Ok(ajaxResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AjaxResultModel<MaterialWarehouseQueryViewModel> ajaxResult = new AjaxResultModel<MaterialWarehouseQueryViewModel>();
            var uid = id.ToGuid();
            if (!uid.IsEmpty())
            {
                var data = await this.warehouseService.GetAsync(w => w.ID.Equals(uid));
                ajaxResult.data = mapper.Map<MaterialWarehouseQueryViewModel>(data);
            }
            return Ok(ajaxResult);
        }
    }
}