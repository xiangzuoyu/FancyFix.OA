using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.XSSF.UserModel;

namespace FancyFix.Tools.Tool
{
    public static class ExcelHelper
    {
        /// <summary>
        /// (webform)流形式直接导出Excel(不推荐)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="page">页面Page类</param>
        public static void ExportResult(DataTable dt, string fileName, Page page)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(sw);

            DataGrid dg = new DataGrid();
            dg.DataSource = dt;
            dg.DataBind();
            dg.RenderControl(htmlWrite);

            HttpResponse response = page.Response;
            response.Clear();
            response.Charset = "UTF-8";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).ToString());
            response.ContentType = "application/ms-excel";
            response.Write(sw.ToString().Replace("<br/>", "<br style='mso-data-placement:same-cell;'/> "));
            response.Flush();
            response.End();
        }

        /// <summary>
        /// (mvc)流形式直接导出Excelm(不推荐)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void ExportResult(DataTable dt, string fileName)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(sw);

            DataGrid dg = new DataGrid();
            dg.DataSource = dt;
            dg.DataBind();
            dg.RenderControl(htmlWrite);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).ToString());
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Output.Write(sw.ToString().Replace("<br/>", "<br style='mso-data-placement:same-cell;'/> "));
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// (mvc)流形式直接导出Excel(不推荐)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        public static void ExportResult<T>(IList<T> list, string fileName)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(sw);

            DataGrid dg = new DataGrid();
            dg.DataSource = list;
            dg.DataBind();
            dg.RenderControl(htmlWrite);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).ToString());
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Output.Write(sw.ToString().Replace("<br/>", "<br style='mso-data-placement:same-cell;'/> "));
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }


        /// <summary>
        /// (mvc)流形式直接导出Excel(不推荐)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        public static void ExportResult(string list, string fileName)
        {
            StringWriter sw = new StringWriter();
            sw.Write(list);
            HtmlTextWriter htmlWrite = new HtmlTextWriter(sw);

            DataGrid dg = new DataGrid();
            dg.RenderControl(htmlWrite);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).ToString());
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Output.Write(sw.ToString().Replace("<br/>", "<br style='mso-data-placement:same-cell;'/> "));
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }



        /// <summary>
        /// 导出到Excel文件到指定路径(.xls)
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置和文件名(例 E:/Test.xls)</param>
        public static void ToExcelClient(DataTable dtSource, string strHeaderText, string pathAndFileName)
        {
            using (MemoryStream ms = DataTableToExcel(dtSource, strHeaderText))
            {
                using (FileStream fs = new FileStream(pathAndFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// 导出到Excel文件(.xls)
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置和文件名(例 E:/Test.xls)</param>
        public static void ToExcelWeb(DataTable dtSource, string strHeaderText, string fileName)
        {
            using (MemoryStream ms = DataTableToExcel(dtSource, strHeaderText))
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).ToString());
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream(.xls)
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        private static MemoryStream DataTableToExcel(DataTable dtSource, string strHeaderText = "", int headRow = 1)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "Harry"; //填加xls文件作者信息
                si.ApplicationName = ""; //填加xls文件创建程序信息
                si.LastAuthor = "Harry"; //填加xls文件最后保存者信息
                si.Comments = "Harry"; //填加xls文件作者信息
                si.Title = ""; //填加xls文件标题信息
                si.Subject = "";//填加文件主题信息
                si.CreateDateTime = System.DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = (HSSFSheet)workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        #region 无用，准备删除
                        //    HSSFRow headerRow = (HSSFRow)sheet.CreateRow(rowIndex);
                        //    headerRow.HeightInPoints = 25;

                        //    HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        //    headerRow.CreateCell(0).SetCellValue(strHeaderText);
                        //    //headStyle.Alignment = HorizontalAlignment.Center;//水平居中
                        //    //headStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                        //    HSSFFont font = (HSSFFont)workbook.CreateFont();
                        //    font.FontHeightInPoints = 20;
                        //    font.Boldweight = 700;
                        //    headStyle.SetFont(font);
                        //    headerRow.GetCell(0).CellStyle = headStyle;
                        //    //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                        //    //headerRow.Dispose();
                        //    rowIndex++;
                        #endregion
                        HSSFCellStyle hssfcellstyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        if (!string.IsNullOrEmpty(strHeaderText))
                        {
                            HSSFRow headerRow = NewHSSFRow(ref sheet, workbook, ref hssfcellstyle, rowIndex, false);

                            headerRow.CreateCell(0).SetCellValue(strHeaderText);
                            rowIndex++;
                        }
                    }
                    #endregion

                    #region 列头及样式
                    {
                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        HSSFRow headerRow = NewHSSFRow(ref sheet, workbook, ref headStyle, rowIndex);
                        #region 无用，准备删除
                        //HSSFRow headerRow = (HSSFRow)sheet.CreateRow(rowIndex);
                        //headerRow.HeightInPoints = 25;

                        //HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        //headStyle.WrapText = true;//自动换行
                        ////headStyle.Alignment = HorizontalAlignment.Center;//水平居中
                        //headStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                        //HSSFFont font = (HSSFFont)workbook.CreateFont();
                        //font.FontHeightInPoints = 10;
                        //font.Boldweight = 700;
                        //headStyle.SetFont(font);
                        #endregion
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, ((arrColWidth[column.Ordinal] + 1) * 256) > 10000 ? 10000 : ((arrColWidth[column.Ordinal] + 1) * 256)); //宽度10000可自定义
                        }
                        //headerRow.Dispose();
                        rowIndex++;

                        
                    }
                    //添加更多列
                    {
                        for (int i = 1; i < headRow; i++)
                        {

                        }
                    }
                    #endregion
                }
                #endregion

                #region 填充内容
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                HSSFCellStyle rowStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                rowStyle.WrapText = true;//自动换行
                rowStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            System.DateTime dateV;
                            System.DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                    dataRow.GetCell(column.Ordinal).CellStyle = rowStyle;
                }
                #endregion

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                ms.Dispose();
                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                return ms;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="workbook"></param>
        /// <param name="headStyle">Excel样式</param>
        /// <param name="rowIndex">插入行的索引</param>
        /// <param name="isHead">是否列头</param>
        /// <param name="wrapText">自动换行</param>
        /// <param name="verticalAlignment">上下对齐样式</param>
        /// <param name="heightInPoints">字体大小</param>
        /// <returns></returns>
        private static HSSFRow NewHSSFRow(ref HSSFSheet sheet, HSSFWorkbook workbook, ref HSSFCellStyle headStyle, int rowIndex, bool isHead = false, bool wrapText = true
            , VerticalAlignment verticalAlignment = VerticalAlignment.Center, int heightInPoints = 25)
        {
            HSSFRow headerRow = (HSSFRow)sheet.CreateRow(rowIndex);
            headerRow.HeightInPoints = heightInPoints;
            headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            if (wrapText)
                headStyle.WrapText = true;//自动换行
            //headStyle.Alignment = HorizontalAlignment.Center;//水平居中
            headStyle.VerticalAlignment = verticalAlignment;//垂直居中
            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = (short)(isHead ? 20 : 10);
            font.Boldweight = 700;
            headStyle.SetFont(font);
            return headerRow;
        }

        private static HSSFWorkbook InitializeWorkbook()
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            //create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            hssfworkbook.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            hssfworkbook.SummaryInformation = si;
            return hssfworkbook;
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="filePath">服务器Excel存放的绝对路径</param>
        /// <param name="sheetIndex">选择的工作文件</param>
        /// <returns></returns>
        public static ISheet ReadExcel(string filePath, int sheetIndex = 0)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            try
            {
                if (filePath.IndexOf(".xlsx") > 0) // 2007版本  
                    workbook = new XSSFWorkbook(file);
                else
                    workbook = new HSSFWorkbook(file);// 2003版本  
                //取第一个工作表
                sheet = workbook.GetSheetAt(sheetIndex);
            }
            catch
            {
                return sheet;
            }
            finally
            {
                if (file != null)
                    file.Close();
                if (workbook != null)
                    workbook.Close();
            }

            return sheet;
        }
    }
}
