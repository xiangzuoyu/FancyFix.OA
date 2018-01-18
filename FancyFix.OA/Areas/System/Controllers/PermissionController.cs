using FancyFix.Core;
using FancyFix.OA.Areas.Controllers;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.System.Controllers
{
    public class PermissionController : OA.Base.BaseAdminController
    {
        private int departId = 0;

        #region 权限列表

        /// <summary>
        /// 查看部门列表
        /// </summary>
        /// <returns></returns>
        //[PermissionFilter("/system/permission/list")]
        public ActionResult GetDepartList()
        {
            departId = RequestInt("departId");
            string showClass = Bll.BllMng_DepartmentClass.Instance().ShowClass(departId, 0, false);
            ViewBag.showClass = showClass;
            return View();
        }

        public ActionResult List()
        {
            departId = RequestInt("departId");

            //超级管理员进来
            if (departId == 0)
                return RedirectToAction("getdepartlist", "permission", new { area = "system" });

            int id = RequestInt("id");
            var list = Bll.BllMng_PermissionGroup.GetAllList(departId, true);
            GetActStr(list);

            ViewBag.list = list;
            ViewBag.id = id;
            ViewBag.departid = departId;
            ViewBag.model = id > 0 ? Bll.BllMng_PermissionGroup.First(o => o.Id == id) : null;
            return View();
        }

        private void GetActStr(List<Mng_PermissionGroup> list)
        {
            if (list != null && list.Count > 0)
            {
                int listCount = list.Count;
                if (listCount > 0)
                {
                    for (int i = 0; i < listCount; i++)
                    {
                        StringBuilder actStr = new StringBuilder();
                        actStr.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"250\"><tr><form name=\"upform\"><td width=\"70\">");
                        if (i > 0)
                        {
                            actStr.Append("<input type=\"button\" name=\"Submit2\" value=\"↑\" class=\"btn btn-default\" onclick=\"window.location.href='/system/permission/up?id=" + list[i].Id + "&departid=" + departId + "'\"> ");
                        }

                        actStr.Append("</td></form><form name=\"downform\"><td  width=\"70\">");

                        if (i < listCount - 1)
                        {
                            actStr.Append("<input type=\"button\" name=\"Submit2\" value=\"↓\" class=\"btn btn-default\" onclick=\"window.location.href='/system/permission/down?id=" + list[i].Id + "&departid=" + departId + "'\"> ");
                        }
                        actStr.Append("</td></form></tr></table>");
                        list[i].actStr = actStr.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 下移分类
        /// </summary>
        public ActionResult Down()
        {
            int moveId = RequestInt("id");
            int departId = RequestInt("departid");
            if (moveId == 0)
            {
                return MessageBoxAndReturn("请选择移动分类！");
            }
            Bll.BllMng_PermissionGroup.Down(moveId, departId);
            return RedirectToAction("list", "permission", new { area = "system", departid = departId });
        }

        /// <summary>
        /// 上移分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Up()
        {
            int moveId = RequestInt("id");
            int departId = RequestInt("departid");
            if (moveId == 0)
            {
                return MessageBoxAndReturn("请选择移动分类！");
            }
            Bll.BllMng_PermissionGroup.Up(moveId, departId);
            return RedirectToAction("list", "permission", new { area = "system", departid = departId });
        }

        /// <summary>
        /// 权限设置
        /// </summary>
        /// <returns></returns>
        public ActionResult SetBeLock()
        {
            int id = RequestInt("id");
            int departId = RequestInt("departid");
            if (id == 0)
            {
                return MessageBoxAndReturn("访问出错！");
            }
            if (Bll.BllMng_PermissionGroup.SetBeLock(id))
                return RedirectToAction("list", "permission", new { area = "system", departid = departId });
            else
                return MessageBoxAndReturn("设置失败！");
        }

        /// <summary>
        /// 删除一个分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete()
        {
            int id = RequestInt("id");
            int departId = RequestInt("departid");
            if (Bll.BllMng_PermissionGroup.Delete(id, departId))
            {
                return MessageBoxAndJump("删除成功！", string.Format("/system/permission/list?departid={0}", departId));
            }
            else
            {
                return MessageBoxAndReturn("删除失败！");
            }
        }

        /// <summary>
        /// 增加一个分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            string groupName = RequestString("GroupName").Trim();
            string title = RequestString("Title").Trim();
            bool isAdmin = RequestBool("isadmin");
            int departId = RequestInt("departid");

            if (groupName == "")
                return MessageBoxAndReturn("请填写组名！");
            if (departId == 0)
                return MessageBoxAndReturn("请选择部门！");

            //新增组
            Mng_PermissionGroup modMng_PermissionGroup = new Mng_PermissionGroup();

            modMng_PermissionGroup.IsAdmin = isAdmin;
            modMng_PermissionGroup.GroupName = groupName;
            modMng_PermissionGroup.Title = title;
            modMng_PermissionGroup.BeLock = false;
            modMng_PermissionGroup.LastEditTime = DateTime.Now;
            modMng_PermissionGroup.LastEditor = MyInfo.Id;
            modMng_PermissionGroup.Sequence = 0;
            modMng_PermissionGroup.DepartId = departId;
            int flag = Bll.BllMng_PermissionGroup.AddNoReturn(modMng_PermissionGroup, departId);
            switch (flag)
            {
                case -1:
                    return MessageBoxAndReturn("存在相同组名！");
                case -2:
                    return MessageBoxAndReturn("增加出错！");
                case 0:
                    return MessageBoxAndJump("增加成功！", string.Format("/system/permission/list?departid={0}", departId));
            }
            return MessageBoxAndReturn("增加出错！");
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Update()
        {
            int id = Request["id"].ToInt32();
            string groupName = RequestString("GroupName").Trim();
            string title = RequestString("Title").Trim();
            bool isAdmin = RequestBool("isadmin");
            int departId = RequestInt("departid");

            if (groupName == "")
            {
                return MessageBoxAndReturn("请填写组名！");
            }
            if (id == 0)
            {
                return MessageBoxAndReturn("请选择编辑类！");
            }
            Mng_PermissionGroup modMng_PermissionGroup = Bll.BllMng_PermissionGroup.First(o => o.Id == id);
            if (modMng_PermissionGroup == null)
            {
                return MessageBoxAndReturn("没有获取到分类信息！");
            }
            else
            {
                modMng_PermissionGroup.IsAdmin = isAdmin;
                modMng_PermissionGroup.GroupName = groupName;
                modMng_PermissionGroup.Title = title;
                modMng_PermissionGroup.LastEditor = MyInfo.Id;
                modMng_PermissionGroup.LastEditTime = DateTime.Now;
                int res = Bll.BllMng_PermissionGroup.Update(modMng_PermissionGroup, o => o.Id == id);
                if (res > 0)
                    return MessageBoxAndJump("修改成功！", string.Format("/system/permission/list?departid={0}", departId));
                else
                    return MessageBoxAndReturn("修改失败！");
            }
        }
        #endregion

        #region 权限设置
        public ActionResult GroupSetTree()
        {
            int groupId = RequestInt("id");

            //初始化组数据
            var permissionModel = Bll.BllMng_PermissionGroup.First(o => o.Id == groupId);
            if (permissionModel == null)
                return LayerAlertErrorAndClose("访问出错！");

            List<Mng_PermissionGroupSet> oaPermissionList = Bll.BllMng_PermissionGroupSet.GetPermissionList(groupId);
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
            return View(permissionModel);
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
                        if ((bool)item.BeLock)
                            strShowClass.Append("<li><i class=\"layui-icon\"></i><i class=\"layui-icon\">&#xe614;</i>");
                        else
                            strShowClass.Append("<li><i class=\"layui-icon\"></i><i class=\"layui-icon\">&#xe621;</i>");

                        strShowClass.Append("<a href=\"javascript:;\"><input type=\"checkbox\" name=\"check\" value=" + item.Id + " " + (SelectedValue.Contains(item.Id) || parId == 0 ? "checked='checked'" : "") + "><cite>" + item.ClassName + "</cite></a>");
                        strShowClass.Append("</li>");
                    }
                }
            }
        }

        //[PermissionFilter("/admin/adminpermission/groupsettree")]
        [HttpPost]
        public ActionResult SavePermission()
        {
            int groupId = RequestInt("id");
            //保存修改人员信息
            var permissionModel = Bll.BllMng_PermissionGroup.First(o => o.Id == groupId);
            if (permissionModel == null)
                return LayerAlertErrorAndClose("访问出错！");

            //保存OA权限
            string check = RequestString("check");
            Bll.BllMng_PermissionGroupSet.SavePermission(groupId, check);

            permissionModel.LastEditTime = DateTime.Now;
            permissionModel.LastEditor = MyInfo.Id;
            Bll.BllMng_PermissionGroup.Update(permissionModel);

            return LayerAlertSuccess("保存成功！");
        }
        #endregion
    }
}