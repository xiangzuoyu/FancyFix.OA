using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
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

        public JsonResult PageList(int job, string startdate = "", string enddate = "")
        {
            long records = 0;
            var userList = Bll.BllMng_User.GetSelectList(0, "Id,RealName", "DepartId=10", "") ?? new List<Mng_User>();
            var designavgscore = Bll.BllDesign_ArtTaskList.GetRankList(startdate, enddate);


            //if (job == 1)
            //{
            var rankList = (from a in designavgscore
                            group a by a.DesignerId into g
                            select new { id = g.Key, name = GetName(userList, g.Key), score = g.Sum(o => o.Score == null ? 0 : o.Score) / (g.Count(o => true)) });

            var ddd = from u in userList
                      join a in rankList
                      on u.Id equals a.id into joinlist
                      from aa in joinlist.DefaultIfEmpty()
                      orderby aa.score descending
                      select new
                      {
                          name = u.RealName,
                          score = aa == null ? 0 : aa.score
                      };


            //rankList = rankList.OrderByDescending(o => o.score);
            //}
            //else if (job == 2)
            //{

            //}
            //else
            //{

            //}

            //foreach (var item in list)
            //{
            //    item.SubmitterName = GetUserNameById(item.SubmitterId, adminlist);
            //    item.DesignerName = GetUserNameById(item.DesignerId, adminlist);
            //}

            return BspTableJson(ddd, records);
        }

        private string GetName(IEnumerable<Mng_User> users, int? userId)
        {
            if (users == null || users.Count() < 1 || userId == null)
                return "";

            return (from o in users where o.Id == userId select o.RealName).FirstOrDefault();
        }
    }
}