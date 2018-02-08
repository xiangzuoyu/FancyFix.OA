using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Valuable.Controllers
{
    public class ValuableController : BaseAdminController
    {
        //进程列表
        public ActionResult WorkerList()
        {
            var yearAndMonth = GetYearAndMonth();
            var yearlist = Bll.BllConfig_Process.GetValuableYears();
            var workerlist = Bll.BllConfig_Process.GetValuableProcess(yearAndMonth.Item1);
            var scorelist = Bll.BllValuable_Records.GetScoreByMonth(workerlist, MyInfo.Id, yearAndMonth.Item1);

            ViewBag.childCount = Bll.BllMng_User.GetChildCount(MyInfo.Id);
            ViewBag.sumscore = scorelist.Values.Sum();
            ViewBag.scorelist = scorelist;
            ViewBag.year = yearAndMonth.Item1;
            ViewBag.yearlist = yearlist;
            ViewBag.workerlist = workerlist;
            return View();
        }

        #region 自评

        public ActionResult ValueList()
        {
            var yearAndMonth = GetYearAndMonth();
            var valuablelist = Bll.BllValuable_List.GetList();
            var recordlist = Bll.BllValuable_Records.GetList(MyInfo.Id, yearAndMonth.Item1, yearAndMonth.Item2);

            ViewBag.currentMonth = yearAndMonth.Item2;
            ViewBag.currentYear = yearAndMonth.Item1;
            ViewBag.recordlist = recordlist;
            ViewBag.valuablelist = valuablelist;
            return View();
        }

        public ActionResult ValueRecord(int id)
        {
            Valuable_List model = Bll.BllValuable_List.First(o => o.Id == id);
            if (model == null)
                return MessageBoxAndReturn("价值观配置不存在！");

            var yearAndMonth = GetYearAndMonth();

            var record = Bll.BllValuable_Records.First(o => o.UserId == MyInfo.Id && o.Vid == model.Id && o.Year == yearAndMonth.Item1 && o.Month == yearAndMonth.Item2);

            IList<Valuable_Sample> samplelist = null;
            if (record != null)
                samplelist = Bll.BllValuable_Sample.GetList(record.Id);

            ViewBag.samplelist = samplelist;
            ViewBag.record = record;
            ViewBag.currentMonth = yearAndMonth.Item2;
            ViewBag.currentYear = yearAndMonth.Item1;
            ViewBag.rankclass = Bll.BllRank_Class.Instance().GetListByParentId(0, false);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save()
        {
            int id = RequestInt("id");
            int rid = RequestInt("rid");
            int rank = RequestInt("rank");
            var yearAndMonth = GetYearAndMonth();
            string[] sampleIds = RequestString("sampleId").TrimEnd(',').Split(',');

            if (id == 0) return MessageBoxAndReturn("价值观配置不存在！");
            if (rank == 0) return MessageBoxAndReturn("请选择一个评分项！");
            Valuable_List model = Bll.BllValuable_List.First(o => o.Id == id);
            if (model == null) return MessageBoxAndReturn("价值观配置不存在！");

            //所选分值对应所需的案例个数
            int sampleNum = Bll.BllRank_Class.Instance().First(rank, "SampleNum")?.SampleNum ?? 0;

            List<Valuable_Sample> samplelist = null;
            if (sampleIds != null && sampleIds.Length > 0)
            {
                samplelist = new List<Valuable_Sample>();
                int sampleId = 0;
                foreach (string sid in sampleIds)
                {
                    if (int.TryParse(sid, out sampleId))
                    {
                        samplelist.Add(new Valuable_Sample()
                        {
                            Content = RequestString("sample" + sampleId)
                        });
                    }
                }
            }

            //判断案例数是否达标
            if (sampleNum > 0 && samplelist != null && samplelist.Count < sampleNum)
                return MessageBoxAndReturn($"请填入至少{sampleNum}个案例！");

            Valuable_Records record = null;
            if (rid > 0)
            {
                record = Bll.BllValuable_Records.First(o => o.Id == rid);
                if (record.IsApprove.Value)
                    return MessageBoxAndReturn("该记录已评审，禁止修改！");

                record.Rank = rank;
                record.RankScore = Bll.BllRank_Class.First(o => o.Id == rank)?.Score ?? 0;
                record.UpdateTime = DateTime.Now;
                record.UserId = MyInfo.Id;
                record.Vid = id;
                Bll.BllValuable_Records.Update(record, o => o.Id == rid);
            }
            else
            {
                record = new Valuable_Records();
                record.CreateTime = DateTime.Now;
                record.Rank = rank;
                record.RankScore = Bll.BllRank_Class.First(o => o.Id == rank)?.Score ?? 0;
                record.Score = 0;
                record.UpdateTime = DateTime.Now;
                record.UserId = MyInfo.Id;
                record.Vid = id;
                record.Year = yearAndMonth.Item1;
                record.Month = yearAndMonth.Item2;
                record.IsApprove = false;
                rid = Bll.BllValuable_Records.Insert(record);
            }
            if (rid > 0)
            {
                //添加记录
                Bll.BllValuable_Sample.Add(rid, samplelist);
                return MessageBoxAndJump("提交成功！", $"/valuable/valuable/valuelist?year={yearAndMonth.Item1}&month={yearAndMonth.Item2}");
            }
            return MessageBoxAndReturn("提交失败！");
        }

        #endregion

        #region 审核下级

        public ActionResult ChildUserList()
        {
            var yearAndMonth = GetYearAndMonth();

            var childlist = Bll.BllMng_User.GetChildList(MyInfo.Id);
            var valuelist = Bll.BllValuable_List.GetList();
            var ids = valuelist.Select(o => o.Id).ToArray();
            foreach (var item in childlist)
            {
                item.IsRecorded = Bll.BllValuable_Records.IsRecorded(item.Id, yearAndMonth.Item1, yearAndMonth.Item2, ids);
                item.IsApproved = Bll.BllValuable_Records.IsApproved(item.Id, yearAndMonth.Item1, yearAndMonth.Item2, ids);
            }
            ViewBag.childlist = childlist;
            ViewBag.currentMonth = yearAndMonth.Item2;
            ViewBag.currentYear = yearAndMonth.Item1;
            return View();
        }

        public ActionResult ChildValueList(int userId, int year, int month)
        {
            month = GetWorkerMonth(year, month);
            var valuablelist = Bll.BllValuable_List.GetList();
            var recordlist = Bll.BllValuable_Records.GetList(userId, year, month);

            ViewBag.currentYear = year;
            ViewBag.currentMonth = month;
            ViewBag.recordlist = recordlist;
            ViewBag.valuablelist = valuablelist;
            ViewBag.userName = Bll.BllMng_User.GetNameById(userId);
            ViewBag.userId = userId;
            return View();
        }

        public ActionResult ChildValueRecord(int id, int userId, int year, int month)
        {
            Valuable_List model = Bll.BllValuable_List.First(o => o.Id == id);
            if (model == null)
                return MessageBoxAndReturn("价值观配置不存在！");

            var record = Bll.BllValuable_Records.First(o => o.UserId == userId && o.Vid == model.Id && o.Year == year && o.Month == month);

            if (record == null)
                return MessageBoxAndReturn("该员工未自评！");

            IList<Valuable_Sample> samplelist = null;
            if (record != null)
                samplelist = Bll.BllValuable_Sample.GetList(record.Id);

            int nextWorkmonth = GetNextWorkerMonth(year, month);

            ViewBag.isLock = (GetWorkerMonth(year, month) != GetWorkerMonth(DateTime.Now.Year, DateTime.Now.Month) || year != DateTime.Now.Year) && !(DateTime.Now.Day <= WorkerEndDay && year == DateTime.Now.Year && DateTime.Now.Month == nextWorkmonth);
            ViewBag.samplelist = samplelist;
            ViewBag.record = record;
            ViewBag.rankclass = Bll.BllRank_Class.Instance().GetListByParentId(0, false);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChildSave()
        {
            int id = RequestInt("id");
            int rid = RequestInt("rid");
            int rank = RequestInt("rank");
            string remark = RequestString("remark");

            if (id == 0) return MessageBoxAndReturn("价值观配置不存在！");
            if (rid == 0) return MessageBoxAndReturn("记录不存在！");
            if (rank == 0) return MessageBoxAndReturn("请选择一个评分项！");

            Valuable_List model = Bll.BllValuable_List.First(o => o.Id == id);
            if (model == null) return MessageBoxAndReturn("价值观配置不存在！");
            Valuable_Records record = Bll.BllValuable_Records.First(o => o.Id == rid);
            if (record == null) return MessageBoxAndReturn("该员工未自评！");

            record.Score = Bll.BllRank_Class.First(o => o.Id == rank)?.Score ?? 0;
            record.ApproveTime = DateTime.Now;
            record.IsApprove = true;
            record.ParUserId = MyInfo.Id;
            record.ParUserName = MyInfo.RealName;
            record.Remark = remark;
            int count = Bll.BllValuable_Records.Update(record, o => o.Id == rid);
            if (count > 0)
                return MessageBoxAndJump("审核成功！", $"/valuable/valuable/childvaluelist?userId={record.UserId}&year={record.Year}&month={record.Month}");
            else
                return MessageBoxAndReturn("审核出错！请联系管理员");
        }
        #endregion

        //获取年，月参数
        public Tuple<int, int> GetYearAndMonth()
        {
            int year = RequestInt("year");
            int month = RequestInt("month");

            if (year < StartYear)
                year = DateTime.Now.Year;
            if (month < 1 || month > 12)
                month = CurrentWorkerMonth;
            else
                month = GetWorkerMonth(year, month);

            return new Tuple<int, int>(year, month);
        }
    }
}