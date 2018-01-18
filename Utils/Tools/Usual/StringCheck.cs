using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FancyFix.Tools.Usual
{
   public class StringCheck
    {

        /// <summary>
        /// 是否合法商铺二级域名
        /// 创建人：吴李辉  时间：2012-2-22
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsShopUrl(string str)
        {
            Regex reg = new Regex(@"^[0-9a-zA-Z_\-]{3,20}$");
            return reg.Match(str).Success;
        }

        /// <summary>
        /// 是否为DZSC网站文件     Author：王建魁     时间：2012-05-02
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDzscFile(string path)
        {
            Regex reg = new Regex(@"^http(s)?://[\w-\.]*?\.dzsc\.com[\w\./]+?$");
            return reg.Match(path).Success;
        }
        /// <summary>
        /// 是否存在中文字符
        /// 创建人：吴李辉  时间：2013-7-19
        /// </summary>
        /// <param name="CString"></param>
        /// <returns></returns>
        public static bool IsHaveChinaChar(string CString)
        {
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) > Convert.ToInt32(Convert.ToChar(128)))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPartNumber(String strNumber)
        {
            Regex objNotNaturalPattern = new Regex(@"^[0-9a-zA-Z#][0-9a-zA-Z-_\+\/\.\\ \(\)#\=%\,:]{2,49}$");
            return objNotNaturalPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// 检测用户名格式
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsUserName(string userName)
        {
            Regex reg = new Regex("^[a-zA-Z]{1}[0-9a-zA-Z_]{5,24}$");
            return reg.IsMatch(userName);
        }
        /// <summary>
        /// 验证Emal等相关信息
        /// 创建人：骆清泉 创建时间：2012-2-8
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// 
        public static bool IsEmail(string email)
        {
            Regex reg = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);
            return reg.IsMatch(email);
        }

        public static bool IsPhone(string phone)
        {
            Regex reg = new Regex(@"^[0-9\.\-]{3,}(\/[0-9\.\-]{3,}){0,2}$");
            return reg.IsMatch(phone);
        }
        public static bool IsMobile(string mobile)
        {
            Regex reg = new Regex(@"^(13|15|18|17)\d{9}$");
            return reg.IsMatch(mobile);
        }

        public static bool IsSigMobile(string mobile)
        {
            Regex reg = new Regex(@"^(13[0-9]|14[7]|15[0-9]|18[0-9])\d{8}$");
            return reg.IsMatch(mobile);
        }

        public static bool IsPostCode(string postCode)
        {
            Regex reg = new Regex(@"^\d{6}$");
            return reg.IsMatch(postCode);
        }

        public static bool IsQQ(string qq)
        {
            Regex reg = new Regex(@"^\d{5,}(\/\d{5,}){0,2}$");
            return reg.IsMatch(qq);
        }
        public static bool IsMSN(string msn)
        {
            Regex reg = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?(\/((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?){0,2}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(msn);
        }

        public static bool IsPwd(string pwd)
        {
            Regex reg = new Regex(@"^\S{6,16}$");
            return reg.IsMatch(pwd);
        }

        /// <summary>
        /// 创建人:吕海斌 创建时间:2012-2-13
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static bool IsFloat(string price)
        {
            Regex reg = new Regex(@"^[0-9\.]+$");
            if (!reg.IsMatch(price))
            {
                return false;
            }
            var pos = price.IndexOf('.');
            if (pos != price.LastIndexOf('.'))
            {
                return false;
            }
            if (pos == 0 || (pos + 1) == price.Length)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否整数
        /// 创建人：吴李辉  时间：2012-2-15
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            Regex reg = new Regex(@"^[\+\-]{0,1}\d+$");
            return reg.Match(str).Success;
        }

        /// <summary>
        /// 是否是价格 小数点后保留后两位 包括正或负
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPrice(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            Regex reg = new Regex(@"^[-+]?(\d{1,}|\d{1,}\.\d{1,2}0{0,})$");
            return reg.Match(str).Success;
        }

        /// <summary>
        /// 是否为URL地址
        /// 创建人：吴李辉 时间：2012-6-28
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            Regex reg = new Regex(@"^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }

        public static bool IsNumber(string checkStr)
        {
            if (string.IsNullOrEmpty(checkStr)) { return false; }
            return Regex.IsMatch(checkStr, @"^[-]{0,1}\d+$");

        }

        /// <summary>
        /// 格式为yyyy-MM-dd
        /// 创建人:吕海斌 创建时间:2012-3-16
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDate(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$");
            return reg.IsMatch(date);
        }

        /// <summary>
        /// 格式为yyyy-MM-dd hh:mm:ss
        /// 创建人:俞忠亮 创建时间:2012-5-1-76
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateTime(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])\s+(1[012]|0?[1-9]){1}:(0?[1-5]|[0-6][0-9]){1}:(0?[0-6]|[0-6][0-9]){1}$");
            return reg.IsMatch(date);
        }

        /// <summary>
        /// 格式为yyyy-MM-dd hh:mm
        /// 创建人:俞忠亮 创建时间:2013-2-3
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateTime2(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])\s+(2[0123]|1[0-9]|0?[0-9]){1}:(0?[0-9]|[1-5][0-9]|60){1}$");
            return reg.IsMatch(date);
        }

        /// <summary>
        /// 严格判断时间格式为 yyyy-MM
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateYM(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])$");
            return reg.IsMatch(date);
        }

        /// <summary>
        /// 严格判断时间格式为 yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateyMd(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$");
            return reg.IsMatch(date);
        }

        public static bool IsFunction(string func)
        {
           
            Regex reg = new Regex(@"^[\w\d\._]+$");
            return reg.IsMatch(func);
        }
    }
}
