using Newtonsoft.Json;
using Shop.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel.Common
{
    public class QueryField
    {
        /// <summary>
        /// 字段名称，一般从viewmodel取值
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }
        /// <summary>
        /// 查询字段的名称
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 该字段拥有的查询条件
        /// </summary>
        [JsonProperty("logics")]
        public LogicEnum Logics { get; set; }
        /// <summary>
        /// 该字段的类型，主要区分日期是字符串
        /// </summary>
        [JsonProperty("fieldType")]
        public string FieldType { get; set; }
    }
}
