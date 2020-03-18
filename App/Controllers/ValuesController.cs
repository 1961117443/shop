using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.Common;
using Shop.Common.Data;
using Shop.Common.Extensions;
using Shop.EntityModel;
using Shop.IService.MaterialServices;
using Shop.ViewModel;
using Shop.ViewModel.Common;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        private readonly IUser _user;
        private readonly IMaterialStockService stockService;
        private readonly IMapper mapper;

        public ValuesController(IUser user, IMaterialStockService stockService, IMapper mapper)
        {
            this._user = user;
            this.stockService = stockService;
            this.mapper = mapper;
        }
        // GET api/values
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {
            var res = await HttpContext.AuthenticateAsync();
            var token = await HttpContext.GetTokenAsync("access_token");
            var user = HttpContext.User;
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"key:{claim.Type}--value:{claim.Value}");
            }
            //var token = res.Properties.GetString("id_token");
            foreach (var prop in res.Properties.Items)
            {
                Console.WriteLine($"key:{prop.Key}--value:{prop.Value}");

            }
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok("value");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("test/tree")]
        [AllowAnonymous]
        public IActionResult TestQueryParamTree(QueryParamTree tree)
        {
            {
                if (tree != null)
                {
                    //tree.SetMapper(this.mapper);
                    var exp0 = tree.ToExpression<MaterialStock, MaterialStockViewModel>();
                    var res0 = this.stockService.GetPageList(this.Page.Index, Page.Size, out int total0, exp0, w => w.ID);
                    LoopQuery1(tree);
                    Expression<Func<MaterialStock, bool>> exp = null;
                    exp = LoopQuery(tree);
                    //var exp= LoopQuery(tree, where);
                    var res = this.stockService.GetPageList(this.Page.Index, Page.Size, out int total1, exp, w => w.ID);
                }

            }
            {

            }
            {
                Expression<Func<MaterialStock, bool>> where = w =>
                    ((w.Product.ProductCode.Contains("1") || w.Product.ProductName.Contains("1"))
                 && (w.Product.ProductCode.Contains("2") || w.Product.ProductName.Contains("2")))
                 || ((w.Product.ProductCode.Contains("3") || w.Product.ProductName.Contains("3")) && w.MaterialWarehouse.Name == "材料A");
                var res = this.stockService.GetPageList(this.Page.Index, Page.Size, out int total, where, w => w.ID);
            }


            AjaxResultPageModel<MaterialStockViewModel> ajaxResult = new AjaxResultPageModel<MaterialStockViewModel>();
            {
                Expression<Func<MaterialStock, bool>> where = w => w.Quantity != 0;
                var condition = Request.Query["condition"].ToString();
                if (!condition.IsEmpty())
                {
                    var jObject = JObject.Parse(condition);
                    var exp = mapper.ToCriteriaExpression<MaterialStock, MaterialStockViewModel>(jObject);
                    where = where.And(exp);
                }
                var data = this.stockService.GetPageList(this.Page.Index, Page.Size, out int total, where, w => w.ID);
                ajaxResult.Data.total = total;
                ajaxResult.Data.data = mapper.MapList<MaterialStockViewModel>(data);
            }
            return Ok(ajaxResult); 
        }

        [HttpPost("test/dto")]
        [AllowAnonymous]
        public IActionResult TestQueryParamDto(QueryParamDto dto)
        {
            {
                if (dto != null)
                {
                    List<QueryParamDto> dtos = new List<QueryParamDto>();
                    Expression<Func<MaterialStock, bool>> where1 = null;
                    dtos.Add(new QueryParamDto
                    {
                        Field = "ProductID_ProductCode",
                        Value = "a"
                    });

                    dtos.Add(new QueryParamDto
                    {
                        Field = "ProductID_ProductCode",
                        Value = "b"
                    }); 
                    var p = this.mapper.MapParamList<MaterialStock, MaterialStockViewModel>(dtos);
                    where1= p.QueryParamToExpression();
                    //}
                    var param = this.mapper.MapParam<MaterialStock, MaterialStockViewModel>(dto);
                    //tree.SetMapper(this.mapper);
                    //var exp0 = tree.ToExpression<MaterialStock, MaterialStockViewModel>();
                    //var res0 = this.stockService.GetPageList(this.Page.Index, Page.Size, out int total0, exp0, w => w.ID);
                    //LoopQuery1(tree);
                    //Expression<Func<MaterialStock, bool>> exp = null;
                    //exp = LoopQuery(tree);
                    ////var exp= LoopQuery(tree, where);
                    var res = this.stockService.GetPageList(this.Page.Index, Page.Size, out int total1, param.ToExpression(), w => w.ID);
                }

            } 


            AjaxResultPageModel<MaterialStockViewModel> ajaxResult = new AjaxResultPageModel<MaterialStockViewModel>();
            {
                Expression<Func<MaterialStock, bool>> where = w => w.Quantity != 0;
                var condition = Request.Query["condition"].ToString();
                if (!condition.IsEmpty())
                {
                    var jObject = JObject.Parse(condition);
                    var exp = mapper.ToCriteriaExpression<MaterialStock, MaterialStockViewModel>(jObject);
                    where = where.And(exp);
                }
                var data = this.stockService.GetPageList(this.Page.Index, Page.Size, out int total, where, w => w.ID);
                ajaxResult.Data.total = total;
                ajaxResult.Data.data = mapper.MapList<MaterialStockViewModel>(data);
            }
            return Ok(ajaxResult);
        }

        private void LoopQuery1(QueryParamTree tree)
        {
            List<Expression<Func<MaterialStock, bool>>> whereList = new List<Expression<Func<MaterialStock, bool>>>();

            Action<QueryParamTree> action = null;
            action = parent =>
            {
                if (parent.HasChildren)
                {
                    foreach (var children in parent.Children)
                    {
                        action(children);
                    }
                }
                var exp = parent.ToExpression<MaterialStock, MaterialStockViewModel>();
                if (exp!=null)
                {
                    whereList.Add(exp);
                }
            };

            action(tree);

        }

        private Expression<Func<MaterialStock, bool>> LoopQuery(QueryParamTree tree)
        {
            Expression<Func<MaterialStock, bool>> where = null;
            if (tree != null && tree.HasChildren)
            {
                foreach (var item in tree.Children)
                {
                    var exp1 = LoopQuery(item);
                    where = item.JoinExpression(where, exp1);
                    //return where;
                    //where = item.Join.JoinExpression(where, LoopQuery(item));
                    //item.EntityField = this.mapper.GetEntityField<MaterialStock, MaterialStockViewModel>(item.Field);
                    //where = item.Join.JoinExpression(where, item.QueryParamToExpression<MaterialStock>());
                }
            }
            var field = this.mapper.GetEntityField<MaterialStock, MaterialStockViewModel>(tree.Field);
            if (!field.IsEmpty())
            {
                var exp3 = new QueryParam(field, tree.Value, tree.Logic).QueryParamToExpression<MaterialStock>();
            }
            
            var exp2 = tree.QueryParamToExpression<MaterialStock>();
            if (tree.HasChildren)
            {
                return exp2.And(where);
            }
            return exp2;
        }
    }
}
