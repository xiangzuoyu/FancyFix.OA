using System.Web.Mvc;
using FancyFix.OA.Filter;
using FancyFix.OA.Base;
using System.Text;
using System.Collections.Generic;
using FancyFix.OA.Model;
using System.Linq;

namespace FancyFix.OA.Controllers
{
    [CheckLogin]
    public class CommonController : BaseAdminController
    {
        #region 部门人员树结构

        public ActionResult GetAdminTree(string callback = "callback", string isSingle = "false")
        {
            int userId = RequestInt("userid");
            var departlist = Bll.BllMng_DepartmentClass.Instance().GetListByParentId(0, false);
            var adminlist = Bll.BllMng_User.GetAllList(true);
            StringBuilder strShowClass = new StringBuilder();
            GetTreeHtml(departlist, 0, adminlist, strShowClass, userId);
            ViewBag.strShowClass = strShowClass;
            ViewBag.callback = callback;
            ViewBag.isSingle = isSingle;
            return View();
        }

        private void GetTreeHtml(IEnumerable<Mng_DepartmentClass> depart, int parId, IEnumerable<Mng_User> adminlist, StringBuilder strShowClass, int userId)
        {
            var childlist = depart.Where(o => o.ParId == parId);
            foreach (var item in childlist)
            {
                var list = adminlist.Where(o => o.DepartId == item.Id);
                if (item.ChildNum > 0)
                {
                    strShowClass.Append("<li><i class=\"layui-icon layui-tree-spread\">&#xe623;</i>");
                    strShowClass.Append("<a href=\"javascript:;\"><cite>" + item.ClassName + "</cite></a>");

                    strShowClass.Append("<ul>"); // <ul class=\"layui-show\">
                    GetTreeHtml(depart, item.Id, adminlist, strShowClass, userId); //递归添加子部门
                    GetAdminHtml(list, strShowClass, userId); //添加当前部门下员工
                    strShowClass.Append("</ul>");

                }
                else
                {
                    strShowClass.Append("<li><i class=\"layui-icon layui-tree-spread\">&#xe623;</i>");
                    strShowClass.Append("<a href=\"javascript:;\"><cite>" + item.ClassName + "</cite></a>");

                    strShowClass.Append("<ul>");
                    GetAdminHtml(list, strShowClass, userId); //添加当前部门下员工
                    strShowClass.Append("</ul>");

                    strShowClass.Append("</li>");
                }
            }
        }

        private void GetAdminHtml(IEnumerable<Mng_User> adminlist, StringBuilder strShowClass, int userId)
        {
            foreach (var admin in adminlist)
            {
                strShowClass.Append("<li><i class=\"layui-icon\"></i><i class=\"layui-icon\">&#xe612;</i>");
                strShowClass.Append("<a href=\"javascript:;\"><input type=\"checkbox\" name=\"check\" value=\"" + admin.Id + "\" " + (userId == admin.Id ? "checked=\"checked\"" : "") + "><cite>" + admin.RealName + "</cite></a>");
                strShowClass.Append("</li>");
            }
        }

        #endregion
    }
}