using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class Extensions
    {
        #region 正则匹配

        public static string Match(this string s, string pattern, RegexOptions options = RegexOptions.None, string defValue = "")
        {
            if (s != null)
            {
                var mat = Regex.Match(s, pattern, options);
                if (mat.Success)
                {
                    if (mat.Groups.Count > 1)
                        return mat.Groups.OfType<Group>().Skip(1).Select(m => m.Value).JoinString();
                    else
                        return mat.Value;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 检测字符串是否满足指定正则表达式。
        /// </summary>
        /// <param name="s">要检测的字符串。</param>
        /// <param name="pattern">用于检测的正则表达式。</param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern, RegexOptions options = RegexOptions.None)
        {
            return s != null && Regex.IsMatch(s, pattern, options);
        }

        #endregion

        #region Join String
        public static string JoinString(this string[] strs, string spliter = "")
        {
            return string.Join(spliter, strs);
        }
        public static string JoinString(this IEnumerable<string> strs, string spliter = "")
        {
            return string.Join(spliter, strs);
        }
        #endregion

        #region Int32 To Chinese

        public static Int32 ToInt32(this char c)
        {
            return (int)c - 48;
        }

        static char[] s_cnNums = new char[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };
        static char[] s_cnUnit = new char[] { '十', '百', '千', '万', '亿' };
        /// <summary>
        /// 将Int32数字转换为中文大写字符串。
        /// </summary>
        /// <param name="num">要转换的数字</param>
        /// <returns></returns>
        public static string ToChinese(this int num)
        {
            var nStr = num.ToString();
            string result = string.Empty;
            var sb = new StringBuilder((nStr.Length * 2) + (nStr.Length / 3));
            if (num < 0)
            {
                sb.Append("负");
                nStr = nStr.Remove(0, 1);
            }
            var len = nStr.Length;
            var pZero = false;
            for (int i = 0; i < len; ++i)
            {
                var c = s_cnNums[nStr[i].ToInt32()];

                var pos = len - i;
                if (!pZero && c.Equals(s_cnNums[0]))
                {
                    sb.Append(c);
                    pZero = true;
                }
                else if (!c.Equals(s_cnNums[0]))
                {
                    pos = pos - 1;
                    sb.Append(c);
                    pZero = false;
                    var idx = (pos & 3) - 1;
                    if (idx >= 0)
                        sb.Append(s_cnUnit[idx]);
                    else
                    {
                        idx = (pos & 4) - 1;
                        if (idx >= 0)
                            sb.Append(s_cnUnit[idx]);
                        else
                        {
                            idx = (pos & 8) - 4;
                            if (idx >= 0)
                                sb.Append(s_cnUnit[idx]);
                        }
                    }
                }
                else if (pZero && ((pos & 4) != 0 || (pos & 8) != 0) && (pos & 3) == 0)
                {
                    var idx = (pos & 4) - 1;
                    if (idx >= 0)
                    {
                        if (!sb[sb.Length - 2].Equals(s_cnUnit[4]))
                            sb.Insert(sb.Length - 1, s_cnUnit[idx]);
                    }
                    else
                    {
                        idx = (pos & 8) - 4;
                        if (idx >= 0)
                            sb.Append(s_cnUnit[idx]);
                    }
                }
            }
            if (sb[sb.Length - 1].Equals(s_cnNums[0]))
                sb.Remove(sb.Length - 1, 1);
            result = sb.ToString();
            sb.Remove(0, sb.Length);
            sb = null;
            return result;
        }

        #endregion
    }
}
