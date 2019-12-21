using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Shop.ViewModel
{
    public class OrderViewModel : BaseViewModel
    {
        [JsonProperty("uuid")]
        public string ID { get; set; }
        [JsonProperty("code")]
        public string BillCode { get; set; }
        public decimal Money { get; set; }
        public int Number { get; set; }
        public decimal Weight { get; set; }

        [JsonProperty("approval_at")]
        public string ApprovalDate { get; set; }
        [JsonProperty("create_at")]
        public string MakeDate { get; set; }
        [JsonProperty("production_at")]
        public string ProductionEndDate { get; set; }
        [JsonProperty("finish_at")]
        public string FinishDate { get; set; }

        public IList<OrderDetailViewModel> Detail { get; set; }
    }

    public class OrderDetailViewModel:BaseViewModel
    { 
        [JsonProperty("id")]
        public long AutoID { get; set; }
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
        [JsonProperty("mz")]
        public object TheoryMeter { get; set; }
        [JsonProperty("dj")]
        public decimal? Price { get; set; }
        [JsonProperty("je")]
        public decimal? Money { get; set; }
    }
}
