using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace FancyFix.Tools.Tool
{
    /// <summary>
    /// 系统路径处理
    /// </summary>
    public class PathHandle
    {
        /// <summary>
        /// 获取网站绝对地址
        /// </summary>
        /// <param name="localPath">相对地址</param>
        /// <returns>绝对地址</returns>
        public static string GetWebPath(string localPath)
        {
            if (HttpContext.Current != null)
            {
                if (localPath == null)
                {
                    return string.Empty;
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(localPath, @"([A-Za-z]):\\([\S]*)"))
                {
                    localPath = "~/" + localPath.Replace(HttpContext.Current.Server.MapPath("~/"), "");
                }
                string path = HttpContext.Current.Request.ApplicationPath;
                string thisPath;
                string thisLocalPath;
                //如果不是根目录就加上"/" 根目录自己会加"/"
                if (path != "/")
                {
                    thisPath = path + "/";
                }
                else
                {
                    thisPath = path;
                }
                if (localPath.StartsWith("~/"))
                {
                    thisLocalPath = localPath.Substring(2);
                }
                else
                {
                    return localPath.Replace(@"\", "/");
                }
                return (thisPath + thisLocalPath).Replace(@"\", "/");
            }
            else
            {
                if (localPath == null)
                {
                    return string.Empty;
                }
                return localPath.Replace(@"\", "/");
            }
        }
        /// <summary>
        ///  获取网站绝对地址
        /// </summary>
        /// <returns></returns>
        public static string GetWebPath()
        {
            string path = System.Web.HttpContext.Current.Request.ApplicationPath;
            string thisPath;
            //如果不是根目录就加上"/" 根目录自己会加"/"
            if (path != "/")
            {
                thisPath = path + "/";
            }
            else
            {
                thisPath = path;
            }
            return thisPath;
        }
        /// <summary>
        /// 获取网站绝对地址
        /// </summary>
        /// <param name="localPath">相对地址</param>
        /// <param name="parms">绝对地址</param>
        /// <returns></returns>
        public static string GetWebPath(string localPath, params object[] parms)
        {
            return string.Format(GetWebPath(localPath), parms);
        }
        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="localPath">相对路径或绝对路径</param>
        /// <returns>绝对路径</returns>
        public static string GetFilePath(string localPath)
        {
            if (localPath == null)
            {
                return string.Empty;
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(localPath, @"([A-Za-z]):\\([\S]*)"))
            {
                return localPath;
            }
            else
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                    return context.Server.MapPath(localPath);
                else
                    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, localPath);
            }
        }
        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="localPath">相对路径或绝对路径</param>
        /// <param name="parms">参数</param>
        /// <returns>绝对路径</returns>
        public static string GetFilePath(string localPath, params object[] parms)
        {
            return string.Format(GetFilePath(localPath), parms);
        }
        /// <summary>
        /// 合并两路径
        /// </summary>
        /// <param name="uriOne"></param>
        /// <param name="uriTwo"></param>
        /// <returns></returns>
        public static string WebPathCombine(string uriOne, string uriTwo)
        {
            string newUri = (uriOne + uriTwo);
            return newUri.Replace("//", "/");
        }
        /// <summary>
        /// 取得根URL
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAppUrl(HttpRequest request)
        {
            string strServer = "http://" + request.ServerVariables["SERVER_NAME"].ToString();
            string strPort = ":" + Convert.ToString(request.ServerVariables["SERVER_PORT"]);
            if (strPort.Trim() == ":80")
            {
                strPort = "";
            }
            string strUrl = strServer + strPort;
            return strUrl;
        }

        /// <summary>
        /// 获取当前页面虚拟目录
        /// </summary>
        /// <returns></returns>
        public static string GetVirtualPath()
        {
            return HttpContext.Current.Request.Path;
        }

        /// <summary>
        /// 获取当前页面虚拟目录+参数
        /// </summary>
        /// <param name="parmStr"></param>
        /// <returns></returns>
        public static string GetVirtualPath(string parmStr)
        {
            string vUrl = GetVirtualPath();
            vUrl = string.Concat(vUrl,"?", parmStr.Replace("?",""));

            return vUrl;
        }
    }
}
