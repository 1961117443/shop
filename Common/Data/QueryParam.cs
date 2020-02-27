using AutoMapper;
using Newtonsoft.Json;
using Shop.Common.Data;
using Shop.Common.Extensions;
using Shop.Common.IData;
using Shop.Common.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace Shop.Common.Data
{
    /// <summary>
    /// 查询参数
    /// </summary>
    [DebuggerDisplay("Field={Field},Value={Value}")]
    public class QueryParam
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

        ///// <summary>
        ///// 数据库的字段 外键字段用 . 表示 例如材料库存关联货品档案 Product.ProductCode
        ///// </summary>
        //public string EntityField { get; set; }
        public QueryParam()
        {
            //GroupIndex = 0;
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
        [Obsolete]
        public virtual Expression<Func<TEntity, bool>> ToExpression<TEntity, TView>()
        {
            //Expression<Func<TEntity, bool>> where = null;
            //if (this.mapper!=null)
            //{
            //var field = this.mapper.GetEntityField<TEntity, TView>(this.Field);
            //    if (!field.IsEmpty())
            //    {
            //        where = EntityHelper<TEntity>.ToExpression(field, this.Value, this.Logic);
            //    }
            //}
            return EntityHelper<TEntity>.ToExpression(this.Field, this.Value, this.Logic);
        }
    }


    public class QueryParam<TEntity,TView> : QueryParam, IQueryParam<TEntity>
    {
        public virtual Expression<Func<TEntity, bool>> ToExpression()
        {
            Expression<Func<TEntity, bool>> where = EntityHelper<TEntity>.ToExpression(Field, Value, Logic);

            return where;
        } 
    }



}
