using Newtonsoft.Json.Linq;
using Shop.Common.Data;
using Shop.Common.Extensions;
using Shop.Common.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// 把前端的查询条件转成表达式目录树
        /// 查询条件通过JObject获取
        /// JObject 需要有对应的 TView 模型
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <typeparam name="TView">TView</typeparam>
        /// <param name="mapper">IMapper</param>
        /// <param name="data">JObject</param>
        /// <returns></returns>
        [Obsolete]
        public static Expression<Func<TEntity, bool>> ToCriteriaExpression<TEntity, TView>(this IMapper mapper, JObject data)
        {
            List<QueryParam> queryParams = new List<QueryParam>();
            if (data != null)
            {
                var viewToEntityMaps = mapper.ConfigurationProvider.FindTypeMapFor<TView, TEntity>();
                if (viewToEntityMaps != null)
                {
                    List<Expression<Func<TEntity, bool>>> expList = new List<Expression<Func<TEntity, bool>>>();
                    foreach (var item in data)
                    {
                        var value = item.Value.Value<string>();
                        if (!value.IsEmpty())
                        {
                            var path = viewToEntityMaps.PathMaps.FirstOrDefault(w => !w.Ignored && w.SourceMember.Name == item.Key);
                            if (path != null)
                            {
                                //var param = new QueryParam(path.DestinationName, value);
                                //var exp1 = param.QueryParamToExpression<TEntity>();
                                //expList.Add(exp1); 
                                queryParams.Add(new QueryParam(path.DestinationName, value, LogicEnum.Like));
                            }
                        }
                    }
                }
            }
            return queryParams.QueryParamToExpression<TEntity>();
        }

        /// <summary>
        /// 通过imapper 根据viewModel的字段获取对应entity的字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="dtoField"></param>
        /// <returns></returns>
        public static string GetEntityField<TEntity, TView>(this IMapper mapper, string dtoField)
        {
            return mapper.GetEntityField(dtoField, typeof(TEntity), typeof(TView));
        }
        public static string GetEntityField(this IMapper mapper, string dtoField, Type entityType, Type viewType)
        {
            string field = string.Empty;
            if (!dtoField.IsEmpty())
            {
                var viewToEntityMaps = mapper.ConfigurationProvider.FindTypeMapFor(viewType, entityType);
                if (viewToEntityMaps != null)
                {
                    var path = viewToEntityMaps.PathMaps.FirstOrDefault(w => !w.Ignored && w.SourceMember.Name == dtoField);
                    if (path != null)
                    {
                        field = path.DestinationName;
                    }
                    else
                    {
                        var property = viewToEntityMaps.PropertyMaps.FirstOrDefault(w => !w.Ignored && w.SourceMember.Name == dtoField);
                        if (property != null)
                        {
                            field = property.DestinationName;
                        }
                    }

                }
            }
            return field;
        }

        public static QueryParam<TEntity, TView> MapParam<TEntity, TView>(this IMapper mapper, object dto)
        {
            return mapper.Map<QueryParam<TEntity, TView>>(dto);
        }

        public static IEnumerable<QueryParam<TEntity, TView>> MapParamList<TEntity, TView>(this IMapper mapper, IEnumerable dto)
        {
            return mapper.MapList<QueryParam<TEntity, TView>>(dto);
        }
        #endregion
    }
}

