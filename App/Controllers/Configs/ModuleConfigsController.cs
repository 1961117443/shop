using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleConfigsController : ControllerBase
    {
        public ModuleConfigsController(IModuleService moduleService)
        {
            this.moduleService = moduleService;
        }
        
        private readonly IModuleService moduleService;

        [HttpGet("{tableName}/{fieldName}")]
        public async Task<IActionResult> GetModuleForeignTable(string tableName, string fieldName)
        {           

            AjaxResultModel<ForeignTableConfigs> ajaxResult = new AjaxResultModel<ForeignTableConfigs>();
            var configs = await this.moduleService.GetModuleForeignTableAsync(tableName, fieldName);
            if (configs != null)
            {
                ajaxResult.data = configs;
            }

            return Ok(ajaxResult);
        }

        [HttpGet("{tableName}")]
        public async Task<IActionResult> GetModuleConfigs(string tableName)
        {
            var configs= await this.moduleService.GetModuleConfigsAsync(tableName);
            AjaxResultModel<ModuleConfigs> ajaxResult = new AjaxResultModel<ModuleConfigs>();
            ajaxResult.data = configs;
            return Ok(ajaxResult);
        }
    }
}