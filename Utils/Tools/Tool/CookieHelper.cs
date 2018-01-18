using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tools.Tool
{
    /// <summary>
    /// Cookie操作类
    /// 创建人：吴李辉   时间：2013-3-12
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="name">cookie名称</param>
        /// <param name="value">值</param>
        /// <param name="domain">作用域</param>
        /// <param name="expiresTime">过期时间</param>
        public static void SetCookie(string name, string value, DateTime expiresTime)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[name];
            cookie.Value = value;
            cookie.Expires = expiresTime;//添加作用时间
            cookie.Domain = ".jiepei.com";
            HttpContext.Current.Request.Cookies[name].Value = value;
        }

        /// <summary>
        /// 设置Cookie值同时更新客户端Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        public static void SetCookie(string name, string value)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[name];
            cookie.Value = value;
            cookie.Domain = ".jiepei.com"; 
            HttpContext.Current.Request.Cookies[name].Value = value;
        }

        /// <summary>
        /// 设置Cookie值同时更新客户端Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        public static void SetClientAndServerCookie(string name, string value)
        {
            HttpCookie cookie =
                HttpContext.Current.Request.Cookies[name] == null ?
                new HttpCookie(name) :
                HttpContext.Current.Request.Cookies[name];//定义cookie对象
            cookie.Value = value;
            cookie.Domain = ".jiepei.com"; 
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Request.Cookies.Add(cookie);
        }

        /// <summary>
        /// 设置Cookie值同时更新客户端Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        /// <param name="expiresTime"></param>
        public static void SetClientAndServerCookie(string name, string value, DateTime expiresTime)
        {
            HttpCookie cookie =
                HttpContext.Current.Request.Cookies[name] == null ?
                new HttpCookie(name) :
                HttpContext.Current.Request.Cookies[name];//定义cookie对象
            cookie.Expires = expiresTime;//添加作用时间
            cookie.Domain = ".jiepei.com";
            cookie.Value = value;

            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Request.Cookies.Add(cookie);
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="name">cookie名称</param>
        /// <returns></returns>
        public static string GetCookie(string name)
        {
            if (HttpContext.Current.Request.Cookies[name] != null && HttpContext.Current.Request.Cookies[name].Value != null)
            {
                return HttpContext.Current.Request.Cookies[name].Value.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 清除cookie
        /// </summary>
        /// <param name="name">cookie名称</param>
        public static void ClearCookie(string name)
        {
            HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies[name].Value = string.Empty;
        }

        /// <summary>
        /// 清除cookie
        /// </summary>
        /// <param name="name">cookie名称</param>
        public static void ClearCookie(string name, string domain)
        {
            if (HttpContext.Current.Request.Cookies[name] != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Domain = domain;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 更新过期时间
        /// </summary>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="expiresTime"></param>
        public static void SetExpires(string name, DateTime expiresTime)
        {
            HttpContext.Current.Response.Cookies[name].Expires = expiresTime;
        }
    }
}
