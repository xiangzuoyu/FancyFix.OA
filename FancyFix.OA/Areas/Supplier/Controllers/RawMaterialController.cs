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
    public class RawMaterialController : BaseAdminController
    {
        // GET: Supplier/RawMaterial

        #region 加载列表
        public ActionResult List()
        {
            ViewBag.prices = Bll.BllSupplier_RawMaterialPrice.GetSelectList(0, "distinct(Years)", "display!=2", "Years") ?? new List<Supplier_RawMaterialPrice>();

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

            var list = Bll.BllSupplier_RawMaterial.PageList(page, pagesize, out records, files, key.Trim(), priceFrequency);

            var supplierList = Bll.BllSupplier_List.GetList(0, "Code,Name", "display!=2", "", files, key) ?? new List<Supplier_List>();
            var mappingList = Bll.BllSupplier_PriceMapping.GetSelectList(0, "Id,RawMaterialCode,VendorCode", "display!=2", "") ?? new List<Supplier_PriceMapping>();
            var rawmaterialprice = Bll.BllSupplier_RawMaterialPrice.GetSelectList(0, "", string.Format(" Years='{0}' and display!=2", years), "")
                ?? new List<Supplier_RawMaterialPrice>();
            //var list2 = new List<Supplier_RawMaterial>();
            foreach (var item in list)
            {
                string vendorCode = mappingList.Where(o => o.Id == item.VendorId).Select(o => o.VendorCode).FirstOrDefault();
                //供应商,如果该数据供应商为空就跳过，说明不符合供应商查询条件
                var supplierModel = supplierList.Where(o => o.Code == vendorCode && o.Display != 2).FirstOrDefault() ?? new Supplier_List();
                item.SupplierCode = supplierModel.Code;
                item.SupplierName = supplierModel.Name;
                if (string.IsNullOrEmpty(item.SupplierCode) && string.IsNullOrEmpty(item.SupplierName))
                    continue;
                //价格
                var rawmaterialpriceModel = rawmaterialprice.Where(o => o.RawMaterialId == item.Id).FirstOrDefault();
                if (rawmaterialpriceModel != null)
                {
                    item.Month1 = rawmaterialpriceModel.Month1;
                    item.Month2 = rawmaterialpriceModel.Month2;
                    item.Month3 = rawmaterialpriceModel.Month3;
                    item.Month4 = rawmaterialpriceModel.Month4;
                    item.Month5 = rawmaterialpriceModel.Month5;
                    item.Month6 = rawmaterialpriceModel.Month6;
                    item.Month7 = rawmaterialpriceModel.Month7;
                    item.Month8 = rawmaterialpriceModel.Month8;
                    item.Month9 = rawmaterialpriceModel.Month9;
                    item.Month10 = rawmaterialpriceModel.Month10;
                    item.Month11 = rawmaterialpriceModel.Month11;
                    item.Month12 = rawmaterialpriceModel.Month12;
                }

                //价格频次
                item.PriceFrequencyName = item.PriceFrequency != null
                    ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.PriceFrequency), item.PriceFrequency.GetValueOrDefault().ToString().ToInt32()).ToString()
                    : "无";

                //list2.Add(item);
            }
            return BspTableJson(list, records);
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
                var type = file.ContentType;
                //判断文件大小和格式

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
            ViewBag.priceFrequencyId = priceFrequency < 1 ? 1 : priceFrequency;
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
            if (!CheckSqlField(arr))
                MessageBox("选择字段异常");
            //arr= arr.Concat(new string[] { "" }).ToArray();


            if (years < 1)
                MessageBox("请先选择导出的年份！");

            //string where = "Display!=2 ";
            //if (!string.IsNullOrEmpty(files) && !string.IsNullOrEmpty(key))
            //    where += string.Format(" {0} like '%{1}%' ", files, key);
            var list = Bll.BllSupplier_RawMaterial.GetList(0, "", "", "", files, key, priceFrequency);
            ToExcel(list, years);

            return LayerClose();
        }

        public void ToExcel(DataTable list, int years)
        {
            if (list == null || list.Rows.Count < 1)
                return;

            DataTable dt = NewDatable(years);
            var supplierList = Bll.BllSupplier_List.GetSelectList(0, "Code,Name", "display!=2", "") ?? new List<Supplier_List>();
            var mappingList = Bll.BllSupplier_PriceMapping.GetSelectList(0, "Id,RawMaterialCode,VendorCode", "display!=2", "") ?? new List<Supplier_PriceMapping>();
            var rawmaterialprice = Bll.BllSupplier_RawMaterialPrice.GetSelectList(0, "", string.Format(" Years='{0}' and display!=2", years), "")
                ?? new List<Supplier_RawMaterialPrice>();

            try
            {
                int col = list.Columns.Count;
                foreach (DataRow item in list.Rows)
                {
                    var row = dt.NewRow();

                    row[0] = item[1];//BU
                    row[1] = item[2];//SAPCode
                    row[2] = item[3];//Description
                    row[3] = item[4];//Category
                    row[4] = item[5];

                    string vendorCode = mappingList.Where(o => o.Id == item[6]?.ToString().ToInt32()).Select(o => o.VendorCode).FirstOrDefault() ?? "";
                    //供应商
                    var supplierModel = supplierList.Where(o => o.Code == vendorCode && o.Display != 2).FirstOrDefault() ?? new Supplier_List();
                    row[5] = supplierModel.Code;
                    row[6] = supplierModel.Name;
                    //价格频次
                    row[7] = item[7] != null
                        ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.PriceFrequency), item[7].ToString().ToInt32()).ToString()
                        : "无";
                    row[8] = item[8];

                    //价格
                    var rawmaterialpriceModel = rawmaterialprice.Where(o => o.RawMaterialId == item[0].ToString().ToInt32()).FirstOrDefault();
                    if (rawmaterialpriceModel != null)
                    {
                        row[9] = rawmaterialpriceModel.Month1;
                        row[10] = rawmaterialpriceModel.Month2;
                        row[11] = rawmaterialpriceModel.Month3;
                        row[12] = rawmaterialpriceModel.Month4;
                        row[13] = rawmaterialpriceModel.Month5;
                        row[14] = rawmaterialpriceModel.Month6;
                        row[15] = rawmaterialpriceModel.Month7;
                        row[16] = rawmaterialpriceModel.Month8;
                        row[17] = rawmaterialpriceModel.Month9;
                        row[18] = rawmaterialpriceModel.Month10;
                        row[19] = rawmaterialpriceModel.Month11;
                        row[20] = rawmaterialpriceModel.Month12;
                    }

                    dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
            }

            string fileName = "凡菲供应商采购价格跟踪系统" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Tools.Tool.ExcelHelper.ToExcelWeb(dt, "凡菲供应商采购价格跟踪系统", fileName + ".xls");
        }

        private DataTable NewDatable(int years)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("业务部门", typeof(String));
            dt.Columns.Add("原材料代码", typeof(String));
            dt.Columns.Add("采购产品名称", typeof(String));
            dt.Columns.Add("品类", typeof(String));
            dt.Columns.Add("采购负责人", typeof(String));
            dt.Columns.Add("供应商代码", typeof(String));
            dt.Columns.Add("供应商名称", typeof(String));
            dt.Columns.Add("价格频次（月/季度/半年/年/单次）", typeof(String));
            dt.Columns.Add("价格单位", typeof(String));
            dt.Columns.Add("年/月", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));
            dt.Columns.Add("", typeof(String));

            var rows = dt.NewRow();
            rows[0] = "BU";
            rows[1] = "SAP Code";
            rows[2] = "Description";
            rows[3] = "Category";
            rows[4] = "Lead Buyer";
            rows[5] = "Vendor Code";
            rows[6] = "Vendor Name";
            rows[7] = "Price Frequency";
            rows[8] = "Currency";
            rows[9] = "'" + years.ToString() + "-01";
            rows[10] = "'" + years.ToString() + "-02";
            rows[11] = "'" + years.ToString() + "-03";
            rows[12] = "'" + years.ToString() + "-04";
            rows[13] = "'" + years.ToString() + "-05";
            rows[14] = "'" + years.ToString() + "-06";
            rows[15] = "'" + years.ToString() + "-07";
            rows[16] = "'" + years.ToString() + "-08";
            rows[17] = "'" + years.ToString() + "-09";
            rows[18] = "'" + years.ToString() + "-10";
            rows[19] = "'" + years.ToString() + "-11";
            rows[20] = "'" + years.ToString() + "-12";

            return dt;
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllSupplier_RawMaterial.HideModel(id, MyInfo.Id) > 0 });
        }

        [HttpPost]
        public JsonResult DeleteBatch(List<Supplier_RawMaterial> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllSupplier_RawMaterial.HideList(list, MyInfo.Id) > 0 });
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
