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

namespace Shop.EntityModel {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class Packing {

		[JsonProperty, Column(IsIdentity = true)]
		public long AutoID { get; set; }

		[JsonProperty, Column(DbType = "varchar(50)")]
		public string BillType { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "date")]
		public DateTime? CheckDate { get; set; }

		[JsonProperty, Column(DbType = "varchar(50)")]
		public string CheckOper { get; set; } = string.Empty;

		[JsonProperty]
		public DateTime? CloseDate { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(50)")]
		public string CloseUser { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "date")]
		public DateTime? CreateDate { get; set; }

		[JsonProperty, Column(DbType = "varchar(50)")]
		public string CreateOper { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "decimal(38,6)")]
		public decimal? Decimal { get; set; }

		[JsonProperty]
		public Guid ID { get; set; }

		[JsonProperty]
		public bool? IsStop { get; set; }

		[JsonProperty]
		public DateTime? MakeDate { get; set; }

		[JsonProperty, Column(DbType = "varchar(30)")]
		public string Maker { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "decimal(38,3)")]
		public decimal? MaxPaperWeight { get; set; }

		[JsonProperty, Column(DbType = "varchar(30)")]
		public string Mender { get; set; } = string.Empty;

		[JsonProperty]
		public DateTime? ModifyDate { get; set; }

		[JsonProperty, Column(DbType = "varchar(100)")]
		public string PackageName { get; set; } = string.Empty;

		[JsonProperty]
		public Guid? PackingCategoryID { get; set; }

		[JsonProperty, Column(DbType = "varchar(100)")]
		public string PackingCode { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "varchar(100)")]
		public string PackingName { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "varchar(500)")]
		public string Remark { get; set; } = string.Empty;

		[JsonProperty]
		public int RowNo { get; set; }

		[JsonProperty, Column(Name = "StandardPaperWeight", DbType = "decimal(38,3)")]
		public decimal? StandardPaperWeight_ { get; set; }

	}

}
