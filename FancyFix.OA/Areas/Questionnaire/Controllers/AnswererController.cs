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

            var answererlist = Bll.BllQuestionnaire_Answerer.PageList(0, true, page, pageSize, out records);
            ViewBag.answererlist = answererlist;
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
    }
}