using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FancyFix.Tools
{
    public class Chinese
    {


        #region 提取汉字的首个拼音字母


        // 简体中文的编码范围从B0A1（45217）一直到F7FE（63486）
        private static int BEGIN = 45217;
        private static int END = 63486;

        // 二十六个字母区间对应二十七个端点
        // GB2312码汉字区间十进制表示
        private static int[] table = new int[27];

        // 按照声母表示，这个表是在GB2312中的出现的第一个汉字，也就是说“啊”是代表首字母a的第一个汉字。
        // i, u, v都不做声母, 自定规则跟随前面的字母
        private static char[] chartable = { '啊', '芭', '擦', '搭', '蛾', '发', '噶', '哈',
            '哈', '击', '喀', '垃', '妈', '拿', '哦', '啪', '期', '然', '撒', '塌', '塌',
            '塌', '挖', '昔', '压', '匝', };

        // 对应首字母区间表
        private static char[] initialtable = { 'a', 'b', 'c', 'd', 'e', 'f', 'g',
            'h', 'h', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            't', 't', 'w', 'x', 'y', 'z', };

        public static string Get(string SourceStr)
        {
            string result = String.Empty;
            int StrLength = SourceStr.Length;
            int i;
            try
            {
                for (i = 0; i < StrLength; i++)
                {
                    result += Char2Initial(SourceStr[i]);
                }
            }
#pragma warning disable CS0168 // 声明了变量“exce”，但从未使用过
            catch (Exception exce)
#pragma warning restore CS0168 // 声明了变量“exce”，但从未使用过
            {

                result = "";
            }
            return result;
        }

        private static char Char2Initial(char ch)
        {
            for (int i = 0; i < 26; i++)
            {
                table[i] = ChineseCharToValue(chartable[i]);// 得到GB2312码的首字母区间端点表，十进制。
            }
            table[26] = END;// 区间表结尾

            // 对英文字母的处理：小写字母转换为大写，大写的直接返回
            if (ch >= 'a' && ch <= 'z')
                return (char)(ch - 'a' + 'A');
            if (ch >= 'A' && ch <= 'Z')
                return ch;

            int gb = ChineseCharToValue(ch);// 汉字转换首字母
            if ((gb < BEGIN) || (gb > END))// 在码表区间之前，直接返回
                return ch;
            int j;

            for (j = 0; j < 26; j++)
            {// 判断匹配码表区间，匹配到就break,判断区间形如“[,)”
                if ((gb >= table[j]) && (gb < table[j + 1]))
                    break;
            }

            if (gb == END)
            {//补上GB2312区间最右端
                j = 25;
            }
            return initialtable[j]; // 在码表区间中，返回首字母
        }

        /// <summary>
        /// 将汉字转换成十进制
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static int ChineseCharToValue(char ch)
        {
            try
            {
                byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(new char[] { ch });
                if (bytes.Length < 2)
                    return 0;
                return (bytes[0] << 8 & 0xff00) + (bytes[1] & 0xff);
            }
#pragma warning disable CS0168 // 声明了变量“exce”，但从未使用过
            catch (Exception exce)
#pragma warning restore CS0168 // 声明了变量“exce”，但从未使用过
            {
                return 0;
            }
        }

        /// <summary>
        /// 判断是否存在汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 数字转汉字中文大写
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string NumberToChinese(double x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\　　.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
        }

        #endregion
    }
}
