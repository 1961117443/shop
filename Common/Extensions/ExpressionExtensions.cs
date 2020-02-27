using Shop.Common.Data;
using Shop.Common.Extensions;
using Shop.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Shop.Common.Extensions
{
    public static class ExpressionExtensions
    { 
        /// <summary>
        /// 把查询参数转换成表达式目录树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> QueryParamToExpression<T>(this QueryParam queryParam)
        {
            return EntityHelper<T>.ToExpression(queryParam.Field, queryParam.Value, queryParam.Logic);
        }

        /// <summary>
        /// 把PageParam转成表达式目录树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> QueryParamToExpression<T>(this IEnumerable<QueryParam> list)
        {
            Expression<Func<T, bool>> where = null;
            if (list!=null && list.Count()>0)
            {
                foreach (var p in list)
                {
                    var exp = p.QueryParamToExpression<T>();
                    if (exp != null)
                    {
                        //where = LambadaExpressionExtensions.And(where, exp);
                        where = where.And(exp);
                    }
                }
            }
            return where;
        }


        public static Expression<Func<TEntity, bool>> QueryParamToExpression<TEntity,TView>(this IEnumerable<QueryParam<TEntity,TView>> list)
        {
            Expression<Func<TEntity, bool>> where = null;
            if (list != null && list.Count() > 0)
            {
                foreach (var p in list)
                {
                    var exp = p.ToExpression();
                    if (exp!=null)
                    {
                        where = p.JoinExpression(where, p.ToExpression());
                    }
                }
            }
            return where;
        }

    }
}
