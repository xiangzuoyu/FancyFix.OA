using FancyFix.OA.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Questionnaire.Controllers
{
    public class AnswererController : BaseAdminController
    {
        public ActionResult DISCAnswererList()
        {
            int page = GetThisPage();
            int pageSize = 15;
            long records = 0;

            string name = RequestString("name");

            var answererlist = Bll.BllQuestionnaire_Answerer.PageList(0, true, name, page, pageSize, out records);
            ViewBag.answererlist = answererlist;
            ViewBag.name = name;
            ViewBag.pageStr = ShowPage((int)records, pageSize, page, 5, "", false);
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var record = Bll.BllQuestionnaire_Answerer.First(o => o.Id == id);
            if (record == null) return Json(new { result = 0, msg = "发生错误，记录不存在！" });
            if (Bll.BllQuestionnaire_Answerer.Delete(record) > 0)
                return Json(new { result = 1, msg = "删除成功！" });
            else
                return Json(new { result = 0, msg = "删除失败，请联系管理员！" });
        }

        public ActionResult AnswererList(int id = 0)
        {
            int page = GetThisPage();
            int pageSize = 15;
            long records = 0;

            var subject = Bll.BllQuestionnaire_Subject.First(o => o.Id == id);
            if (subject == null) return MessageBoxAndReturn("问卷不存在！");

            string name = RequestString("name");

            var answererlist = Bll.BllQuestionnaire_Answerer.PageList(id, false, name, page, pageSize, out records);
            ViewBag.answererlist = answererlist;
            ViewBag.name = name;
            ViewBag.pageStr = ShowPage((int)records, pageSize, page, 5, "", false);
            return View(subject);
        }

        public ActionResult Info(int id)
        {
            var answerer = Bll.BllQuestionnaire_Answerer.First(o => o.Id == id);
            if (answerer == null) return MessageBoxAndReturn("提卷不存在！");

            var list = Bll.BllQuestionnaire_Result.GetList(answerer.Id);

            ViewBag.list = list;
            return View(answerer);
        }
    }
}