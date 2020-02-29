using System;
using System.Collections.Generic;

namespace Shop.EntityModel
{
    public interface IBaseEntity
    {
        int RowNo { get; set; }
    }

    public interface IBaseEntity<T> : IBaseEntity
    {
        T ID { get; set; }
    }

    public interface IBaseMasterEntity : IBaseEntity<Guid>
    {

    }

    public interface IBaseDetailEntity : IBaseEntity<Guid>
    {
        Guid MainID { get; set; }
    }

    public interface IBaseMasterEntity<TDetail> : IBaseMasterEntity
    {
        IList<TDetail> Details { get; set; }
    }
}