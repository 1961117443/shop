using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class ModuleConfigs
    {
        [JsonProperty("masterTableConfigs")]
        public TableConfigs MasterTableConfigs { get; set; }
        [JsonProperty("detailTableConfigs")]
        public GridTableConfigs DetailTableConfigs { get; set; }
        [JsonProperty("toolbars")]
        public IList<Toolbar> Toolbars { get; set; }
    }
}
