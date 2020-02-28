using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.EntityModel
{
    public abstract class BaseEntity
    { 
        public int RowNo { get; set; }
    }
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class BaseEntity<T>: BaseEntity
    {
        public T ID { get; set; }
    }

    public abstract class BaseMasterEntity:BaseEntity<Guid>
    {
        
    }
    
    public abstract class BaseDetailEntity : BaseEntity<Guid>
    {
        public Guid MainID { get; set; }
    }

    public abstract class BaseMasterEntity<TDetail> : BaseMasterEntity
    {
        public virtual IList<TDetail> Details { get; set; }
    }
}
