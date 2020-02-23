using AutoMapper;
using Shop.Common.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Shop.Common.Extensions
{
    public static class DataExtensions
    {

        private static Dictionary<string, MethodInfo> logicMehtodCache = new Dictionary<string, MethodInfo>();
        private static readonly object logic_mehtod_locker = new object();

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

        #region DateTime
        public static string ToShortDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        public static string ToLongDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToShortDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortDate();
            }
            return "";
        }
        public static string ToLongDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToLongDate();
            }
            return "";
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

        /// <summary>
        /// 获取查询逻辑对应的C#方法
        /// </summary>
        /// <param name="logicEnum"></param>
        /// <returns></returns>
        public static MethodInfo ToMethod(this LogicEnum logicEnum)
        {
            var key = logicEnum.ToString();
            MethodInfo method = null;
            if (!logicMehtodCache.ContainsKey(key))
            {
                lock (logic_mehtod_locker)
                {
                    if (!logicMehtodCache.ContainsKey(key))
                    {
                        switch (logicEnum)
                        {
                            case LogicEnum.Like:
                            case LogicEnum.NoLike:
                                method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                                break;
                            case LogicEnum.LikeLeft:
                                method = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
                                break;
                            case LogicEnum.LikeRight:
                                method = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
                                break;
                            case LogicEnum.IsNullOrEmpty: 
                            default:
                                break;
                                
                        }
                        if (method ==null)
                        {
                            throw new Exception("没找到合适的方法");
                        }
                        logicMehtodCache.Add(key, method);
                    }
                } 
            }
            return logicMehtodCache[key];
        }

        /// <summary>
        /// 通用的类型转换方法
        /// </summary>
        /// <returns></returns>
        public static object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }

    }
}
