using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System.Data;
using FancyFix.OA.Filter;

namespace FancyFix.OA.Areas.Kpi.Controllers
{
    [CheckLogin]
    public class KpiListController : BaseAdminController
    {
        public ActionResult UnApproveList(int year = 0, int month = 0, int inJob = 1)
        {
            CheckDate(ref year, ref month);

            var dt = Bll.BllKpi_Records.GetUnApproveList(year, month, inJob > 0);

            ViewBag.list = dt;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.inJob = inJob;
            ViewBag.startyear = StartYear;
            return View();
        }

        public ActionResult UnCreateList(int year = 0, int month = 0, int departId = 0, int inJob = 1)
        {
            CheckDate(ref year, ref month);

            DataTable dt = new DataTable();
            dt.Columns.Add("RealName", typeof(String));
            dt.Columns.Add("DepartName", typeof(String));
            dt.Columns.Add("ScoreSum", typeof(String));

            var userlist = Bll.BllMng_User.GetAllList(inJob > 0, departId);
            var departlist = Bll.BllMng_DepartmentClass.GetAll();
            var kpiprocess = Bll.BllKpi_Process.Query(o => o.Year == year && o.Month == month);

            foreach (var item in userlist)
            {
                var process = kpiprocess?.Find(o => o.UserId == item.Id);
                if (process == null || process.IsCreated == false)
                {
                    int scoreSum = Bll.BllKpi_Records.GetUserScoreSum(item.Id, year, month);
                    var row = dt.NewRow();
                    row["RealName"] = item.RealName;
                    row["DepartName"] = departlist.Find(o => o.Id == item.DepartId)?.ClassName ?? "";
                    row["ScoreSum"] = scoreSum.ToString();
                    dt.Rows.Add(row);
                }
            }

            ViewBag.departHtml = GetDepartOptions(departId);
            ViewBag.list = dt;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.inJob = inJob;
            ViewBag.startyear = StartYear;
            return View();
        }

        public ActionResult CreatedList(int year = 0, int month = 0, int departId = 0, int inJob = 1)
        {
            CheckDate(ref year, ref month);

            DataTable dt = new DataTable();
            dt.Columns.Add("RealName", typeof(String));
            dt.Columns.Add("DepartName", typeof(String));
            dt.Columns.Add("ScoreSum", typeof(String));
            dt.Columns.Add("UserId", typeof(String));
            dt.Columns.Add("IsApprove", typeof(Boolean));

            var userlist = Bll.BllMng_User.GetAllList(inJob > 0, departId);
            var departlist = Bll.BllMng_DepartmentClass.GetAll();
            var kpiprocess = Bll.BllKpi_Process.Query(o => o.Year == year && o.Month == month);

            foreach (var item in userlist)
            {
                var process = kpiprocess?.Find(o => o.UserId == item.Id);
                if (process != null && process.IsCreated == true)
                {
                    int scoreSum = Bll.BllKpi_Records.GetUserScoreSum(item.Id, year, month);
                    var row = dt.NewRow();
                    row["RealName"] = item.RealName;
                    row["DepartName"] = departlist.Find(o => o.Id == item.DepartId)?.ClassName ?? "";
                    row["ScoreSum"] = scoreSum.ToString();
                    row["UserId"] = item.Id.ToString();
                    row["IsApprove"] = process.IsApprove;
                    dt.Rows.Add(row);
                }
            }

            ViewBag.departHtml = GetDepartOptions(departId);
            ViewBag.list = dt;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.inJob = inJob;
            ViewBag.startyear = StartYear;
            return View();
        }

        public JsonResult CancelCreate(CancelModel model)
        {
            var result = Bll.BllKpi_Process.CancelCreate(model.userIds, model.year, model.month);
            return Json(new { status = result });
        }

        public class CancelModel
        {
            public List<int> userIds { get; set; }
            public int year { get; set; }
            public int month { get; set; }
        }
    }
}