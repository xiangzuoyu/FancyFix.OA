using FancyFix.OA.Base;
using FancyFix.Tools.Config;
using System;
using FancyFix.OA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;
using System.Data;

namespace FancyFix.OA.Areas.Supplier.Controllers
{
    public class SupplierManagerController : BaseAdminController
    {
        // GET: Supplier/SupplierManager
        public ActionResult List()
        {
            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0, int displayid = 0)
        {
            long records = 0;
            var list = Bll.BllSupplier_List.PageList(page, pagesize, out records, displayid);
            foreach (var item in list)
            {
                item.LabelName = item.LabelId != null ? (Enum.Parse(typeof(Models.SupplierLabel), item.LabelId.GetValueOrDefault().ToString())).ToString() : "未分配";
            }


            return BspTableJson(list, records);
        }

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
                file.SaveAs(filePath);

                var sheet = Tools.Tool.ExcelHelper.ReadExcel(filePath);
                var modelList = ExcelToList(sheet, 1);

                if (modelList == null || modelList.Count < 1)
                    LayerAlertSuccessAndRefresh("数据为空，导出失败！");

                if (Bll.BllSupplier_List.Insert(modelList) < 1)
                    return LayerAlertErrorAndReturn("导入失败");
            }
            catch (Exception ex)
            {
                return LayerAlertErrorAndReturn("导入失败:" + ex.Message.ToString());
            }

            return Redirect("list");
        }

        public ActionResult ExportExcel()
        {
            return View();
        }

        public void ToExcel()
        {
            int lalbel = RequestInt("lalbel");
            var list = new List<Supplier_List>();
            if (list == null || list.Count < 1)
                return;

            DataTable dt = new DataTable();
            dt.Columns.Add("供应商代码", typeof(String));
            dt.Columns.Add("供应商名称", typeof(String));
            dt.Columns.Add("供应商名称缩写", typeof(String));
            dt.Columns.Add("供应商类型（RM/PM/FG/Parts/Convert)", typeof(String));
            dt.Columns.Add("经营范围/供应物料", typeof(String));
            dt.Columns.Add("联系人（1）/电话/邮箱", typeof(String));
            dt.Columns.Add("联系人（2）/电话/邮箱", typeof(String));
            dt.Columns.Add("网址", typeof(String));
            dt.Columns.Add("地址", typeof(String));
            dt.Columns.Add("合作时间（起止）", typeof(String));
            dt.Columns.Add("合格/黑名单/潜在", typeof(String));
            dt.Columns.Add("账期", typeof(String));
            dt.Columns.Add("备注", typeof(String)); 

            foreach (var item in list)
            {
                var row = dt.NewRow();
                row[0] = item.Code;
                row[1] = item.Name;
                row[2] = item.SupplierAb;
                row[3] = item.SupplierType != null ? (Enum.Parse(typeof(Models.SupplierLabel), item.SupplierType.GetValueOrDefault().ToString())).ToString() : "未分配";
                row[4] = item.BusinessScope;
                row[5] = item.Contact1;
                row[6] = item.Contact2;
                row[7] = item.Site;
                row[8] = item.Address;
                row[9] = item.StartDate;
                row[10] = item.LabelId != null ? (Enum.Parse(typeof(Models.SupplierLabel), item.LabelId.GetValueOrDefault().ToString())).ToString() : "未分配";
                row[11] = item.AccountDate;
                row[12] = item.Note;
            }

            string fileName = "供应商列表"+DateTime.Now.ToString("yyyyMMddHHmmss");

            Tools.Tool.ExcelHelper.ToExcelWeb(dt, fileName, fileName + ".xls");
        }

        private List<Supplier_List> ExcelToList(ISheet sheet, int startRow)
        {
            List<Supplier_List> supList = new List<Supplier_List>();
            if (sheet == null)
                return supList;

            //第一行为标题
            IRow headRow = sheet.GetRow(0);
            int cellCount = headRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            for (int i = startRow; i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                    continue;

                var model = new Supplier_List()
                {
                    Code = row.GetCell(0)?.ToString(),
                    Name = row.GetCell(1)?.ToString(),
                    SupplierAb = row.GetCell(2)?.ToString(),
                    SupplierType = Enum.IsDefined(typeof(Models.SupplierType), row.GetCell(3)?.ToString())
                    ? (int)Enum.Parse(typeof(Models.SupplierType), row.GetCell(3)?.ToString())
                    : 1,
                    BusinessScope = row.GetCell(4)?.ToString(),
                    Contact1 = row.GetCell(5)?.ToString(),
                    Contact2 = row.GetCell(6)?.ToString(),
                    Site = row.GetCell(7)?.ToString(),
                    Address = row.GetCell(8)?.ToString(),
                    StartDate = row.GetCell(9)?.ToString().ToDateTime(),
                    //EndDate = row.GetCell(10)?.ToString().ToDateTime(),
                    LabelId = Enum.IsDefined(typeof(Models.SupplierLabel), row.GetCell(10)?.ToString())
                    ? (int)Enum.Parse(typeof(Models.SupplierLabel), row.GetCell(10)?.ToString())
                    : 1,
                    AccountDate = row.GetCell(11)?.ToString(),
                    Note = row.GetCell(12)?.ToString(),
                    AddDate = DateTime.Now,
                    AddUserId = MyInfo.Id,
                    LastDate = DateTime.Now,
                    LastUserId = MyInfo.Id,
                };

                //如果关键几个字段没有数据，就跳过
                if (string.IsNullOrEmpty(model.Code) &&
                    string.IsNullOrEmpty(model.Name) &&
                    string.IsNullOrEmpty(model.BusinessScope))
                    continue;
                supList.Add(model);
            }

            return supList;
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllSupplier_List.HideModel(id) > 0 });
        }

        [HttpPost]
        public JsonResult DeleteBatch(List<Supplier_List> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllSupplier_List.HideList(list) > 0 });
        }


    }
}