using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Common.Extensions;
using Shop.ViewModel.Common;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected Page Page
        {
            get
            {
                Page page = new Page()
                {
                    Index = 1,
                    Size = 20
                };
                foreach (var item in new string[] { "pageIndex", "Page" })
                {
                    if (Request.Query.ContainsKey(item))
                    {
                        page.Index = int.Parse(Request.Query[item]);
                        break;
                    }
                }
                foreach (var item in new string[] { "pageSize", "limit" })
                {
                    if (Request.Query.ContainsKey(item))
                    {
                        page.Size = int.Parse(Request.Query[item]);
                        break;
                    }
                }
                return page;
            }
        }

        private string _ipAddress;
        protected string IpAddress
        {
            get
            {
                if (_ipAddress.IsEmpty())
                {
                    _ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                }
                return _ipAddress;
            }
        }

        protected CSRedis.CSRedisClient Redis
        {
            get
            {
                return RedisHelper.Instance;
            }
        }

        
    }

    /// <summary>
    /// 单据操作接口
    /// </summary>
    public interface IBillActionController
    {
        /// <summary>
        /// 获取单条主表记录
        /// </summary>
        /// <param name="id">主表id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        Task<IActionResult> Get(string id);
        /// <summary>
        /// 获取主表列表 默认取前20条
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        Task<IActionResult> GetList();
        /// <summary>
        /// 获取从表信息
        /// </summary>
        /// <param name="id">主表的id</param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        Task<IActionResult> GetDetail(string id);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        Task<IActionResult> Post(JObject data);
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        [HttpPost("delete/{id}")]
        Task<IActionResult> Delete(string id);
        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("audit/{id}")]
        Task<IActionResult> Audit(string id);
        /// <summary>
        /// 反审单据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("unaudit/{id}")]
        Task<IActionResult> UnAudit(string id);
    }
}