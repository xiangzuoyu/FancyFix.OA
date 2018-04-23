using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.ArtTask.Controllers
{
    public class ArtTaskRankController : BaseAdminController
    {
        // GET: ArtTask/ArtTaskRank
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult PageList(int job, string datetime = "")
        {
            long records = 0;
            var userList = Bll.BllMng_User.GetSelectList(0, "Id,RealName", "DepartId=10", "") ?? new List<Mng_User>();
            var rankList = Bll.BllDesign_ArtTaskList.GetRankList(datetime, job);

            var avgScoreList = (from o in rankList.AsEnumerable()
                       select new
                       {
                           //id = o.Field<int>("id"),
                           realName = o.Field<string>("RealName"),
                           avgScore = o.Field<int?>("平均分") ?? 0
                       });

            avgScoreList = avgScoreList.OrderByDescending(o => o.avgScore);

            return BspTableJson(avgScoreList, records);
        }
     
    }
}