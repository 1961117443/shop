using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Common.Data;
using Shop.Common.Utils;
using Shop.IService.MetaServices;
using Shop.ViewModel.Common;
using Shop.ViewModel.Enums;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalConfigsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IEntityService entityService;

        public GlobalConfigsController(IMapper mapper,IEntityService entityService)
        {
            this.mapper = mapper;
            this.entityService = entityService;
        }
        [HttpGet("data-state")]
        [AllowAnonymous]
        public IActionResult GetGlobalDataState()
        {
            AjaxResultModel<Dictionary<string, int>> ajaxResult = new AjaxResultModel<Dictionary<string, int>>
            {
                Data = new Dictionary<string, int>()
            };
            //foreach (var item in Enum.GetNames(typeof(DataState)))
            //{

            //}
            foreach (var item in Enum.GetValues(typeof(DataState)))
            {
                ajaxResult.Data.Add(item.ToString(), (int)item);
            }
            

            return Ok(ajaxResult);
               
        }

        [HttpGet("/api/entity/qf")]
        [AllowAnonymous]
        public IActionResult GetQueryFields(string view)
        {
            var list = entityService.GetQueryFields(view);
            return Ok(list);
        }
    }
}