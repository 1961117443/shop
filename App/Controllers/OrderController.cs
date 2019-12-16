using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public OrderController(IOrderService orderService,IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
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
                    where = w => w.AuditDate != null && w.ApprovalDate != null && w.ProductionEndDate != null;
                    break;
                case "fh":
                    where = w => w.AuditDate != null && w.ApprovalDate != null && w.FinishDate != null;
                    break;
                default:
                    break;
            } 

            var list = await this.orderService.GetPageListAsync(Page.Index, Page.Size, where);
            var data = this.mapper.Map<IList<OrderViewModel>>(list);
            return Ok(data);
        }
    }
}