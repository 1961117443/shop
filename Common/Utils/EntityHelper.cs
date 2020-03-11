using Shop.Common.Data;
using Shop.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Shop.Common.Utils
{
    /// <summary>
    /// 实体帮助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityHelper<T>
    {
        #region 静态属性
        private static Dictionary<string, MemberExpression> memberExpressionCache = new Dictionary<string, MemberExpression>();
        private static readonly object member_expression_locker = new object();
        
        #endregion

        static EntityHelper()
        {

        }
        /// <summary>
        /// 所有的成员属性
        /// </summary>
        public static PropertyInfo[] PublicInstance = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        private static PropertyInfo[] dbFields;

        /// <summary>
        /// 映射数据库的字段
        /// </summary>
        public static PropertyInfo[] DbFields
        {
            get
            {
                if (dbFields == null || dbFields.Length == 0)
                {
                    List<PropertyInfo> list = new List<PropertyInfo>();
                    foreach (var property in PublicInstance)
                    {
                        if (!property.CanWrite || property.PropertyType.IsClass || property.SetMethod.IsVirtual)
                        {
                            continue;
                        }
                        list.Add(property);
                        dbFields = list.ToArray();
                    }
                }
                return dbFields;
            }
        }

        /// <summary>
        /// 根据字段名获取表达式目录树 
        /// fieldName="ClientFile.ClientName" ： a=>a.ClientFile.ClientName
        /// </summary>
        /// <param name="parameterExpression"></param> 
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static MemberExpression GetMemberExpressionCore(Expression parameterExpression, string fieldName)
        {
            int index = fieldName.IndexOf('.');
            string f = index >= 0 ? fieldName.Substring(0, index) : fieldName;
            fieldName = fieldName.Substring(Math.Min(f.Length + 1, fieldName.Length));

            var propertyInfo = parameterExpression.Type.GetProperty(f);
            if (propertyInfo == null)
            {
                return null;
            }
            var expression = Expression.Property(parameterExpression, propertyInfo);
            if (index >= 0)
            {
                return GetMemberExpressionCore(expression, fieldName);
            }
            return expression;
        }

 
        /// <summary>
        /// 根据字段名获取表达式目录树 
        /// fieldName="ClientFile.ClientName" ： a=>a.ClientFile.ClientName
        /// </summary> 
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression(string fieldName)
        {
            if (!memberExpressionCache.ContainsKey(fieldName))
            {
                lock (member_expression_locker)
                {
                    if (!memberExpressionCache.ContainsKey(fieldName))
                    {
                        ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "a");
                        memberExpressionCache.Add(fieldName, GetMemberExpressionCore(parameterExpression, fieldName));
                    }
                }
            }            
            return memberExpressionCache[fieldName];
        }

        /// <summary>
        /// 获取特定的查询字段
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<T, T>> GetQueryMember()
        {
            return null;
            string qf = "ID,BillCode,Audit,ClientName,ClientName.ClientName";
            List<MemberBinding> ms = new List<MemberBinding>();
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "c");
            foreach (var f in qf.Split(','))
            {
                var exp = GetMemberExpression(f);
                if (exp != null)
                {
                    ms.Add(Expression.Bind(exp.Member, exp));

                }
            }
            Expression<Func<T, T>> expression = null;
            if (ms.Count > 0)
            {

                expression = Expression.Lambda<Func<T, T>>(Expression.MemberInit(Expression.New(typeof(T)), ms.ToArray()), parameterExpression);
            }
            return expression;
        }


        /// <summary>
        /// 把查询的内容拼接成表达式目录树
        /// </summary>
        /// <param name="field">查询字段，Entity的字段，如果是外键 需要用.分开，入Product.ProductCode</param>
        /// <param name="value"></param>
        /// <param name="logic"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> ToExpression(string field, string value, LogicEnum logic = LogicEnum.Equal)
        {
            Expression member =  GetMemberExpression(field);
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
            constantValue = DataExtensions.ChangeType(value, valueType);
            Expression constant = Expression.Constant(constantValue, valueType);
            //把参数类型转换一下
            constant = Expression.Convert(constant, memberType);
            Expression<Func<T, bool>> where = null;
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "a");
            switch (logic)
            {
                //等于
                case LogicEnum.Equal:
                case LogicEnum.IsNullOrEmpty:
                    return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameterExpression);
                //包含 右包含 左包含
                case LogicEnum.Like:
                case LogicEnum.NoLike:
                case LogicEnum.LikeLeft: //右包含
                case LogicEnum.LikeRight: //左包含
                    {
                        var method = logic.ToMethod();
                        Expression mehtodCallExpression = Expression.Call(member, method, constant);
                        //  mehtodCallExpression.Not()
                        where = Expression.Lambda<Func<T, bool>>(mehtodCallExpression, parameterExpression);
                        if (logic == LogicEnum.NoLike)
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
                //case LogicEnum.IsNullOrEmpty:
                //    break;
                case LogicEnum.IsNot:
                    break;
                default:
                    return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameterExpression);
            }
            return null;
        }
    }
}
