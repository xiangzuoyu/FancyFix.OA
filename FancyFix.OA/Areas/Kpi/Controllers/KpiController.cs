using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Kpi.Controllers
{
    public class KpiController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.childCount = Bll.BllMng_User.GetChildCount(MyInfo.Id);
            return View();
        }

        #region KPI进程列表页

        public ActionResult KpiList(int year = 0, int month = 0, List<int> rid = null)
        {
            CheckDate(ref year, ref month);

            //读取/创建进程
            var process = Bll.BllKpi_Process.GetModel(MyInfo.Id, year, month);
            if (process == null)
            {
                process = new Kpi_Process()
                {
                    IsCreated = false,
                    IsApprove = false,
                    Month = month,
                    Year = year,
                    Score = 0,
                    UserId = MyInfo.Id,
                    Remark = ""
                };
                int id = Bll.BllKpi_Process.Insert(process);
                if (id > 0)
                    process.Id = id;
                else
                    return MessageBoxAndReturn("当前进程创建失败，请联系管理员！");
            }
            //目标项
            List<Kpi_Records> recordlist = Bll.BllKpi_Records.GetList(process.Id);

            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.startyear = StartYear;
            ViewBag.recordlist = recordlist;
            return View(process);
        }

        //查看指标信息
        public ActionResult KpiInfo(int id = 0)
        {
            if (id == 0) return MessageBoxAndReturn("参数有误！");
            var record = Bll.BllKpi_Records.First(o => o.Id == id);
            if (record == null) MessageBoxAndReturn("指标不存在！");

            ViewBag.isInner = RequestBool("isInner"); //是否使用内页模版
            return View(record);
        }

        [HttpPost]
        //指标删除
        public ActionResult KpiDelete(int id)
        {
            if (id == 0) return Json(new { result = 0, msg = "参数有误！" });

            //指标判断
            var record = Bll.BllKpi_Records.First(o => o.Id == id);
            if (record == null) MessageBoxAndReturn("指标不存在！");
            if (record.IsApprove.Value) return Json(new { result = 0, msg = "该指标已被评分，不能删除！" });

            //进程判断
            if (record.Pid > 0)
            {
                var process = Bll.BllKpi_Process.First(o => o.Id == record.Pid);
                if (process?.IsCreated ?? false) return Json(new { result = 0, msg = "该指标所在进程已生成，不能删除！" });
            }

            id = Bll.BllKpi_Records.Delete(o => o.Id == id);

            if (id > 0)
                return Json(new { result = 1, msg = "删除成功！" });
            else
                return Json(new { result = 0, msg = "删除失败，请联系管理员！" });
        }

        //指标增加修改
        public ActionResult KpiAdd(int id = 0, int year = 0, int month = 0)
        {
            int pid = RequestInt("pid");

            CheckDate(ref year, ref month);

            var process = Bll.BllKpi_Process.First(o => o.Id == pid);
            if (process != null && (process.IsCreated ?? false))
                return MessageBoxAndReturn("该进程已生成，禁止继续添加指标！");

            Kpi_Records record = null;
            if (id > 0)
            {
                record = Bll.BllKpi_Records.First(o => o.Id == id);
                if (record != null && record.IsApprove.Value)
                    return MessageBoxAndReturn("该指标已被评分，不能修改！");
            }

            ViewBag.pid = pid;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.startyear = StartYear;
            return View(record);
        }

        //保存指标
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult KpiSave(int year = 0, int month = 0)
        {
            CheckDate(ref year, ref month);
            int id = RequestInt("id");
            int pid = RequestInt("pid");
            string name = RequestString("name");
            string content = RequestString("content");
            string target_highest = RequestString("target_highest");
            string target_normal = RequestString("target_normal");
            string target_lowest = RequestString("target_lowest");
            int score = RequestInt("score");
            string targetremark = RequestString("targetremark");
            int paruserid = RequestInt("paruserid");

            var process = Bll.BllKpi_Process.First(o => o.Id == pid);
            if (process != null && (process.IsCreated ?? false))
                return MessageBoxAndReturn("该进程已生成，禁止继续添加指标！");

            Kpi_Records record = null;
            if (id > 0)
            {
                record = Bll.BllKpi_Records.First(o => o.Id == id);
                if (record != null && record.IsApprove.Value)
                    return MessageBoxAndReturn("该指标已被评分，不能修改！");

                year = record.Year.Value;
                month = record.Month.Value;

                record.Name = name;
                record.Content = content;
                record.Target_Highest = target_highest;
                record.Target_Lowest = target_lowest;
                record.Target_Normal = target_normal;
                record.Score = score;
                record.TargetRemark = targetremark;
                record.ParUserId = paruserid > 0 ? paruserid : MyInfo.ParUserId;
                record.ParUserName = Bll.BllMng_User.GetNameById(record.ParUserId.Value);
                record.UserId = MyInfo.Id;
                record.UserName = MyInfo.RealName;
                id = Bll.BllKpi_Records.Update(record, o => o.Id == id);
            }
            else
            {
                record = new Kpi_Records();
                record.Pid = pid;
                record.Year = year;
                record.Month = month;
                record.Name = name;
                record.Content = content;
                record.Target_Highest = target_highest;
                record.Target_Lowest = target_lowest;
                record.Target_Normal = target_normal;
                record.Score = score;
                record.FinishScore = 0;
                record.ParScore = 0;
                record.TargetRemark = targetremark;
                record.ParUserId = paruserid > 0 ? paruserid : MyInfo.ParUserId;
                record.ParUserName = Bll.BllMng_User.GetNameById(record.ParUserId.Value);
                record.UserId = MyInfo.Id;
                record.UserName = MyInfo.RealName;
                record.CreateTime = DateTime.Now;
                record.Remark = "";
                record.IsApprove = false;
                record.IsCreated = false;
                id = Bll.BllKpi_Records.Insert(record);
            }
            if (id > 0)
                return MessageBoxAndJump("保存成功！", $"/kpi/kpi/kpilist?year={year}&month={month}");
            else
                return MessageBoxAndReturn("提交失败，请联系管理员！");
        }

        //进程生成
        [HttpPost]
        public ActionResult ProcessCreate(int id, List<int> rid = null)
        {
            if (id == 0) return MessageBoxAndReturn("参数有误！");
            var process = Bll.BllKpi_Process.First(o => o.Id == id);
            if (process != null && (process.IsCreated ?? false))
                return MessageBoxAndReturn("该进程指标已生成，请勿重新生成！");

            //验证id集合
            if (rid == null || rid.Count == 0)
                return MessageBoxAndReturn("生成失败，请添加指标项，且权重值合计必须为100%！");

            //验证指标
            var records = Bll.BllKpi_Records.GetListByIds(rid);
            if (records == null || records.Count == 0)
                return MessageBoxAndReturn("生成失败，请添加指标项，且权重值合计必须为100%！");

            //验证权重值合计
            int? score = records.Sum(o => o.Score);
            if (score == null || score != 100)
                return MessageBoxAndReturn("生成失败，权重值合计必须为100%！");

            if (Bll.BllKpi_Process.CreateProcess(process, records))
                return MessageBoxAndJump("生成成功！", $"/kpi/kpi/kpilist?year={process.Year}&month={process.Month}");
            else
                return MessageBoxAndReturn("生成失败，请联系管理员！");
        }
        #endregion

        #region KPI模版导入

        public ActionResult KpiSet(int year = 0, int month = 0)
        {
            CheckDate(ref year, ref month);

            var modelList = Bll.BllKpi_Process.GetModelList(MyInfo.Id);

            ViewBag.modelList = modelList;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.startyear = StartYear;
            return View();
        }

        //获取模版指标列表
        [HttpPost]
        public ActionResult GetProcessKpi(int id)
        {
            var process = Bll.BllKpi_Process.First(o => o.Id == id);
            if (process == null)
                return Json(false, JsonRequestBehavior.DenyGet);

            var list = Bll.BllKpi_Records.GetList(process.Id);
            return Json(list, JsonRequestBehavior.DenyGet);
        }

        //进程是否已创建
        [HttpPost]
        public ActionResult IsProcessCreate(int year = 0, int month = 0)
        {
            CheckDate(ref year, ref month);

            var process = Bll.BllKpi_Process.GetModel(MyInfo.Id, year, month);
            if (process != null && (process.IsCreated ?? false))
                return Json(true, JsonRequestBehavior.DenyGet);

            return Json(false, JsonRequestBehavior.DenyGet);
        }

        //导入模版
        [HttpPost]
        public ActionResult KpiModelCreate(List<int> rid, int year = 0, int month = 0)
        {
            if (rid == null || rid.Count == 0) return MessageBoxAndReturn("请选择指标！");

            CheckDate(ref year, ref month);

            //生成进程
            var process = Bll.BllKpi_Process.GetModel(MyInfo.Id, year, month);
            if (process == null)
            {
                process = new Kpi_Process()
                {
                    IsCreated = false,
                    IsApprove = false,
                    Month = month,
                    Year = year,
                    Score = 0,
                    UserId = MyInfo.Id,
                    Remark = ""
                };
                int pid = Bll.BllKpi_Process.Insert(process);
                if (pid > 0)
                    process.Id = pid;
                else
                    return MessageBoxAndReturn("当前进程创建失败，请联系管理员！");
            }

            if (process.IsCreated ?? false)
                return MessageBoxAndReturn("该进程已生成，请勿重新生成！");

            var recordlist = Bll.BllKpi_Records.GetListByIds(rid);
            if (recordlist == null)
                return MessageBoxAndReturn("该进程模版不存在！");

            foreach (var item in recordlist)
            {
                item.DeAttach();//实体恢复默认状态
                item.Id = 0;
                item.Pid = process.Id;
                item.Year = year;
                item.Month = month;
                item.IsApprove = false;
                item.IsCreated = false;
                item.CreateTime = DateTime.Now;
            }
            int id = Bll.BllKpi_Records.Insert(recordlist);
            if (id > 0)
                return Redirect($"/kpi/kpi/kpilist?year={process.Year}&month={process.Month}");
            else
                return MessageBoxAndReturn("导入失败！");
        }
        #endregion

        #region 评审员工列表

        public ActionResult ChildUserList(int year = 0, int month = 0)
        {
            CheckDate(ref year, ref month);

            var userlist = Bll.BllKpi_Records.GetUserList(MyInfo.Id, year, month);
            foreach (var item in userlist)
            {
                item.Score = Bll.BllKpi_Process.First(o => o.UserId == item.Id && o.Year == year && o.Month == month)?.Score ?? 0;
            }

            ViewBag.userlist = userlist;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.startyear = StartYear;
            return View();
        }

        public ActionResult ChildKpiList(int id, int year = 0, int month = 0)
        {
            CheckDate(ref year, ref month);

            var process = Bll.BllKpi_Process.First(o => o.UserId == id && o.Year == year && o.Month == month);
            if (process == null) return MessageBoxAndReturn("进程不存在！");

            var userInfo = Bll.BllMng_User.First(o => o.Id == id);
            if (userInfo == null) return MessageBoxAndReturn("员工不存在！");

            var recordlist = Bll.BllKpi_Records.GetListByUserId(id, year, month);

            ViewBag.recordlist = recordlist;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.startyear = StartYear;
            return View(userInfo);
        }

        public ActionResult ChildKpiApprove(int id)
        {
            var record = Bll.BllKpi_Records.First(o => o.Id == id);
            if (record == null) return MessageBoxAndReturn("指标不存在！");
            if (!(record.IsCreated ?? false)) return MessageBoxAndReturn("该指标未生成，暂时不能评分！");
            return View(record);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChildKpiSave(int id)
        {
            var record = Bll.BllKpi_Records.First(o => o.Id == id);
            if (record == null) return MessageBoxAndReturn("指标不存在！");
            if (!(record.IsCreated ?? false)) return MessageBoxAndReturn("该指标未生成，暂时不能评分！");

            int parscore = RequestInt("parscore");

            record.IsApprove = true;
            record.ApproveTime = DateTime.Now;
            record.FinishScore = (parscore * record.Score.Value) / 100;
            record.ParScore = parscore;
            record.Remark = RequestString("remark");

            int result = Bll.BllKpi_Records.Update(record, o => o.Id == id);
            if (result > 0)
            {
                //更新进程状态
                Bll.BllKpi_Process.UpdateProcessStatus(record);
                return MessageBoxAndJump("提交成功！", $"/kpi/kpi/childkpilist/{record.UserId}?year={record.Year}&month={record.Month}");
            }
            else
                return MessageBoxAndReturn("提交失败，请联系管理员！");
        }
        #endregion
    }
}