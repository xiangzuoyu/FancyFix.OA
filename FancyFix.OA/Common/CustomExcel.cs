using FancyFix.OA.Areas.Supplier.Models;
using FancyFix.OA.Model;
using FancyFix.Tools.Tool;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FancyFix.OA.Common
{
    /// <summary>
    /// 自定义导出Excel
    /// </summary>
    public class CustomExcel
    {
        /// <summary>
        /// 创建单个供应商导出的Excel格式
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="vendor"></param>
        /// <returns></returns>
        public static HSSFWorkbook SingleSupperliExport(Supplier_List supplier, Supplier_VendorInfo vendor)
        {
            NPOIHelper sheet = new NPOIHelper();

            //将Excel背景色改为白色
            for (int i = 0; i < 40; i++)
                sheet.SetDefaultColumnStyle(i, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.Yellow()));

            //设置列宽
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.SetColumnWidth(8, 15 * 256);
            sheet.SetColumnWidth(9, 35 * 256);
            //row1
            sheet.CreateRow(1);

            SetTopBorder(ref sheet);

            sheet.SetHeight(25 * 20);
            sheet.CreateCell(9, "表格编号：" + vendor.TableNumber,
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), va: VerticalAlignment.Bottom, top: BorderStyle.Thin, right: BorderStyle.Thin));
            //row2
            sheet.CreateRow(2);
            sheet.CreateCell(0, "所有者Owners" + vendor.Owners);
            sheet.CreateCell(9, "版本号：" + vendor.Version,
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), va: VerticalAlignment.Bottom, right: BorderStyle.Thin));
            //row3
            sheet.CreateRow(3);
            sheet.CreateCell(9, "生效日期：" + vendor?.EffectiveDate?.ToString("yyyy-MM-dd") ?? "",
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), va: VerticalAlignment.Bottom, right: BorderStyle.Thin, bottom: BorderStyle.Thin));
            //row4
            sheet.MergeCells(4, 4, 0, 9);
            sheet.CreateRow(4);
            sheet.CreateCell(0, " New  Supplier  Registration and Approval  Form ",
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold)));

            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), right: BorderStyle.Thin));
            //row5
            sheet.MergeCells(5, 5, 5, 8);
            sheet.CreateRow(5);
            sheet.CreateCell(5, "（新供应商信息登记与审批表）",
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold)));

            sheet.CreateCell(9, "供应商代码：" + supplier.Code,
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), right: BorderStyle.Thin, font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold)));
            //row6 
            sheet.MergeCells(6, 6, 0, 9);
            sheet.CreateRow(6);
            SetTopBorder(ref sheet);

            sheet.CreateCell(0, vendor.VersionType != null
                ? Tools.Enums.Tools.GetEnumDescription(typeof(GetVersionTypeId), vendor.VersionType.GetValueOrDefault().ToString().ToInt32()).ToString()
                : "",
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, top: BorderStyle.Thin,
                font: sheet.FontStyle(boldweight: (short)FontBoldWeight.Bold)));

            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), top: BorderStyle.Thin, right: BorderStyle.Thin));
            //row7
            sheet.CreateRow(7);
            sheet.CreateCell(1, "Date （日期）：" + vendor?.WriteDate?.ToString("yyyy-MM-dd") ?? "",
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, right: BorderStyle.Thin));
            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), right: BorderStyle.Thin));
            //row8
            sheet.CreateRow(8);
            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), right: BorderStyle.Thin));
            //row9
            sheet.MergeCells(9, 9, 0, 9);
            sheet.CreateRow(9);
            sheet.CreateCell(0, "    Information Submitted By Supplier  （由供应商提交的信息）");
            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), right: BorderStyle.Thin));

            RowStencil1(10, ref sheet, "1", "Company Name 公司中文名称", supplier.Name);
            RowStencil1(11, ref sheet, "2", "Company Name 公司英文名称", vendor.CompanyEnglishName);
            RowStencil1(12, ref sheet, "3", "Address of Registered Office 公司注册地址", supplier.Address);
            RowStencil1(13, ref sheet, "4", "Post Code 邮编", vendor.PostCode1);
            RowStencil1(14, ref sheet, "5", "Purchase Order addressed to 合同/订单送达地址", vendor.PurchaseOrderAddressedTo);
            RowStencil1(15, ref sheet, "6", "Post Code 邮编", vendor.PostCode2);
            RowStencil1(16, ref sheet, "7", "Main Contactor  主要联系人", supplier.Contact1);
            RowStencil1(17, ref sheet, "8", "Telephone Number 联系方式：含固话、手机和邮箱", supplier.Contact1);
            RowStencil1(18, ref sheet, "9", "Fax Number 传真号码", vendor.FaxNumber);
            RowStencil1(19, ref sheet, "10", "E-mail for PO  订单及日常工作邮箱", supplier.Contact1);

            //row20
            sheet.CreateRow(20);
            SetTopBorder(ref sheet);

            sheet.CreateCell(0, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), top: BorderStyle.Thin));
            sheet.MergeCells(10, 10, 1, 6);
            sheet.CreateCell(1, "Finance Contact 财务联系人电话和邮箱", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Left, VerticalAlignment.Center,
                left: BorderStyle.Thin, top: BorderStyle.Thin));

            string[] arr = new string[3];
            arr = supplier.Contact2.Split('/') ?? new string[3];

            sheet.CreateCell(7, arr?[0].ToString() ?? "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Bottom,
                left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(8, arr?[1].ToString() ?? "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Bottom,
                left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(9, arr?[2].ToString() ?? "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Bottom,
                left: BorderStyle.Thin, top: BorderStyle.Thin, right: BorderStyle.Thin));
            //row21
            RowStencil1(21, ref sheet, "11", "Tax Number 税号", vendor.TaxNumber);
            RowStencil1(22, ref sheet, "12", "Tax Rate 税率", vendor.TaxRate);
            RowStencil2(23, ref sheet, "13", "Order Currency 交易币种", vendor.OrderCurrency);
            RowStencil2(24, ref sheet, "14", "Payment Terms 付款条件", vendor.PaymentTerms);
            RowStencil2(25, ref sheet, "15", "Incoterms 交易方式", vendor.Incoterms);
            RowStencil2(26, ref sheet, "16", "Bank Key 开户行行号", vendor.BankKey);
            RowStencil2(27, ref sheet, "17", "Swift code 银行国际代码", vendor.SwiftCode);
            RowStencil2(28, ref sheet, "18", "Name of Bank 开户行名称", vendor.NameOfBank);
            RowStencil2(29, ref sheet, "19", "A/C No. 银行账号", vendor.ACNo);
            //row30
            sheet.CreateRow(30);
            SetTopBorder(ref sheet);
            sheet.MergeCells(30, 31, 0, 0);
            sheet.MergeCells(30, 30, 1, 9);
            sheet.CreateCell(0, "20", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center, top: BorderStyle.Thin));
            sheet.CreateCell(1, "Form of Business ( please tick )      请选择以下公司性质，请打勾",
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center, left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), top: BorderStyle.Thin, right: BorderStyle.Thin));
            //row31
            sheet.CreateRow(31);
            SetTopBorder(ref sheet, 1);
            sheet.MergeCells(31, 31, 1, 9);
            sheet.CreateCell(1, vendor.FormOfBusiness != null
                ? Tools.Enums.Tools.GetEnumDescription(typeof(GetFormOfBusinessId), vendor.FormOfBusiness.GetValueOrDefault().ToString().ToInt32()).ToString()
                : "",
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center, left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), top: BorderStyle.Thin, right: BorderStyle.Thin));
            sheet.SetHeight(20 * 20);
            //row32
            RowStencil1(32, ref sheet, "21", "Business Registration Number 公司注册码", vendor.BusinessRegistrationNumber);
            RowStencil1(33, ref sheet, "22", "Registered Capital, other Assets Amounts 注册资金", vendor.RegisteredCapital);
            RowStencil1(34, ref sheet, "23", "Certificate of Corporation (if any) 资质文件（若有）", vendor.CertificateOfCorporation);
            RowStencil1(35, ref sheet, "24", "Company Website Address 公司网址", supplier.Site);
            RowStencil1(36, ref sheet, "25", "Products or Service Sales, MOQ  and lead time\r\n交易的产品或服务名称、最小订购量和交货周期",
                $" {vendor.ProductsOrServiceSales}    /    {vendor.MOQ}    /     {vendor.LeadTime} ");
            sheet.SetHeight(30 * 20);
            //row37
            sheet.CreateRow(37);
            sheet.SetHeight(20 * 20);
            SetTopBorder(ref sheet);
            sheet.MergeCells(37, 39, 0, 0);
            sheet.MergeCells(37, 37, 1, 9);
            sheet.CreateCell(0, "26", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center, top: BorderStyle.Thin));
            sheet.CreateCell(1, "Name of Director(s) / Legal Representative & Key Management Menber  董事会人员姓名/法人代表及主要管理者姓名",
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center, left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), top: BorderStyle.Thin, right: BorderStyle.Thin));
            //row38
            RowStencil3(38, ref sheet, "Name （姓名）", vendor.ManagementMenberName1, vendor.ManagementMenberName2, vendor.ManagementMenberName3,
                vendor.ManagementMenberName4);
            //row39
            RowStencil3(39, ref sheet, "Title (职务)", vendor.ManagementMenberTitle1, vendor.ManagementMenberTitle2, vendor.ManagementMenberTitle3,
                vendor.ManagementMenberTitle4);
            //row40
            sheet.CreateRow(40);
            sheet.SetHeight(20 * 20);
            SetTopBorder(ref sheet);
            sheet.MergeCells(40, 43, 0, 0);
            sheet.MergeCells(40, 40, 1, 9);
            sheet.CreateCell(0, "27", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center, top: BorderStyle.Thin));
            sheet.CreateCell(1, "Contact Persons : ( pls also state email address if any )  其他联系人信息(作为主要联系人的补充)",
                sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center, left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), top: BorderStyle.Thin, right: BorderStyle.Thin));
            //row41
            RowStencil3(41, ref sheet, "Name 姓名", "Title 职务", "Tel. No.联系方式", "Fax No.传真号码", "Email 邮箱");
            RowStencil3(42, ref sheet, vendor.ContactPersonsName1, vendor.ContactPersonsTitle1, vendor.ContactPersonsTel1, vendor.ContactPersonsFax1,
                vendor.ContactPersonsEmail1);
            RowStencil3(43, ref sheet, vendor.ContactPersonsName2, vendor.ContactPersonsTitle2, vendor.ContactPersonsTel2, vendor.ContactPersonsFax2,
                vendor.ContactPersonsEmail2);
            //row44
            sheet.CreateRow(44);
            SetTopBorder(ref sheet);
            return sheet.GetWorkbook();
        }

        private static void RowStencil1(int rowIndex, ref NPOIHelper sheet, string cell1, string cell2, string cell3)
        {
            sheet.CreateRow(rowIndex);
            sheet.SetHeight(20 * 20);
            SetTopBorder(ref sheet);

            sheet.CreateCell(0, cell1, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center, top: BorderStyle.Thin));
            sheet.MergeCells(rowIndex, rowIndex, 1, 6);
            sheet.CreateCell(1, cell2, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Left, VerticalAlignment.Center,
                left: BorderStyle.Thin, top: BorderStyle.Thin));

            sheet.MergeCells(rowIndex, rowIndex, 7, 9);
            sheet.CreateCell(7, cell3, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Bottom,
                left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), top: BorderStyle.Thin, right: BorderStyle.Thin));
        }

        private static void RowStencil2(int rowIndex, ref NPOIHelper sheet, string cell1, string cell2, string cell3)
        {
            sheet.CreateRow(rowIndex);
            sheet.SetHeight(20 * 20);

            SetTopBorder(ref sheet);

            sheet.CreateCell(0, cell1, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center, top: BorderStyle.Thin));
            sheet.MergeCells(rowIndex, rowIndex, 1, 3);
            sheet.CreateCell(1, cell2, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Left, VerticalAlignment.Center,
                left: BorderStyle.Thin, top: BorderStyle.Thin));

            sheet.MergeCells(rowIndex, rowIndex, 4, 9);
            sheet.CreateCell(4, cell3, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), HorizontalAlignment.Center, VerticalAlignment.Center,
                left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(9, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), top: BorderStyle.Thin, right: BorderStyle.Thin));
        }

        private static void RowStencil3(int rowIndex, ref NPOIHelper sheet, string cell0, string cell1, string cell2, string cell3, string cell4)
        {
            sheet.CreateRow(rowIndex);
            sheet.SetHeight(20 * 20);
            SetTopBorder(ref sheet, 1);
            sheet.MergeCells(rowIndex, rowIndex, 1, 4);
            sheet.CreateCell(1, cell0, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(),
                HorizontalAlignment.Center, VerticalAlignment.Bottom, left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.MergeCells(rowIndex, rowIndex, 5, 6);
            sheet.CreateCell(5, cell1, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(),
                HorizontalAlignment.Center, VerticalAlignment.Bottom, left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(7, cell2, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(),
                HorizontalAlignment.Center, VerticalAlignment.Bottom, left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(8, cell3, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(),
                HorizontalAlignment.Center, VerticalAlignment.Bottom, left: BorderStyle.Thin, top: BorderStyle.Thin));
            sheet.CreateCell(9, cell4, sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(),
                HorizontalAlignment.Center, VerticalAlignment.Bottom, left: BorderStyle.Thin, top: BorderStyle.Thin, right: BorderStyle.Thin));
        }

        /// <summary>
        /// 为某行添加上边框
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="cellLenth"></param>
        private static void SetTopBorder(ref NPOIHelper sheet, int startCell = 0, int endCell = 10)
        {
            for (int i = startCell; i < endCell; i++)
                sheet.CreateCell(i, "", sheet.CellStyle(new NPOI.HSSF.Util.HSSFColor.White(), top: BorderStyle.Thin));
        }
    }
}

