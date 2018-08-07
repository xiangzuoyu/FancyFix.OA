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
                    var val = row.GetCell(1)?.ToString();
                    var val1 = row.GetCell(2)?.ToString();
                    var val2 = row.GetCell(3)?.ToString();
                    var val3 = row.GetCell(5)?.ToString();
                    var val4 = row.GetCell(6)?.ToString();
                    var val5 = row.GetCell(8)?.ToString();
                    //年月日、销售人员、客户、产品名称为空时跳过
                    if (row == null ||
                        string.IsNullOrWhiteSpace(row.GetCell(1)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(2)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(3)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(5)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(6)?.ToString()) ||
                        string.IsNullOrWhiteSpace(row.GetCell(8)?.ToString()))
                        continue;

                    var rawMaterialModel = CreateEveryDaySaleLogModel(row, addTime);
                    if (rawMaterialModel == null)
                        continue;

                    int id = 0;
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
                MaterialTotalPrice = getCellVal(row.GetCell(17)),
                ProcessTotalPrice = getCellVal(row.GetCell(18)),
                GrossProfit = getCellVal(row.GetCell(19)),
                GrossProfitRate = getCellVal(row.GetCell(20)),
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

            SaleIncome = getCellVal(row.GetCell(14)),
            model.SaleDate = $"{model.Year}-{model.Month}-{model.Day}".ToDateTime();

            if (model.SaleDate == Tools.Usual.Common.InitDateTime() &&
                string.IsNullOrEmpty(model.SaleName) &&
                string.IsNullOrEmpty(model.Customer) &&
                string.IsNullOrEmpty(model.ProductName))
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


        //private bool AddPrice(IRow row, IRow headRow, int priceId, int cellTotal, DateTime logDate)
        //{
        //    if (row == null || headRow == null)
        //        return false;
        //    //List<Supplier_Price> list = new List<Supplier_Price>();
        //    //遍历个每行原材料后面的价格
        //    try
        //    {
        //        for (int i = 9; i < cellTotal; i++)
        //        {
        //            string price = row.GetCell(i).ToString();
        //            //如果价格为空，跳过
        //            if (string.IsNullOrEmpty(price))
        //                continue;

        //            var date = headRow.GetCell(i).ToString().Split('-');
        //            if (date.Length != 2)
        //                continue;
        //            var years = date[0].TrimStart('\'').ToInt32();

        //            var priceModel = new Supplier_Price
        //            {
        //                RawMaterialPriceId = priceId,
        //                Years = years,
        //                Month = date[1].ToInt32(),
        //                Price = price.ToDecimal(),
        //                AddDate = logDate,
        //                AddUserId = MyInfo.Id,
        //                LastDate = logDate,
        //                LastUserId = MyInfo.Id,
        //                Display = 1,
        //                YearsMonth = $"{years.ToString()}-{date[1].ToString()}-1".ToDateTime()
        //            };

        //            Bll.BllSupplier_Price.DeleteAndAdd(priceModel);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;

        //}
        #endregion
    }
}