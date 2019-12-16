using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.EntityModel;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionBarController : ControllerBase
    {
        private readonly ISectionBarService sectionBarService;

        public SectionBarController(ISectionBarService sectionBarService)
        {
            this.sectionBarService = sectionBarService;
        }
        // GET: api/SectionBar
        [HttpGet]
        public async Task<IEnumerable<SectionBar>> Get()
        {
            var list = await this.sectionBarService.GetList();
            return list;
        }
    }
}
