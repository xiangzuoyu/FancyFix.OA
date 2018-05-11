using FancyFix.OA.Base;
using FancyFix.OA.Filter;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Questionnaire.Controllers
{
    [CheckLogin]
    public class SubjectController : BaseAdminController
    {
        public ActionResult List()
        {
            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0, int displayid = 0)
        {
            long records = 0;
            var list = Bll.BllQuestionnaire_Subject.PageList(page, pagesize, out records);
            int i = 0;
            foreach (var item in list)
            {
                GetActStr(item, list.Count, i);
                i++;
            }
            return BspTableJson(list, records);
        }

        private void GetActStr(Questionnaire_Subject model, int listCount, int i)
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

        /// <summary>
        /// 下移分类
        /// </summary>
        /// <returns></returns>
        public JsonResult Down()
        {
            int moveId = RequestInt("id");
            int step = RequestInt("step");
            if (step == 0)
                return Json(new { result = false, msg = "请选择上移数" });
            if (moveId == 0)
                return Json(new { result = false, msg = "请选择移动分类" });
            if (Bll.BllQuestionnaire_Subject.SequenceDownSeqByColumn(moveId, "Sequence", step))
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
            if (Bll.BllQuestionnaire_Subject.SequenceUpSeqByColumn(moveId, "Sequence", step))
                return Json(new { result = true, msg = "" });
            else
                return Json(new { result = false, msg = "提交出错" });
        }

        [HttpPost]
        public JsonResult SetShow(int id)
        {
            return Json(new { result = Bll.BllQuestionnaire_Subject.SetShow(id) });
        }

        public ActionResult Edit(int id = 0)
        {
            Questionnaire_Subject model = null;
            if (id > 0)
                model = Bll.BllQuestionnaire_Subject.First(o => o.Id == id);
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllQuestionnaire_Subject.Delete(o => o.Id == id) > 0 });
        }

        [HttpPost]
        public JsonResult DeleteBatch(List<Questionnaire_Subject> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllQuestionnaire_Subject.Delete(list) > 0 });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save()
        {
            int id = RequestInt("id");
            string title = RequestString("title");
            string remark = RequestString("remark");
            int timelimit = RequestInt("timelimit");
            int score = RequestInt("score");
            bool isshow = RequestBool("isshow");

            Questionnaire_Subject model = null;
            string classHtml = string.Empty;

            if (id > 0)
            {
                model = Bll.BllQuestionnaire_Subject.First(o => o.Id == id);
                if (model == null) return MessageBoxAndReturn("问卷不存在！");
                model.Title = title;
                model.Remark = remark;
                model.Timelimit = timelimit;
                model.Score = score;
                model.IsShow = isshow;

                if (Bll.BllQuestionnaire_Subject.Update(model, o => o.Id == id) > 0)
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
                model = new Questionnaire_Subject();
                model.Title = title;
                model.Remark = remark;
                model.Timelimit = timelimit;
                model.Score = score;
                model.IsShow = isshow;
                model.Sequence = Bll.BllSys_Class<Questionnaire_Subject>.Instance().GetNextSequence("");
                if (Bll.BllQuestionnaire_Subject.Insert(model) > 0)
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