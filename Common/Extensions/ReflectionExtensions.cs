using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}
