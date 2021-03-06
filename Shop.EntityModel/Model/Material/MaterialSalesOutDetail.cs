//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//     Website: http://www.freesql.net
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace Shop.EntityModel
{

	[JsonObject(MemberSerialization.OptIn)]
	public partial class MaterialSalesOutDetail {

		[JsonProperty, Column(DbType = "decimal(38,2)")]
		public decimal? Amount { get; set; }

		[JsonProperty, Column(IsIdentity = true)]
		public long AutoID { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(30)")]
		public string BarCode { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "nvarchar(50)")]
		public string BatNo { get; set; } = string.Empty;

		[JsonProperty]
		public DateTime? CloseDate { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(50)")]
		public string CloseUser { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "decimal(38,2)")]
		public decimal? CostAmount { get; set; }

		[JsonProperty, Column(DbType = "decimal(38,4)")]
		public decimal? CostPrice { get; set; }

        [JsonProperty]
        public override Guid ID { get; set; }

        [JsonProperty]
		public Guid? InID { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(500)")]
		public string ItRemark { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "decimal(38,4)")]
		public decimal? LastUnitPrice { get; set; }

		[JsonProperty]
		public override Guid MainID { get; set; }

		[JsonProperty]
		public Guid? MaterialWareHouseID { get; set; }

		[JsonProperty]
		public Guid? MaterialWarehouseShelfNumberID { get; set; }

		[JsonProperty]
		public bool? MoneySync { get; set; }

		[JsonProperty, Column(DbType = "decimal(38,4)")]
		public decimal? Price { get; set; }

		[JsonProperty]
		public Guid? ProductID { get; set; }

		[JsonProperty]
		public override int RowNo { get; set; }

		[JsonProperty, Column(DbType = "decimal(38,3)")]
		public decimal? TotalQuantity { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(50)")]
		public string Unit { get; set; } = string.Empty;

	}

}
