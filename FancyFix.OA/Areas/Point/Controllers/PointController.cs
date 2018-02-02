using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Point.Controllers
{
    public class PointController : BaseAdminController
    {
        public ActionResult PointIndex()
        {
            ViewBag.childCount = Bll.BllMng_User.GetChildCount(MyInfo.Id);
            return View();
        }

        #region 积分列表

        public ActionResult PointList()
        {
            int page = GetThisPage();
            int pageSize = 10;
            long records = 0;
            int isSelf = RequestInt("isself");

            var recordlist = Bll.BllPoint_Records.PageList(MyInfo.Id, isSelf, page, pageSize, out records);
            ViewBag.recordlist = recordlist;
            ViewBag.pageStr = ShowPage((int)records, pageSize, page, 5, "", false);
            ViewBag.isSelf = isSelf;
            return View();
        }

        public ActionResult PointEdit(int id = 0)
        {
            int pointId = RequestInt("pointId");

            Point_Records record = null;
            if (id > 0)
            {
                record = Bll.BllPoint_Records.First(o => o.Id == id);
                if (record == null) return MessageBoxAndReturn("发生错误，记录不存在！");
                pointId = record.PointId.Value;
            }

            Point_List pointModel = null;
            if (pointId > 0)
            {
                pointModel = Bll.BllPoint_List.First(o => o.Id == pointId);
                if (pointModel == null) return MessageBoxAndReturn("积分项不存在！");
            }
            ViewBag.pointId = pointId;
            ViewBag.pointModel = pointModel;
            return View(record);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PointSave()
        {
            int id = RequestInt("id");
            int pointId = RequestInt("pointId");
            int pointscore = RequestInt("pointscore");
            string eventtime = RequestString("eventtime");
            string content = RequestString("content");
            string userIds = RequestString("userid");

            if (string.IsNullOrEmpty(content)) return MessageBoxAndReturn("请填写事件描述！");
            if (string.IsNullOrEmpty(eventtime)) return MessageBoxAndReturn("请填写事发时间！");
            if (!Tools.Usual.Utils.IsDate(eventtime)) return MessageBoxAndReturn("请填写正确时间格式！");
            if (pointscore == 0) return MessageBoxAndReturn("请选择添加分值，不能为空或0！");

            //多选用户Id
            var uids = userIds.TrimEnd(',').Split(',');
            int userId = uids[0].ToInt32();

            Point_Records record = null;
            if (id > 0)
            {
                record = Bll.BllPoint_Records.First(o => o.Id == id);
                if (record == null) return MessageBoxAndReturn("发生错误，记录不存在！");
                if (record.IsApprove.Value) return MessageBoxAndReturn("该记录已审批过，禁止修改！");

                //自定义申请，目标员工
                record.UserId = userId > 0 ? userId : MyInfo.Id;
                record.UserName = userId > 0 ? Bll.BllMng_User.GetNameById(userId) : MyInfo.RealName;
                record.Score = pointscore;
                record.EventTime = eventtime.ToDateTime();
                record.Content = content;
                record.PointId = pointId;
                record.IsPass = false;
                record.UpdateTime = DateTime.Now;
                record.CreateUserId = MyInfo.Id;

                if (Bll.BllPoint_Records.Update(record, o => o.Id == id) > 0)
                    return MessageBoxAndJump("修改成功！", "/point/point/pointlist");
                else
                    return MessageBoxAndReturn("修改失败，请联系管理员！");
            }
            else
            {
                var dateTimeNow = DateTime.Now;
                DateTime time = eventtime.ToDateTime();

                if (uids == null || uids.Length == 0 || (uids.Length == 1 && userId == 0))
                {
                    //未选择，默认当前用户
                    record = new Point_Records();
                    record.Score = pointscore;
                    record.EventTime = time;
                    record.Content = content;
                    record.PointId = pointId;
                    record.UserId = MyInfo.Id;
                    record.UserName = MyInfo.RealName;
                    record.IsApprove = false;
                    record.IsPass = false;
                    record.Month = time.Month;
                    record.Year = time.Year;
                    record.ParUserId = 0;
                    record.ParUserName = "";
                    record.CreateTime = dateTimeNow;
                    record.CreateUserId = MyInfo.Id;
                    record.UpdateTime = dateTimeNow;
                    id = Bll.BllPoint_Records.Insert(record);
                }
                else
                {
                    List<Point_Records> list = new List<Point_Records>();
                    //批量申请
                    foreach (var uid in uids)
                    {
                        if (int.TryParse(uid, out userId) && userId > 0)
                        {
                            record = new Point_Records();
                            record.Score = pointscore;
                            record.EventTime = time;
                            record.Content = content;
                            record.PointId = pointId;
                            record.UserId = userId;
                            record.UserName = Bll.BllMng_User.GetNameById(userId);
                            record.IsApprove = true;
                            record.IsPass = true;
                            record.Month = time.Month;
                            record.Year = time.Year;
                            record.ParUserId = MyInfo.Id;
                            record.ParUserName = MyInfo.RealName;
                            record.CreateTime = dateTimeNow;
                            record.CreateUserId = MyInfo.Id;
                            record.UpdateTime = dateTimeNow;
                            record.Remark = "";
                            list.Add(record);
                        }
                    }
                    id = Bll.BllPoint_Records.Insert(list);
                }
                if (id > 0)
                    return MessageBoxAndJump("提交成功！", "/point/point/pointlist");
                else
                    return MessageBoxAndReturn("提交失败，请联系管理员！");
            }
        }

        [HttpPost]
        public ActionResult PointDelete(int id)
        {
            var record = Bll.BllPoint_Records.First(o => o.Id == id);
            if (record == null)
                return Json(new { result = 0, msg = "发生错误，记录不存在！" });
            if (record.IsApprove.Value && !IsSuperAdmin && !IsDepartAdmin)
                return Json(new { result = 0, msg = "该记录已审批过，禁止删除！" });

            if (Bll.BllPoint_Records.Delete(record) > 0)
                return Json(new { result = 1, msg = "删除成功！" });
            else
                return Json(new { result = 0, msg = "删除失败，请联系管理员！" });
        }

        public ActionResult PointChoose(int id = 0)
        {
            var classlist = Bll.BllPoint_Class.Instance().ShowClass(0, 0, false);

            ViewBag.classlist = classlist;
            return View();
        }

        public ActionResult GetPointList(int id)
        {
            var list = Bll.BllPoint_List.Query(o => o.ClassId == id, o => o.Sequence, "asc");
            return Json(list, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region 下级积分列表

        public ActionResult ChildUserList(int departId = 0)
        {
            int childCount = Bll.BllMng_User.GetChildCount(MyInfo.Id);
            if (childCount == 0 && !(IsDepartAdmin || IsSuperAdmin))
                return MessageBoxAndReturn("禁止访问，您无下级或无权限！");

            var childlist = IsSuperAdmin || IsDepartAdmin
                ? Bll.BllMng_User.GetListByDepart(departId, MyInfo.Id, true)
                : Bll.BllMng_User.GetChildList(MyInfo.Id);
            foreach (var item in childlist)
            {
                item.IsApproved = Bll.BllPoint_Records.IsApproved(item.Id);
            }
            ViewBag.childlist = childlist.OrderBy(o => o.IsApproved).ToList();
            ViewBag.departclasslist = Bll.BllMng_DepartmentClass.Instance().ShowClass(0, departId, false);
            return View();
        }

        public ActionResult ChildPointList(int id, bool isApproved = false)
        {
            if (id == 0) return MessageBoxAndReturn("用户不存在！");
            var userInfo = Bll.BllMng_User.First(o => o.Id == id);
            if (userInfo == null) return MessageBoxAndReturn("用户不存在！");

            int page = GetThisPage();
            int pageSize = 5;
            long records = 0;

            var recordlist = Bll.BllPoint_Records.PageList(id, isApproved, page, pageSize, out records);
            ViewBag.recordlist = recordlist;
            ViewBag.pageStr = ShowPage((int)records, pageSize, page, 5, "", false);
            ViewBag.userInfo = userInfo;
            ViewBag.isApproved = isApproved;
            return View();
        }

        public ActionResult ChildPointEdit(int id)
        {
            if (id == 0) return MessageBoxAndReturn("发生错误，记录不存在！");
            Point_Records record = Bll.BllPoint_Records.First(o => o.Id == id);
            if (record == null) return MessageBoxAndReturn("发生错误，记录不存在！");

            Point_List pointModel = null;
            if (record.PointId > 0)
            {
                pointModel = Bll.BllPoint_List.First(o => o.Id == record.PointId.Value);
                if (pointModel == null) return MessageBoxAndReturn("积分项不存在！");
            }
            ViewBag.pointId = record.PointId;
            ViewBag.pointModel = pointModel;
            return View(record);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChildPointSave()
        {
            int id = RequestInt("id");
            if (id == 0) return MessageBoxAndReturn("发生错误，记录不存在！");

            bool isPass = RequestBool("ispass");
            string remark = RequestString("remark");

            Point_Records record = Bll.BllPoint_Records.First(o => o.Id == id);
            if (record == null) return MessageBoxAndReturn("发生错误，记录不存在！");
            if (record.IsApprove.Value) return MessageBoxAndReturn("该记录已审批过，禁止修改！");

            record.IsApprove = true;
            record.Remark = remark;
            record.IsPass = isPass;
            record.ParUserId = MyInfo.Id;
            record.ParUserName = MyInfo.RealName;
            record.ApproveTime = DateTime.Now;

            if (Bll.BllPoint_Records.Update(record, o => o.Id == id) > 0)
                return MessageBoxAndJump("审批成功！", "/point/point/childpointlist/" + record.UserId);
            else
                return MessageBoxAndReturn("审批失败，请联系管理员！");
        }
        #endregion
    }
}
