using FancyFix.OA.Model;
using FancyFix.Tools.Tool;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FancyFix.OA.Areas.FinanceStatistics.Common
{
    /// <summary>
    /// 自定义导出 Finance_Statistics 表到Excel
    /// </summary>
    public class StatisticsExport
    {
        //总列数索引
        public readonly static int otherColIndex = 12;

        public static HSSFWorkbook CustomStatisticsExport(IEnumerable<Finance_Statistics> statisticsList, List<string> departmentList)
        {
            int colTotal = otherColIndex + departmentList.Count() * 7;

            NPOIHelper sheet = new NPOIHelper(defaultFontSize: 12);

            //将Excel背景色改为白色
            //for (int i = 0; i <= colTotal; i++)
            //    //sheet.SetDefaultColumnStyle(i, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.Yellow()));
            //    sheet.SetDefaultColumnStyle(i, sheet.CellStyle(new NPOI.XSSF.UserModel.XSSFColor().RGB)));

            //设置列宽
            for (int i = 3; i <= colTotal; i++)
                sheet.SetColumnWidth(i, 15 * 256);

            #region row0
            sheet.CreateRow(0);
            sheet.MergeCells(0, 0, 0, colTotal);
            sheet.CreateCell(0, "各部门销售日控表", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, font: sheet.FontStyle(fontsize: 18)));
            sheet.SetHeight(25 * 25);
            #endregion

            #region row1
            sheet.CreateRow(1);
            sheet.MergeCells(1, 1, 0, 2);
            sheet.SetHeight(20 * 20);
            SetTopBorder(ref sheet, 0, colTotal, BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12));
            //SetTopBorder(ref sheet, 0, colTotal, BorderStyle.Medium, sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12));
            sheet.CreateCell(0, "项目", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center
                , top: BorderStyle.Medium, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
            sheet.CreateCell(1, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));
            sheet.CreateCell(2, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            sheet.MergeCells(1, 1, 3, otherColIndex);
            sheet.CreateCell(3, "公司", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            for (int i = 4; i < 12; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            sheet.CreateCell(12, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
            //循环显示部门
            int departmentCol = otherColIndex + 1;
            foreach (var item in departmentList)
            {
                sheet.MergeCells(1, 1, departmentCol, departmentCol + 6);
                sheet.CreateCell(departmentCol, item, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center
                    , top: BorderStyle.Medium, left: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

                sheet.CreateCell(departmentCol + 1, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
                sheet.CreateCell(departmentCol + 2, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));
                sheet.CreateCell(departmentCol + 3, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));
                sheet.CreateCell(departmentCol + 4, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
                sheet.CreateCell(departmentCol + 5, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

                sheet.CreateCell(departmentCol + 6, "", sheet.CellStyle(top: BorderStyle.Medium, right: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

                departmentCol += 7;
            }
            #endregion

            #region row2
            sheet.CreateRow(2);
            sheet.SetHeight(20 * 20);
            sheet.MergeCells(2, 2, 0, 2);
            SetTopBorder(ref sheet, 0, colTotal, BorderStyle.Thin, sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12));
            sheet.CreateCell(0, "日期", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin));
            sheet.CreateCell(2, "", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin));

            sheet.MergeCells(2, 2, 3, 5);
            sheet.CreateCell(3, "营业收入情况", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin));

            sheet.CreateCell(6, "收款情况", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center
               , top: BorderStyle.Thin, left: BorderStyle.Thin));

            sheet.MergeCells(2, 2, 7, 9);
            sheet.CreateCell(7, "销售发货情况", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center
               , top: BorderStyle.Thin, left: BorderStyle.Thin));

            sheet.MergeCells(2, 2, 10, 12);
            sheet.CreateCell(10, "产品中心", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center
               , top: BorderStyle.Thin, left: BorderStyle.Thin));
            sheet.CreateCell(12, "", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin));

            //循环显示部门
            departmentCol = otherColIndex + 1;
            foreach (var item in departmentList)
            {
                sheet.MergeCells(2, 2, departmentCol, departmentCol + 2);
                sheet.CreateCell(departmentCol, "营业收入情况", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Medium));
                sheet.CreateCell(departmentCol + 1, "", sheet.CellStyle(top: BorderStyle.Thin, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
                sheet.CreateCell(departmentCol + 2, "", sheet.CellStyle(top: BorderStyle.Thin, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
                sheet.CreateCell(departmentCol + 3, "收款情况", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin));
                sheet.MergeCells(2, 2, departmentCol + 4, departmentCol + 6);
                sheet.CreateCell(departmentCol + 4, "销售发货情况", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin));
                sheet.CreateCell(departmentCol + 5, "", sheet.CellStyle(top: BorderStyle.Thin, right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
                sheet.CreateCell(departmentCol + 6, "", sheet.CellStyle(top: BorderStyle.Thin, right: BorderStyle.Medium));

                departmentCol += 7;
            }
            #endregion

            #region row3
            sheet.CreateRow(3);
            sheet.SetHeight(25 * 25);
            SetTopBorder(ref sheet, 0, colTotal);
            sheet.CreateCell(0, "年", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            List<string> colStr = new List<string> { "月", "日", "营业收入", "营业收入预算值", "营业收入达成率", "实际收款", "实际发货\r\n订单数量", "计划发货\r\n订单数量", "发货准时率", "生产完工率", "采购入库及时率" };

            //循环显示
            for (int i = 0; i < colStr.Count; i++)
                sheet.CreateCell(1 + i, colStr[i], sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));

            sheet.CreateCell(otherColIndex, "订单发货准时率", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Medium));

            //循环显示部门
            colStr.RemoveRange(0, 2);
            colStr.RemoveRange(7, 2);
            departmentCol = otherColIndex + 1;
            foreach (var item in departmentList)
            {
                for (int i = 0; i < 7; i++)
                    if (i < 6)
                        sheet.CreateCell(departmentCol + i, colStr[i], sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Thin));
                    else
                        sheet.CreateCell(departmentCol + i, colStr[i], sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Medium));

                departmentCol += 7;
            }
            #endregion

            #region 循环其余行
            var saledateList = (from o in statisticsList orderby o.SaleDate ascending select o.SaleDate)?.Distinct().ToList();

            int year = 0;
            int month = 0;
            int year2 = 0;
            int month2 = 0;
            int rowIndex = 3;
            DateTime lastDate = saledateList.Last().GetValueOrDefault();

            foreach (var saledate in saledateList)
            {
                var saledate2 = saledate.GetValueOrDefault();
                var statisticsList2 = new List<Finance_Statistics>();

                #region 普通结算
                //最后一条记录时闲执行普通结算
                if (saledate2 == lastDate)
                {
                    statisticsList2 = (from o in statisticsList where o.SaleDate == saledate2 select o).ToList();
                    sheet.CreateRow(++rowIndex);
                    sheet.SetHeight(20 * 20);
                    SetTopBorder(ref sheet, 0, colTotal);
                    //年
                    sheet.CreateCell(0, saledate2.Year.ToString(), sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
                    //月
                    sheet.CreateCell(1, saledate2.Month.ToString(), sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12), left: BorderStyle.Thin));
                    //日
                    sheet.CreateCell(2, saledate2.Day.ToString(), sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12), left: BorderStyle.Thin));

                    LoadDepartmentDate(ref sheet, statisticsList2, departmentList, sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12));
                }
                #endregion

                year2 = year;
                year = saledate2.Year;
                month2 = month;
                month = saledate2.Month;

                #region 月底结算 
                if ((month != month2 && month2 != 0) || saledate2 == lastDate)
                {
                    statisticsList2 = (from o in statisticsList where o.Year == year2 && o.Month == month2 select o).ToList();

                    sheet.CreateRow(++rowIndex);
                    sheet.SetHeight(20 * 20);
                    SetTopBorder(ref sheet, 0, colTotal);
                    sheet.MergeCells(rowIndex, rowIndex, 0, 2);
                    sheet.CreateCell(0, $"{month2}月合计", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center,
                        font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12), top: BorderStyle.Thin));

                    LoadDepartmentDate(ref sheet, statisticsList2, departmentList, sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12));
                }
                #endregion

                #region 年底结算
                if ((year != year2 && year2 != 0) || saledate2 == lastDate)
                {
                    statisticsList2 = (from o in statisticsList where o.Year == year2 select o).ToList();

                    sheet.CreateRow(++rowIndex);
                    sheet.SetHeight(20 * 20);
                    SetTopBorder(ref sheet, 0, colTotal);
                    sheet.MergeCells(rowIndex, rowIndex, 0, 2);
                    sheet.CreateCell(0, year + "年累计", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center
                        , font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12), top: BorderStyle.Thin));

                    LoadDepartmentDate(ref sheet, statisticsList2, departmentList, sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12));
                }
                #endregion

                #region 普通结算
                if (saledate2 != lastDate)
                {
                    statisticsList2 = (from o in statisticsList where o.SaleDate == saledate2 select o).ToList();
                    sheet.CreateRow(++rowIndex);
                    sheet.SetHeight(20 * 20);
                    SetTopBorder(ref sheet, 0, colTotal);
                    //年
                    sheet.CreateCell(0, saledate2.Year.ToString(), sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12)));
                    //月
                    sheet.CreateCell(1, saledate2.Month.ToString(), sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12), left: BorderStyle.Thin));
                    //日
                    sheet.CreateCell(2, saledate2.Day.ToString(), sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12), left: BorderStyle.Thin));

                    LoadDepartmentDate(ref sheet, statisticsList2, departmentList, sheet.FontStyle(boldweight: (short)FontBoldWeight.Normal, fontsize: 12));
                }
                #endregion
            }

            //结尾
            sheet.CreateRow(++rowIndex);
            sheet.SetHeight(20 * 20);
            for (int i = 0; i <= colTotal; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(top: BorderStyle.Medium, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold, fontsize: 12)));

            #endregion

            return sheet.GetWorkbook();
        }

        private static void LoadDepartmentDate(ref NPOIHelper sheet, List<Finance_Statistics> statisticsList, IEnumerable<string> departmentList, IFont font)
        {
            decimal? valA = 0, valB = 0;
            //营业收入
            valA = (from o in statisticsList select o.BusinessIncome)?.Sum() ?? null;
            sheet.CreateCell(3, valA?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: font));
            //营业收入预算值
            valB = (from o in statisticsList select o.BudgetaryValue)?.Sum() ?? null;
            sheet.CreateCell(4, valB?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: font));
            //营业收入达成率
            sheet.CreateCell(5, dataFormat2(Bll.BllFinance_Statistics.GetBusinessRate(valA, valB)) ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center,
            top: BorderStyle.Thin, left: BorderStyle.Thin, dataFormat: 1, font: font));
            //实际收款
            valA = (from o in statisticsList select o.ActualReceipts)?.Sum() ?? null;
            sheet.CreateCell(6, valA?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: font));
            //实际发货订单数量
            valA = (from o in statisticsList select o.ActualDeliveryOrderNumber)?.Sum() ?? null;
            sheet.CreateCell(7, valA?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: font));
            //计划发货订单数量
            valB = (from o in statisticsList select o.PlanDeliveryOrderNumber)?.Sum() ?? null;
            sheet.CreateCell(8, valB?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, font: font));
            //发货准时率
            sheet.CreateCell(9, dataFormat2(Bll.BllFinance_Statistics.GetBusinessRate(valA, valB)) ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center,
                top: BorderStyle.Thin, left: BorderStyle.Thin, dataFormat: 1, font: font));
            //生产完工率
            sheet.CreateCell(10, "", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center,
                top: BorderStyle.Thin, left: BorderStyle.Thin, dataFormat: 1, font: font));
            //采购入库及时率
            sheet.CreateCell(11, "", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center,
                top: BorderStyle.Thin, left: BorderStyle.Thin, dataFormat: 1, font: font));
            //订单发货准时率
            sheet.CreateCell(12, "", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center,
                top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Medium, dataFormat: 1, font: font));

            //循环每个部门
            int departmentCol = otherColIndex + 1;
            foreach (var item in departmentList)
            {
                var statisticsList2 = (from o in statisticsList where o.DepartmentName == item select o).ToList();
                //营业收入
                valA = (from o in statisticsList2 select o.BusinessIncome)?.Sum() ?? null;
                sheet.CreateCell(departmentCol, valA?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin
                    , left: BorderStyle.Thin, right: BorderStyle.Thin, font: font));
                //营业收入预算值
                valB = (from o in statisticsList2 select o.BudgetaryValue)?.Sum() ?? null;
                sheet.CreateCell(departmentCol + 1, valB?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin
                    , left: BorderStyle.Thin, right: BorderStyle.Thin, font: font));
                //营业收入达成率
                sheet.CreateCell(departmentCol + 2, dataFormat2(Bll.BllFinance_Statistics.GetBusinessRate(valA, valB)) ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center
                    , va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, dataFormat: 1, font: font));
                //实际收款
                valA = (from o in statisticsList2 select o.ActualReceipts)?.Sum() ?? null;
                sheet.CreateCell(departmentCol + 3, valA?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin
                    , left: BorderStyle.Thin, right: BorderStyle.Thin, font: font));
                //实际发货订单数量
                valA = (from o in statisticsList2 select o.ActualDeliveryOrderNumber)?.Sum() ?? null;
                sheet.CreateCell(departmentCol + 4, valA?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin
                    , left: BorderStyle.Thin, right: BorderStyle.Thin, font: font));
                //计划发货订单数量
                valB = (from o in statisticsList2 select o.PlanDeliveryOrderNumber)?.Sum() ?? null;
                sheet.CreateCell(departmentCol + 5, valB?.ToString("f2") ?? null, sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin
                    , left: BorderStyle.Thin, right: BorderStyle.Thin, font: font));
                //发货准时率
                sheet.CreateCell(departmentCol + 6, dataFormat2(Bll.BllFinance_Statistics.GetBusinessRate(valA, valB)), sheet.CellStyle(ha: HorizontalAlignment.Center
                    , va: VerticalAlignment.Center, top: BorderStyle.Thin, left: BorderStyle.Thin, right: BorderStyle.Medium, dataFormat: 1, font: font));

                departmentCol += 7;
            }
        }

        /// <summary>
        /// 为行添加上边框
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        private static void SetTopBorder(ref NPOIHelper sheet, int startCell = 0, int endCell = 0, BorderStyle borderStyle = BorderStyle.Thin, IFont font = null)
        {
            for (int i = startCell; i <= endCell; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(ha: HorizontalAlignment.Center, va: VerticalAlignment.Center, top: BorderStyle.Thin, font: font));
        }

        private static string dataFormat2(decimal? val)
        {
            if (val == null)
                return null;

            return val == null ? null : (val.GetValueOrDefault().ToString("f2") + "%");
        }

    }
}