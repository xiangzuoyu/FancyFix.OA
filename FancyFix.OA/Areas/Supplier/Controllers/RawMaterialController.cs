using FancyFix.OA.Base;
using FancyFix.OA.Model;
using FancyFix.Tools.Config;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Supplier.Controllers
{
    public class RawMaterialController : BaseAdminController
    {
        // GET: Supplier/RawMaterial

        #region 加载列表
        public ActionResult List()
        {
            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0, string file = "", string key = "")
        {
            long records = 0;
            var list = Bll.BllSupplier_RawMaterial.PageList(page, pagesize, out records, file, key);
            var supplierList = Bll.BllSupplier_List.GetSelectList(0, "Code,Name", "display!=2", "");
            var mappingList = Bll.BllSupplier_PriceMapping.GetSelectList(0, "Id,RawMaterialCode,VendorCode", "display!=2", "");

            foreach (var item in list)
            {
                string vendorCode = mappingList.Where(o => o.Id == item.VendorId).Select(o => o.VendorCode).FirstOrDefault();
                var supplierModel = supplierList.Where(o => o.Code == vendorCode && o.Display != 2).FirstOrDefault() ?? new Supplier_List();
                item.SupplierCode = supplierModel.Code;
                item.SupplierName = supplierModel.Name;

                item.PriceFrequencyName = item.PriceFrequency != null
                    ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.PriceFrequency), item.PriceFrequency.GetValueOrDefault().ToString().ToInt32()).ToString()
                    : "无";
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
    }
}