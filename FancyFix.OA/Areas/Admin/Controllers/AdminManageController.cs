using FancyFix.OA.Filter;
using FancyFix.OA.Model;
using FancyFix.OA.ViewModel;
using FancyFix.Tools.Log4netExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Admin.Controllers
{
    public class AdminManageController : Base.BaseAdminController
    {
        #region 人员列表
        public ActionResult List()
        {
            if (IsSuperAdmin)
                ViewBag.showClass = Bll.BllMng_DepartmentClass.Instance().ShowClass(0, 0, false);
            else
                ViewBag.showClass = GetDepartOptions();
            return View();
        }

        //[PermissionFilter("/admin/adminmanage/list")]
        public JsonResult PageList()
        {
            long records = 0;
            int page = RequestInt("page");
            int pagesize = RequestInt("pagesize");
            int searchType = RequestInt("searhtype");
            string keyword = RequestString("keyword").Trim2();
            int departId = RequestInt("departid");
            int groupId = RequestInt("groupid");

            string userName = string.Empty;
            string realName = string.Empty;
            if (searchType == 1) userName = keyword;
            if (searchType == 2) realName = keyword;

            var list = Bll.BllMng_User.PageList(userName, realName, departId, groupId, page, pagesize, ref records);
            var adminlist = AdminData.GetList();
            foreach (var item in list)
            {
                item.DepartMentName = item.ClassName;
                if (item.ParUserId != null && item.ParUserId > 0 && adminlist != null)
                    item.ParUserName = adminlist.Find(o => o.Id == item.ParUserId)?.RealName ?? "";
                else
                    item.ParUserName = "";
            }
            return BspTableJson(list, records);
        }

        [HttpPost]
        public JsonResult GetGroup(int id)
        {
            var list = Bll.BllMng_PermissionGroup.Query(o => o.DepartId == id);
            return Json(list);
        }

        /// <summary>
        /// 设置在职
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public int SetJob(int id)
        {
            var model = Bll.BllMng_User.First(o => o.Id == id);
            if (model == null) return 0;
            if (CheckSuperAdmin(model.Id))
                model.InJob = true;
            else
                model.InJob = !model.InJob;
            int count = Bll.BllMng_User.Update(model, o => o.Id == id);
            if (count > 0) AdminData.Reload();
            return count;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public int Delete(int id)
        {
            //自己不可删
            if (MyInfo.Id == id)
                return 0;
            int count = Bll.BllMng_User.Delete(o => o.Id == id);
            if (count > 0) AdminData.Reload();
            return count;
        }
        #endregion

        #region 人员编辑

        public ActionResult Add()
        {
            ViewBag.showLayout = RequestBool("layout");
            ViewBag.departmentHtml = GetDepartOptions();
            return View();
        }

        public ActionResult Edit(int id)
        {
            var model = Bll.BllMng_User.First(o => o.Id == id);
            if (model == null)
                return MessageBoxAndReturn("用户不存在！");

            ViewBag.departmentHtml = GetDepartOptions(model.DepartId.Value);
            return View(model);
        }


        //[PermissionFilter("/admin/adminmanage/edit,/admin/adminmanage/add")]
        [HttpPost]
        [ModelValidFilter(IgnoreField = "password")]
        public ActionResult Save(AdminInfoModel model)
        {
            bool showlayout = RequestBool("showlayout");
            var mod = new Mng_User();
            if (model.id == 0 && string.IsNullOrEmpty(model.password))
            {
                if (showlayout)
                    return MessageBoxAndReturn("请填写密码！");
                else
                    return LayerAlertErrorAndReturn("请填写密码！");
            }
            if (!string.IsNullOrEmpty(model.password))
                mod.Password = Tools.Common.Common.Md5(model.password, 32);
            if (model.departid == 0)
            {
                if (showlayout)
                    return MessageBoxAndReturn("请选择部门！");
                else
                    return LayerAlertErrorAndReturn("请选择部门！");
            }
            mod.UserName = model.username.Trim2();
            mod.RealName = model.realname.Trim2();
            mod.DepartId = model.departid;
            mod.DepartMentName = Bll.BllMng_DepartmentClass.Instance().GetClassName(model.departid);
            mod.GroupId = model.groupid;
            mod.ParUserId = model.paruserid;
            mod.Sex = model.sex;

            int result = 0;
            if (model.id > 0)
            {
                result = Bll.BllMng_User.UpdateInfo(mod, model.id);
                if (result > 0)
                {
                    AdminData.Reload();
                    return LayerAlertSuccessAndRefresh("保存成功！");
                }
            }
            else
            {
                mod.LoginIp = "127.0.0.1";
                mod.LoginTime = DateTime.Parse("1999-01-01");
                mod.LoginTimes = 0;
                mod.Pic = "";
                mod.InJob = true;
                result = Bll.BllMng_User.AddNoReturn(mod);
                if (result > 0)
                {
                    if (showlayout)
                    {
                        AdminData.Reload();
                        return MessageBoxAndJump("保存成功！", "/admin/adminmanage/add?layout=true");
                    }
                    else
                        return LayerAlertSuccessAndRefresh("保存成功！");
                }
            }
            if (result == -1)
            {
                if (showlayout)
                    return MessageBoxAndReturn("用户名已存在！");
                else
                    return LayerAlertErrorAndReturn("用户名已存在！");
            }
            else
            {
                if (showlayout)
                    return MessageBoxAndReturn("保存失败！");
                else
                    return LayerAlertErrorAndReturn("保存失败！");
            }
        }

        //[PermissionFilter("/admin/adminmanage/edit,/admin/adminmanage/add")]
        [HttpPost]
        public JsonResult GetPermissionGroup(int departId)
        {
            var list = Bll.BllMng_PermissionGroup.GetAllList(departId);
            return Json(list, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region 部门人员树结构

        public ActionResult GetAdminTree()
        {
            int userId = RequestInt("userid");
            var departlist = Bll.BllMng_DepartmentClass.Instance().GetListByParentId(0, false);
            var adminlist = Bll.BllMng_User.GetAllList(true);
            StringBuilder strShowClass = new StringBuilder();
            GetTreeHtml(departlist, 0, adminlist, strShowClass, userId);
            ViewBag.strShowClass = strShowClass;
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