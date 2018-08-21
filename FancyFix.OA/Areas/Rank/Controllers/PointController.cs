using FancyFix.OA.Base;
using FancyFix.OA.Filter;
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
    [CheckLogin]
    public class PointController : BaseAdminController
    {
        public ActionResult Index(int year = 0, int fromMonth = 0, int toMonth = 0, int departId = 0)
        {
            double top = RequestDouble("top");
            var injob = RequestInt("injob")==0?1: RequestInt("injob");
            CheckDate(ref year, ref fromMonth, ref toMonth);
            List<Rank_Point> ranklist = new List<Rank_Point>();
            ranklist = GetRankPointList(year, fromMonth, toMonth, departId, injob,top);

            ViewBag.departHtml = GetDepartOptions(departId);
            ViewBag.year = year;
            ViewBag.fromMonth = fromMonth;
            ViewBag.toMonth = toMonth;
            ViewBag.top = top;
            ViewBag.departId = departId;
            ViewBag.startyear = StartYear;
            ViewBag.ranklist = ranklist;
            ViewBag.injob = injob;
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
            int departId = RequestInt("departId");
             var injob = RequestInt("injob");
            List<Rank_Point> ranklist = new List<Rank_Point>();
            ranklist = GetRankPointList(year, fromMonth, toMonth, departId, injob, top);
            if (ranklist != null && ranklist.Count > 0)
            {
 
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


        private List<Rank_Point> GetRankPointList(int year = 0, int fromMonth = 0, int toMonth = 0, int departId = 0, int injob = 1,double top=0)
        {
            var userlist = Bll.BllMng_User.GetAllUser();
            var ranklist = Bll.BllPoint_Records.GetRankUserList(year, fromMonth, toMonth);


            if (ranklist != null && ranklist.Count > 0)
            {
                foreach (var item in ranklist)
                {
                    var userInfo = userlist.Find(o => o.Id == item.UserId);
                    if (userInfo != null)
                    {
                        item.DepartId = userInfo.DepartId.Value;
                        item.DepartName = userInfo.DepartMentName;
                        item.GroupName = userInfo.GroupName;
                        item.InJob = userInfo.InJob.HasValue ? userInfo.InJob.Value : false;
                    }
                }
            }
            if (injob > 0)
            {
                if (injob == 1)
                {
                    ranklist = ranklist.Where(p => p.InJob == true).ToList();
                }
                else
                {
                    ranklist = ranklist.Where(p => p.InJob == false).ToList();
                }
            }
            //部门筛选
            if (departId > 0)
                ranklist = ranklist.FindAll(o => o.DepartId == departId);

            //开始排名
            int count = 0;
            if (ranklist != null && ranklist.Count > 0)
            {
                int i = 0;
                int lastScore = 0;
                foreach (var item in ranklist)
                {
                    if (item.Score != lastScore) i++;
                    item.Rank = i;
                    lastScore = item.Score;
                }
                count = ranklist.Count;
            }

            //筛选百分比
            if (count > 0 && top > 0)
            {
                var groupRankList = ranklist.GroupBy(o => o.Rank);
                double val = top * groupRankList.Last().Key / 100; //取到最大排名
                int take = val < 1 ? 1 : (int)Math.Round(val, MidpointRounding.AwayFromZero); //四舍五入取整
                if (take > 0)
                    ranklist = ranklist.Where(o => o.Rank <= take).ToList();
            }
            return ranklist;
        }
    }
}