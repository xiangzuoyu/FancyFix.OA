using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Tool
{
    public class NPOIHelper2
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();
        private ISheet temporarySheet;
        private IRow temporaryRow;
        private ICell temporaryCell;
        private ICellStyle temporaryCellStyle;
        private IFont temporaryFont;
        private IFont defaultTemporaryFont;//默认字体
        private IDataFormat dataformat;
        //初始化一个工作表
        public NPOIHelper2(string sheetName = "sheet1", int defaultFontSize = 0)
        {
            temporarySheet = workbook.CreateSheet(sheetName);

            //设置默认字体样式
            if (defaultFontSize > 0)
                defaultTemporaryFont = FontStyle(fontsize: defaultFontSize);

        }
        public XSSFWorkbook GetWorkbook()
        {
            //workbook.Dispose();
            return workbook;
        }
        //设置单元列默认样式
        public void SetDefaultColumnStyle(int column, ICellStyle cellStyle)
        {
            temporarySheet.SetDefaultColumnStyle(column, cellStyle);
        }
        //设置列宽
        public void SetColumnWidth(int columnIndex, int width)
        {
            temporarySheet.SetColumnWidth(columnIndex, width);
        }
        //新建行
        public void CreateRow(int rownum)
        {
            temporaryRow = temporarySheet.CreateRow(rownum);
        }
        //新建列
        public void CreateCell(int column, string cellValue = "", ICellStyle cellstyle = null)
        {
            temporaryCell = temporaryRow.CreateCell(column);
            if (cellstyle != null)
                temporaryCell.CellStyle = cellstyle;

            if (!string.IsNullOrEmpty(cellValue))
                temporaryCell.SetCellValue(cellValue);
        }
        //设置列样式
        public void SetCellStyle(ICellStyle cellStyle)
        {
            temporaryCell.CellStyle = cellStyle;
        }
        //设置行高
        public void SetHeight(short height)
        {
            temporaryRow.Height = height;
        }
        //获取单元格格式
        public ICellStyle GetCellFormat()
        {
            temporaryCellStyle = workbook.CreateCellStyle();
            dataformat = workbook.CreateDataFormat();
            //temporaryCellStyle.DataFormat = dataformat.GetFormat("0.00%");//百分数【小数点后有几个0表示精确到显示小数点后几位】
            //temporaryCellStyle.DataFormat = XSSFDataFormat.GetBuiltinFormat("0.00");//百分数【小数点后有几个0表示精确到显示小数点后几位】
            return temporaryCellStyle;
        }
        //设置列的默认格式
        public void SetDefaultColumnStyles(int column, ICellStyle cellStyle)
        {
            temporarySheet.SetDefaultColumnStyle(column, cellStyle);
        }

        /// <summary>
        /// 获取字体样式
        /// </summary>
        /// <param name="fontfamily">字体名</param>
        /// <param name="fontcolor">字体颜色</param>
        /// <param name="boldweight">字体粗细</param>
        /// <param name="fontsize">字体大小</param>
        /// <param name="isItalic">是否斜体</param>
        /// <returns></returns>
        public IFont FontStyle(string fontfamily = "", HSSFColor fontcolor = null, short boldweight = 0, int fontsize = 0, bool isItalic = false)
        {
            temporaryFont = workbook.CreateFont();
            if (!string.IsNullOrEmpty(fontfamily))
                temporaryFont.FontName = fontfamily;

            if (fontcolor != null)
                temporaryFont.Color = fontcolor.Indexed;

            if (boldweight > 0)
                temporaryFont.Boldweight = boldweight;

            if (fontsize > 0)
                temporaryFont.FontHeightInPoints = (short)fontsize;

            temporaryFont.IsItalic = isItalic;

            return temporaryFont;
        }

        /// <summary>
        /// 获取单元格样式
        /// </summary>
        /// <param name="font">单元格字体</param>
        /// <param name="fillPattern">图案样式</param>
        /// <param name="fillBackgroundColor">单元格背景</param>
        /// <param name="ha">垂直对齐方式</param>
        /// <param name="va">垂直对齐方式</param>
        /// <param name="top">上对齐</param>
        /// <param name="right">右对齐</param>
        /// <param name="bottom">下对齐</param>
        /// <param name="left">左对齐</param>
        /// <param name="fillForegroundColor">图案的颜色</param>
        /// <returns></returns>
        public ICellStyle CellStyle(HSSFColor fillBackgroundColor = null, HorizontalAlignment ha = HorizontalAlignment.Left, VerticalAlignment va = VerticalAlignment.Center,
            BorderStyle top = BorderStyle.None, BorderStyle right = BorderStyle.None, BorderStyle bottom = BorderStyle.None, BorderStyle left = BorderStyle.None,
            IFont font = null, HSSFColor fillForegroundColor = null, FillPattern fillPattern = new FillPattern(), int? dataFormat = null)
        {
            temporaryCellStyle = workbook.CreateCellStyle();
            temporaryCellStyle.FillPattern = fillPattern;
            temporaryCellStyle.Alignment = ha;
            temporaryCellStyle.VerticalAlignment = va;

            if (fillForegroundColor != null)
                temporaryCellStyle.FillForegroundColor = fillForegroundColor.Indexed;

            if (fillBackgroundColor != null)
                temporaryCellStyle.FillBackgroundColor = fillBackgroundColor.Indexed;

            if (font != null)
                temporaryCellStyle.SetFont(font);
            else if (defaultTemporaryFont != null)
                temporaryCellStyle.SetFont(defaultTemporaryFont);

            //temporaryCellStyle.FillPattern = FillPattern.SolidForeground;
            //加边框

            if (top != BorderStyle.None)
                temporaryCellStyle.BorderTop = top;
            if (right != BorderStyle.None)
                temporaryCellStyle.BorderRight = right;
            if (bottom != BorderStyle.None)
                temporaryCellStyle.BorderBottom = bottom;
            if (left != BorderStyle.None)
                temporaryCellStyle.BorderLeft = left;
            //单元格格式
            //if (dataFormat != null)
            //    temporaryCellStyle.DataFormat = DataStyle(dataFormat.GetValueOrDefault());

            return temporaryCellStyle;
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowStart"></param>
        /// <param name="rowEnd"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        public void MergeCells(int rowStart, int rowEnd, int colStart, int colEnd)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowStart, rowEnd, colStart, colEnd);
            temporarySheet.AddMergedRegion(cellRangeAddress);
        }

        public short DataStyle(int type)
        {
            dataformat = workbook.CreateDataFormat();
            //switch (type)
            //{
            //    case 1:
            //        return XSSFDataFormat.GetBuiltinFormat("0.00%");//百分数【小数点后有几个0表示精确到显示小数点后几位】
            //}

            return 0;
        }
    }
}
