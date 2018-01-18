using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyFix.Tools.Special
{
    public class Common
    {

        public static bool IsOpenHttps
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["IsOpenHttps"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["IsOpenHttps"].ToString().ToBool();
                return false;
            }
        }
        /// <summary>
        /// 获取主域名
        /// </summary>
        /// <returns></returns>
        public static string GetDomain()
        {
            return System.Configuration.ConfigurationManager.AppSettings["domain"].ToString2();
        }

        /// <summary>
        /// 获取上传页面
        /// </summary>
        /// <returns></returns>
        public static string GetImgUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["imgurl"].ToString2();
            return UrlTransfer(res);
        }

        public static string GetImgJsUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["ImgJsUrl"].ToString2();
            return UrlTransfer(res);
        }

        /// <summary>
        /// CRM后台域
        /// </summary>
        /// <returns></returns>
        public static string GetManageUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["manageurl"].ToString2();
            return UrlTransfer(res);
        }

        /// <summary>
        /// 获取抓取图片Url
        /// </summary>
        /// <returns></returns>
        public static string GetWaterImg()
        {
            return System.Configuration.ConfigurationManager.AppSettings["waterImg"].ToString2();
        }

        /// <summary>
        /// 获取抓取图片Url
        /// </summary>
        /// <returns></returns>
        public static string GetImgSpiderUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["imgSpiderUrl"].ToString2();
            return UrlTransfer(res);
        }

        /// <summary>
        /// 获取outside图片url
        /// </summary>
        /// <returns></returns>
        public static string GetImgOutsideUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["imgOutsideUrl"].ToString2();
            return UrlTransfer(res);
        }

        /// <summary>
        /// 邮件发送系统地址
        /// </summary>
        /// <returns></returns>
        public static string GetMailUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["mailUrl"].ToString2();
            return UrlTransfer(res);
        }
        /// <summary>
        /// 获取抓取图片Url
        /// </summary>
        /// <returns></returns>
        public static string GetImgSpiderImagDir()
        {
            return System.Configuration.ConfigurationManager.AppSettings["imgSpiderDir"].ToString2();
        }

        /// <summary>
        /// 获取outside抓取图片url
        /// </summary>
        /// <returns></returns>
        public static string GetImgOutsideImagDir()
        {
            return System.Configuration.ConfigurationManager.AppSettings["imgOutsideDir"].ToString2();
        }

        /// <summary>
        /// 获取web域名
        /// </summary>
        /// <returns></returns>
        public static string GetWebUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["weburl"].ToString2();
            return UrlTransfer(res);
        }

        /// <summary>
        /// 获取blog域名
        /// </summary>
        /// <returns></returns>
        public static string GetBlogUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["blogurl"].ToString2();
            return UrlTransfer(res);
        }

        /// <summary>
        /// 获取m域名
        /// </summary>
        /// <returns></returns>
        public static string GetMobileUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["mobileurl"].ToString2();
            return UrlTransfer(res);
        }

        /// <summary>
        /// 获取outside域名
        /// </summary>
        /// <returns></returns>
        public static string GetOutsideUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["outsideurl"].ToString2();
            return UrlTransfer(res);
        }

        /// <summary>
        /// 获取about域名
        /// </summary>
        /// <returns></returns>
        public static string GetAboutUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["abouturl"].ToString2();
            //  return UrlTransfer(res);

        }

        /// <summary>
        /// 获取member域名
        /// </summary>
        /// <returns></returns>
        public static string GetMemberUrl()
        {
            var res = System.Configuration.ConfigurationManager.AppSettings["memberurl"].ToString2();
            return UrlTransfer(res);
        }

        /// <summary>
        /// 获取Sitemap 保存路径
        /// </summary>
        /// <returns></returns>
        public static string GetSitemapDir()
        {
            return System.Configuration.ConfigurationManager.AppSettings["sitemapDir"].ToString2();
        }

        public static string GetSitemapDirGlobal()
        {
            return System.Configuration.ConfigurationManager.AppSettings["sitemapDirGlobal"].ToString2();
        }

        public static string GetSolrUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SolrUrl"].ToString2();
        }

        /// <summary>
        /// 获取是否Redis共享Session
        /// </summary>
        /// <returns></returns>
        public static bool GetIsRedisSession()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["RedisSession"] != null)
                return System.Configuration.ConfigurationManager.AppSettings["RedisSession"].ToString().ToBool();
            return false;
        }

        /// <summary>
        /// 获取美元汇率
        /// </summary>
        /// <returns></returns>
        public static decimal GetDollarExchangeRate()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["ExchangeRate"] != null)
                return System.Configuration.ConfigurationManager.AppSettings["ExchangeRate"].ToString().ToDecimal();
            return (decimal)6.8;
        }

        /// <summary>
        /// 获取默认运费（人民币）
        /// </summary>
        /// <returns></returns>
        public static decimal GetDefaultShippingPrice()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["ShippingPrice"] != null)
                return System.Configuration.ConfigurationManager.AppSettings["ShippingPrice"].ToString().ToDecimal();
            return 10000;
        }

        /// <summary>
        /// 获取Facebook帐号ID
        /// </summary>
        /// <returns></returns>
        public static long GetFacebookAdminId()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["facebookadminid"] != null)
                return System.Configuration.ConfigurationManager.AppSettings["facebookadminid"].ToString().ToInt64();
            return 0000000000000000;
        }

        /// <summary>
        /// Url转换，开启https时 转换为https链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string UrlTransfer(string url)
        {
            // if (IsOpenHttps)
            //  url = url.Replace("http://", "https://");
            return url;
        }
    }
}
