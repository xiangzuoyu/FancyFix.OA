using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Management;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace FancyFix.Tools.Utility
{
    public class CheckClient
    {
        #region 获取浏览器版本号

        /// <summary>  
        /// 获取浏览器版本号  
        /// </summary>  
        /// <returns></returns>  
        public static string GetBrowser()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Browser + bc.Version;
        }


        #endregion

        #region 获取用户操作系统的语言
        public static string GetOsLanguage()
        {
            try
            {
                return HttpContext.Current.Request.UserLanguages[0];
            }
#pragma warning disable CS0168 // 声明了变量“ee”，但从未使用过
            catch (Exception ee)
#pragma warning restore CS0168 // 声明了变量“ee”，但从未使用过
            {
                return "";
            }
        }
        #endregion

        #region 获取操作系统版本号

        /// <summary>  
        /// 获取操作系统版本号  
        /// </summary>  
        /// <returns></returns>  
        public static string GetOSVersion()
        {
            //UserAgent   
            var userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            var osVersion = "未知";

            if (userAgent.Contains("NT 6.1"))
            {
                osVersion = "Windows 7";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "Windows Vista/Server 2008";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "Windows Server 2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "Windows XP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows 2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "Windows NT4";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "Windows Me";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows 98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows 95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }

        /// <summary>  
        /// 获取用户代理信息  
        /// </summary>  
        /// <returns></returns>  
        public static string GetUserAgent()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
                return HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            return "";
        }

        /// <summary>
        /// 判断是否爬虫过滤
        /// </summary>
        /// <param name="useragent"></param>
        /// <returns>True 爬虫 False 非爬虫</returns>
        public static bool CheckBotUserAgent(string useragent)
        {
            if (useragent == "")
                return false;
            //过滤爬虫
            string[] bots = {
                                "Googlebot",
                                "bingbot",
                                "Yahoo! Slurp China",
                                "Yahoo! Slurp",
                                "Yahoo ContentMatch Crawler",
                                "msnbot",
                                "Baiduspider",
                                "Sosospider",
                                "360Spider",
                                "YodaoBot",
                                "Sogou web spider",
                                "Sogou Orion spider",
                                "Sogou-Test-Spider",
                                "Twiceler",
                                "CollapsarWEB qihoobot"
                            };
            foreach (string bot in bots)
            {
                if (useragent.Contains(bot))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 获取客户端IP地址

        /// <summary>  
        /// 获取客户端IP地址  
        /// </summary>  
        /// <returns></returns>  
        public static string GetIP()
        {
            try
            {
                string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(result))
                {
                    return "0.0.0.0";
                }
                return result;
            }
            catch (Exception)
            {

                return "";
            }
           
        }

        #endregion

        /// <summary>
        /// 获取前导页
        /// </summary>
        /// <returns></returns>
        public static string GetReffPage()
        {
            try { return HttpUtility.UrlDecode(HttpContext.Current.Request.UrlReferrer.AbsoluteUri, System.Text.Encoding.GetEncoding("utf-8")); }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            { return "直接访问"; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetNowPage()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <returns></returns>
        public static string ShowClientInfo()
        {
            string result = string.Empty;
            result += string.Format("操作系统：{0} ", GetOSVersion());
            result += string.Format("操作系统语言：{0} ", GetOsLanguage());
            result += string.Format("浏览器：{0} ", GetBrowser());
            result += string.Format("IP：{0} ", GetIP());
            result += string.Format("前导页：{0} ", GetReffPage());
            result += string.Format("当前页：{0} ", GetNowPage());
            return result;
        }

        /// <summary>
        /// 获取Utm_Source
        /// </summary>
        /// <returns></returns>
        public static string GetUtmSource(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (url.Contains("utm_source"))
                {
                    url = url.Substring(url.IndexOf('?'));//ParseQueryString获取不到？后第一个参数
                    return HttpUtility.ParseQueryString(url).Get("utm_source");
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取移动客户端访问
        /// </summary>
        /// <returns></returns>
        public static bool IsMobileDevice()
        {
            string[] mobileAgents = { "iphone", "android", "phone","ipad", "mobile", "wap",
                                        "opera mobi", "opera mini", "ucweb",
                                        "symbian", "series", "webos", "sony", "blackberry", "dopod",
                                        "nokia", "samsung", "palmsource", "xda", "pieplus", "meizu",
                                        "midp", "cldc", "motorola","up.browser",
                                        "blazer", "helio", "hosin", "huawei", "novarra",
                                        "coolpad", "webos", "techfaith", "palmsource", "alcatel", "amoi",
                                        "ktouch", "nexian", "philips", "sagem",
                                        "haier",
                                        "googlebot-mobile" };
            string userAgent = GetUserAgent().ToLower();
            if (userAgent == "")
                return false;

            for (int i = 0; i < mobileAgents.Length; i++)
            {
                if (userAgent.IndexOf(mobileAgents[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static ClientType GetDevice()
        {
            string userAgent = GetUserAgent().ToLower();
            if (userAgent == "")
                return ClientType.Pc;

            Regex regexPhone = new Regex("^.*(iPhone|iPod|Android.*Mobile|Windows\\sPhone|UcWeb|Symbian|Sony|Sagem|Huawei|Motorola|Nokia|IEMobile|BlackBerry|Mobile).*$", RegexOptions.IgnoreCase);

            Regex regexTablet = new Regex("^.*iPad.*$|^.*tablet.*$|^.*Android\\s3.*$|^(?!.*Mobile.*).*Android.*$", RegexOptions.IgnoreCase);

            //平板
            if (regexTablet.IsMatch(userAgent))
                return ClientType.Pad;
            //手机
            if (regexPhone.IsMatch(userAgent))
                return ClientType.Phone;

            return ClientType.Pc;
        }

        public enum ClientType : byte
        {
            [Description("PC端")]
            Pc = 1,

            [Description("手机")]
            Phone = 2,

            [Description("Pad")]
            Pad = 3
        }
    }
}
