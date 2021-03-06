﻿using FancyFix.OA.Bll;
using FancyFix.Core.Admin;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FancyFix.OA.Filter
{
    /// <summary>
    /// 权限验证过滤器【宽松模式】
    /// 只有加了该标签的controller或action才会被验证权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionFilterAttribute : AuthorizeAttribute
    {
        /// <summary>
        ///权限Id
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// 权限Url，多个可用逗号隔开
        /// </summary>
        public string PermissionUrl { get; set; }

        /// <summary>
        /// 当前管理员对象 
        /// </summary>
        private Mng_User MyInfo { get; set; }

        /// <summary>
        /// 标签构造
        /// </summary>
        /// <param name="permissionId"></param>
        public PermissionFilterAttribute(int permissionId)
        {
            this.PermissionId = permissionId;
        }

        /// <summary>
        /// 标签构造
        /// </summary>
        /// <param name="permissionUrl"></param>
        public PermissionFilterAttribute(string permissionUrl)
        {
            this.PermissionUrl = permissionUrl;
        }

        /// <summary>
        /// 验证核心
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            try
            {
                MyInfo = new AdminState(httpContext).GetUserInfo();

                if (MyInfo == null || !(bool)MyInfo.InJob) return false;

                //判断权限
                if (PermissionId > 0)
                    return PermissionManager.CheckPermission(MyInfo, PermissionId);
                else if (!string.IsNullOrEmpty(PermissionUrl))
                {
                    var urls = PermissionUrl.TrimEnd(',').ToLower().Split(',');
                    foreach (var url in urls)
                    {
                        //有一个Url允许，则通过
                        if (PermissionManager.CheckPermission(MyInfo, url))
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(PermissionFilterAttribute), ex, 0, "");
                return false;
            }
        }

        /// <summary>
        /// 验证失败处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpUnauthorizedResult(); //Ajax请求，返回401
            }
            else
            {
                //获取当前路由信息
                string areaName = filterContext.RouteData.DataTokens["area"]?.ToString() ?? "";
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                string actionName = filterContext.RouteData.Values["action"].ToString();

                //拼接Url
                string url = "/" + controllerName + "/" + actionName;
                if (areaName != "")
                    url = "/" + areaName + url;

                //直接JS跳回上页，并提示
                var content = new ContentResult()
                {
                    Content = "<script type=\"text/javascript\">" +
                    "window.history.go(-1);" +
                    "alert('您没有该操作权限！');" +
                    "</script>"
                };
                //或者直接跳转页面
                filterContext.Result = new RedirectResult("/Home/UnAuthorized");//content
            }
            return;
        }
    }
}