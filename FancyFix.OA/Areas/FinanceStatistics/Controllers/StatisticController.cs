using FancyFix.OA.Areas.FinanceStatistics.Common;
using FancyFix.OA.Base;
using FancyFix.OA.Model;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.FinanceStatistics.Controllers
{
    /// <summary>
    /// 财务统计
    /// </summary>
    /// <returns></returns>
    public class StatisticController : BaseAdminController
    {
        #region 加载数据
        public ActionResult List()
        {
            ViewBag.departmentList = Bll.BllFinance_EveryDaySaleLog.GetBusinessOrder("", "", "", "", "")?.Select(o => o.DepartmentName)?.Distinct()?.ToList() ?? null;
            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0, string files = "", string key = "", DateTime? startdate = null, DateTime? enddate = null
            , string department = "")
        {
            long records = 0;
            //Sql注入检测
            string file = Tools.Usual.Utils.CheckSqlKeyword(files);
            string keys = Tools.Usual.Utils.CheckSqlKeyword(key).Trim();
            string departmentName = Tools.Usual.Utils.CheckSqlKeyword(department).Trim();
            var list = Bll.BllFinance_Statistics.PageList(page, pagesize, out records, files, keys, startdate, enddate, departmentName);
            return BspTableJson(list, records);
        }
        #endregion

        #region 导出
        [HttpPost]
        public ActionResult ExportExcel(string files = "", string key = "", DateTime? startdate = null, DateTime? enddate = null, string department = "")
        {
            //Sql注入检测
            string file = Tools.Usual.Utils.CheckSqlKeyword(files);
            string keys = Tools.Usual.Utils.CheckSqlKeyword(key).Trim();
            string departmentName = Tools.Usual.Utils.CheckSqlKeyword(department).Trim();
            var list = Bll.BllFinance_Statistics.GetList(files, key, startdate, enddate, departmentName);
            if (list == null || list.Count() < 1)
                return MessageBoxAndReturn("加载数据失败，未找到该数据");
            var list2 = Bll.BllFinance_EveryDaySaleLog.GetBusinessOrder(files, key, startdate?.ToString() ?? "", enddate?.ToString() ?? "", department);
            if (list2 == null || list2.Count() < 1)
                return MessageBoxAndReturn("加载数据失败，未找到该数据");

            var departmentList = (from o in list2 select o.DepartmentName).Distinct().ToList();

            HSSFWorkbook workbook = StatisticsExport.CustomStatisticsExport(list, departmentList);
            if (workbook == null)
                return MessageBoxAndReturn("加载数据失败，workbook返回为空");

            //导出
            string fileName = "销售统计信息" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Tools.Tool.ExcelHelper.ToExcelWeb(fileName + ".xls", workbook);

            return View("List");
        }

        #endregion

        #region 编辑
        public ActionResult Save(int id = 0)
        {
            Finance_Statistics model = null;
            if (id > 0)
            {
                model = Bll.BllFinance_Statistics.First(o => o.Id == id && o.Display != 2);
                if (model == null)
                    return LayerAlertSuccessAndRefresh("加载数据失败，未找到该数据");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Finance_Statistics statistics)
        {
            //关键字段不能为空
            if (statistics.SaleDate == null ||
                string.IsNullOrWhiteSpace(statistics.DepartmentName))
            {
                return LayerMsgErrorAndReturn("销售日期或部门不能为空！");
            }

            //数据是否重复
            Finance_Statistics model = Bll.BllFinance_Statistics.First(o => o.Id != statistics.Id &&
                                                                                o.SaleDate == statistics.SaleDate &&
                                                                                o.DepartmentName == statistics.DepartmentName &&
                                                                                o.Display != 2);
            if (model != null)
                return LayerMsgErrorAndReturn("添加数据失败，该条数据已存在，请确认后重新提交！");

            if (statistics.Id < 1)
                model = new Finance_Statistics();
            else
                model = Bll.BllFinance_Statistics.First(o => o.Id == statistics.Id && o.Display != 2) ?? new Finance_Statistics();

            model.Year = statistics.SaleDate.GetValueOrDefault().Year;
            model.Month = statistics.SaleDate.GetValueOrDefault().Month;
            model.Day = statistics.SaleDate.GetValueOrDefault().Day;
            model.SaleDate = statistics.SaleDate;
            model.DepartmentName = statistics.DepartmentName;
            model.BusinessIncome = statistics.BusinessIncome;
            model.BudgetaryValue = statistics.BudgetaryValue;
            model.BusinessIncomeRate = Bll.BllFinance_Statistics.GetBusinessRate(model.BusinessIncome, model.BudgetaryValue);
            model.ActualReceipts = statistics.ActualReceipts;
            model.ActualDeliveryOrderNumber = statistics.ActualDeliveryOrderNumber;
            model.PlanDeliveryOrderNumber = statistics.PlanDeliveryOrderNumber;
            model.DeliveryPunctualityRate = Bll.BllFinance_Statistics.GetBusinessRate(model.ActualDeliveryOrderNumber, model.PlanDeliveryOrderNumber);

            model.LastDate = DateTime.Now;
            model.LastUserId = MyInfo.Id;

            bool isok = false;
            //没有ID就新增，反之修改
            if (model.Id < 1)
            {
                model.AddDate = model.LastDate;
                model.AddUserId = model.LastUserId;
                model.Display = 1;

                isok = Bll.BllFinance_Statistics.Insert(model) > 0;
            }
            else
                isok = Bll.BllFinance_Statistics.Update(model) > 0;

            return LayerMsgSuccessAndRefresh("保存" + (isok ? "成功" : "失败"));
        }


        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllFinance_Statistics.Delete(id, MyInfo.Id) > 0 });
        }

        [HttpPost]
        public JsonResult DeleteBatch(List<Finance_Statistics> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllFinance_Statistics.DeleteList(list, MyInfo.Id) > 0 });
        }
        #endregion

    }
}