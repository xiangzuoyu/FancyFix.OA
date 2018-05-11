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
    public class DISCController : BaseAdminController
    {
        public ActionResult List()
        {
            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0, int displayid = 0)
        {
            long records = 0;
            var list = Bll.BllQuestionnaire_DISC.PageList(page, pagesize, out records);
            int i = 0;
            foreach (var item in list)
            {
                var disclist = Tools.Tool.JsonHelper.Deserialize<Tools.Json.DISC>(item.DISC);
                item.DISC = "";
                foreach (var disc in disclist)
                    item.DISC += "<p>" + disc.n + ":" + disc.v + "</p>";
                GetActStr(item, list.Count, i);
                i++;
            }
            return BspTableJson(list, records);
        }

        private void GetActStr(Questionnaire_DISC model, int listCount, int i)
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
            if (Bll.BllQuestionnaire_DISC.SequenceDownSeqByColumn(moveId, "Sequence", step))
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
            if (Bll.BllQuestionnaire_DISC.SequenceUpSeqByColumn(moveId, "Sequence", step))
                return Json(new { result = true, msg = "" });
            else
                return Json(new { result = false, msg = "提交出错" });
        }

        public ActionResult Edit(int id = 0)
        {
            Questionnaire_DISC model = null;
            if (id > 0)
                model = Bll.BllQuestionnaire_DISC.First(o => o.Id == id);
            return View(model);
        }

        [HttpPost]
        public JsonResult SetShow(int id)
        {
            return Json(new { result = Bll.BllQuestionnaire_DISC.SetShow(id) });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllQuestionnaire_DISC.Delete(o => o.Id == id) > 0 });
        }

        [HttpPost]
        public JsonResult DeleteBatch(List<Questionnaire_DISC> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllQuestionnaire_DISC.Delete(list) > 0 });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(int id, List<string> name)
        {
            bool isShow = RequestBool("isshow");

            if (name == null || name.Count != 4)
                return MessageBoxAndReturn("请完善DISC各项值！");
            if (!name.Contains("D") || !name.Contains("I") || !name.Contains("S") || !name.Contains("C"))
                return MessageBoxAndReturn("请完善DISC各项值！");

            List<Tools.Json.DISC> entity = new List<Tools.Json.DISC>();

            foreach (var n in name)
            {
                entity.Add(new Tools.Json.DISC()
                {
                    n = n,
                    v = RequestString(n + "_value")
                });
            }

            Questionnaire_DISC model = null;
            string classHtml = string.Empty;

            if (id > 0)
            {
                model = Bll.BllQuestionnaire_DISC.First(o => o.Id == id);
                model.DISC = Tools.Tool.JsonHelper.Serialize(entity);
                model.D = entity.Find(o => o.n == "D").v;
                model.I = entity.Find(o => o.n == "I").v;
                model.S = entity.Find(o => o.n == "S").v;
                model.C = entity.Find(o => o.n == "C").v;
                model.IsShow = isShow;
                if (model == null) return MessageBoxAndReturn("题目不存在！");

                if (Bll.BllQuestionnaire_DISC.Update(model, o => o.Id == id) > 0)
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
                model = new Questionnaire_DISC();
                model.DISC = Tools.Tool.JsonHelper.Serialize(entity);
                model.D = entity.Find(o => o.n == "D").v;
                model.I = entity.Find(o => o.n == "I").v;
                model.S = entity.Find(o => o.n == "S").v;
                model.C = entity.Find(o => o.n == "C").v;
                model.IsShow = isShow;
                model.Sequence = Bll.BllSys_Class<Questionnaire_DISC>.Instance().GetNextSequence("");
                if (Bll.BllQuestionnaire_DISC.Insert(model) > 0)
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