using FancyFix.OA.Model;
using FancyFix.Tools.Tool;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Tools.Tool;

namespace FancyFix.OA.Areas.FinanceStatistics.Common
{
    /// <summary>
    /// 自定义导出 Finance_EveryDaySaleLogExport 表到Excel
    /// </summary>
    public class EveryDaySaleLogExport
    {
        //总列索引数
        public readonly static int ColTotalIndex = 33;

        public static XSSFWorkbook CustomEveryDaySaleLogExport(IEnumerable<Finance_EveryDaySaleLog> statisticsList, string department)
        {
            int rowIndex = 0;
            NPOIHelper2 sheet = new NPOIHelper2(defaultFontSize: 12);

            //将Excel背景色改为白色
            for (int i = 0; i <= ColTotalIndex; i++)
                sheet.SetDefaultColumnStyle(i, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White()));

            var cellStyleDefault1 = sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12));

            var cellStyleDefault2 = sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12));

            var cellStyleDefault3 = sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12));

            var cellStyleDefault4 = sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12));
            //设置列宽
            for (int i = 3; i <= ColTotalIndex; i++)
                sheet.SetColumnWidth(i, 15 * 256);

            #region row0
            sheet.CreateRow(rowIndex);
            sheet.MergeCells(0, 0, 0, ColTotalIndex);
            sheet.CreateCell(0, (department == "0" ? "" : department) + "价值分析表", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, font: sheet.FontStyle(fontsize: 18)));
            sheet.SetHeight(25 * 25);
            #endregion

            #region row1
            sheet.CreateRow(++rowIndex);
            for (int i = 0; i <= ColTotalIndex; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));
            sheet.MergeCells(1, 1, 0, 22);
            sheet.CreateCell(0, "每日", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Medium, font: sheet.FontStyle(fontsize: 18)));

            sheet.MergeCells(1, 1, 23, ColTotalIndex);
            sheet.CreateCell(23, "每月", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Medium, font: sheet.FontStyle(fontsize: 18)));

            sheet.CreateCell(ColTotalIndex, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));
            #endregion

            #region row2
            sheet.CreateRow(++rowIndex);
            for (int i = 0; i <= ColTotalIndex; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(top: BorderStyle.Thin, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.CreateCell(0, "内容", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            sheet.MergeCells(2, 2, 1, 3);
            sheet.CreateCell(1, "日期", cellStyleDefault1);

            string[] arr = new string[] { "部门", "销售人员", "客户/店铺", "合同号", "供应商", "产品名称", "SPU", "产品SKU", "产品规格", "销售数量", "销售单价", "币种", "汇率", "销售收入" };
            int colIndex = RepeatSetCell(ref sheet, 4, cellStyleDefault1, arr);

            colIndex += 4;
            sheet.MergeCells(2, 2, colIndex - 3, colIndex);
            sheet.CreateCell(colIndex - 3, "直接成本", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(2, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "毛益额", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(2, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "毛益率", cellStyleDefault1);

            colIndex += 2;
            sheet.MergeCells(2, 2, colIndex - 1, colIndex);
            sheet.CreateCell(colIndex - 1, "变动费用", cellStyleDefault1);

            colIndex += 2;
            sheet.MergeCells(2, 2, colIndex - 1, colIndex);
            sheet.CreateCell(colIndex - 1, "经营贡献", cellStyleDefault1);

            colIndex += 3;
            sheet.MergeCells(2, 2, colIndex - 2, colIndex);
            sheet.CreateCell(colIndex - 2, "应收账款平均资金成本", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            colIndex += 2;
            sheet.MergeCells(2, 2, colIndex - 1, colIndex);
            sheet.CreateCell(colIndex - 1, "应收账款平均资金成本", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            colIndex++;
            sheet.MergeCells(2, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "是否关注", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
            #endregion

            #region row3
            sheet.CreateRow(++rowIndex);

            sheet.MergeCells(3, 4, 0, 0);
            sheet.CreateCell(0, "序号", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            sheet.MergeCells(3, 4, 1, 1);
            sheet.CreateCell(1, "年", cellStyleDefault1);

            sheet.MergeCells(3, 4, 2, 2);
            sheet.CreateCell(2, "月", cellStyleDefault1);

            sheet.MergeCells(3, 4, 3, 3);
            sheet.CreateCell(3, "日", cellStyleDefault1);

            for (int i = 4, j = 0; j < arr.Length; i++, j++)
                sheet.CreateCell(i, "", sheet.CellStyle(left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            colIndex = 4 + arr.Length;
            colIndex += 1;
            sheet.MergeCells(3, 3, colIndex - 1, colIndex);
            sheet.CreateCell(colIndex - 1, "单件价格", cellStyleDefault1);
            sheet.CreateCell(colIndex, "", sheet.CellStyle(top: BorderStyle.Thin, left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            colIndex += 2;
            sheet.MergeCells(3, 3, colIndex - 1, colIndex);
            sheet.CreateCell(colIndex - 1, "合计金额", cellStyleDefault1);
            sheet.CreateCell(colIndex, "", sheet.CellStyle(top: BorderStyle.Thin, left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));
            sheet.CreateCell(colIndex + 1, "", sheet.CellStyle(left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));
            sheet.CreateCell(colIndex + 2, "", sheet.CellStyle(left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));
            colIndex += 2;

            colIndex++;
            sheet.MergeCells(3, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "量", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(3, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "事", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(3, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "金额", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(3, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "比例", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(3, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "未到期", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(3, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "当期", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(3, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "逾期", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(3, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "金额", cellStyleDefault1);

            colIndex++;
            sheet.MergeCells(3, 4, colIndex, colIndex);
            sheet.CreateCell(colIndex, "比例", cellStyleDefault1);

            sheet.CreateCell(ColTotalIndex, "", sheet.CellStyle(top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
            #endregion

            #region row4
            sheet.CreateRow(++rowIndex);

            colIndex = 4 + arr.Length;
            for (int i = 0; i < colIndex; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.CreateCell(++colIndex, "料", cellStyleDefault1);
            sheet.CreateCell(++colIndex, "工", cellStyleDefault1);
            sheet.CreateCell(++colIndex, "料", cellStyleDefault1);
            sheet.CreateCell(++colIndex, "工", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            colIndex++;
            for (int i = colIndex; i < ColTotalIndex; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.CreateCell(ColTotalIndex, "", sheet.CellStyle(right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));
            #endregion

            #region 循环其余行

            int month = 0;
            int month2 = 0;
            int year = 0;
            int year2 = 0;
            int dataIndex = 0;
            int lastId = statisticsList.Select(o => o.Id).Last();
            List<Finance_EveryDaySaleLog> list = null;
            foreach (var item in statisticsList)
            {
                dataIndex++;
                //if (dataIndex > 200000)
                //    break;

                #region 普通记录 在最后一条数据时显示
                if (item.Id == lastId)
                    AddData(ref sheet, dataIndex.ToString(), ++rowIndex, item, cellStyleDefault1, cellStyleDefault3);
                #endregion

                year2 = year;
                year = item.Year.GetValueOrDefault();
                month2 = month;
                month = item.Month.GetValueOrDefault();

                #region 月统计记录
                if ((month != month2 && month2 != 0) || lastId == item.Id)
                {
                    list = (from o in statisticsList where o.Year == year2 && o.Month == month2 select o).ToList();
                    AddData(ref sheet, $"{month2}月合计", ++rowIndex, CountData(list, year2, month2), cellStyleDefault2, cellStyleDefault4);
                }
                #endregion

                #region 年统计记录
                if ((year != year2 && year2 != 0) || lastId == item.Id)
                {
                    list = (from o in statisticsList where o.Year == year2 select o).ToList();
                    AddData(ref sheet, $"{year2}年合计", ++rowIndex, CountData(list, year2, month2), cellStyleDefault2, cellStyleDefault4);
                }
                #endregion

                #region 合计统计记录
                if (lastId == item.Id)
                    AddData(ref sheet, "合计", ++rowIndex, CountData(list, year2, month2), cellStyleDefault2, cellStyleDefault4);

                if (item.Id != lastId)
                    AddData(ref sheet, dataIndex.ToString(), ++rowIndex, item, cellStyleDefault1, cellStyleDefault3);
                #endregion
            }

            #endregion

            #region 收尾
            sheet.CreateRow(++rowIndex);
            for (int i = 0; i <= ColTotalIndex; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(top: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
            #endregion
            return sheet.GetWorkbook();
        }

        private static Finance_EveryDaySaleLog CountData(List<Finance_EveryDaySaleLog> list, int year, int month)
        {
            var model = new Finance_EveryDaySaleLog();
            model.Year = year;
            model.Month = month;
            model.SaleCount = list.Select(o => o.SaleCount)?.Sum() ?? 0;
            model.SalePrice = list.Select(o => o.SalePrice)?.Sum() ?? 0;
            model.ExchangeRate = list.Select(o => o.ExchangeRate)?.Sum() ?? 0;
            model.SaleIncome = list.Select(o => o.SaleIncome)?.Sum() ?? 0;
            model.MaterialUnitPrice = list.Select(o => o.MaterialUnitPrice)?.Sum() ?? 0;
            model.ProcessUnitPrice = list.Select(o => o.ProcessUnitPrice)?.Sum() ?? 0;
            model.MaterialTotalPrice = list.Select(o => o.MaterialTotalPrice)?.Sum() ?? 0;
            model.ProcessTotalPrice = list.Select(o => o.ProcessTotalPrice)?.Sum() ?? 0;
            model.GrossProfit = model.SaleIncome - model.MaterialTotalPrice - model.ProcessTotalPrice;
            model.GrossProfitRate = Bll.BllFinance_Statistics.GetBusinessRate(model.GrossProfit, model.SaleIncome);

            return model;
        }

        private static void AddData(ref NPOIHelper2 sheet, string dataIndex, int rowIndex, Finance_EveryDaySaleLog item, ICellStyle cellStyle, ICellStyle cellStyle2)
        {

            sheet.CreateRow(rowIndex);
            int i = 0;
            //序号
            sheet.CreateCell(i++, dataIndex, cellStyle);
            //年
            sheet.CreateCell(i++, item.Year.ToString(), cellStyle);
            //月
            sheet.CreateCell(i++, item.Month.ToString(), cellStyle);
            //日
            sheet.CreateCell(i++, item.Day.ToString(), cellStyle);
            //部门
            sheet.CreateCell(i++, item.DepartmentName, cellStyle);
            //销售人员
            sheet.CreateCell(i++, item.SaleName, cellStyle);
            //客户/店铺
            sheet.CreateCell(i++, item.Customer, cellStyle);
            //合同号
            sheet.CreateCell(i++, item.ContractNumber, cellStyle);
            //供应商
            sheet.CreateCell(i++, item.Supplier, cellStyle);
            //产品名称
            sheet.CreateCell(i++, item.ProductName, cellStyle);
            //SPU
            sheet.CreateCell(i++, item.SPU, cellStyle);
            //产品SKU
            sheet.CreateCell(i++, item.ProductSKU, cellStyle);
            //产品规格
            sheet.CreateCell(i++, item.ProductSpecification, cellStyle);
            //销售数量
            sheet.CreateCell(i++, item.SaleCount?.ToString("f2") ?? "0", cellStyle);
            //销售单价
            sheet.CreateCell(i++, item.SalePrice?.ToString("f2") ?? "0", cellStyle);
            //货币
            sheet.CreateCell(i++, item.Currency, cellStyle);
            //汇率
            sheet.CreateCell(i++, item.ExchangeRate?.ToString("f2") ?? "0", cellStyle);
            //销售收入
            sheet.CreateCell(i++, item.SaleIncome?.ToString("f2") ?? "0", cellStyle);
            //单价料
            sheet.CreateCell(i++, item.MaterialUnitPrice?.ToString("f2") ?? "0", cellStyle);
            //单价工
            sheet.CreateCell(i++, item.ProcessUnitPrice?.ToString("f2") ?? "0", cellStyle);
            //合计料
            sheet.CreateCell(i++, item.MaterialTotalPrice?.ToString("f2") ?? "0", cellStyle);
            //合计工
            sheet.CreateCell(i++, item.ProcessTotalPrice?.ToString("f2") ?? "0", cellStyle);
            //毛益额
            sheet.CreateCell(i++, item.GrossProfit?.ToString("f2") ?? "0", cellStyle);
            //毛益率
            sheet.CreateCell(i++, item.GrossProfitRate?.ToString("f2") + "%" ?? "", cellStyle);
            //量
            sheet.CreateCell(i++, item.ChangeCostNumber?.ToString("f2") ?? "0", cellStyle);
            //事
            sheet.CreateCell(i++, item.ChangeCostMatter?.ToString("f2") ?? "0", cellStyle);
            //金额
            sheet.CreateCell(i++, item.ContributionMoney?.ToString("f2") ?? "0", cellStyle);
            //比例
            sheet.CreateCell(i++, item.ContributionRatio?.ToString("f2") ?? "0", cellStyle);
            //未到期
            sheet.CreateCell(i++, item.AvgCoatUndue?.ToString("f2") ?? "0", cellStyle);
            //当期
            sheet.CreateCell(i++, item.AvgCoatCurrentdue?.ToString("f2") ?? "0", cellStyle);
            //逾期
            sheet.CreateCell(i++, item.AvgCoatOverdue?.ToString("f2") ?? "0", cellStyle);
            //金额
            sheet.CreateCell(i++, item.CustomerContributionMoney?.ToString("f2") ?? "0", cellStyle);
            //比例
            sheet.CreateCell(i++, item.ContributionRatio?.ToString("f2") ?? "0", cellStyle);
            //是否关注
            sheet.CreateCell(i++, (item.Follow != null ? (item.Follow == true ? "是" : "否") : "否"), cellStyle2);
        }

        private static int RepeatSetCell(ref NPOIHelper2 sheet, int startCellIndex, ICellStyle cellStyle, string[] cellNames)
        {
            for (int i = startCellIndex, j = 0; j < cellNames.Length; i++, j++)
            {
                sheet.MergeCells(2, 4, i, i);
                sheet.CreateCell(i, cellNames[j], cellstyle: cellStyle);
                startCellIndex = i;
            }
            return startCellIndex;
        }

        /// <summary>
        /// 为行添加上边框
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        private static void SetTopBorder(ref NPOIHelper2 sheet, int startCell = 0, int endCell = 0, BorderStyle borderStyle = BorderStyle.Thin, IFont font = null)
        {
            for (int i = startCell; i <= endCell; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: font));
        }
    }
}