using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web;

namespace System
{
    public static class StringExtension
    {
        /// <summary>
        /// 安全输出字符串(页面内容)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSafe(this string s)
        {
            return System.Web.HttpUtility.HtmlEncode(s);
        }

        /// <summary>
        /// 转换INT类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defautValue"></param>
        /// <returns></returns>
        public static int ToInt32(this string s, int defautValue)
        {
            int result = defautValue;
            if (int.TryParse(s, out result))
            {
                return result;
            }
            return defautValue;
        }

        /// <summary>
        /// 转换INT类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt32(this string s)
        {
            return ToInt32(s, 0);
        }

        /// <summary>
        /// 转换INT64类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defautValue"></param>
        /// <returns></returns>
        public static long ToInt64(this string s, long defautValue)
        {
            long result = defautValue;
            if (long.TryParse(s, out result))
            {
                return result;
            }
            return defautValue;
        }

        /// <summary>
        /// 转换INT64类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ToInt64(this string s)
        {
            return ToInt64(s, 0);
        }

        /// <summary>
        /// 转换INT类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInt(this string s)
        {
            int result = 0;
            if (int.TryParse(s, out result))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 转换成FLOAT类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defautValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string s, float defautValue)
        {
            float result = defautValue;
            float.TryParse(s, out result);
            return result;
        }

        /// <summary>
        /// 转换成FLOAT类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static float ToFloat(this string s)
        {

            return ToFloat(s, 0);
        }

        /// <summary>
        /// 转换成Double类型
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string s, double defaultValue)
        {
            double result = 0;
            if (double.TryParse(s, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成Double类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double ToDouble(this string s)
        {
            return ToDouble(s, 0);
        }


        public static decimal ToDecimal(this string s, decimal defaultValue)
        {

            decimal result = 0;
            if (decimal.TryParse(s, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 1.1000进来 1.1返回
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>

        public static string ToString3(this string s)
        {
            int len = s.Length;
            if (s.Substring(len - 1) == "0")
            {
                return ToString3(s.Substring(0, len - 1));
            }
            else if (s.Substring(len - 1) == ".")
            {
                return s.Substring(0, len - 1);
            }
            else
            {
                return s;
            }
        }

        public static decimal ToDecimal(this string s)
        {
            return ToDecimal(s, 0);
        }

        public static byte ToByte(this string s)
        {
            byte b = 0;
            byte.TryParse(s, out b);
            return b;
        }

        /// <summary>
        /// 转换日期格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string s)
        {
            DateTime time;
            if (DateTime.TryParse(s, out time))
            {
                return time;
            }
            else
            {
                return FancyFix.Tools.Usual.Common.InitDateTime();
            }
        }

        public static string ToForeignDateTime(this DateTime time, string format)
        {
            return time.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// 转化日期格式 国外格式：December 04,2015
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToForeignDateTimeShort2(this string s)
        {
            DateTime time;
            if (DateTime.TryParse(s, out time))
            {
                return time.ToForeignDateTime("MMM dd,yyyy");
            }
            else
            {
                return FancyFix.Tools.Usual.Common.InitDateTime().ToString();
            }
        }

        public static string ToForeignDateTimeShort2(this DateTime time)
        {
            return time.ToForeignDateTime("MMM dd,yyyy");
        }

        /// <summary>
        /// 转化日期格式 国外格式：04 December 2015
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToForeignDateTimeShort(this string s)
        {
            DateTime time;
            if (DateTime.TryParse(s, out time))
            {
                return time.ToForeignDateTime("dd MMM yyyy");
            }
            else
            {
                return FancyFix.Tools.Usual.Common.InitDateTime().ToString();
            }
        }

        public static string ToForeignDateTimeShort(this DateTime time)
        {
            return time.ToForeignDateTime("dd MMM yyyy");
        }

        /// <summary>
        /// 转化日期格式 国外格式：04 December 2015 19:30:00
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToForeignDateTimeLong(this string s)
        {
            DateTime time;
            if (DateTime.TryParse(s, out time))
            {
                return time.ToForeignDateTime("dd MMM yyyy HH:mm:ss");
            }
            else
            {
                return FancyFix.Tools.Usual.Common.InitDateTime().ToString();
            }
        }

        public static string ToForeignDateTimeLong(this DateTime time)
        {
            return time.ToForeignDateTime("dd MMM yyyy HH:mm:ss");
        }

        /// <summary>
        /// 判断是否日期格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string s)
        {
            DateTime time;
            if (DateTime.TryParse(s, out time))
            {
                return true;
            }
            return false;
        }

        public static string ToString2(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            return s;
        }

        public static string ToUrlEncode(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            return System.Web.HttpUtility.UrlEncode(s.Trim());
        }

        public static string ToUrlDecode(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            return System.Web.HttpUtility.UrlDecode(s.Trim());
        }

        public static string Trim2(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            return s.Trim();
        }
        public static bool ToBool(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            s = s.ToLower();
            if (s == "true" || s == "1")
            {
                return true;
            }
            return false;

        }
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        /// <summary>
        /// 保留小数点后面2至4位,去除末尾多余0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToString2(this decimal d)
        {
            string s = d.ToString("F4");
            int i = s.IndexOf('.') + 3;
            return s.Substring(0, i) + s.Substring(i).TrimEnd('0');
        }
    }
}
