using Shop.Common.Data;
using System;
using System.Linq.Expressions;

namespace Shop.Common.IData
{
    public interface IQueryParam<TEntity>
    {
        string Field { get; set; }
        JoinEnum Join { get; set; }
        LogicEnum Logic { get; set; }
        string Value { get; set; }

        Expression<Func<TEntity, bool>> ToExpression();
    }
}