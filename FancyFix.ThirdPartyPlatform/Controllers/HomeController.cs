using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.ThirdPartyPlatform.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ToWeixin()
        {
            string url = Request.Params["url"].ToString2().ToUrlEncode();
            if (string.IsNullOrEmpty(url) || !url.Contains("http://") || !url.Contains("https://"))
                url = "https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzA3OTM5NTkxNA==&scene=124#wechat_redirect";
            return Redirect(url);
        }
    }
}