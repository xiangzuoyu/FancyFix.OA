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
        public readonly static int ColTotalIndex = 32;

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

            string[] arr = new string[] { "部门", "销售人员", "客户/店铺", "合同号", "供应商", "产品名称", "产品SKU", "产品规格", "销售数量", "销售单价", "币种", "汇率", "销售收入" };
            RepeatSetCell(ref sheet, 4, 16, cellStyleDefault1, arr);

            sheet.MergeCells(2, 2, 17, 20);
            sheet.CreateCell(17, "直接成本", cellStyleDefault1);

            sheet.MergeCells(2, 4, 21, 21);
            sheet.CreateCell(21, "毛益额", cellStyleDefault1);

            sheet.MergeCells(2, 4, 22, 22);
            sheet.CreateCell(22, "毛益率", cellStyleDefault1);

            sheet.MergeCells(2, 2, 23, 24);
            sheet.CreateCell(23, "变动费用", cellStyleDefault1);

            sheet.MergeCells(2, 2, 25, 26);
            sheet.CreateCell(25, "经营贡献", cellStyleDefault1);

            sheet.MergeCells(2, 2, 27, 29);
            sheet.CreateCell(27, "应收账款平均资金成本", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            sheet.MergeCells(2, 2, 30, 31);
            sheet.CreateCell(30, "应收账款平均资金成本", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            sheet.MergeCells(2, 4, 32, 32);
            sheet.CreateCell(32, "是否关注", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
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

            for (int i = 4; i <= 16; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.MergeCells(3, 3, 17, 18);
            sheet.CreateCell(17, "单件价格", cellStyleDefault1);

            sheet.CreateCell(18, "", sheet.CellStyle(top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.MergeCells(3, 3, 19, 20);
            sheet.CreateCell(19, "合计金额", cellStyleDefault1);

            sheet.CreateCell(20, "", sheet.CellStyle(top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.CreateCell(21, "", sheet.CellStyle(left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.CreateCell(22, "", sheet.CellStyle(left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.MergeCells(3, 4, 23, 23);
            sheet.CreateCell(23, "量", cellStyleDefault1);

            sheet.MergeCells(3, 4, 24, 24);
            sheet.CreateCell(24, "事", cellStyleDefault1);

            sheet.MergeCells(3, 4, 25, 25);
            sheet.CreateCell(25, "金额", cellStyleDefault1);

            sheet.MergeCells(3, 4, 26, 26);
            sheet.CreateCell(26, "比例", cellStyleDefault1);

            sheet.MergeCells(3, 4, 27, 27);
            sheet.CreateCell(27, "未到期", cellStyleDefault1);

            sheet.MergeCells(3, 4, 28, 28);
            sheet.CreateCell(28, "当期", cellStyleDefault1);

            sheet.MergeCells(3, 4, 29, 29);
            sheet.CreateCell(29, "逾期", cellStyleDefault1);

            sheet.MergeCells(3, 4, 30, 30);
            sheet.CreateCell(30, "金额", cellStyleDefault1);

            sheet.MergeCells(3, 4, 31, 31);
            sheet.CreateCell(31, "比例", cellStyleDefault1);

            sheet.CreateCell(ColTotalIndex, "", sheet.CellStyle(top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
            #endregion

            #region row4
            sheet.CreateRow(++rowIndex);

            for (int i = 0; i <= 16; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.CreateCell(17, "料", cellStyleDefault1);
            sheet.CreateCell(18, "工", cellStyleDefault1);
            sheet.CreateCell(19, "料", cellStyleDefault1);
            sheet.CreateCell(20, "工", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            for (int i = 21; i < ColTotalIndex; i++)
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
            //序号
            sheet.CreateCell(0, dataIndex, cellStyle);
            //年
            sheet.CreateCell(1, item.Year.ToString(), cellStyle);
            //月
            sheet.CreateCell(2, item.Month.ToString(), cellStyle);
            //日
            sheet.CreateCell(3, item.Day.ToString(), cellStyle);
            //部门
            sheet.CreateCell(4, item.DepartmentName, cellStyle);
            //销售人员
            sheet.CreateCell(5, item.SaleName, cellStyle);
            //客户/店铺
            sheet.CreateCell(6, item.Customer, cellStyle);
            //合同号
            sheet.CreateCell(7, item.ContractNumber, cellStyle);
            //供应商
            sheet.CreateCell(8, item.Supplier, cellStyle);
            //产品名称
            sheet.CreateCell(9, item.ProductName, cellStyle);
            //产品SKU
            sheet.CreateCell(10, item.ProductSKU, cellStyle);
            //产品规格
            sheet.CreateCell(11, item.ProductSpecification, cellStyle);
            //销售数量
            sheet.CreateCell(12, item.SaleCount?.ToString("f2") ?? "0", cellStyle);
            //销售单价
            sheet.CreateCell(13, item.SalePrice?.ToString("f2") ?? "0", cellStyle);
            //货币
            sheet.CreateCell(14, item.Currency, cellStyle);
            //汇率
            sheet.CreateCell(15, item.ExchangeRate?.ToString("f2") ?? "0", cellStyle);
            //销售收入
            sheet.CreateCell(16, item.SaleIncome?.ToString("f2") ?? "0", cellStyle);
            //单价料
            sheet.CreateCell(17, item.MaterialUnitPrice?.ToString("f2") ?? "0", cellStyle);
            //单价工
            sheet.CreateCell(18, item.ProcessUnitPrice?.ToString("f2") ?? "0", cellStyle);
            //合计料
            sheet.CreateCell(19, item.MaterialTotalPrice?.ToString("f2") ?? "0", cellStyle);
            //合计工
            sheet.CreateCell(20, item.ProcessTotalPrice?.ToString("f2") ?? "0", cellStyle);
            //毛益额
            sheet.CreateCell(21, item.GrossProfit?.ToString("f2") ?? "0", cellStyle);
            //毛益率
            sheet.CreateCell(22, item.GrossProfitRate?.ToString("f2") + "%" ?? "", cellStyle);
            //量
            sheet.CreateCell(23, item.ChangeCostNumber?.ToString("f2") ?? "0", cellStyle);
            //事
            sheet.CreateCell(24, item.ChangeCostMatter?.ToString("f2") ?? "0", cellStyle);
            //金额
            sheet.CreateCell(25, item.ContributionMoney?.ToString("f2") ?? "0", cellStyle);
            //比例
            sheet.CreateCell(26, item.ContributionRatio?.ToString("f2") ?? "0", cellStyle);
            //未到期
            sheet.CreateCell(27, item.AvgCoatUndue?.ToString("f2") ?? "0", cellStyle);
            //当期
            sheet.CreateCell(28, item.AvgCoatCurrentdue?.ToString("f2") ?? "0", cellStyle);
            //逾期
            sheet.CreateCell(29, item.AvgCoatOverdue?.ToString("f2") ?? "0", cellStyle);
            //金额
            sheet.CreateCell(30, item.CustomerContributionMoney?.ToString("f2") ?? "0", cellStyle);
            //比例
            sheet.CreateCell(31, item.ContributionRatio?.ToString("f2") ?? "0", cellStyle);
            //是否关注
            sheet.CreateCell(32, (item.Follow != null ? (item.Follow == true ? "是" : "否") : "否"), cellStyle2);
        }

        private static void RepeatSetCell(ref NPOIHelper2 sheet, int startCellIndex, int EndCellIndex, ICellStyle cellStyle, string[] cellNames)
        {
            for (int i = startCellIndex, j = 0; i <= EndCellIndex; i++, j++)
            {
                sheet.MergeCells(2, 4, i, i);
                sheet.CreateCell(i, cellNames[j], cellstyle: cellStyle);
            }
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