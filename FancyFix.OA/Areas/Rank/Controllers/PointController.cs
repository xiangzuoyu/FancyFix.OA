using FancyFix.OA.Base;
using FancyFix.OA.Model;
using FancyFix.OA.Model.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Rank.Controllers
{
    public class PointController : BaseAdminController
    {
        public ActionResult Index(int year = 0, int fromMonth = 0, int toMonth = 0)
        {
            CheckDate(ref year, ref fromMonth, ref toMonth);

            var departList = Bll.BllMng_DepartmentClass.Query(o => o.BeLock == false);
            var userlist = Bll.BllMng_User.Query(o => o.InJob == true);
            var ranklist = Bll.BllPoint_Records.GetRankUserList(year, fromMonth, toMonth);
            double top = RequestDouble("top");

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
                double val = top * count / 100;
                int take = val < 1 ? 1 : (int)Math.Round(val, MidpointRounding.AwayFromZero);
                if (take > 0)
                    ranklist = ranklist.Take(take).ToList();
            }

            ViewBag.year = year;
            ViewBag.fromMonth = fromMonth;
            ViewBag.toMonth = toMonth;
            ViewBag.top = top;
            ViewBag.startyear = StartYear;
            ViewBag.ranklist = ranklist;
            return View();
        }

        public ActionResult PointList(int id, int year = 0, int fromMonth = 0, int toMonth = 0)
        {
            CheckDate(ref year, ref fromMonth, ref toMonth);

            var userInfo = Bll.BllMng_User.First(o => o.Id == id);
            if (userInfo == null) return MessageBoxAndReturn("用户不存在！");
            var recordlist = Bll.BllPoint_Records.GetListByUser(id, year, fromMonth, toMonth);

            ViewBag.startyear = StartYear;
            ViewBag.year = year;
            ViewBag.fromMonth = fromMonth;
            ViewBag.toMonth = toMonth;
            ViewBag.recordlist = recordlist;
            ViewBag.userInfo = userInfo;
            return View();
        }

        public void ToExcel()
        {
            int year = RequestInt("year");
            int fromMonth = RequestInt("fromMonth");
            int toMonth = RequestInt("toMonth");
            double top = RequestDouble("top");

            var ranklist = Bll.BllPoint_Records.GetRankUserList(year, fromMonth, toMonth);
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
                    double val = top * ranklist.Count / 100;
                    int take = val < 1 ? 1 : (int)Math.Round(val, MidpointRounding.AwayFromZero);
                    if (take > 0)
                        ranklist = ranklist.Take(take).ToList();
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("排名", typeof(String));
                dt.Columns.Add("员工", typeof(String));
                dt.Columns.Add("部门", typeof(String));
                dt.Columns.Add("岗位", typeof(String));
                dt.Columns.Add("总分", typeof(String));
                dt.Columns.Add("积分项", typeof(String));
                foreach (var item in ranklist)
                {
                    var row = dt.NewRow();
                    row["排名"] = item.Rank;
                    row["员工"] = item.RealName;
                    row["部门"] = item.DepartName;
                    row["岗位"] = item.GroupName;
                    row["总分"] = item.Score;

                    string sampleStr = string.Empty;
                    var samples = Bll.BllPoint_Records.GetListByUser(item.UserId, year, fromMonth, toMonth);
                    if (samples != null && samples.Count > 0)
                    {
                        for (int j = 0; j < samples.Count; j++)
                        {
                            sampleStr += $"【{samples[j].ClassName } | {samples[j].EventTime.Value.ToString("yyyy/MM/dd")}】：{samples[j].Content } ({samples[j].Score}分)" + (j < samples.Count - 1 ? "\n\n" : "");
                        }
                    }
                    row["积分项"] = sampleStr;
                    dt.Rows.Add(row);
                }
                string fileName = "进程：" + year + "年" + fromMonth + "月" + "积分" + (top > 0 ? "前" + top + "%" : "") + "排名";
                Tools.Tool.ExcelHelper.ToExcelWeb(dt, fileName, fileName + ".xls");
            }
        }

        public ActionResult PointInfo(int id)
        {
            var record = Bll.BllPoint_Records.First(o => o.Id == id);
            if (record == null) return MessageBoxAndReturn("记录不存在！");
            if (!record.IsApprove.Value) return MessageBoxAndReturn("记录未审核！");

            ViewBag.record = record;
            ViewBag.classinfo = Bll.BllPoint_List.First(o => o.Id == record.PointId);
            return View();
        }
    }
}