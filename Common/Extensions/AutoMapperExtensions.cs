using Shop.Common.Extensions;
using Shop.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AutoMapper
{
    public static class AutoMapperExtensions
    {
        #region IMapper
        public static IEnumerable<T> MapList<T>(this IMapper mapper, object source)
        {
            return mapper.Map<IEnumerable<T>>(source);
        }

        public static Expression<Func<TDestination, TDestination>> MapExpression<TSource, TDestination>(this IMapper mapper, TSource source, TDestination destination)
        {
            //mapper.Map
            Expression<Func<TDestination, TDestination>> expression = null;
            // 复制destination映射到新的destinationClone
            var destinationClone = destination.Clone();
            // source 映射到 destination
            mapper.Map(source, destination);
            if (destinationClone.Equals(destination))
            {
                Console.WriteLine(true);
            }

            List<MemberBinding> bindings = new List<MemberBinding>();
            foreach (var prop in typeof(TDestination).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                // 虚属性，跳过
                if (prop.GetMethod.IsVirtual)
                {
                    continue;
                }
                object newValue = prop.GetValue(destination, null);
                object oldValue = prop.GetValue(destinationClone, null);
                // 值相等，跳过
                if (newValue.Equal(oldValue))
                {
                    continue;
                }
                //try
                //{
                //    if (prop.PropertyType == typeof(Guid))
                //    {
                //        newValue = Guid.Parse(newValue.ToString());
                //    }
                //    else
                //    {
                //        newValue = Convert.ChangeType(newValue, prop.PropertyType);
                //    }
                //}
                //catch (Exception ex)
                //{

                //}
                ConstantExpression constant = Expression.Constant(newValue, prop.PropertyType);
                bindings.Add(Expression.Bind(prop, constant));
            }
            if (bindings.Count > 0)
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TDestination), "a");
                expression = Expression.Lambda<Func<TDestination, TDestination>>(Expression.MemberInit(Expression.New(typeof(TDestination)), bindings.ToArray()), parameterExpression);
            }
            return expression;
        }
        #endregion
    }
}
