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
using Shop.Common.Utils;
using Shop.EntityModel;
using Shop.IService;
using Shop.ViewModel;
using Shop.ViewModel.Common;

namespace App.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        private readonly ILogger<OrderController> logger;

        public OrderController(IOrderService orderService, IMapper mapper, ILogger<OrderController> logger)
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
                case "qb":
                    where = w => w.AutoID > 0;
                    break;
                default:
                    break;
            }
            where = where.Or(w => w.AutoID < 0);
            var list = await this.orderService.GetPageListAsync(Page.Index, Page.Size, where);
            var data = this.mapper.Map<IList<OrderViewModel>>(list);
            return Ok(data);
        }

        [HttpGet("state_count")]
        public async Task<IActionResult> GetOrderState()
        {
            //下单（ApprovalDate == null）->审价（w.ApprovalDate != null）->审核（w.AuditDate != null）->生产（w.ProductionEndDate != null）->发货->完结（w.ProductionEndDate != null）
            Expression<Func<SalesOrder, bool>> where = w => w.FinishDate == null && w.CloseDate == null; 
            int sj = await this.orderService.Count(where.And(w => w.ApprovalDate == null && w.AuditDate == null));
            int fk = await this.orderService.Count(where.And(w => w.ApprovalDate != null && w.AuditDate == null));
            int sc = await this.orderService.Count(where.And(w => w.AuditDate != null && w.ProductionEndDate == null));
            int fh = await this.orderService.Count(where.And(w => w.ProductionEndDate != null));
            Dictionary<string, int> state = new Dictionary<string, int>(); 
            if (sj>0)
            {
                state.Add("sj", sj);
            }
            if (fk > 0)
            {
                state.Add("fk", fk);
            }
            if (sc > 0)
            {
                state.Add("sc", sc);
            }
            if (fh > 0)
            {
                state.Add("fh", fh);
            }
            return Ok(state);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        { 
            var data = await Redis.CacheShellAsync(id, 10, () => this.orderService.GetEntityAsync(w => w.BillCode == id));

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
            if (order == null || order.ApprovalDate != null)
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

        /// <summary>
        /// 订单关闭
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/close")]
        public async Task<IActionResult> CloseOrder(string id)
        {
            var order = await this.orderService.GetAsync(w => w.ID.Equals(id));
            if (order == null)
            {
                return NotFound();
            }

            MessageResultModel<OrderViewModel> resultModel = new MessageResultModel<OrderViewModel>();
            if (order.CloseDate.HasValue)
            {
                resultModel.message = "订单已关闭";
            }
            else
            {
                bool res = await this.orderService
                    .UpdateAsync(order,
                    o => new SalesOrder() { CloseDate = DateTime.Now, CloseUser = "admin" },
                    w => w.CloseDate == null);
                 
                if (res)
                {
                    order.CloseDate = DateTime.Now;
                    order.CloseUser = "admin";
                    resultModel.success = true;
                    resultModel.data = this.mapper.Map<OrderViewModel>(order);
                }
                else
                {
                    resultModel.message = "操作失败，请稍后再试！";
                }
            }
            return Ok(resultModel);
        }
    }
}