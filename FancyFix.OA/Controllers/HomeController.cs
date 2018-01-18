using System.Web.Mvc;
using FancyFix.OA.Filter;
using FancyFix.OA.Base;

namespace FancyFix.OA.Controllers
{
    [CheckLogin]
    public class HomeController : BaseAdminController
    {
        public ActionResult Index()
        {
            var adminInfo = Bll.BllMng_User.First(o => o.Id == MyInfo.Id);
            return View(adminInfo);
        }

        public ActionResult Default()
        {
            var adminInfo = Bll.BllMng_User.First(o => o.Id == MyInfo.Id);
            //ViewBag.role = string.Join(",", Bll.BllMng_PermissionGroup.GetGruopNames(adminInfo.GroupManage, MyDepartId););
            return View(adminInfo);
        }

        public ActionResult Error()
        {
            ViewBag.msg = RequestString("msg");
            return View("error");
        }

        public ActionResult UnAuthorized()
        {
            return View("unauthorized");
        }
    }
}