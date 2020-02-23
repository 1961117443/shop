using System;
using System.Collections.Generic;
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
        private static ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "a");
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
        public static MemberExpression GetMemberExpression(Expression parameterExpression, string fieldName)
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
                return GetMemberExpression(expression, fieldName);
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
            return GetMemberExpression(parameterExpression, fieldName);
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
    }
}
