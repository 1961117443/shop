using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.IService;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDataTranService dataTranService;

        public AdminController(IDataTranService dataTranService)
        {
            this.dataTranService = dataTranService;
        }
        [HttpGet("tran_db")]
        public async Task<IActionResult> TranDb()
        {
            var res= await dataTranService.Tran();
            return Ok(res);
        }

        /// <summary>
        /// 获取更新架构的语句
        /// </summary>
        /// <returns></returns>
        [HttpGet("schema_sql")]
        public async Task<IActionResult> GetSchemaSql()
        {
            var sql= await dataTranService.StructureSQL();
            return Content(sql);
        }
    }
}