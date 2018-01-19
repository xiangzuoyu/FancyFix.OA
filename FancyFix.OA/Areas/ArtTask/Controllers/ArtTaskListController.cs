using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.ArtTask.Controllers
{
    public class ArtTaskListController : BaseAdminController
    {
        // GET: ArtTask/ArtTaskList
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Edit(int id = 0)
        {
            ArtTaskList model = null;

            if (id > 0)
                model = Bll.BllArtTaskList.First(o => o.Id == id);

            return View(model);
        }

        public JsonResult PageList()
        {
            long records = 0;
            int page = RequestInt("page");
            int pagesize = RequestInt("pagesize");

            var list = Bll.BllArtTaskList.PageList(page, pagesize, out records);

            return BspTableJson(list, records);
        }

        [HttpPost]
        public ActionResult Save(ArtTaskList artTaskList)
        {
            artTaskList.SubmitterId = MyInfo.Id;
            artTaskList.SubmittedDate = DateTime.Now;

            if (Bll.BllArtTaskList.Insert(artTaskList) > 0)
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