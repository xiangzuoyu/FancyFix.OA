using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;

namespace FancyFix.OA.Areas.ArtTask.Controllers
{
    public class ArtTaskListController : BaseAdminController
    {
        // GET: ArtTask/ArtTaskList
        public ActionResult List()
        {
            //获取设计部分配任务权限ID
            string arrIds = ConfigurationManager.AppSettings["ArtTaskIds"];
            string adminIds = ConfigurationManager.AppSettings["ArtTaskAdminIds"];
            ViewBag.CurrentId = MyInfo.Id;
            ViewBag.IsDesigner = (arrIds.Contains($",{MyInfo.Id},") || adminIds.Contains($",{MyInfo.Id},"));
            ViewBag.IsDesignerAdmin = adminIds.Contains($",{MyInfo.Id},");
            ViewBag.AdminIds = adminIds;
            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0, int displayid = 0)
        {
            long records = 0;
            var list = Bll.BllArtTaskList.PageList(page, pagesize, out records, displayid);
            var adminlist = AdminData.GetList();
            foreach (var item in list)
            {
                item.SubmitterName = GetUserNameById(item.SubmitterId, adminlist);
                item.DesignerName = GetUserNameById(item.DesignerId, adminlist);
            }

            return BspTableJson(list, records);
        }

        //根据用户ID获取用户昵称
        private string GetUserNameById(int? id, List<Mng_User> list)
        {
            string name = "";
            if (id != null && id > 0 && list != null)
                name = list.Find(o => o.Id == id)?.RealName ?? "";

            return name;
        }

        //取消需求
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllArtTaskList.CancelTask(id) });
        }

        //完成需求
        public JsonResult Complete(int id)
        {
            return Json(new { result = Bll.BllArtTaskList.CompleteTask(id) });
        }

        #region 添加需求
        //新增需求
        public ActionResult Insert()
        {
            ViewBag.DemandTypeList = Bll.BllDemandType.GetList() ?? new List<DemandType>();
            ViewBag.DetailTypeList = Bll.BllDetailType.GetList() ?? new List<DetailType>();
            return View();
        }
        /// <summary>
        /// 添加需求
        /// </summary>
        /// <param name="artTaskList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(ArtTaskList artTaskList)
        {
            artTaskList.SubmitterId = MyInfo.Id;
            artTaskList.SubmittedDate = DateTime.Now;
            artTaskList.Display = 1;
            artTaskList.DepartmentId = MyInfo.DepartId;

            string msg = Bll.BllArtTaskList.Insert(artTaskList) > 0 ? "成功" : "失败";
            return LayerMsgSuccessAndRefresh("添加" + msg);
        }
        #endregion

        #region 分配需求

        //修改需求
        public ActionResult Edit(int id = 0, string datetime = "", int designerId = 0)
        {
            //加载需求
            var adminlist = AdminData.GetList() ?? new List<Mng_User>();
            List<Mng_User> userList = null;
            userList = adminlist.FindAll(o => o.DepartId == 10) ?? new List<Mng_User>();
            //没有分配权限时，只显示自己
            if (ConfigurationManager.AppSettings["ArtTaskIds"].ToString().Contains($",{MyInfo.Id},"))
            {
                designerId = MyInfo.Id;
                userList = userList.FindAll(o => o.Id == MyInfo.Id);
            }
            ViewBag.designerList = userList ?? new List<Mng_User>();

            ArtTaskList model = null;
            if (id > 0)
                model = Bll.BllArtTaskList.First(o => o.Id == id);
            if (model == null)
                return LayerAlertSuccessAndRefresh("加载需求失败，未找到该需求");

            //判断需求是否已被领取
            if (model.Display == 2 && model.DesignerId != MyInfo.Id&& !ConfigurationManager.AppSettings["ArtTaskAdminIds"].ToString().Contains($",{MyInfo.Id},"))
                return LayerAlertAndCallback("改需求已被其他人领取，请重新选择", "getTasklist.addDataFail()");

            //设置开始时间
            if (!string.IsNullOrEmpty(datetime) && model.Display == 1)
            {
                model.DesignerId = designerId;
                model.EstimatedStartDate = datetime.ToDateTime();
            }

            //显示部门名称
            if (model.DepartmentId != null && model.DepartmentId > 0)
                model.DepartmentName = Bll.BllMng_DepartmentClass.First(o => o.Id == model.DepartmentId)?.ClassName;
            //提交人
            if (model.SubmitterId != null && model.SubmitterId > 0)
                model.SubmitterName = Bll.BllMng_User.First(o => o.Id == model.SubmitterId)?.RealName;

            ViewBag.StartDate = model.EstimatedStartDate;
            ViewBag.DemandTypeList = Bll.BllDemandType.GetList() ?? new List<DemandType>();
            ViewBag.DetailTypeList = Bll.BllDetailType.GetList() ?? new List<DetailType>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ArtTaskList artTaskList)
        {
            ArtTaskList model = Bll.BllArtTaskList.First(o => o.Id == artTaskList.Id);
            if (model == null)
                return LayerAlertSuccessAndRefresh("分配需求失败，未找到该需求");
            //未领取过的任务，保存时需要判断是否已被领取
            bool IsExist = artTaskList.Display == 1;

            model.DesignerId = artTaskList.DesignerId;
            model.AMPM = artTaskList.AMPM;
            model.EstimatedStartDate = artTaskList.EstimatedStartDate;
            model.EstimatedEndDate = artTaskList.EstimatedEndDate;
            model.Display = 2;

            //判断需求是否已被领取
            if (IsExist)
            {
                ArtTaskList newModel = Bll.BllArtTaskList.First(o => o.Id == artTaskList.Id);
                if (newModel != null && newModel.Display == 2)
                    return LayerAlertAndCallback("改需求已被其他人领取，请重新选择", "getTasklist.addDataFail()");
            }

            string msg = Bll.BllArtTaskList.Update(model) > 0 ? "成功" : "失败";
            return LayerAlertAndCallback("编辑" + msg, "getTasklist.addDataSuccess()");
        }

        #endregion

        #region 日历显示
        public ActionResult ShowCalendar(int id = 0)
        {
            ArtTaskList model = null;
            //选中当前需求
            if (id > 0)
                model = Bll.BllArtTaskList.First(o => o.Id == id);
            ViewBag.TaskId = id;

            ViewBag.CurrentDesigner = 0;
            if (model != null && model.Display == 2)
                ViewBag.CurrentDesigner = model?.DesignerId;
            //如果当前登录是设计师且没有分配的权限，优先显示他的任务
            if (ConfigurationManager.AppSettings["ArtTaskIds"].ToString().Contains($",{MyInfo.Id},"))
                ViewBag.CurrentDesigner = MyInfo.Id;

            //获取设计部人员列表
            var adminlist = AdminData.GetList() ?? new List<Mng_User>();
            ViewBag.designerList = adminlist.FindAll(o => o.DepartId == 10) ?? new List<Mng_User>();

            return View(model);
        }

        //加载指定日期的最近三个月的任务
        [HttpGet]
        public JsonResult TaskList(int designerId = 0)
        {
            //设置日历显示的日期范围
            DateTime starttime = RequestString("starttime").Trim().ToDateTime().AddMonths(-1);
            DateTime endtime = RequestString("endtime").Trim().ToDateTime().AddMonths(1);
            var list = Bll.BllArtTaskList.GetList(starttime, endtime, designerId);
            return Json(new { result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 给设计师打call
        public ActionResult DaCall(int id = 0)
        {
            if (id < 1)
                return LayerMsgErrorAndClose("加载需求ID失败");

            //ViewBag.TaskId = id;

            return View(id);
        }

        [HttpPost]
        public ActionResult DaCall(int id = 0, int rating = 1)
        {
            if (id < 1)
                return LayerMsgErrorAndClose("加载需求ID失败，请联系管理员！");

            ArtTaskList model = null;
            model = Bll.BllArtTaskList.First(o => o.Id == id);
            if (model == null)
                return LayerMsgErrorAndClose("未找到该需求，请联系管理员！");

            model.Score = rating;
            model.Display = 5;
            string msg = Bll.BllArtTaskList.Update(model) > 0 ? "成功" : "失败";
            return LayerMsgSuccessAndRefresh("打分" + msg);
        }
        #endregion
    }
}