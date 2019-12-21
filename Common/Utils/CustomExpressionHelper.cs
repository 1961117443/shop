using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text; 

namespace Shop.Common.Utils
{
    public class CustomExpressionHelper
    {
        private readonly IMapper mapper;

        public CustomExpressionHelper(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public Expression<Func<TEntity, TEntity>> ViewToEntity1<TView,TEntity>(string key, JObject data)
        {
            Expression<Func<TEntity, TEntity>> expression = null;
            
            if (data.ContainsKey(key))
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "a");
                List<MemberBinding> bindings = new List<MemberBinding>();
                TypeMap typeMaps = this.mapper.ConfigurationProvider.FindTypeMapFor<TEntity, TView>();
                if (typeMaps!=null)
                {
                    var v = data[key]; 
                    var porp = typeMaps.PropertyMaps.FirstOrDefault(w => w.DestinationMember.JsonPropertyName() == key);
                    if (porp!=null)
                    {
                        var dProp = porp.DestinationMember as PropertyInfo;
                        var sProp = porp.SourceMember as PropertyInfo;
                        if (dProp != null && sProp != null)
                        {
                            object dtoValue = null;
                            try
                            {
                                if (dProp.PropertyType == typeof(Guid))
                                {
                                    dtoValue = Guid.Parse(v.ToString());
                                }
                                else
                                {
                                    dtoValue = Convert.ChangeType(v, sProp.PropertyType);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            ConstantExpression constant = Expression.Constant(dtoValue, sProp.PropertyType);
                            bindings.Add(Expression.Bind(sProp, constant));
                            expression = Expression.Lambda<Func<TEntity, TEntity>>(Expression.MemberInit(Expression.New(typeof(TEntity)), bindings.ToArray()), parameterExpression);
                        }
                    }
                    
                } 
            } 
            return expression;
        }
        public Expression<Func<TEntity, TEntity>> ViewToEntity<TView, TEntity>(TView data,params string[] keys)
        {
            Expression<Func<TEntity, TEntity>> expression = null; 
            TypeMap typeMaps = this.mapper.ConfigurationProvider.FindTypeMapFor<TEntity, TView>();
            if (typeMaps != null)
            { 
                List<MemberBinding> bindings = new List<MemberBinding>(); 
                foreach (var key in keys)
                {
                    var porp = typeMaps.PropertyMaps.FirstOrDefault(w => w.DestinationMember.JsonPropertyName() == key);
                    if (porp != null)
                    {
                        var dProp = porp.DestinationMember as PropertyInfo;
                        var sProp = porp.SourceMember as PropertyInfo;
                        if (dProp != null && sProp != null)
                        {
                            object dtoValue = dProp.GetValue(data, null);
                            try
                            {
                                if (dProp.PropertyType == typeof(Guid))
                                {
                                    dtoValue = Guid.Parse(dtoValue.ToString());
                                }
                                else
                                {
                                    dtoValue = Convert.ChangeType(dtoValue, sProp.PropertyType);
                                }
                            }
                            catch (Exception ex)
                            {

                            } 
                            ConstantExpression constant = Expression.Constant(dtoValue, sProp.PropertyType);
                            bindings.Add(Expression.Bind(sProp, constant));
                        }
                    }
                }
                if (bindings.Count>0)
                {
                    ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "a");
                    expression = Expression.Lambda<Func<TEntity, TEntity>>(Expression.MemberInit(Expression.New(typeof(TEntity)), bindings.ToArray()), parameterExpression);
                } 
            }
            return expression;
        }
    }
}
