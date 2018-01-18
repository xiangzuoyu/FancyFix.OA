using FancyFix.Core.Admin;
using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Web.Mvc;

namespace FancyFix.OA.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private static string sitePreName = "FancyFix_";
        private static string ErrorRate = "ErrorRate";
        private int LimitTime = 3; 

        public ActionResult Login() 
        {
            string username = string.Empty;
            if (Request.Cookies[sitePreName + "AdminName"] != null)
                username = Request.Cookies[sitePreName + "AdminName"].Value.ToString2();
            ViewBag.username = username;
            ViewBag.showCode = ShowCode();
            return View();
        }

        public ActionResult SignIn()
        {
            string userName = RequestString("username");
            string password = RequestString("password");
            string checkCode = RequestString("checkcode").ToUpper();

            if (userName == "" || password == "")
                return MessageBoxAndReturn("请把表单填写完整！");

            if (ShowCode())
            {
                if (checkCode == "")
                    return MessageBoxAndReturn("请把表单填写完整！");
                if (Session["ImageCode"] == null)
                    return MessageBoxAndJump("验证码过期，请重新登录！", "/auth/login");
                if (Session["ImageCode"].ToString() != checkCode)
                    return MessageBoxAndReturn("验证码出错！");
            }

            Mng_User model = Bll.BllMng_User.CheckLogin(userName, password, Request.UserHostAddress.ToString());
            if (model == null || model.Id == 0)
            {
                //登录错误次数
                if (Session[ErrorRate] == null)
                    Session.Add(ErrorRate, 1);
                else
                {
                    Session[ErrorRate] = (int)Session[ErrorRate] + 1;
                    if ((int)Session[ErrorRate] == LimitTime)
                        return Redirect("/auth/login");
                }

                return MessageBoxAndReturn("用户名或密码出错！");
            }
            else
            {
                Response.Cookies[sitePreName + "AdminName"].Value = userName;
                Response.Cookies[sitePreName + "AdminName"].Expires = DateTime.Now.AddDays(30);

                //更新最近一次登录部门Id
                Tools.Utility.Admin.SetSession(model.Id, model);

                //清除权限缓存
                PermissionManager.ClearPermissions(model.Id);

                //清除登录次数限制
                if (Session[ErrorRate] != null)
                    Session.Remove(ErrorRate);

                return Redirect("/");
            }
        }

        public ActionResult LogOut()
        {
            //清除当前用户缓存和权限缓存
            Tools.Utility.Admin.Remove();

            //清除权限
            int adminId = Tools.Utility.Admin.GetAdminId();
            PermissionManager.ClearPermissions(adminId);

            return Redirect("/auth/login");
        }


        private bool ShowCode()
        {
            return Session[ErrorRate] != null && (int)Session[ErrorRate] >= LimitTime;
        }
    }
}