using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Shop.ViewModel;

namespace App.Controllers
{
    [Route("api/history")]
    [ApiController]
    public class HistorySearchController : BaseController
    {
        private async Task<IList<SelectItem>> GetHistoryTagsCore(string key)
        {
            var list = await Redis.GetAsync<IList<SelectItem>>(key);
            if (list == null)
            {
                list = new List<SelectItem>();
                Redis.Set(key, list);
            }
            return list;
        }

        [HttpGet("his_tags")]
        public async Task<IActionResult> GetHistoryTags(string key)
        {
            return Ok(await GetHistoryTagsCore(key));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(JObject jObject)
        {
            var key = jObject.Value<string>("key");
            var value = jObject.Value<string>("value");
            IList<SelectItem> tags = await GetHistoryTagsCore(key);
            MessageResultModel<SelectItem> resultModel = new MessageResultModel<SelectItem>();
            var item = tags.FirstOrDefault(w => w.text == value);
            if (item == null)
            {
                item = new SelectItem()
                {
                    id = Guid.NewGuid().ToString(),
                    text = value
                };
                tags.Add(item);
                resultModel.success = await Redis.SetAsync(key, tags);
                resultModel.data = item;
            }
            return Ok(resultModel);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(JObject jObject)
        {
            var key = jObject.Value<string>("key");
            var id = jObject.Value<string>("id"); 
            MessageResultModel resultModel = new MessageResultModel();
            if (jObject.Value<bool>("all"))
            {
                var res = await Redis.DelAsync(key);
                resultModel.success = res > 0;
            }
            else
            {
                IList<SelectItem> tags = await GetHistoryTagsCore(key);
                var item = tags.FirstOrDefault(w => w.id == id);
                if (item != null)
                {
                    tags.Remove(item);
                    resultModel.success = await Redis.SetAsync(key, tags); 
                }
            }

            return Ok(resultModel);

        }
    }
}