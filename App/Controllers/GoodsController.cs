using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        private readonly IGoodsService goodsService;
        private readonly IMapper mapper;

        public GoodsController(IGoodsService goodsService,IMapper mapper)
        {
            this.goodsService = goodsService;
            this.mapper = mapper;
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetCateList(string id)
        {
            Guid Id = Guid.Empty;
            Guid.TryParse(id, out Id);
            var list = await this.goodsService.GetCateoryListAsync(Id);
            var data = this.mapper.Map<IList<SelectItem>>(list);
            return Ok(data);
        }
    }
}