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
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "a");
            Expression member = EntityHelper<T>.GetMemberExpression(parameterExpression, queryParam.Field);
            if (member == null)
            {
                return null;
            }
            //获取当前属性的类型
            Type memberType = member.Type;
            object constantValue = null;
            //当前传入值的类型（实际查询的类型）
            Type valueType = memberType.IsNullableType() ? memberType.GetGenericArguments().First() : memberType;

            //int类型转double查询
            if (valueType == typeof(int))
            {
                valueType = typeof(double);
            }
            //把查询的值转换为对应的值
            constantValue = DataExtensions.ChangeType(queryParam.Value, valueType);
            Expression constant = Expression.Constant(constantValue, valueType);
            //把参数类型转换一下
            constant = Expression.Convert(constant, memberType);
            Expression<Func<T, bool>> where = null;
            switch (queryParam.Logic)
            {
                //等于
                case LogicEnum.Equal:
                    return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameterExpression);
                //包含 右包含 左包含
                case LogicEnum.Like:
                case LogicEnum.NoLike:
                case LogicEnum.LikeLeft: //右包含
                case LogicEnum.LikeRight: //左包含
                    {
                        var method = queryParam.Logic.ToMethod();
                        Expression mehtodCallExpression = Expression.Call(member, method, constant);
                        //  mehtodCallExpression.Not()
                        where = Expression.Lambda<Func<T, bool>>(mehtodCallExpression, parameterExpression);
                        if (queryParam.Logic == LogicEnum.NoLike)
                        {
                            where = where.Not();
                        }
                        return where;
                    }
                //大于
                case LogicEnum.GreaterThan:
                    return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameterExpression);
                //大于等于
                case LogicEnum.GreaterThanOrEqual:
                    return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameterExpression);
                //少于
                case LogicEnum.LessThan:
                    return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameterExpression);
                //少于等于
                case LogicEnum.LessThanOrEqual:
                    return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameterExpression);
                // 
                case LogicEnum.In:
                    break;
                case LogicEnum.NotIn:
                    break;
                //不等于
                case LogicEnum.NoEqual:
                    return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameterExpression);
                case LogicEnum.IsNullOrEmpty:
                    break;
                case LogicEnum.IsNot:
                    break;
            }
            return null;
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
    }
}
