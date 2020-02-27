using Newtonsoft.Json;
using Shop.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel.Common
{
    public class QueryParamDto
    {
        /// <summary>
        /// 查询字段
        /// 字段全称:Demand.ClientFile.ClientName
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }
        /// <summary>
        /// 查询值
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        [JsonProperty("logic")]
        public LogicEnum Logic { get; set; }

        /// <summary>
        /// 连接条件 and 还是 or
        /// </summary>
        [JsonProperty("join")]
        public JoinEnum Join { get; set; }
    }
}
