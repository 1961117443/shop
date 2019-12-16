using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shop.ViewModel
{
    public class PackingViewModel
    { 
        [JsonProperty("id")]
        public int id { get; set; }
        public string code { get; set; }
        [JsonProperty("text")]
        public string name { get; set; }
    }
}
