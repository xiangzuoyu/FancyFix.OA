using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;
using FancyFix.OA.Filter;
using System.Linq;

namespace FancyFix.OA.Areas.ArtTask.Controllers
{
    [CheckLogin]
    public class ArtTaskListController : BaseAdminController
    {
        //设计部门Ids
        List<int> designDepartIds = DesignDepartId.Split(',').Select(o => o.ToInt32()).ToList();

        //获取设计部分配任务权限ID
        List<int> arrIds = Bll.BllDesign_ArtTaskList.DesignIds(DesignDepartId, 0);
        List<int> adminIds = Bll.BllDesign_ArtTaskList.DesignIds(DesignDepartId, 1);

        // GET: ArtTask/ArtTaskList
        public ActionResult List()
        {
            ViewBag.CurrentId = MyInfo.Id;
            ViewBag.IsDesigner = (arrIds.Contains(MyInfo.Id) || adminIds.Contains(MyInfo.Id));
            ViewBag.IsDesignerAdmin = adminIds.Contains(MyInfo.Id);
            ViewBag.AdminIds = adminIds;
            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0, int displayid = 0)
        {
            //非设计部门，只能查看自己的需求
            int submitterId = 0;
            if (!arrIds.Contains(MyInfo.Id) && !adminIds.Contains(MyInfo.Id))
                submitterId = MyInfo.Id;
            long records = 0;
            var list = Bll.BllDesign_ArtTaskList.PageList(submitterId, page, pagesize, out records, displayid);
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
                name = list.Find(o => o.Id == id)?.RealName;

            return name;
        }

        //取消需求
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllDesign_ArtTaskList.CancelTask(id) });
        }

        //完成需求
        public JsonResult Complete(int id)
        {
            return Json(new { result = Bll.BllDesign_ArtTaskList.CompleteTask(id) });
        }

        #region 添加需求
        //新增需求
        public ActionResult Insert()
        {
            ViewBag.Tel = Bll.BllMng_User.FirstSelect(o => o.Id == MyInfo.Id, o => o.Tel)?.Tel ?? "";
            ViewBag.DemandTypeList = Bll.BllDesign_DemandType.GetList() ?? new List<Design_DemandType>();
            ViewBag.DetailTypeList = Bll.BllDesign_DetailType.GetList() ?? new List<Design_DetailType>();
            return View();
        }
        /// <summary>
        /// 添加需求
        /// </summary>
        /// <param name="artTaskList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(Design_ArtTaskList artTaskList)
        {
            var nowdate = DateTime.Now;
            artTaskList.Number = $"{nowdate.ToString("yyyyMMddHHmmss")}{MyInfo.Id}";
            artTaskList.SubmitterId = MyInfo.Id;
            artTaskList.SubmittedDate = nowdate;
            artTaskList.Display = 1;
            artTaskList.DepartmentId = MyInfo.DepartId;
            artTaskList.Uri1 = GetFile();

            string msg = Bll.BllDesign_ArtTaskList.Insert(artTaskList) > 0 ? "成功" : "失败";
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
            userList = adminlist.FindAll(o => o.InJob == true && designDepartIds.Contains(o.DepartId.Value)) ?? new List<Mng_User>();
            //没有分配权限时，只显示自己
            if (arrIds.Contains(MyInfo.Id)
                && !ConfigurationManager.AppSettings["SuperAdminIds"].ToString().Contains($"{MyInfo.Id}"))
            {
                designerId = MyInfo.Id;
                userList = userList.FindAll(o => o.Id == MyInfo.Id);
            }
            ViewBag.designerList = userList ?? new List<Mng_User>();

            Design_ArtTaskList model = null;
            if (id > 0)
                model = Bll.BllDesign_ArtTaskList.First(o => o.Id == id);
            if (model == null)
                return LayerAlertSuccessAndRefresh("加载需求失败，未找到该需求");

            //判断需求是否已被领取
            if (model.Display == 2 && model.DesignerId != MyInfo.Id && !adminIds.Contains(MyInfo.Id))
                return LayerAlertAndCallback("该需求已被其他人领取，请重新选择", "getTasklist.addDataFail()");

            //设置表单初始值
            if (!string.IsNullOrEmpty(datetime) && model.Display == 1)
            {
                if (designerId > 0)
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
            ViewBag.DemandTypeList = Bll.BllDesign_DemandType.GetList() ?? new List<Design_DemandType>();
            ViewBag.DetailTypeList = Bll.BllDesign_DetailType.GetList() ?? new List<Design_DetailType>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Design_ArtTaskList artTaskList)
        {
            Design_ArtTaskList model = Bll.BllDesign_ArtTaskList.First(o => o.Id == artTaskList.Id);
            if (model == null)
                return LayerAlertSuccessAndRefresh("分配需求失败，未找到该需求");
            //未领取过的任务，保存时需要判断是否已被领取
            bool IsExist = artTaskList.Display == 1;

            model.DesignerId = artTaskList.DesignerId;
            model.AMPM = artTaskList.AMPM;
            model.EstimatedStartDate = artTaskList.EstimatedStartDate;
            model.EstimatedEndDate = artTaskList.EstimatedEndDate;
            if (model.AssigneeId == null)
                model.AssigneeId = MyInfo.Id;
            model.Display = 2;
            model.Uri2 = GetPic();

            //判断需求是否已被领取
            if (IsExist)
            {
                Design_ArtTaskList newModel = Bll.BllDesign_ArtTaskList.First(o => o.Id == artTaskList.Id);
                if (newModel != null && newModel.Display == 2)
                    return LayerAlertAndCallback("改需求已被其他人领取，请重新选择", "getTasklist.addDataFail()");
            }

            string msg = Bll.BllDesign_ArtTaskList.Update(model) > 0 ? "成功" : "失败";
            return LayerAlertAndCallback("编辑" + msg, "getTasklist.addDataSuccess()");
        }

        #endregion

        #region 日历显示
        public ActionResult ShowCalendar(int id = 0)
        {
            Design_ArtTaskList model = null;
            //选中当前需求
            if (id > 0)
                model = Bll.BllDesign_ArtTaskList.First(o => o.Id == id);
            ViewBag.TaskId = id;

            ViewBag.CurrentDesigner = 0;
            if (model != null && model.Display == 2)
                ViewBag.CurrentDesigner = model.DesignerId ?? 0;
            //如果当前登录是设计师且没有分配的权限，优先显示他的任务
            if (arrIds.Contains(MyInfo.Id)
                && !adminIds.Contains(MyInfo.Id))
                ViewBag.CurrentDesigner = MyInfo.Id;

            //获取设计部人员列表
            var adminlist = AdminData.GetList() ?? new List<Mng_User>();
            ViewBag.designerList = adminlist.FindAll(o => o.InJob == true && designDepartIds.Contains(o.DepartId.Value)) ?? new List<Mng_User>();

            return View(model);
        }

        //加载指定日期的最近三个月的任务
        [HttpGet]
        public JsonResult TaskList(int designerId = 0)
        {
            //设置日历显示的日期范围
            DateTime starttime = RequestString("starttime").Trim().ToDateTime().AddMonths(-1);
            DateTime endtime = RequestString("endtime").Trim().ToDateTime().AddMonths(1);
            var list = Bll.BllDesign_ArtTaskList.GetList(starttime, endtime, designerId);
            var adminlist = AdminData.GetList();
            foreach (var item in list)
                item.DesignerName = GetUserNameById(item.DesignerId, adminlist);
            return Json(new { result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public ActionResult DesignUpLoad(int id = 0)
        {
            Design_ArtTaskList model = null;
            //选中当前需求
            if (id > 0)
                model = Bll.BllDesign_ArtTaskList.First(o => o.Id == id);
            if (model == null)
                return LayerMsgErrorAndClose("加载需求失败");

            //ViewBag.TaskId = id;

            return View(model);
        }

        [HttpPost]
        public ActionResult DesignUpLoadPost(int id = 0)
        {
            Design_ArtTaskList model = null;
            //选中当前需求
            if (id > 0)
                model = Bll.BllDesign_ArtTaskList.First(o => o.Id == id);
            if (model == null)
                return LayerMsgErrorAndClose("加载需求失败");

            model.Uri2 = GetPic();

            string msg = Bll.BllDesign_ArtTaskList.Update(model) > 0 ? "成功" : "失败";
            return LayerMsgSuccessAndRefresh("上传" + msg);
        }

        #region 给设计师打call
        public ActionResult DaCall(int id = 0)
        {
            if (id < 1)
                return LayerMsgErrorAndClose("加载需求失败");

            Design_ArtTaskList model = null;
            //选中当前需求
            if (id > 0)
                model = Bll.BllDesign_ArtTaskList.First(o => o.Id == id);
            if (model == null)
                return LayerMsgErrorAndClose("加载需求失败");

            ViewBag.Assignee = model.AssigneeId == MyInfo.Id;


            return View(id);
        }

        [HttpPost]
        public ActionResult DaCall(int id = 0, int rating = 10, string comment = "")
        {
            if (id < 1)
                return LayerMsgErrorAndClose("加载需求失败，请联系管理员！");

            Design_ArtTaskList model = null;
            model = Bll.BllDesign_ArtTaskList.First(o => o.Id == id);
            if (model == null)
                return LayerMsgErrorAndClose("未找到该需求，请联系管理员！");

            //当前用户ID=任务分配ID时，为总监打分,否则为客户打分
            if (model.AssigneeId == MyInfo.Id)
            {
                model.AssigneeScore = rating;
                model.AssigneeComment = comment;
            }
            else
            {
                model.Score = rating;
                model.Comment = comment;
            }

            // 当总监和客户全部打完分状态或是总监发布的任务才会改成已完成
            if ((model.Score != null && model.Comment != null && model.AssigneeScore != null && model.AssigneeComment != null)
                || model.SubmitterId == model.AssigneeId)
                model.Display = 5;
            string msg = Bll.BllDesign_ArtTaskList.Update(model) > 0 ? "成功" : "失败";
            return LayerMsgSuccessAndRefresh("打分" + msg);
        }
        #endregion

        [HttpGet]
        public ActionResult SeeDetails(int id = 0)
        {
            Design_ArtTaskList model = null;
            //选中当前需求
            if (id > 0)
                model = Bll.BllDesign_ArtTaskList.First(o => o.Id == id);
            if (model == null)
                return LayerMsgErrorAndClose("加载需求失败");

            //显示部门名称
            //if (model.DepartmentId != null && model.DepartmentId > 0)
            //    model.DepartmentName = Bll.BllMng_DepartmentClass.First(o => o.Id == model.DepartmentId)?.ClassName;

            string weburl = ConfigurationManager.AppSettings["weburl"];
            var adminlist = AdminData.GetList() ?? new List<Mng_User>();
            if (model.SubmitterId != null && model.SubmitterId > 0)
                model.SubmitterName = adminlist.Find(o => o.Id == model.SubmitterId)?.RealName;
            if (model.DesignerId != null && model.DesignerId > 0)
                model.DesignerName = adminlist.Find(o => o.Id == model.DesignerId)?.RealName;
            if (model.DepartmentId != null && model.DepartmentId > 0)
                model.DepartmentName = Bll.BllMng_DepartmentClass.First(o => o.Id == model.DepartmentId)?.ClassName;
            if (model.DemandTypeId != null && model.DemandTypeId > 0)
                model.DemandTypeName = Bll.BllDesign_DemandType.First(o => o.ClassId == model.DemandTypeId)?.Name;
            if (model.DetailTypeId != null && model.DetailTypeId > 0)
                model.DetailTypeName = Bll.BllDesign_DetailType.First(o => o.ClassId == model.DetailTypeId)?.Name;

            model.Uri1 = !string.IsNullOrEmpty(model.Uri1) ? model.Uri1 : "-";
            model.Uri2 = !string.IsNullOrEmpty(model.Uri2) ? model.Uri2 : "-";
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public void SeeDetails(string content)
        {
            Tools.Tool.ExcelHelper.ExportResult(content, DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }
    }
}