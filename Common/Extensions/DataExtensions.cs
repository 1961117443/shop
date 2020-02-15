using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Extensions
{
    public static class DataExtensions
    {
        #region String
        /// <summary>
        /// 判断字符串是否为空（null 或者 空格）
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string val)
        {
            return val.IsNullOrEmpty() || val.IsNullOrWhiteSpace();
        }

        public static bool IsNullOrEmpty(this string val)
        {
            return string.IsNullOrEmpty(val);
        }
        public static bool IsNullOrWhiteSpace(this string val)
        {
            return string.IsNullOrWhiteSpace(val);
        }

        public static Guid ToGuid(this string val)
        {
            Guid uid = Guid.Empty;
            Guid.TryParse(val, out uid);
            return uid;
        }
        #endregion

        #region Guid
        public static bool IsEmpty(this Guid guid)
        {
            return guid.Equals(Guid.Empty);
        } 
        #endregion

        public static bool Equal(this object obj1,object obj2)
        {
            if (obj1==null && obj2==null)
            {
                return true;
            }
            else if (obj1==null)
            {
                return obj2.Equals(obj1);
            }
            else
            {
                return obj1.Equals(obj2);
            }
        }



    }
}
