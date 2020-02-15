using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class UserViewModel
    {
        //[JsonProperty("id")]
        //public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("introduction")]
        public string Introduction { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("roles")]
        public string[] Roles { get; set; }
    }
}
