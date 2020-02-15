using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Common;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUser _user;

        public ValuesController(IUser user)
        {
            this._user = user;
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
    }
}
