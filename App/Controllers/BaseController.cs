using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
}