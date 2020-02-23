using Newtonsoft.Json;
using Shop.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class Toolbar
    {
        /// <summary>
        /// 按钮id
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// js方法
        /// </summary>
        [JsonProperty("command")]
        public string Command { get; set; }
        /// <summary>
        /// 可用状态
        /// </summary>
        [JsonProperty("status")]
        public DataState Status { get; set; }

        [JsonProperty("buttonType")]
        public string ButtonType { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
