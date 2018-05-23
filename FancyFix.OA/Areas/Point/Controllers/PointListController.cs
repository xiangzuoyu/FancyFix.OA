using FancyFix.OA.Base;
using FancyFix.OA.Filter;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Point.Controllers
{
    public class PointListController : BaseAdminController
    {
        public ActionResult List()
        {
            int classid = RequestInt("classid");
            ViewBag.classHtml = Bll.BllSys_Class<Point_Class>.Instance().ShowClass(0, classid, false);
            ViewBag.classid = classid;
            return View();
        }

        //[PermissionFilter("/point/pointlist/list")]
        public JsonResult PageList()
        {
            long records = 0;
            int page = RequestInt("page");
            int pagesize = RequestInt("pagesize");
            int classid = RequestInt("classid");

            var list = Bll.BllPoint_List.PageList(classid, page, pagesize, out records);
            if (list != null && list.Any())
            {
                if (classid > 0)
                {
                    int i = 0;
                    int listCount = list.Count();
                    foreach (var item in list)
                    {
                        GetActStr(item, listCount, i);
                        i++;
                    }
                }
                else
                {
                    foreach (var item in list)
                    {
                        item.actStr = "请筛选具体类型后排序";
                    }
                }
            }
            return BspTableJson(list, records);
        }

        private void GetActStr(Point_List model, int listCount, int i)
        {
            StringBuilder actStr = new StringBuilder();
            actStr.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"250\"><tr><form name=\"upform\"><td width=\"70\">");
            if (i > 0)
            {
                actStr.Append($"<select id=\"stepUp{model.Id}\" > ");
                for (int j = 1; j <= i; j++)
                {
                    actStr.Append("<option value=\"" + j + "\">" + j + "</option>");
                }
                actStr.Append($"</select><input type=\"button\" name=\"Submit2\" value=\"↑\" class=\"btn btn-default\" onclick=\"SetMove('{model.Id}','{model.ClassId}','up')\"> ");
            }
            actStr.Append("</td></form><form name=\"downform\"><td  width=\"70\">");
            if (i < listCount - 1)
            {
                actStr.Append($"<select id=\"stepDown{model.Id}\" > ");
                for (int j = 1; j <= listCount - i - 1; j++)
                {
                    actStr.Append("<option value=\"" + j + "\">" + j + "</option>");
                }
                actStr.Append($"</select><input type=\"button\" name=\"Submit2\" value=\"↓\" class=\"btn btn-default\" onclick=\"SetMove('{model.Id}','{model.ClassId}','down')\"> ");
            }
            actStr.Append("</td></form></tr></table>");
            model.actStr = actStr.ToString();
        }

        public JsonResult Down()
        {
            int moveId = RequestInt("id");
            int step = RequestInt("step");
            int classid = RequestInt("byId");
            if (step == 0)
                return Json(new { result = false, msg = "请选择上移数" });
            if (moveId == 0)
                return Json(new { result = false, msg = "请选择移动分类" });
            if (Bll.BllPoint_List.SequenceDown(moveId, classid, "ClassId", step))
                return Json(new { result = true, msg = "" });
            else
                return Json(new { result = false, msg = "提交出错" });
        }

        public JsonResult Up()
        {
            int moveId = RequestInt("id");
            int step = RequestInt("step");
            int classid = RequestInt("byId");
            if (step == 0)
                return Json(new { result = false, msg = "请选择上移数" });
            if (moveId == 0)
                return Json(new { result = false, msg = "请选择移动分类" });
            if (Bll.BllPoint_List.SequenceUp(moveId, classid, "ClassId", step))
                return Json(new { result = true, msg = "" });
            else
                return Json(new { result = false, msg = "提交出错" });
        }

        public ActionResult Edit(int id = 0)
        {
            Point_List model = null;
            string classHtml = string.Empty;

            if (id > 0)
            {
                model = Bll.BllPoint_List.First(o => o.Id == id);
                classHtml = Bll.BllPoint_Class.Instance().ShowClass(0, model.ClassId.Value, true);
            }
            else
            {
                classHtml = Bll.BllPoint_Class.Instance().ShowClass(0, 0, true);
            }
            ViewBag.classHtml = classHtml;
            return View(model);
        }


        [HttpPost]
        public JsonResult SetShow(int id)
        {
            return Json(new { result = Bll.BllPoint_List.SetShow(id) });
        }

        [HttpPost]
        public JsonResult SetSelf(int id)
        {
            return Json(new { result = Bll.BllPoint_List.SetSelf(id) });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllPoint_List.Delete(o => o.Id == id) > 0 });
        }

        ////[PermissionFilter("/point/pointlist/delete")]
        [HttpPost]
        public JsonResult DeleteBatch(List<Point_List> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllPoint_List.Delete(list) > 0 });
        }

        ////[PermissionFilter("/point/pointlist/edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save()
        {
            int id = RequestInt("id");
            int classid = RequestInt("classid");
            string content = RequestString("content");
            string pointname = RequestString("pointname");
            int pointscore = RequestInt("pointscore");
            string scoreremark = RequestString("scoreremark");
            string counttime = RequestString("counttime");
            bool countbyself = true;//RequestBool("countbyself");
            string remark = RequestString("remark");

            Point_List model = null;
            string classHtml = string.Empty;

            if (id > 0)
            {
                model = Bll.BllPoint_List.First(o => o.Id == id);
                model.ClassId = classid;
                model.PointName = pointname;
                model.PointScore = pointscore;
                model.ScoreRemark = scoreremark;
                model.CountBySelf = countbyself;
                model.CountTime = counttime;
                model.Remark = remark;
                model.ClassName = Bll.BllSys_Class<Point_Class>.Instance().GetClassName(classid);
                if (Bll.BllPoint_List.Update(model, o => o.Id == id) > 0)
                {
                    return LayerAlertSuccessAndRefresh("修改成功");
                }
                else
                {
                    return LayerAlertSuccessAndRefresh("修改失败");
                }
            }
            else
            {
                model = new Point_List();
                model.BeLock = false;
                model.ClassId = classid;
                model.PointName = pointname;
                model.PointScore = pointscore;
                model.ClassName = Bll.BllSys_Class<Point_Class>.Instance().GetClassName(classid);
                model.CreateTime = DateTime.Now;
                model.CountBySelf = countbyself;
                model.CountTime = counttime;
                model.Remark = remark;
                model.Sequence = Bll.BllSys_Class<Point_List>.Instance().GetNextSequence("ClassId=" + classid);
                if (Bll.BllPoint_List.Insert(model) > 0)
                {
                    return LayerAlertSuccessAndRefresh("添加成功");
                }
                else
                {
                    return LayerAlertSuccessAndRefresh("添加失败");
                }
            }
        }
    }
}