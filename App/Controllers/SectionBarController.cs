using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.EntityModel;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    [Route("api/SectionBar")]
    [ApiController]
    public class SectionBarController : BaseController
    {
        private readonly ISectionBarService sectionBarService;
        private readonly IMapper mapper;

        public SectionBarController(ISectionBarService sectionBarService,IMapper mapper)
        {
            this.sectionBarService = sectionBarService;
            this.mapper = mapper;
        }
       
    }
}
