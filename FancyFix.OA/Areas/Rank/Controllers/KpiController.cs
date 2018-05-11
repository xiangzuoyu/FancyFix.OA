using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using FancyFix.OA.Filter;

namespace FancyFix.OA.Areas.Rank.Controllers
{
    [CheckLogin]
    public class KpiController : BaseAdminController
    {
        public ActionResult Index(int year = 0, int fromMonth = 0, int toMonth = 0, int departId = 0)
        {
            CheckDate(ref year, ref fromMonth, ref toMonth);

            string realname = RequestString("realname");
            var ranklist = Bll.BllKpi_Process.GetUserRankList(year, fromMonth, toMonth, departId, realname);

            //把离职的单独排到最后去
            int i = 0;
            int lastScore = 0;
            foreach (var item in ranklist)
            {
                if (item.InJob.HasValue && !item.InJob.Value)
                    item.RealName = item.RealName + "【离职】";
                if (item.Score != lastScore) i++;
                item.Rank = i;
                lastScore = item.Score;
            }

            ViewBag.departHtml = GetDepartOptions(departId);
            ViewBag.year = year;
            ViewBag.fromMonth = fromMonth;
            ViewBag.toMonth = toMonth;
            ViewBag.realname = realname;
            ViewBag.departId = departId;
            ViewBag.startyear = StartYear;
            ViewBag.ranklist = ranklist;
            return View();
        }

        public ActionResult KpiList(int id, int year, int fromMonth = 0, int toMonth = 0, int departId = 0)
        {
            CheckDate(ref year, ref fromMonth, ref toMonth);

            var userInfo = Bll.BllMng_User.First(o => o.Id == id);
            if (userInfo == null) return MessageBoxAndReturn("用户不存在！");

            var processlist = Bll.BllKpi_Process.GetList(year, fromMonth, toMonth, id);
            if (processlist == null || processlist.Count == 0) return MessageBoxAndReturn("进程不存在！");

            List<Kpi_Records> recordlist = new List<Kpi_Records>();
            foreach (var process in processlist)
            {
                var list = Bll.BllKpi_Records.GetList(process.Id);
                if (list != null && list.Count > 0)
                {
                    foreach (var record in list)
                    {
                        record.FinishScore = record.FinishScore ?? 0;
                        recordlist.Add(record);
                    }
                }
            }

            ViewBag.startyear = StartYear;
            ViewBag.recordlist = recordlist;
            ViewBag.userInfo = userInfo;
            ViewBag.departId = departId;
            ViewBag.year = year;
            ViewBag.fromMonth = fromMonth;
            ViewBag.toMonth = toMonth;
            return View(processlist);
        }

        public ActionResult KpiInfo(int id, int fromMonth = 0, int toMonth = 0, int departId = 0)
        {
            var record = Bll.BllKpi_Records.First(o => o.Id == id);
            if (record == null) return MessageBoxAndReturn("指标不存在！");
            //if (!record.IsCreated.Value) return MessageBoxAndReturn("指标未生成！");
            //if (!record.IsApprove.Value) return MessageBoxAndReturn("指标未评分！");

            int year = 0;
            CheckDate(ref year, ref fromMonth, ref toMonth);

            var process = Bll.BllKpi_Process.First(o => o.UserId == record.UserId && o.Year == record.Year && o.Month == record.Month);
            if (process == null) return MessageBoxAndReturn("进程不存在！");

            ViewBag.pid = process.Id;
            ViewBag.departId = RequestInt("departId");
            ViewBag.year = record.Year;
            ViewBag.fromMonth = fromMonth;
            ViewBag.toMonth = toMonth;
            return View(record);
        }

        public void ToExcelRank()
        {
            int year = RequestInt("year");
            int fromMonth = RequestInt("fromMonth");
            int toMonth = RequestInt("toMonth");
            int departId = RequestInt("departId");
            string realname = RequestString("realname");

            var ranklist = Bll.BllKpi_Process.GetUserRankList(year, fromMonth, toMonth, departId, realname);

            //把离职的单独排到最后去
            int i = 0;
            int lastScore = 0;
            foreach (var item in ranklist)
            {
                if (item.InJob.HasValue && !item.InJob.Value)
                    item.RealName = item.RealName + "【离职】";
                if (item.Score != lastScore) i++;
                item.Rank = i;
                lastScore = item.Score;
            }

            if (ranklist != null && ranklist.Count() > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("排名", typeof(String));
                dt.Columns.Add("员工", typeof(String));
                dt.Columns.Add("部门", typeof(String));
                dt.Columns.Add("岗位", typeof(String));
                dt.Columns.Add("自评分", typeof(String));
                dt.Columns.Add("上级评分", typeof(String));
                dt.Columns.Add("最终得分", typeof(String));
                dt.Columns.Add("统计次数", typeof(String));
                foreach (var item in ranklist)
                {
                    var row = dt.NewRow();
                    row["排名"] = item.Rank;
                    row["员工"] = item.RealName;
                    row["部门"] = item.DepartMentName;
                    row["岗位"] = item.GroupName;
                    row["自评分"] = item.SelfScore;
                    row["上级评分"] = item.Score;
                    row["最终得分"] = item.Score;
                    row["统计次数"] = item.Count;
                    dt.Rows.Add(row);
                }
                string fileName = "进程：" + year + "年" + fromMonth + (toMonth > fromMonth ? "至" + toMonth : "") + "月" + "KPI考核排名";
                Tools.Tool.ExcelHelper.ToExcelWeb(dt, fileName, fileName + ".xls");
            }
        }
    }
}