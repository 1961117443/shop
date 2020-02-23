using Shop.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Data
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class QueryParam
    {
        /// <summary>
        /// 查询字段
        /// 字段全称:Demand.ClientFile.ClientName
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public LogicEnum Logic { get; set; }
        /// <summary>
        /// 查询值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 条件所属分组:
        /// (1=1 and 1=2) and (2=2 or 3=3) 
        /// </summary>
        public int GroupIndex { get; set; }

        public QueryParam()
        {
            GroupIndex = 0;
        }
        public QueryParam(string field, string value) : this(field, value, LogicEnum.Equal)
        {
        }
        public QueryParam(string field, string value, LogicEnum logic) : this()
        {
            Field = field;
            Value = value;
            Logic = logic;
        }

        //  public QueryField qField { get; set; }
    }

    
}
