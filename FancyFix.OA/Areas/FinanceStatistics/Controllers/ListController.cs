using FancyFix.OA.Base;
using FancyFix.OA.Model;
using FancyFix.Tools.Config;
using FancyFix.Tools.Tool;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tools.Tool;

namespace FancyFix.OA.Areas.FinanceStatistics.Controllers
{
    public class ListController : BaseAdminController
    {
        #region 加载数据
        /// <summary>
        /// 财务统计
        /// </summary>
        /// <returns></returns>
        // GET: FinanceStatistics/List
        public ActionResult List()
        {
            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0)
        {
            long records = 0;
            //Sql注入检测
            string files = Tools.Usual.Utils.CheckSqlKeyword(RequestString("files"));
            string key = Tools.Usual.Utils.CheckSqlKeyword(RequestString("key")).Trim();
            var list = Bll.BllFinance_EveryDaySaleLog.PageList(page, pagesize, out records, files, key);
            return BspTableJson(list, records);
        }
        #endregion

        #region 导入Excel
        [HttpPost]
        public ActionResult List(HttpPostedFileBase file)
        {
            try
            {
                Tools.Config.UploadConfig config = UploadProvice.Instance();
                SiteOption option = config.SiteOptions["local"];
                string filePath = option.Folder + config.Settings["file"].FilePath + DateTime.Now.ToString("yyyyMMddHHmmss")
                        + (file.FileName.IndexOf(".xlsx") > 0 ? ".xlsx" : ".xls");

                string result = FileHelper.ValicationAndSaveFileToPath(file, filePath);
                if (result != "0")
                    return MessageBoxAndJump($"上传失败，{result}", "list");

                var sheet = ExcelHelper.ReadExcel(filePath);
                string msg = ExcelToList(sheet, 5);

                if (msg != "0")
                    return MessageBoxAndJump("导入失败," + GetMsg(msg), "list");
            }
            catch (Exception ex)
            {
                return MessageBoxAndJump("导入失败:" + ex.Message.ToString(), "list");
            }

            return Redirect("list");
        }

        private string GetMsg(string result)
        {
            string msg = string.Empty;
            switch (result)
            {
                case "1":
                    msg = "Excel数据为空";
                    break;
                case "2":
                    msg = "Excel格式不正确";
                    break;
                case "4":
                    msg = "修改数据失败";
                    break;
                case "-1":
                    msg = "导入异常";
                    break;
            }
            return msg;
        }

        private string ExcelToList(ISheet sheet, int startRow)
        {
            if (sheet == null)
                return "1";

            var addTime = DateTime.Now;
            //第一行为标题
            try
            {
                IRow headRow = sheet.GetRow(startRow);
                //int cellTotal = headRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                for (int i = startRow; i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    //年月日、销售人员、客户、产品名称、产品SKU为空时跳过
                    if (row == null ||
                        string.IsNullOrWhiteSpace(row.GetCell(1)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(2)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(3)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(5)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(6)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(8)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(9)?.ToString()))
                        continue;

                    var rawMaterialModel = CreateEveryDaySaleLogModel(row, addTime);
                    if (rawMaterialModel == null)
                        continue;

                    string isok = Bll.BllFinance_EveryDaySaleLog.Add(rawMaterialModel);
                    if (isok != "0")
                        return isok;

                    //AddPrice(row, headRow, id, cellTotal, logDate);
                }

                return "0";
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        /// <summary>
        /// 数据填充映射模型
        /// </summary>
        /// <param name="row"></param>
        /// <param name="logDate"></param>
        /// <returns></returns>
        private Finance_EveryDaySaleLog CreateEveryDaySaleLogModel(IRow row, DateTime addTime)
        {
            if (row == null) return null;

            var model = new Finance_EveryDaySaleLog()
            {
                Year = row.GetCell(1)?.ToString().ToInt32(),
                Month = row.GetCell(2)?.ToString().ToInt32(),
                Day = row.GetCell(3)?.ToString().ToInt32(),
                DepartmentName = row.GetCell(4)?.ToString(),
                SaleName = row.GetCell(5)?.ToString(),
                Customer = row.GetCell(6)?.ToString(),
                ContractNumber = row.GetCell(7)?.ToString(),
                ProductName = row.GetCell(8)?.ToString(),
                ProductSKU = row.GetCell(9)?.ToString(),
                SaleCount = getCellVal(row.GetCell(10)),
                SalePrice = getCellVal(row.GetCell(11)),
                Currency = row.GetCell(12)?.ToString(),
                ExchangeRate = getCellVal(row.GetCell(13)),
                MaterialUnitPrice = getCellVal(row.GetCell(15)),
                ProcessUnitPrice = getCellVal(row.GetCell(16)),
                ChangeCostNumber = getCellVal(row.GetCell(21)),
                ChangeCostMatter = getCellVal(row.GetCell(22)),
                ContributionMoney = getCellVal(row.GetCell(23)),
                ContributionRatio = getCellVal(row.GetCell(24)),
                AvgCoatUndue = getCellVal(row.GetCell(25)),
                AvgCoatCurrentdue = getCellVal(row.GetCell(26)),
                AvgCoatOverdue = getCellVal(row.GetCell(27)),
                CustomerContributionMoney = getCellVal(row.GetCell(28)),
                CustomerContributionRatio = getCellVal(row.GetCell(29)),
                Follow = getCellVal2(row.GetCell(30)),
                AddDate = addTime,
                AddUserId = MyInfo.Id,
                LastDate = addTime,
                LastUserId = MyInfo.Id
            };
            model.SaleDate = $"{model.Year}-{model.Month}-{model.Day}".ToDateTime();

            return CountSaleDate(model);
        }
        /// <summary>
        /// 根据数据计算收入成本
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Finance_EveryDaySaleLog CountSaleDate(Finance_EveryDaySaleLog model)
        {
            //销售收入
            model.SaleIncome = (model.SaleCount != null && model.SalePrice != null && model.ExchangeRate != null)
                            ? model.SaleCount * model.SalePrice * model.ExchangeRate
                            : null;
            //材料成本合计
            model.MaterialTotalPrice = (model.SaleCount != null && model.MaterialUnitPrice != null)
                                    ? model.SaleCount * model.MaterialUnitPrice
                                    : null;
            //加工成本合计    
            model.ProcessTotalPrice = (model.SaleCount != null && model.ProcessUnitPrice != null)
                                    ? model.SaleCount * model.ProcessUnitPrice
                                    : null;
            //毛利额
            if (model.SaleIncome != null)
            {
                model.GrossProfit = model.SaleIncome;
                if (model.MaterialTotalPrice != null)
                    model.GrossProfit -= model.MaterialTotalPrice;
                if (model.ProcessTotalPrice != null)
                    model.GrossProfit -= model.ProcessTotalPrice;
            }
            //毛利率
            model.GrossProfitRate = (model.GrossProfit / model.SaleIncome);

            if (model.SaleDate == Tools.Usual.Common.InitDateTime() ||
                string.IsNullOrEmpty(model.SaleName) ||
                string.IsNullOrEmpty(model.Customer) ||
                string.IsNullOrEmpty(model.ProductName) ||
                string.IsNullOrEmpty(model.ProductSKU))
                return null;

            return model;
        }
        /// <summary>
        /// 单元格值为空时返回null
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private decimal? getCellVal(ICell cell)
        {
            var val = cell.ToString();
            if (string.IsNullOrWhiteSpace(val))
                return null;
            else
                return val.ToDecimal();
        }
        private bool? getCellVal2(ICell cell)
        {
            var val = cell.ToString();
            if (string.IsNullOrWhiteSpace(val))
                return null;
            else
                return val == "是";
        }

        #endregion

        #region 编辑
        public ActionResult Save(int id = 0)
        {
            Finance_EveryDaySaleLog model = null;
            if (id > 0)
            {
                model = Bll.BllFinance_EveryDaySaleLog.First(o => o.Id == id && o.Display != 2);
                if (model == null)
                    return LayerAlertSuccessAndRefresh("加载数据失败，未找到该数据");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Finance_EveryDaySaleLog everyDaySaleLog)
        {
            everyDaySaleLog.Follow = Request["follow"] == "on" ? true : false;

            //关键字段不能为空
            if (everyDaySaleLog.SaleDate == null ||
                string.IsNullOrWhiteSpace(everyDaySaleLog.SaleName) ||
                string.IsNullOrWhiteSpace(everyDaySaleLog.Customer) ||
                string.IsNullOrWhiteSpace(everyDaySaleLog.ProductName) ||
                string.IsNullOrWhiteSpace(everyDaySaleLog.ProductSKU))
            {
                return LayerMsgErrorAndReturn("字段不能为空！");
            }

            //数据是否重复
            Finance_EveryDaySaleLog model = Bll.BllFinance_EveryDaySaleLog.First(o => o.Id != everyDaySaleLog.Id &&
                                                                                o.SaleDate == everyDaySaleLog.SaleDate &&
                                                                                o.SaleName == everyDaySaleLog.SaleName &&
                                                                                o.Customer == everyDaySaleLog.Customer &&
                                                                                o.ProductName == everyDaySaleLog.ProductName &&
                                                                                o.ProductSKU == everyDaySaleLog.ProductSKU &&
                                                                                o.Display != 2);
            if (model != null)
                return LayerMsgErrorAndReturn("添加数据失败，该条数据已存在，请确认后重新提交！");

            if (everyDaySaleLog.Id < 1)
                model = new Finance_EveryDaySaleLog();
            else
                model = Bll.BllFinance_EveryDaySaleLog.First(o => o.Id == everyDaySaleLog.Id && o.Display != 2) ?? new Finance_EveryDaySaleLog();

            model.Year = everyDaySaleLog.SaleDate.GetValueOrDefault().Year;
            model.Month = everyDaySaleLog.SaleDate.GetValueOrDefault().Month;
            model.Day = everyDaySaleLog.SaleDate.GetValueOrDefault().Day;
            model.DepartmentName = everyDaySaleLog.DepartmentName;
            model.ContractNumber = everyDaySaleLog.ContractNumber;
            model.SaleCount = everyDaySaleLog.SaleCount;
            model.SalePrice = everyDaySaleLog.SalePrice;
            model.Currency = everyDaySaleLog.Currency;
            model.ExchangeRate = everyDaySaleLog.ExchangeRate;
            model.MaterialUnitPrice = everyDaySaleLog.MaterialUnitPrice;
            model.ProcessUnitPrice = everyDaySaleLog.ProcessUnitPrice;
            model.ChangeCostNumber = everyDaySaleLog.ChangeCostNumber;
            model.ChangeCostMatter = everyDaySaleLog.ChangeCostMatter;
            model.ContributionMoney = everyDaySaleLog.ContributionMoney;
            model.ContributionRatio = everyDaySaleLog.ContributionRatio;
            model.AvgCoatUndue = everyDaySaleLog.AvgCoatUndue;
            model.AvgCoatCurrentdue = everyDaySaleLog.AvgCoatCurrentdue;
            model.AvgCoatOverdue = everyDaySaleLog.AvgCoatOverdue;
            model.CustomerContributionMoney = everyDaySaleLog.CustomerContributionMoney;
            model.CustomerContributionRatio = everyDaySaleLog.CustomerContributionRatio;
            model.Follow = everyDaySaleLog.Follow;

            model.LastDate = DateTime.Now;
            model.LastUserId = MyInfo.Id;

            model = CountSaleDate(model);

            bool isok = false;
            //没有ID就新增，反之修改
            if (model.Id < 1)
            {
                model.AddDate = model.LastDate;
                model.AddUserId = model.LastUserId;
                model.Display = 1;

                isok = Bll.BllFinance_EveryDaySaleLog.Insert(model) > 0;
            }
            else
                isok = Bll.BllFinance_EveryDaySaleLog.Update(model) > 0;

            return LayerMsgSuccessAndRefresh("保存" + (isok ? "成功" : "失败"));
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllFinance_EveryDaySaleLog.Delete(id, MyInfo.Id) > 0 });
        }

        [HttpPost]
        public JsonResult DeleteBatch(List<Finance_EveryDaySaleLog> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllFinance_EveryDaySaleLog.DeleteList(list, MyInfo.Id) > 0 });
        }
        #endregion
    }
}