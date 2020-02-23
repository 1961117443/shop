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
	public partial class MaterialUseOutStore {

		[JsonProperty]
		public DateTime? AccountDate { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(50)")]
		public string Accounter { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "nvarchar(30)")]
		public string Audit { get; set; } = string.Empty;

		[JsonProperty]
		public DateTime? AuditDate { get; set; }

		[JsonProperty, Column(IsIdentity = true)]
		public long AutoID { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(30)")]
		public string BillCode { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "nvarchar(50)")]
		public string BillType { get; set; } = string.Empty;

		[JsonProperty]
		public DateTime? CloseDate { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(50)")]
		public string CloseUser { get; set; } = string.Empty;

		[JsonProperty]
		public DateTime? FirstAuditDate { get; set; }

		[JsonProperty]
		public Guid ID { get; set; }

		[JsonProperty]
		public bool? IfTranToK3 { get; set; }

		[JsonProperty]
		public bool? IsStop { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(30)")]
		public string LeadingPerson { get; set; } = string.Empty;

		[JsonProperty]
		public DateTime? MakeDate { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(30)")]
		public string Maker { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "nvarchar(50)")]
		public string ManualNumber { get; set; } = string.Empty;

		[JsonProperty]
		public Guid? MaterialDepnameID { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(30)")]
		public string MaterialUseTeam { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "nvarchar(30)")]
		public string MaterialUseTeamHead { get; set; } = string.Empty;

		[JsonProperty, Column(DbType = "nvarchar(30)")]
		public string Mender { get; set; } = string.Empty;

		[JsonProperty]
		public DateTime? ModifyDate { get; set; }

		[JsonProperty, Column(DbType = "date")]
		public DateTime? OutStoreDate { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(500)")]
		public string Remark { get; set; } = string.Empty;

		[JsonProperty]
		public DateTime? ResetDateK3 { get; set; }

		[JsonProperty, Column(DbType = "nvarchar(30)")]
		public string ResetOperK3 { get; set; } = string.Empty;

		[JsonProperty]
		public int RowNo { get; set; }

		[JsonProperty]
		public int? States { get; set; }

	}

}
