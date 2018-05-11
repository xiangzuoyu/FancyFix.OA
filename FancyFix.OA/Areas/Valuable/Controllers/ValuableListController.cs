using FancyFix.OA.Base;
using FancyFix.OA.Filter;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Valuable.Controllers
{
    public class ValuableListController : BaseAdminController
    {
        public ActionResult List()
        {
            return View();
        }

        [PermissionFilter("/valuable/valuablelist/list")]
        public JsonResult PageList()
        {
            long records = 0;
            int page = RequestInt("page");
            int pagesize = RequestInt("pagesize");
            string title = RequestString("title");

            var list = Bll.BllValuable_List.PageList(page, pagesize, out records);
            int i = 0;
            int listCount = list.Count();
            foreach (var item in list)
            {
                GetActStr(item, listCount, i);
                i++;
            }
            return BspTableJson(list, records);
        }

        private void GetActStr(Valuable_List model, int listCount, int i)
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
                actStr.Append($"</select><input type=\"button\" name=\"Submit2\" value=\"↑\" class=\"btn btn-default\" onclick=\"SetMove('{model.Id}','up')\"> ");
            }
            actStr.Append("</td></form><form name=\"downform\"><td  width=\"70\">");
            if (i < listCount - 1)
            {
                actStr.Append($"<select id=\"stepDown{model.Id}\" > ");
                for (int j = 1; j <= listCount - i - 1; j++)
                {
                    actStr.Append("<option value=\"" + j + "\">" + j + "</option>");
                }
                actStr.Append($"</select><input type=\"button\" name=\"Submit2\" value=\"↓\" class=\"btn btn-default\" onclick=\"SetMove('{model.Id}','down')\"> ");
            }
            actStr.Append("</td></form></tr></table>");
            model.actStr = actStr.ToString();
        }

        public JsonResult Down()
        {
            int moveId = RequestInt("id");
            int step = RequestInt("step");
            if (step == 0)
                return Json(new { result = false, msg = "请选择上移数" });
            if (moveId == 0)
                return Json(new { result = false, msg = "请选择移动分类" });
            if (Bll.BllValuable_List.SequenceDownSeqByColumn(moveId, "Sequence", step))
                return Json(new { result = true, msg = "" });
            else
                return Json(new { result = false, msg = "提交出错" });
        }

        /// <summary>
        /// 上移分类
        /// </summary>
        /// <returns></returns>
        public JsonResult Up()
        {
            int moveId = RequestInt("id");
            int step = RequestInt("step");
            if (step == 0)
                return Json(new { result = false, msg = "请选择上移数" });
            if (moveId == 0)
                return Json(new { result = false, msg = "请选择移动分类" });
            if (Bll.BllValuable_List.SequenceUpSeqByColumn(moveId, "Sequence", step))
                return Json(new { result = true, msg = "" });
            else
                return Json(new { result = false, msg = "提交出错" });
        }

        public ActionResult Edit(int id = 0)
        {
            Valuable_List model = null;
            string classHtml = string.Empty;

            if (id > 0)
            {
                model = Bll.BllValuable_List.First(o => o.Id == id);
                classHtml = Bll.BllValuable_Class.Instance().ShowClass(0, model.ClassId.Value, true);
            }
            else
            {
                classHtml = Bll.BllValuable_Class.Instance().ShowClass(0, 0, true);
            }
            ViewBag.classHtml = classHtml;
            return View(model);
        }


        [HttpPost]
        public JsonResult SetShow(int id)
        {
            return Json(new { result = Bll.BllValuable_List.SetShow(id) });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllValuable_List.Delete(o => o.Id == id) > 0 });
        }

        //[PermissionFilter("/valuable/valuablelist/delete")]
        [HttpPost]
        public JsonResult DeleteBatch(List<Valuable_List> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllValuable_List.Delete(list) > 0 });
        }

        //[PermissionFilter("/valuable/valuablelist/edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save()
        {
            int id = RequestInt("id");
            int classid = RequestInt("classid");
            string content = RequestString("content");
            bool belock = RequestBool("belock");
            int score = RequestInt("score");

            Valuable_List model = null;
            string classHtml = string.Empty;

            if (id > 0)
            {
                model = Bll.BllValuable_List.First(o => o.Id == id);
                model.Content = content;
                model.BeLock = !belock;
                model.ClassId = classid;
                model.Score = score;
                model.ClassName = Bll.BllSys_Class<Valuable_Class>.Instance().GetClassName(classid);
                model.UserId = MyInfo.Id;

                if (Bll.BllValuable_List.Update(model, o => o.Id == id) > 0)
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
                model = new Valuable_List();
                model.Content = content;
                model.BeLock = !belock;
                model.ClassId = classid;
                model.Score = score;
                model.ClassName = Bll.BllSys_Class<Valuable_Class>.Instance().GetClassName(classid);
                model.UserId = MyInfo.Id;
                model.CreateTime = DateTime.Now;
                model.Sequence = Bll.BllSys_Class<Valuable_List>.Instance().GetNextSequence("");
                if (Bll.BllValuable_List.Insert(model) > 0)
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