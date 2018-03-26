using FancyFix.OA.Base;
using FancyFix.OA.Model;
using FancyFix.Tools.Config;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tools;

namespace FancyFix.OA.Areas.Supplier.Controllers
{
    public class RawMaterialPriceController : BaseAdminController
    {
        // GET: Supplier/RawMaterialPrice

        #region 加载列表
        public ActionResult List()
        {
            ViewBag.prices = Bll.BllSupplier_RawMaterialPrice.GetSelectList(0, "distinct(Years)", "display!=2", "Years");

            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0, int priceFrequency = 0, int years = 0)
        {
            long records = 0;
            if (years == 0)
                years = DateTime.Now.Year;
            //Sql注入检测
            string files = Tools.Usual.Utils.CheckSqlKeyword(RequestString("files"));
            string key = Tools.Usual.Utils.CheckSqlKeyword(RequestString("key")).Trim();

            var list = Bll.BllSupplier_RawMaterialPrice.PageList(page, pagesize, out records, files, key.Trim(), years, priceFrequency);
            foreach (var item in list)
            {
                //价格频次
                item.PriceFrequencyName = item.PriceFrequency != null
                    ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.PriceFrequency), item.PriceFrequency.GetValueOrDefault().ToString().ToInt32()).ToString()
                    : "无";
            }
            return BspTableJson(list, records);
        }

        public JsonResult YearsList(int years = 0)
        {
            var list = Bll.BllSupplier_RawMaterialPrice.GetSelectList(0, "distinct(Years)", "display!=2", "Years");
            string result = string.Empty;
            foreach (var item in list)
                result += "<option value=\"" + item.Years + "\" " + (item.Years == years ? "selected" : "") + ">" + item.Years + "年</option>";

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 导入Excel
        [HttpPost]
        public ActionResult List(HttpPostedFileBase file)
        {
            if (file == null)
                Redirect("List");

            try
            {
                string filePath = UploadProvice.Instance().Settings["file"].FilePath + DateTime.Now.ToString("yyyyMMddHHmmss")
                        + (file.FileName.IndexOf(".xlsx") > 0 ? ".xlsx" : "xls");
                var size = file.ContentLength;
                int maxFileSize = UploadProvice.Instance().Settings["file"].MaxFileSize;
                

                var type = file.ContentType;
                //判断文件大小和格式
                if (size > maxFileSize)
                    return MessageBoxAndJump("上传失败，上传的文件太大", "list");

                file.SaveAs(filePath);

                var sheet = Tools.Tool.ExcelHelper.ReadExcel(filePath);
                var modelList = ExcelToList(sheet, 2);

                if (modelList == null || modelList.Count < 1)
                    return MessageBoxAndJump("数据为空，导入失败！", "list");

                if (!Bll.BllSupplier_RawMaterial.Add(modelList))
                    return MessageBoxAndJump("导入失败", "list");
            }
            catch (Exception ex)
            {
                return MessageBoxAndJump("导入失败:" + ex.Message.ToString(), "list");
            }

            return Redirect("list");
        }

        private List<Supplier_RawMaterial> ExcelToList(ISheet sheet, int startRow)
        {
            List<Supplier_RawMaterial> supList = new List<Supplier_RawMaterial>();
            if (sheet == null)
                return supList;

            //第一行为标题
            try
            {
                IRow headRow = sheet.GetRow(0);
                int cellCount = headRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                int? year = 0;

                for (int i = startRow; i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                        continue;

                    //获取价格年份
                    if (i == startRow)
                    {
                        year = row.GetCell(9)?.ToString().TrimStart('\'').ToDateTime().Year;
                        if (year == null || year < 1)
                            return new List<Supplier_RawMaterial>();

                        continue;
                    }

                    var model = new Supplier_RawMaterial()
                    {
                        BU = row.GetCell(0)?.ToString(),
                        SAPCode = row.GetCell(1)?.ToString(),
                        Description = row.GetCell(2)?.ToString(),
                        Category = row.GetCell(3)?.ToString(),
                        LeadBuyer = row.GetCell(4)?.ToString(),
                        SupplierCode = row.GetCell(5)?.ToString(),
                        SupplierName = row.GetCell(6)?.ToString(),
                        PriceFrequency = Tools.Enums.Tools.GetValueByName(typeof(Models.PriceFrequency), row.GetCell(7)?.ToString()),
                        Currency = row.GetCell(8)?.ToString(),
                        Years = year,
                        Month1 = row.GetCell(9)?.ToString().ToDecimal(),
                        Month2 = row.GetCell(10)?.ToString().ToDecimal(),
                        Month3 = row.GetCell(11)?.ToString().ToDecimal(),
                        Month4 = row.GetCell(12)?.ToString().ToDecimal(),
                        Month5 = row.GetCell(13)?.ToString().ToDecimal(),
                        Month6 = row.GetCell(14)?.ToString().ToDecimal(),
                        Month7 = row.GetCell(15)?.ToString().ToDecimal(),
                        Month8 = row.GetCell(16)?.ToString().ToDecimal(),
                        Month9 = row.GetCell(17)?.ToString().ToDecimal(),
                        Month10 = row.GetCell(18)?.ToString().ToDecimal(),
                        Month11 = row.GetCell(19)?.ToString().ToDecimal(),
                        Month12 = row.GetCell(20)?.ToString().ToDecimal(),
                        AddDate = DateTime.Now,
                        AddUserId = MyInfo.Id,
                        LastDate = DateTime.Now,
                        LastUserId = MyInfo.Id
                    };

                    //如果关键几个字段没有数据，就跳过
                    if (string.IsNullOrEmpty(model.SAPCode) &&
                        string.IsNullOrEmpty(model.Description) &&
                        string.IsNullOrEmpty(model.SupplierCode))
                        continue;
                    supList.Add(model);
                }
            }
            catch (Exception)
            {
                return supList;
            }

            return supList;
        }
        #endregion

        #region 导出Excel
        public ActionResult ExportExcel(string files = "", string key = "", int priceFrequency = 0, int years = 0)
        {
            ViewBag.files = files;
            ViewBag.key = key;
            ViewBag.priceFrequencyId = priceFrequency;
            ViewBag.years = years < 1 ? DateTime.Now.Year : years;
            ViewBag.pricesList = Bll.BllSupplier_RawMaterialPrice.GetSelectList(0, "distinct(Years)", "display!=2", "Years");

            return View();
        }

        [HttpPost]
        public ActionResult ExportExcel(int priceFrequency = 0, int years = 0)
        {
            string files = Tools.Usual.Utils.CheckSqlKeyword(RequestString("files"));
            string key = Tools.Usual.Utils.CheckSqlKeyword(RequestString("key"));
            string cols = RequestString("cols");
            var arr = cols.Split(',');
            if (string.IsNullOrEmpty(cols))
                return MessageBoxAndReturn("请先至少选择一个要导出的字段");

            if (!CheckSqlField(arr))
                return MessageBoxAndReturn("选择导出的字段异常");

            if (years < 1)
                return MessageBoxAndReturn("请先选择导出的年份！");

            DataTable dt = NewExportDt(arr, years, ref cols);

            var list = Bll.BllSupplier_RawMaterialPrice.GetList(files, key, years, priceFrequency);
            ToExcel(list, dt, years, cols);

            return MessageBoxAndClose("导出成功");
        }

        private DataTable NewExportDt(string[] arr, int years, ref string cols)
        {
            DataTable dt = new DataTable();
            DataRow rows = dt.NewRow();
            int rowIndex = 0;
            foreach (var item in arr)
            {
                switch (item)
                {
                    case "BU":
                        NewDtHead(ref dt, ref rows, rowIndex, "业务部门", "BU");
                        break;
                    case "SAPCode":
                        NewDtHead(ref dt, ref rows, rowIndex, "原材料代码", "SAP Code");
                        break;
                    case "Description":
                        NewDtHead(ref dt, ref rows, rowIndex, "采购产品名称", "Description");
                        break;
                    case "Category":
                        NewDtHead(ref dt, ref rows, rowIndex, "品类", "Category");
                        break;
                    case "LeadBuyer":
                        NewDtHead(ref dt, ref rows, rowIndex, "采购负责人", "Lead Buyer");
                        break;
                    case "Code":
                        NewDtHead(ref dt, ref rows, rowIndex, "供应商代码", "Vendor Code");
                        break;
                    case "Name":
                        NewDtHead(ref dt, ref rows, rowIndex, "供应商名称", "Vendor Name");
                        break;
                    case "PriceFrequency":
                        NewDtHead(ref dt, ref rows, rowIndex, "价格频次", "Price Frequency");
                        break;
                    case "Currency":
                        NewDtHead(ref dt, ref rows, rowIndex, "价格单位", "Currency");
                        break;
                }
                rowIndex++;
            }

            for (int i = 1; i < 13; i++)
            {
                string columns = string.Format("'{0}/{1}", years.ToString(), i);
                dt.Columns.Add(columns, typeof(String));
                rows[rowIndex] = string.Format("'{0}-{1}", years.ToString(), i);
                cols += ",Month" + i;
                rowIndex++;
            }

            dt.Rows.Add(rows);
            return dt;
        }

        public void ToExcel(DataTable list, DataTable excelDt, int years, string cols)
        {
            if (list == null || list.Rows.Count < 1)
                return;

            DataTable dt = excelDt;
            try
            {
                int col = list.Columns.Count;
                foreach (DataRow item in list.Rows)
                {
                    var row = dt.NewRow();

                    for (int i = 0, j = 0; j < dt.Columns.Count; i++, j++)
                    {
                        var colum = list.Columns[i].ToString();
                        //当前字段不在导出范围内则跳过
                        if (!cols.Contains(colum.Replace(" ", "")))
                        {
                            j--;
                            continue;
                        }

                        //价格频次
                        if (colum == "PriceFrequency")
                            row[j] = item[i] != null
                                ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.PriceFrequency), item[i].ToString().ToInt32()).ToString()
                                : "无";
                        else
                            row[j] = item[i]?.ToString();
                    }
                    dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
            }

            string fileName = "供应商采购价格系统" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Tools.Tool.ExcelHelper.ToExcelWeb(dt, "凡菲供应商采购价格跟踪系统", fileName + ".xls", 2);
        }

        private void NewDtHead(ref DataTable dt, ref DataRow rows, int rowIndex, string cellName, string cellName2)
        {
            dt.Columns.Add(cellName, typeof(String));
            rows[rowIndex] = cellName2;
        }

        #endregion

        #region 编辑
        public ActionResult Save(int id = 0)
        {
            Supplier_RawMaterialPrice model = null;
            ViewBag.years = DateTime.Now.Year;

            if (id > 0)
            {
                model = Bll.BllSupplier_RawMaterialPrice.GetModel(id);
                if (model == null)
                    return LayerAlertErrorAndReturn("加载原材料价格信息失败，未找到该原材料价格");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Supplier_RawMaterialPrice rawmaterialPrice)
        {
            Supplier_RawMaterialPrice model = Bll.BllSupplier_RawMaterialPrice.First(o => o.Id == rawmaterialPrice.Id && o.Display != 2) ?? new Supplier_RawMaterialPrice();

            //根据原材料不存在，返回提示
            var rawMaterialModel = Bll.BllSupplier_RawMaterial.First(o => o.SAPCode == rawmaterialPrice.SAPCode && o.Display != 2);
            if (rawMaterialModel == null)
                return LayerAlertErrorAndReturn("原材料代码不存在，请重新输入");
            //根据供应商信息，查询供应商ID，如果供应商不存在，返回提示
            var supplierModel = Bll.BllSupplier_List.First(o => o.Code == rawmaterialPrice.Code && o.Display != 2 && o.Id > 0);
            if (supplierModel == null)
                return LayerAlertErrorAndReturn("供应商代码不存在，请重新输入");
            //保存价格
            model.RawMaterialId = rawMaterialModel.Id;
            model.VendorId = supplierModel.Id;
            model.Years = rawmaterialPrice.Years;
            model.PriceFrequency = rawmaterialPrice.PriceFrequency;
            model.Month1 = rawmaterialPrice.Month1;
            model.Month2 = rawmaterialPrice.Month2;
            model.Month3 = rawmaterialPrice.Month3;
            model.Month4 = rawmaterialPrice.Month4;
            model.Month5 = rawmaterialPrice.Month5;
            model.Month6 = rawmaterialPrice.Month6;
            model.Month7 = rawmaterialPrice.Month7;
            model.Month8 = rawmaterialPrice.Month8;
            model.Month9 = rawmaterialPrice.Month9;
            model.Month10 = rawmaterialPrice.Month10;
            model.Month11 = rawmaterialPrice.Month11;
            model.Month12 = rawmaterialPrice.Month12;
            model.LastUserId = MyInfo.Id;
            model.LastDate = DateTime.Now;
            model.Display = 1;
            bool isok = false;
            if (rawmaterialPrice.Id < 1)
            {
                model.AddDate = model.LastDate;
                model.AddUserId = model.LastUserId;
                isok = Bll.BllSupplier_RawMaterialPrice.Insert(model) > 0;
            }
            else
            {
                isok = Bll.BllSupplier_RawMaterialPrice.Update(model) > 0;
            }

            return LayerAlertAndCallback("保存" + (isok ? "成功" : "失败"), "fun.yearsAjax()");
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllSupplier_RawMaterialPrice.HideModel(id, MyInfo.Id) > 0 });
        }

        [HttpPost]
        public JsonResult DeleteBatch(List<Supplier_RawMaterialPrice> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllSupplier_RawMaterialPrice.HideList(list, MyInfo.Id) > 0 });
        }
        #endregion

        #region 辅助方法
        private bool CheckSqlField(string[] arr)
        {
            foreach (var item in arr)
            {
                if (!Tools.Usual.Utils.CheckSqlField(item))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
