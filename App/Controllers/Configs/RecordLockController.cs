using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Common;
using Shop.Entity;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    /// <summary>
    /// 记录锁定控制
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RecordLockController : BaseController
    {
        private readonly IRecordLockService recordLockService;
        private readonly IUser user;

        public RecordLockController(IRecordLockService recordLockService,IUser user)
        {
            this.recordLockService = recordLockService;
            this.user = user;
        }

        /// <summary>
        /// 记录加锁
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Lock/{tableName}/{id}")]
        public async Task<IActionResult> Lock(string tableName,string id)
        {
            RecordLockViewModel lockViewModel = new RecordLockViewModel()
            {
                TableName = tableName,
                KeyId = id,
                UserId = this.user.Id,
                UserName = user.Name,
                LockAt  = DateTime.Now.Ticks,
                IP = base.IpAddress
            };
            var data= await this.recordLockService.Lock(lockViewModel);
            AjaxResultModel<RecordLockViewModel> ajaxResultModel = new AjaxResultModel<RecordLockViewModel>(data);
            return Ok(ajaxResultModel);
        }

        /// <summary>
        /// 记录解锁
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("UnLock/{tableName}/{id}")]
        public async Task<IActionResult> UnLock(string tableName, string id)
        {
            var data = await this.recordLockService.UnLock(tableName,id);
            AjaxResultModel<RecordLockViewModel> ajaxResultModel = new AjaxResultModel<RecordLockViewModel>(data);
            return Ok(ajaxResultModel);
        }

    }
}