using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.EntityModel;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    [Route("api/order")]
    [ApiController] 
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        private readonly ILogger<OrderController> logger;

        public OrderController(IOrderService orderService,IMapper mapper, ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetList(string type)
        {
            Expression<Func<SalesOrder, bool>> where = null;
            switch (type)
            {
                case "sj":
                    where = w => w.AuditDate != null && w.ApprovalDate == null;
                    break;
                case "sc":
                    where = w => w.AuditDate != null && w.ApprovalDate != null && w.ProductionEndDate == null && w.FinishDate == null;
                    break;
                case "fh":
                    where = w => w.AuditDate != null && w.ApprovalDate != null && w.FinishDate != null;
                    break;
                default:
                    break;
            }
            where = where.Or(w => w.AutoID < 0);
            var list = await this.orderService.GetPageListAsync(Page.Index, Page.Size, where);
            var data = this.mapper.Map<IList<OrderViewModel>>(list);
            return Ok(data);
        }

        [HttpGet("state/count")]
        public async Task<IActionResult> GetOrderState()
        {
            var state = new
            {
                sj = await this.orderService.Count(w => w.AuditDate != null && w.ApprovalDate == null),
                sc = await this.orderService.Count(w => w.AuditDate != null && w.ApprovalDate != null && w.ProductionEndDate == null && w.FinishDate == null),
                fh = await this.orderService.Count(w => w.AuditDate != null && w.ApprovalDate != null && w.FinishDate != null)
            };
            return Ok(state);
        }
    }
}