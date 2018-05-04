using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FancyFix.Tools.Common
{
    public class StringProcess
    {
        /// <summary>
        /// 剪切字符串(中文算2个字符)
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public static string CutString(string strInput, int intLen)
        {
            if (String.IsNullOrEmpty(strInput))
                return strInput;
            strInput = strInput.Trim();
            byte[] buffer1 = Encoding.Default.GetBytes(strInput);
            if (buffer1.Length > intLen)
            {
                string text1 = "";
                for (int num1 = 0; num1 < strInput.Length; num1++)
                {
                    byte[] buffer2 = Encoding.Default.GetBytes(text1 + strInput.Substring(num1, 1));
                    if (buffer2.Length > intLen)
                    {
                        break;
                    }
                    text1 = text1 + strInput.Substring(num1, 1);
                }
                return (text1 + "...");
            }
            return strInput;
        }


        /// <summary>
        /// 去除HTML及空格 常用于标题 Keywords 输出
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToLineText(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = RemoveHtml(str);
            str = str.Replace("　", "");
            str = str.Replace("\r\n", " ");
            string text1 = "\\s+";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, " ");
            str = System.Web.HttpUtility.HtmlEncode(str);
            return str.Trim();
        }

        /// <summary>
        /// 文本框内容输出成html显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHtmlText(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(str);
            builder1.Replace("&", "&amp;");
            builder1.Replace("<", "&lt;");
            builder1.Replace(">", "&gt;");
            builder1.Replace("\"", "&quot;");
            builder1.Replace("\r", "<br>");
            builder1.Replace(" ", "&nbsp;");
            return builder1.ToString();
        }


        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtml(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string text1 = "<.*?>";
                Regex regex1 = new Regex(text1);
                str = regex1.Replace(str, "");
                str = str.Replace("&nbsp;", " ");
            }
            else
            {
                str = ""; 
            }
            return str;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="strOriginal">原字符</param>
        /// <param name="strFirst">开始字符</param>
        /// <param name="strLast">结束字符</param>
        /// <param name="trim">是否不包括开始结束字符</param>
        /// <returns></returns>
        public static string GetContent(string strOriginal, string strFirst, string strLast, bool trim = true)
        {
            if (string.IsNullOrEmpty(strOriginal) == true)
                return "";

            string s = "";
            int t1, t2, t3;
            
            if (trim)
            {
                string strOriginal1 = strOriginal, strFirst1 = strFirst, strLast1 = strLast;
                t1 = strOriginal1.IndexOf(strFirst1);
                if (t1 >= 0)
                {
                    t2 = strOriginal1.Length;
                    t3 = t1 + strFirst1.Length;
                    strOriginal1 = strOriginal1.Substring(t3);

                    t1 = strOriginal1.IndexOf(strLast1);
                    t3 = t1;
                    if (t3 > 0)
                        s = strOriginal1.Substring(0, t3);
                }
            }
            else
            {
                s = GetContent(strOriginal, strFirst, strLast);
                s = strFirst + s + strLast;
            }
            return s;
        }

    }
}
