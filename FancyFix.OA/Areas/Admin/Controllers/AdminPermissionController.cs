using FancyFix.OA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Admin.Controllers
{
    public class AdminPermissionController : OA.Base.BaseAdminController
    {
        public ActionResult GroupSetTree()
        {
            int adminid = RequestInt("id");

            //初始化组数据
            var adminModel = Bll.BllMng_User.First(o => o.Id == adminid);
            if (adminModel == null)
                return LayerAlertErrorAndReturn("访问出错！");

            List<Mng_PermissionPersonSet> oaPermissionList = Bll.BllMng_PermissionPersonSet.GetPermissionList(adminid);
            List<int> oaPermissions = new List<int>();
            if (oaPermissionList != null && oaPermissionList.Count > 0)
            {
                foreach (var item in oaPermissionList)
                    oaPermissions.Add((int)item.PermissionId);
            }

            IEnumerable<Mng_MenuClass> list = Bll.BllSys_MenuClass.Instance().GetListByParentId(0, true);
            StringBuilder strShowClass = new StringBuilder();

            GetListByParId(list, 0, strShowClass, oaPermissions);

            ViewBag.strShowClass = strShowClass;
            return View(adminModel);
        }

        private void GetListByParId(IEnumerable<Mng_MenuClass> list, int parId, StringBuilder strShowClass, List<int> SelectedValue)
        {
            var childlist = list.Where(o => o.ParId == parId);
            if (childlist != null && childlist.Count() > 0)
            {
                foreach (var item in childlist)
                {
                    if (item.ChildNum > 0)
                    {
                        strShowClass.Append("<li><i class=\"layui-icon layui-tree-spread\">&#xe623;</i><i class=\"layui-icon\">&#xe622;</i>");
                        strShowClass.Append("<a href=\"javascript:;\"><input type=\"checkbox\" name=\"check\" value=" + item.Id + " " + (SelectedValue.Contains(item.Id) || parId == 0 ? "checked='checked'" : "") + "><cite>" + item.ClassName + "</cite></a>");
                        strShowClass.Append("<ul class=\"layui-show\">");

                        GetListByParId(list, item.Id, strShowClass, SelectedValue);

                        strShowClass.Append("</ul>");
                    }
                    else
                    {
                        if (item.BeLock.HasValue && item.BeLock.Value)
                            strShowClass.Append("<li><i class=\"layui-icon\"></i><i class=\"layui-icon\">&#xe614;</i>");
                        else
                            strShowClass.Append("<li><i class=\"layui-icon\"></i><i class=\"layui-icon\">&#xe621;</i>");

                        strShowClass.Append("<a href=\"javascript:;\"><input type=\"checkbox\" name=\"check\" value=" + item.Id + " " + (SelectedValue.Contains(item.Id) ? "checked='checked'" : "") + "><cite>" + item.ClassName + "</cite></a>");
                        strShowClass.Append("</li>");
                    }
                }
            }
        }

        //[PermissionFilter("/admin/adminpermission/groupsettree")]
        [HttpPost]
        public ActionResult SavePermission()
        {
            int adminid = RequestInt("id");
            //保存修改人员信息
            var adminModel = Bll.BllMng_User.First(o => o.Id == adminid);
            if (adminModel == null)
                return LayerAlertErrorAndReturn("访问出错！");

            //保存OA权限
            string check = RequestString("check");
            Bll.BllMng_PermissionPersonSet.SavePermission(adminid, check);
            return LayerAlertSuccess("保存成功！");
        }
    }
}