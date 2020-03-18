using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    public abstract class BaseEntity : IBaseEntity
    { 
        public virtual int RowNo { get; set; }
    }
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class BaseEntity<T>: BaseEntity,IBaseEntity
    {
        public virtual T ID { get; set; }
    }

    public abstract class BaseMasterEntity:BaseEntity<Guid>,IBaseEntity<Guid>
    {
        /// <summary>
        /// 制单人
        /// </summary>
        public virtual string Maker { get; set; } = string.Empty;
        /// <summary>
        /// 制单时间
        /// </summary>
        public virtual DateTime? MakeDate { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual string Audit { get; set; } = string.Empty;
        /// <summary>
        /// 审核时间
        /// </summary>

        public virtual DateTime? AuditDate { get; set; }


    }
    
    public abstract class BaseDetailEntity : BaseEntity<Guid>, IBaseEntity<Guid>
    {
        public virtual Guid MainID { get; set; }
    }

    public abstract class BaseMasterEntity<TDetail> : BaseMasterEntity, IBaseMasterEntity
    {
        [FreeSql.DataAnnotations.Column(IsIgnore = true)]
        public virtual IList<TDetail> Details { get; set; }
    }
}
