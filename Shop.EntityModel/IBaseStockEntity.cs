using Shop.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Shop.EntityModel
{
    public interface IBaseStockEntity<T> :IEquatable<T> where T:IBaseStockEntity<T>
    {
        /// <summary>
        /// 获取更新库存条件的值
        /// </summary>
        string GetUniqueValues { get; }
        /// <summary>
        /// 获取关联的条件
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, bool>> GetUniqueSql();
    }

    public abstract class BaseStockEntity<T> : IBaseStockEntity<T> where T : IBaseStockEntity<T>
    { 
        /// <summary>
        /// 关联条件 匹配条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract bool Equals(T entity);

        public virtual Expression<Func<T, bool>> GetUniqueSql()
        {
            //Expression<Func<T, bool>> where = null;
            ParameterExpression parameter = Expression.Parameter(typeof(T), "a"); 

            //var p = typeof(T).GetProperty("ProductID");
            //Expression.Equal(Expression.Property(parameter, p), Expression.Constant(p.GetValue(entity, null), p.PropertyType));
            Expression expression = null;
            foreach (var property in uniqueKeys)
            {
                var left = Expression.Equal(Expression.Property(parameter, property), Expression.Constant(property.GetValue(this, null), property.PropertyType));
                expression = expression == null ? left : Expression.AndAlso(expression, left);
            } 

            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }

        /// <summary>
        /// 通过特性获取联合主键 根据这些主键更新库存
        /// tolist 才会访问一次
        /// </summary>
        private static IEnumerable<PropertyInfo> uniqueKeys = EntityHelper<T>.PublicInstance.Where(w => w.GetCustomAttribute<StockUniqueKeyAttribute>() != null).ToList();

        public string GetUniqueValues
        {
            get
            {
                var values = uniqueKeys.Select(p => p.GetValue(this, null)).ToArray();
                return string.Join("|", values);
            }
        }
    }
}
