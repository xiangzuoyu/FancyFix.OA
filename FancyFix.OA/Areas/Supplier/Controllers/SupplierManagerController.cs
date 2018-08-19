using FancyFix.OA.Base;
using FancyFix.Tools.Config;
using System;
using FancyFix.OA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.SS.UserModel;
using System.Data;
using FancyFix.OA.Filter;
using Tools.Tool;
using NPOI.HSSF.UserModel;
using FancyFix.Tools.Tool;
using FancyFix.OA.Common;
using FancyFix.OA.Areas.Supplier.Models;


namespace FancyFix.OA.Areas.Supplier.Controllers
{
    public class SupplierManagerController : BaseAdminController
    {
        // GET: Supplier/SupplierManager
        #region 加载列表
        public ActionResult List()
        {
            return View();
        }

        //[PermissionFilter("/supplier/suppliermanager/showcharts")]
        public JsonResult PageList(int page = 0, int pagesize = 0, int selectLabelid = 0, string files = "", string key = "")
        {
            long records = 0;
            var list = Bll.BllSupplier_List.PageList(page, pagesize, out records, selectLabelid, files, key);
            foreach (var item in list)
            {
                item.SupplierTypeName = item.SupplierType != null
                    ? Tools.Enums.Tools.GetEnumDescription(typeof(SupplierType), item.SupplierType.GetValueOrDefault().ToString().ToInt32()).ToString()
                    : "未分配";
                item.LabelName = item.LabelId != null
                    ? Tools.Enums.Tools.GetEnumDescription(typeof(SupplierLabel), item.LabelId.GetValueOrDefault().ToString().ToInt32()).ToString()
                    : "未分配";
            }

            return BspTableJson(list, records);
        }
        #endregion

        #region 批量导入Excel
        [HttpPost]
        public ActionResult BatchImportExcel(HttpPostedFileBase file)
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
                var modelList = BatchExcelToList(sheet, 1);

                if (modelList == null || modelList.Count < 1)
                    return MessageBoxAndJump("数据为空，导入失败！", "list");

                if (Bll.BllSupplier_List.Insert(modelList) < 1)
                    return MessageBoxAndJump("导入失败", "list");
            }
            catch (Exception ex)
            {
                return MessageBoxAndJump("导入失败:" + ex.Message.ToString(), "list");
            }

            return Redirect("list");
        }

        private List<Supplier_List> BatchExcelToList(ISheet sheet, int startRow)
        {
            List<Supplier_List> supList = new List<Supplier_List>();
            if (sheet == null)
                return supList;

            //第一行为标题
            try
            {
                //IRow headRow = sheet.GetRow(0);
                //int cellCount = headRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                for (int i = startRow; i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                        continue;

                    string code = row.GetCell(0)?.ToString();
                    //供应商代码如果已存在，就跳过
                    var models = Bll.BllSupplier_List.First(o => o.Display != 2 && o.Code == code);
                    if (models != null)
                        continue;

                    Supplier_List model = new Supplier_List();
                    model.Code = code;
                    model.Name = row.GetCell(1)?.ToString();
                    model.SupplierAb = row.GetCell(2)?.ToString();
                    var arr = row.GetCell(3)?.ToString().Split('/') ?? new string[1];
                    model.SupplierType = Tools.Enums.Tools.GetValueByName(typeof(SupplierType), arr[0]);
                    model.BusinessScope = row.GetCell(4)?.ToString();
                    model.Contact1 = row.GetCell(5)?.ToString();
                    model.Contact2 = row.GetCell(6)?.ToString();
                    model.Site = row.GetCell(7)?.ToString();
                    model.Address = row.GetCell(8)?.ToString();
                    DateTime startdate;
                    model.StartDate = DateTime.TryParse(row.GetCell(9)?.ToString(), out startdate)
                        ? startdate.ToString("yyyy-MM-dd")
                        : row.GetCell(9)?.ToString();
                    model.LabelId = Tools.Enums.Tools.GetValueByName(typeof(SupplierLabel), row.GetCell(10)?.ToString());
                    model.AccountDate = row.GetCell(11)?.ToString();
                    model.Note = row.GetCell(12)?.ToString();
                    model.AddDate = DateTime.Now;
                    model.AddUserId = MyInfo.Id;
                    model.LastDate = DateTime.Now;
                    model.LastUserId = MyInfo.Id;

                    //如果关键几个字段没有数据，就跳过
                    if (string.IsNullOrEmpty(model.Code) ||
                        string.IsNullOrEmpty(model.Name))
                        continue;
                    supList.Add(model);
                }
            }
            catch (Exception ex)
            {
                return supList;
            }

            return supList;
        }

        #endregion

        #region 批量导出Excel
        public ActionResult ExportExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExportExcel(int selectLabelid = 0)
        {
            string cols = RequestString("cols");
            var arr = cols.Split(',');
            if (!Tools.Usual.Utils.CheckSqlField(arr))
                return MessageBox("选择字段异常");

            if (arr.Length < 1)
                return LayerMsgErrorAndReturn("导出失败，请先勾选字段");

            DataTable dt = Bll.BllSupplier_List.NewExportDt(arr);

            string where = "Display!=2 ";
            where += selectLabelid > 0 ? " and LabelId=" + selectLabelid : "";
            var list = Bll.BllSupplier_List.GetList(0, cols, where, "Code");
            ToExcel(list, dt);

            return LayerClose();
        }

        public void ToExcel(DataTable list, DataTable dt)
        {
            if (list == null || list.Rows.Count < 1)
                return;

            try
            {
                int col = list.Columns.Count;
                foreach (DataRow item in list.Rows)
                {
                    var row = dt.NewRow();
                    for (int i = 0; i < col; i++)
                    {
                        if (list.Columns[i].ToString() == "SupplierType")
                            row[i] = item[i] != null
                                ? Tools.Enums.Tools.GetEnumDescription(typeof(SupplierType), item[i].ToString().ToInt32()).ToString()
                                : "未分配";
                        else if (list.Columns[i].ToString() == "LabelId")
                            row[i] = item[i] != null
                                ? Tools.Enums.Tools.GetEnumDescription(typeof(SupplierLabel), item[i].ToString().ToInt32()).ToString()
                                : "未分配";
                        else
                            row[i] = item[i]?.ToString();
                    }
                    dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
            }

            string fileName = "供应商列表" + DateTime.Now.ToString("yyyyMMddHHmmss");
            ExcelHelper.ToExcelWeb(dt, "", fileName + ".xls");
        }

        #endregion

        #region 单个导入Excel
        [HttpPost]
        public ActionResult SingleImportExcel(HttpPostedFileBase file)
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

                var sheet = Tools.Tool.ExcelHelper.ReadExcel(filePath);
                var modelList = SingleExcelToList(sheet, 1, ref result);

                if (modelList == null)
                    return MessageBoxAndJump($"导入失败，{result}！", "list");

                result = Bll.BllSupplier_VendorInfo.AddVendorInfo(modelList);
                if (result != "0")
                    return MessageBoxAndJump("导入失败，" + result, "list");
            }
            catch (Exception ex)
            {
                return MessageBoxAndJump("导入失败:请联系管理员", "list");
            }

            return Redirect("list");
        }

        private Supplier_VendorInfo SingleExcelToList(ISheet sheet, int startRow, ref string msg)
        {
            Supplier_VendorInfo vendorInfo = new Supplier_VendorInfo();
            if (sheet == null || sheet.LastRowNum < 64)
            {
                msg = "Excel内容为空！";
                return null;
            }

            //第一行为标题
            try
            {
                string code = sheet.GetRow(5)?.GetCell(9)?.ToString().Replace("供应商代码：", "").Trim() ?? "";
                if (string.IsNullOrWhiteSpace(code))
                {
                    msg = "供应商代码不能为空";
                    return null;
                }
                //供应商代码如果已存在，就跳过
                var models = Bll.BllSupplier_List.First(o => o.Code == code && o.Display != 2);
                if (models != null)
                {
                    msg = "供应商代码已存在";
                    return null;
                }

                #region 填充数据  

                vendorInfo.Code = code;
                vendorInfo.TableNumber = sheet.GetRow(1)?.GetCell(9)?.ToString().Replace("表格编号：", "").Trim() ?? "";
                IRow row = sheet.GetRow(2);
                vendorInfo.Owners = row?.GetCell(0)?.ToString().Replace("所有者Owners:", "").Trim() ?? "";
                vendorInfo.Version = row?.GetCell(9)?.ToString().Replace("版本号：", "").Trim() ?? "";
                vendorInfo.EffectiveDate = sheet.GetRow(3)?.GetCell(9)?.ToString().Replace("生效日期：", "").Trim().ToDateTime() ?? null;
                vendorInfo.VersionType = Tools.Enums.Tools.GetValueByName(typeof(GetVersionTypeId), sheet.GetRow(6)?.GetCell(0)?.ToString().Trim());
                vendorInfo.WriteDate = sheet.GetRow(7)?.GetCell(1)?.ToString().Replace("Date （日期）：", "").Trim().ToDateTime() ?? null;

                vendorInfo.Name = sheet.GetRow(10)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.CompanyEnglishName = sheet.GetRow(11)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.AddressOfRegisteredOffice = sheet.GetRow(12)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.PostCode1 = sheet.GetRow(13)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.PurchaseOrderAddressedTo = sheet.GetRow(14)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.PostCode2 = sheet.GetRow(15)?.GetCell(7)?.ToString().Trim() ?? "";

                vendorInfo.MainContactor = sheet.GetRow(16)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.TelephoneNumber = sheet.GetRow(17)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.FaxNumber = sheet.GetRow(18)?.GetCell(7)?.ToString().Trim() ?? "";

                vendorInfo.EmailForPO = sheet.GetRow(19)?.GetCell(7)?.ToString().Trim() ?? "";

                row = sheet.GetRow(20);
                vendorInfo.FinanceContact = row?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.FinanceContactPhone = row?.GetCell(8)?.ToString().Trim() ?? "";
                vendorInfo.FinanceContactEmail = row?.GetCell(9)?.ToString().Trim() ?? "";
                vendorInfo.TaxNumber = sheet.GetRow(21)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.TaxRate = sheet.GetRow(22)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.OrderCurrency = sheet.GetRow(23)?.GetCell(4)?.ToString().Trim() ?? "";
                vendorInfo.PaymentTerms = sheet.GetRow(24)?.GetCell(4)?.ToString().Trim() ?? "";
                vendorInfo.Incoterms = sheet.GetRow(25)?.GetCell(4)?.ToString().Trim() ?? "";
                vendorInfo.BankKey = sheet.GetRow(26)?.GetCell(4)?.ToString().Trim() ?? "";
                vendorInfo.SwiftCode = sheet.GetRow(27)?.GetCell(4)?.ToString().Trim() ?? "";
                vendorInfo.NameOfBank = sheet.GetRow(28)?.GetCell(4)?.ToString().Trim() ?? "";
                vendorInfo.ACNo = sheet.GetRow(29)?.GetCell(4)?.ToString().Trim() ?? "";
                vendorInfo.FormOfBusiness = Tools.Enums.Tools.GetValueByName(typeof(GetFormOfBusinessId), sheet.GetRow(31)?.GetCell(1)?.ToString().Trim());
                vendorInfo.BusinessRegistrationNumber = sheet.GetRow(32)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.RegisteredCapital = sheet.GetRow(33)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.CertificateOfCorporation = sheet.GetRow(34)?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.CompanyWebsiteAddress = sheet.GetRow(35)?.GetCell(7)?.ToString().Trim() ?? "";

                row = sheet.GetRow(36);
                string[] arr = new string[3];
                arr = row?.GetCell(7)?.ToString().Trim().Split('/') ?? new string[3];
                vendorInfo.ProductsOrServiceSales = arr[0]?.Trim() ?? "";
                vendorInfo.MOQ = arr[1]?.Trim().ToInt32() ?? 0;
                vendorInfo.LeadTime = arr[2]?.Trim() ?? "";

                row = sheet.GetRow(38);
                vendorInfo.ManagementMenberName1 = row?.GetCell(5)?.ToString().Trim() ?? "";
                vendorInfo.ManagementMenberName2 = row?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.ManagementMenberName3 = row?.GetCell(8)?.ToString().Trim() ?? "";
                vendorInfo.ManagementMenberName4 = row?.GetCell(9)?.ToString().Trim() ?? "";

                row = sheet.GetRow(39);
                vendorInfo.ManagementMenberTitle1 = row?.GetCell(5)?.ToString().Trim() ?? "";
                vendorInfo.ManagementMenberTitle2 = row?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.ManagementMenberTitle3 = row?.GetCell(8)?.ToString().Trim() ?? "";
                vendorInfo.ManagementMenberTitle4 = row?.GetCell(9)?.ToString().Trim() ?? "";

                row = sheet.GetRow(42);
                vendorInfo.ContactPersonsName1 = row?.GetCell(1)?.ToString().Trim() ?? "";
                vendorInfo.ContactPersonsTitle1 = row?.GetCell(5)?.ToString().Trim() ?? "";
                vendorInfo.ContactPersonsTel1 = row?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.ContactPersonsFax1 = row?.GetCell(8)?.ToString().Trim() ?? "";
                vendorInfo.ContactPersonsEmail1 = row?.GetCell(9)?.ToString().Trim() ?? "";

                row = sheet.GetRow(43);
                vendorInfo.ContactPersonsName2 = row?.GetCell(1)?.ToString().Trim() ?? "";
                vendorInfo.ContactPersonsTitle2 = row?.GetCell(5)?.ToString().Trim() ?? "";
                vendorInfo.ContactPersonsTel2 = row?.GetCell(7)?.ToString().Trim() ?? "";
                vendorInfo.ContactPersonsFax2 = row?.GetCell(8)?.ToString().Trim() ?? "";
                vendorInfo.ContactPersonsEmail2 = row?.GetCell(9)?.ToString().Trim() ?? "";

                vendorInfo.AddDate = DateTime.Now;
                vendorInfo.AddUserId = MyInfo.Id;
                vendorInfo.LastDate = vendorInfo.AddDate;
                vendorInfo.LastUserId = vendorInfo.AddUserId;
                vendorInfo.Dispaly = 1;
                #endregion
            }
            catch (Exception)
            {
                msg = "请联系管理员";
                return null;
            }

            return vendorInfo;
        }
        #endregion

        #region 单个导出Excel
        [HttpGet]
        public ActionResult SingleExportExcel(int id = 0)
        {
            if (id < 1)
                return MessageBoxAndReturn("导出失败，请先勾选供应商");

            var supplier = Bll.BllSupplier_List.First(o => o.Id == id && o.Display != 2);
            if (supplier == null)
                return MessageBoxAndReturn("导出失败，未找到该供应商信息");

            var vendor = Bll.BllSupplier_VendorInfo.First(o => o.VendorId == supplier.Id && o.Dispaly != 2);
            if (vendor == null)
                return MessageBoxAndReturn("导出失败，未找到供应商副表信息");

            ToSingleExcel(supplier, vendor);

            return View();
        }

        private void ToSingleExcel(Supplier_List supplier, Supplier_VendorInfo vendor)
        {
            HSSFWorkbook workbook = CustomExcel.SingleSupperliExport(supplier, vendor);

            //导出
            string fileName = "供应商信息" + DateTime.Now.ToString("yyyyMMddHHmmss");
            ExcelHelper.ToExcelWeb(fileName + ".xls", workbook);
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllSupplier_List.HideModel(id, MyInfo.Id) > 0 });
        }

        //[PermissionFilter("/supplier/suppliermanager/delete")]
        [HttpPost]
        public JsonResult DeleteBatch(List<Supplier_List> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllSupplier_List.HideList(list, MyInfo.Id) > 0 });
        }

        /// <summary>
        /// 删除供应商副表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteVendorInfo(int id)
        {
            return Json(new { result = Bll.BllSupplier_VendorInfo.HideModel(id, MyInfo.Id) > 0 });
        }
        #endregion

        #region 修改供应商标签
        [HttpPost]
        public JsonResult UpdateLabelBatch(List<Supplier_List> list, int labelId)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new
            {
                result = Bll.BllSupplier_List.UpdateLabel(list, labelId, MyInfo.Id) > 0
            });
        }

        #endregion

        #region 供应商主表编辑
        public ActionResult Save(int id = 0)
        {
            Supplier_List model = null;
            if (id > 0)
            {
                model = Bll.BllSupplier_List.First(o => o.Id == id && o.Display != 2);
                if (model == null)
                    return LayerAlertSuccessAndRefresh("加载供应商信息失败，未找到该供应商");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Supplier_List supplierList)
        {
            Supplier_List model = Bll.BllSupplier_List.First(o => o.Id != supplierList.Id && (o.Code == supplierList.Code || o.Name == supplierList.Name) && o.Display != 2);
            if (model != null)
                return LayerMsgErrorAndReturn("供应商代码或名称重复，请重新输入！");

            model = Bll.BllSupplier_List.First(o => o.Id == supplierList.Id && o.Display != 2) ?? new Supplier_List();
            model.Code = supplierList.Code;
            model.Name = supplierList.Name;
            model.SupplierAb = supplierList.SupplierAb;
            model.SupplierType = supplierList.SupplierType;
            model.BusinessScope = supplierList.BusinessScope;
            model.Contact1 = supplierList.Contact1;
            model.Contact2 = supplierList.Contact2;
            model.Site = supplierList.Site;
            model.Address = supplierList.Address;
            model.StartDate = supplierList.StartDate;
            model.LabelId = supplierList.LabelId;
            model.AccountDate = supplierList.AccountDate;
            model.Note = supplierList.Note;
            model.LastDate = DateTime.Now;
            model.LastUserId = MyInfo.Id;
            model.Attachment = GetFiles();
            bool isok = false;
            //没有ID就新增，反之修改
            if (supplierList.Id < 1)
            {
                model.AddDate = model.LastDate;
                model.AddUserId = model.LastUserId;
                model.Display = 1;

                isok = Bll.BllSupplier_List.Insert(model) > 0;
            }
            else
            {
                isok = Bll.BllSupplier_List.Update(model) > 0;
            }

            return LayerMsgSuccessAndRefresh("保存" + (isok ? "成功" : "失败"));
        }

        //判断供应商code或name是否重复
        [HttpGet]
        public JsonResult SupplierIsRepeat(string code = "", string name = "", int id = 0)
        {
            var model = Bll.BllSupplier_List.SupplierIsRepeat(id, code, name);
            string result = model == null ? "false" : "true";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 供应商副表编辑
        public ActionResult Addition(int vendorId = 0)
        {
            Supplier_VendorInfo model = null;
            if (vendorId > 0)
            {
                model = Bll.BllSupplier_VendorInfo.First(o => o.VendorId == vendorId && o.Dispaly != 2);
                if (model == null)
                {
                    model = new Supplier_VendorInfo();
                    model.VendorId = vendorId;
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Addition(Supplier_VendorInfo supplier_VendorInfo)
        {

            bool isok = Bll.BllSupplier_VendorInfo.SaveVendorInfo(supplier_VendorInfo, MyInfo.Id);

            return LayerMsgSuccessAndRefresh("保存" + (isok ? "成功" : "失败"));
        }

        #endregion

        #region 辅助方法

        #endregion
    }
}
