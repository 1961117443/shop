using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class GoodsViewModel
    { 
        public string uuid { get; set; }
        [JsonProperty("img_url")]
        public string imgUrl { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
    }
}
