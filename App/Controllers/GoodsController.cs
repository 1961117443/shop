using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    /// <summary>
    /// 商品相关api
    /// </summary>
    [Route("api/goods")]
    [ApiController]
    public class GoodsController : BaseController
    {
        private readonly IGoodsService goodsService;
        private readonly IMapper mapper;
        private readonly ISectionBarService sectionBarService;

        public GoodsController(IGoodsService goodsService,IMapper mapper, ISectionBarService sectionBarService)
        {
            this.goodsService = goodsService;
            this.mapper = mapper;
            this.sectionBarService = sectionBarService;
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetCateList(string id)
        {  
            Guid Id = Guid.Empty;
            Guid.TryParse(id, out Id);

            var key = $"{nameof(GoodsController)}:GetCateList:{Id}";

            var list = await Redis.CacheShellAsync(key, RedisHelperExtensins.DAY_SECONDS, () => goodsService.GetCateoryListAsync(Id)); 

            var data = this.mapper.Map<IList<SelectItem>>(list);
            return Ok(data);
        }

        // GET: api/goods
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            Expression<Func<SectionBar, bool>> where = w => !w.IsStop.Value;
            var list = await this.sectionBarService.GetPageListAsync(Page.Index, Page.Size, where);
            var data = this.mapper.Map<List<GoodsViewModel>>(list);
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Expression<Func<SectionBar, bool>> where = w => !w.IsStop.Value && w.ID.Equals(id);
            var goods = await this.sectionBarService.GetAsync(where);
            var data = this.mapper.Map<GoodsViewModel>(goods);
            return Ok(data);
        }
    }
}