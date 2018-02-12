using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FancyFix.OA.Base;
using FancyFix.OA.Model;

namespace FancyFix.OA.Areas.Kpi.Controllers
{
    public class KpiListController : BaseAdminController
    {
        public ActionResult UnApproveList(int year = 0, int month = 0)
        {
            CheckDate(ref year, ref month);

            var dt = Bll.BllKpi_Records.GetUnApproveList(year, month);

            ViewBag.list = dt;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.startyear = StartYear;
            return View();
        }

        public ActionResult UnCreateList(int year = 0, int month = 0, int departId = 0)
        {
            CheckDate(ref year, ref month);

            Dictionary<string, string> users = new Dictionary<string, string>();
            var userlist = Bll.BllMng_User.GetAllList(true, departId);
            var departlist = Bll.BllMng_DepartmentClass.GetAll();
            var kpiprocess = Bll.BllKpi_Process.Query(o => o.Year == year && o.Month == month);
            foreach (var item in userlist)
            {
                var process = kpiprocess?.Find(o => o.UserId == item.Id);
                if (process == null || process.IsCreated == false)
                    users.Add(item.RealName, departlist.Find(o => o.Id == item.DepartId)?.ClassName ?? "");
            }

            ViewBag.departHtml = GetDepartOptions(departId);
            ViewBag.list = users;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.startyear = StartYear;
            return View();
        }
    }
}