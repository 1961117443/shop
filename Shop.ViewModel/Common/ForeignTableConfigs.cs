using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.ViewModel
{
    public class TableConfigs
    {
        /// <summary>
        /// 外键字段所在的表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 获取数据列表的api地址
        /// </summary>
        public string Url { get; set; }
    }

    public class GridTableConfigs: TableConfigs
    {
        /// <summary>
        /// 列
        /// </summary>
        public List<ElementTableColumn> Columns { get; set; }
    }
    public class ForeignTableConfigs: GridTableConfigs
    {
        ///// <summary>
        ///// 外键字段所在的表名
        ///// </summary>
        //public string TableName { get; set; }
        /// <summary>
        /// 外键字段名
        /// </summary>
        public string ForeignKey { get; set; }
        /// <summary>
        /// 外键表的主键
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 获取数据列表的api地址
        /// </summary>
        //public string Url { get; set; }
        /// <summary>
        /// 获取单个实体的api地址
        /// </summary>
        public string ModelUrl { get; set; }

        //public List<ElementTableColumn> Columns { get; set; }
    }
}
