using Newtonsoft.Json;
using Shop.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Shop.Common.Utils
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 获取JsonProperty的名称
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string JsonPropertyName(this MemberInfo property)
        {
            string name = property.Name;
            var atts = property.GetCustomAttributes(typeof(JsonPropertyAttribute), true);
            if (atts.Length>0)
            {
                if (!string.IsNullOrEmpty((atts[0] as JsonPropertyAttribute).PropertyName))
                {
                    name = (atts[0] as JsonPropertyAttribute).PropertyName;
                } 
            }
            return name;
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string Description(this MemberInfo property)
        {
            string name = property.Name;
            var atts = property.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (atts.Length > 0)
            {
                var val = (atts[0] as DescriptionAttribute).Description;
                if (!val.IsEmpty())
                {
                    name = val;
                }
            }
            return name;
        }

        /// <summary>
        /// 复制一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Clone<T>(this T entity)
        {
            T data = Activator.CreateInstance<T>();
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                prop.SetValue(data, prop.GetValue(entity, null));
            }
            return data;
        }
    }
}
