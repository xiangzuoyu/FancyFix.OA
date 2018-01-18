using FancyFix.OA.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FancyFix.OA.Areas.Rank.Controllers
{
    public class KpiController : BaseAdminController
    {
        public ActionResult Index(int year = 0, int month = 0)
        {
            CheckDate(ref year, ref month);

            var departList = Bll.BllMng_DepartmentClass.Query(o => o.BeLock == false);
            var ranklist = Bll.BllKpi_Process.GetRankUserList(year, month);
            int i = 0;
            int lastScore = 0;
            foreach (var item in ranklist)
            {
                var departInfo = departList.Find(o => o.Id == item.DepartId);
                if (departInfo != null)
                {
                    item.DepartmentName = departInfo.ClassName;
                }
                if (item.Score != lastScore) i++;
                item.Rank = i;
                lastScore = item.Score.Value;
            }
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.startyear = StartYear;
            ViewBag.ranklist = ranklist;
            return View();
        }

        public ActionResult KpiList(int id)
        {
            var process = Bll.BllKpi_Process.First(o => o.Id == id);
            if (process == null) return MessageBoxAndReturn("进程不存在！");

            var userInfo = Bll.BllMng_User.First(o => o.Id == process.UserId);
            if (userInfo == null) return MessageBoxAndReturn("用户不存在！");

            var recordlist = Bll.BllKpi_Records.GetList(process.Id);

            ViewBag.startyear = StartYear;
            ViewBag.recordlist = recordlist;
            ViewBag.userInfo = userInfo;
            return View(process);
        }

        public ActionResult KpiInfo(int id)
        {
            var record = Bll.BllKpi_Records.First(o => o.Id == id);
            if (record == null) return MessageBoxAndReturn("指标不存在！");
            if (!record.IsCreated.Value) return MessageBoxAndReturn("指标未生成！");
            if (!record.IsApprove.Value) return MessageBoxAndReturn("指标未评分！");

            var process = Bll.BllKpi_Process.First(o => o.UserId == record.UserId && o.Year == record.Year && o.Month == record.Month);
            if (process == null) return MessageBoxAndReturn("进程不存在！");

            ViewBag.pid = process.Id;
            return View(record);
        }
    }
}