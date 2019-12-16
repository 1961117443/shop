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
    [Route("api/packing")]
    [ApiController]
    public class PackingController : ControllerBase
    {
        private readonly IPackingService packingService;
        private readonly IMapper mapper;

        public PackingController(IPackingService packingService,IMapper mapper)
        {
            this.packingService = packingService;
            this.mapper = mapper;
        }
        // GET: api/packing
        [HttpGet("category")]
        public async Task<IEnumerable<PackingViewModel>> GetCategory()
        {
            var list = await this.packingService.GetListAsync();
            return this.mapper.Map<IEnumerable<PackingViewModel>>(list);
        }
    }
}