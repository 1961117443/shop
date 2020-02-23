using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.ViewModel.Enums;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalConfigsController : ControllerBase
    {
        [HttpGet("data-state")]
        public IActionResult GetGlobalDataState()
        {
            AjaxResultModel<Dictionary<string, int>> ajaxResult = new AjaxResultModel<Dictionary<string, int>>();
            ajaxResult.data = new Dictionary<string, int>();
            //foreach (var item in Enum.GetNames(typeof(DataState)))
            //{
                 
            //}
            foreach (var item in Enum.GetValues(typeof(DataState)))
            {
                ajaxResult.data.Add(item.ToString(), (int)item);
            }
            

            return Ok(ajaxResult);
               
        }
    }
}