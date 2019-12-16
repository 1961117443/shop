using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                if (Request.Query.ContainsKey("pageIndex"))
                {
                    page.Index = int.Parse(Request.Query["pageIndex"]);
                }
                if (Request.Query.ContainsKey("pageSize"))
                {
                    page.Size = int.Parse(Request.Query["pageSize"]);
                }
                return page;
            }
        }
    }
}