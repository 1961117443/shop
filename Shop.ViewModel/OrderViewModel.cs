using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Shop.ViewModel
{
    public class OrderViewModel
    {
        [JsonProperty("code")]
        public string BillCode { get; set; }
        public decimal Money { get; set; }
        public int Number { get; set; }
        public decimal Weight { get; set; }

        public IList<OrderDetailViewModel> Detail { get; set; }
    }

    public class OrderDetailViewModel
    {
        [JsonProperty("xh")]
        public string SectionbarCode { get; set; }
        [JsonProperty("bm")]
        public string SurfaceName { get; set; }
        [JsonProperty("cz")]
        public string TextureName { get; set; }
        [JsonProperty("bz")]
        public string PackingName { get; set; }
        [JsonProperty("bh")]
        public string WallThickness { get; set; }
        [JsonProperty("cd")]
        public decimal OrderLength { get; set; }
        [JsonProperty("sl")]
        public int TotalQuantity { get; set; }
        [JsonProperty("zl")]
        public decimal Weight { get; set; }
        [JsonProperty("img")]
        public string ImageUrl { get; set; }
    }
}
