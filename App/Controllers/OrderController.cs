using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Shop.EntityModel;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    [Route("api/orders")]
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
        [HttpGet]
        public async Task<IActionResult> GetList(string type)
        {
            Expression<Func<SalesOrder, bool>> where = null;
            switch (type)
            {
                case "sj":
                    where = w => w.AuditDate != null && w.ApprovalDate == null && w.FinishDate != null;
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
                sj = await this.orderService.Count(w => w.AuditDate != null && w.ApprovalDate == null && w.FinishDate != null),
                sc = await this.orderService.Count(w => w.AuditDate != null && w.ApprovalDate != null && w.ProductionEndDate == null && w.FinishDate == null),
                fh = await this.orderService.Count(w => w.AuditDate != null && w.ApprovalDate != null && w.FinishDate != null)
            };
            return Ok(state);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetOrderById(string id)
        { 
            var data = await this.orderService.GetEntityAsync(w => w.BillCode == id);
            return Ok(this.mapper.Map<OrderViewModel>(data));
        }

        /// <summary>
        /// 订单审价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/approval")]
        public async Task<IActionResult> ApprovalOrder(string id)
        {
            var order = await this.orderService.GetAsync(w => w.ID.Equals(id));
            if (order==null || order.ApprovalDate !=null)
            {
                return NotFound();
            }

            bool res = await this.orderService.UpdateAsync(order, o => new SalesOrder() { ApprovalDate = DateTime.Now, Approval = "admin" });

            if (res)
            {
                return Ok(new { message = "操作成功！" });
            }
            return NotFound();
        }
    }
}