using FancyFix.OA.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Rank.Controllers
{
    public class ValuableController : BaseAdminController
    {
        public ActionResult Index()
        {
            int year = RequestInt("year");
            int fromMonth = RequestInt("fromMonth");
            int toMonth = RequestInt("toMonth");
            double top = RequestDouble("top");

            if (year < StartYear) year = DateTime.Now.Year;
            var workerMonthList = GetWorkerMonthList(year);
            if (fromMonth == 0) fromMonth = workerMonthList.Count > 0 ? workerMonthList[0] : 1;
            if (toMonth == 0) toMonth = workerMonthList.Count > 0 ? workerMonthList[workerMonthList.Count - 1] : 1;

            var yearlist = Bll.BllConfig_Process.GetValuableYears();
            var departList = Bll.BllMng_DepartmentClass.Query(o => o.BeLock == false);
            var userlist = Bll.BllMng_User.Query(o => o.InJob == true);
            var ranklist = Bll.BllValuable_Records.GetRankList(year, fromMonth, toMonth);
            int count = 0;
            int i = 0;
            int lastScore = 0;
            if (ranklist != null && ranklist.Count > 0)
            {
                foreach (var item in ranklist)
                {
                    var userInfo = userlist.Find(o => o.Id == item.UserId);
                    if (userInfo != null)
                    {
                        var departInfo = departList.Find(o => o.Id == userInfo.DepartId);
                        if (departInfo != null)
                        {
                            item.DepartId = departInfo.Id;
                            item.DepartName = departInfo.ClassName;
                        }
                    }
                    if (item.Score != lastScore) i++;
                    item.Rank = i;
                    lastScore = item.Score;
                }
                count = ranklist.Count;
            }

            //筛选
            if (count > 0 && top > 0)
            {
                var groupRankList = ranklist.GroupBy(o => o.Rank);
                double val = top * groupRankList.Last().Key / 100; //取到最大排名
                int take = val < 1 ? 1 : (int)Math.Round(val, MidpointRounding.AwayFromZero); //四舍五入取整
                if (take > 0)
                    ranklist = ranklist.Where(o => o.Rank <= take).ToList();
            }

            ViewBag.year = year;
            ViewBag.fromMonth = fromMonth;
            ViewBag.toMonth = toMonth;
            ViewBag.top = top;
            ViewBag.startyear = StartYear;
            ViewBag.yearlist = yearlist;
            ViewBag.monthlist = workerMonthList;
            ViewBag.ranklist = ranklist;
            return View();
        }

        public ActionResult ValueList(int id)
        {
            int year = RequestInt("year");
            int fromMonth = RequestInt("fromMonth");
            int toMonth = RequestInt("toMonth");

            if (year == 0) return MessageBoxAndReturn("请选择年份！");
            if (fromMonth == 0) return MessageBoxAndReturn("请选择起始月份！");
            if (toMonth == 0) return MessageBoxAndReturn("请选择终止月份！");

            if (year < StartYear) year = DateTime.Now.Year;
            if (fromMonth > toMonth) toMonth = fromMonth;

            var userInfo = Bll.BllMng_User.First(o => o.Id == id);
            if (userInfo == null) return MessageBoxAndReturn("用户不存在！");
            var recordlist = Bll.BllValuable_Records.GetListByMonthRank(id, year, fromMonth, toMonth);

            ViewBag.year = year;
            ViewBag.fromMonth = fromMonth;
            ViewBag.toMonth = toMonth;
            ViewBag.recordlist = recordlist;
            ViewBag.userInfo = userInfo;
            return View();
        }

        public void ToExcelRank()
        {
            int year = RequestInt("year");
            int fromMonth = RequestInt("fromMonth");
            int toMonth = RequestInt("toMonth");
            double top = RequestDouble("top");

            var ranklist = Bll.BllValuable_Records.GetRankList(year, fromMonth, toMonth);
            int i = 0;
            int lastScore = 0;
            if (ranklist != null && ranklist.Count > 0)
            {
                var departList = Bll.BllMng_DepartmentClass.Query(o => o.BeLock == false);
                var grouplist = Bll.BllMng_PermissionGroup.Query(o => o.BeLock == false);
                var userlist = Bll.BllMng_User.Query(o => o.InJob == true);
                foreach (var item in ranklist)
                {
                    var userInfo = userlist.Find(o => o.Id == item.UserId);
                    if (userInfo != null)
                    {
                        var departInfo = departList.Find(o => o.Id == userInfo.DepartId);
                        if (departInfo != null)
                        {
                            item.DepartId = departInfo.Id;
                            item.DepartName = departInfo.ClassName;
                            item.GroupName = grouplist?.Find(o => o.Id == userInfo.GroupId)?.GroupName ?? "";
                        }
                    }
                    if (item.Score != lastScore) i++;
                    item.Rank = i;
                    lastScore = item.Score;
                }

                //筛选
                if (top > 0)
                {
                    var groupRankList = ranklist.GroupBy(o => o.Rank);
                    double val = top * groupRankList.Last().Key / 100; //取到最大排名
                    int take = val < 1 ? 1 : (int)Math.Round(val, MidpointRounding.AwayFromZero); //四舍五入取整
                    if (take > 0)
                        ranklist = ranklist.Where(o => o.Rank <= take).ToList();
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("排名", typeof(String));
                dt.Columns.Add("员工", typeof(String));
                dt.Columns.Add("部门", typeof(String));
                dt.Columns.Add("岗位", typeof(String));
                dt.Columns.Add("得分", typeof(String));
                foreach (var item in ranklist)
                {
                    var row = dt.NewRow();
                    row["排名"] = item.Rank;
                    row["员工"] = item.RealName;
                    row["部门"] = item.DepartName;
                    row["岗位"] = item.GroupName;
                    row["得分"] = item.Score;
                    dt.Rows.Add(row);
                }
                string fileName = "进程：" + year + "年" + fromMonth + (toMonth > fromMonth ? "至" + toMonth : "") + "月" + "价值观" + (top > 0 ? "前" + top + "%" : "") + "排名";
                Tools.Tool.ExcelHelper.ToExcelWeb(dt, fileName, fileName + ".xls");
            }
        }

        public void ToExcel(int id)
        {
            var userinfo = Bll.BllMng_User.First(o => o.Id == id);
            if (userinfo != null)
            {
                int year = RequestInt("year");
                int fromMonth = RequestInt("fromMonth");
                int toMonth = RequestInt("toMonth");
                var recordlist = Bll.BllValuable_Records.GetExcelList(userinfo.Id, year, fromMonth, toMonth);
                string fileName = userinfo.RealName + "-" + year + "年" + fromMonth + (toMonth > fromMonth ? "至" + toMonth : "") + "月" + "价值观内容.xls";
                Tools.Tool.ExcelHelper.ToExcelWeb(recordlist, "", fileName);
            }
        }

        public ActionResult SampleList(int id)
        {
            var record = Bll.BllValuable_Records.First(o => o.Id == id);
            if (record == null) return MessageBoxAndReturn("记录不存在！");
            if (!record.IsApprove.Value) return MessageBoxAndReturn("记录未审核！");

            var valuable = Bll.BllValuable_List.First(o => o.Id == record.Vid);
            if (valuable == null) return MessageBoxAndReturn("价值观不存在！");

            ViewBag.record = record;
            ViewBag.samplelist = Bll.BllValuable_Sample.GetList(record.Id);
            return View(valuable);
        }
    }
}