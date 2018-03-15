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
                item.LabelName = item.LabelId != null
                    ? (Enum.Parse(typeof(Models.SupplierLabel), item.LabelId.GetValueOrDefault().ToString())).ToString() : "未分配";
                item.SupplierTypeName = item.SupplierType != null
                    ? (Enum.Parse(typeof(Models.SupplierType), item.SupplierType.GetValueOrDefault().ToString())).ToString() : "未分配";
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
                    return LayerAlertSuccessAndRefresh("数据为空，导入失败！");

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

        [HttpPost]
        public ActionResult ExportExcel(int selectLabelid)
        {
            string code = RequestString("code");
            string name = RequestString("name");
            string supplierab = RequestString("supplierab");
            string suppliertype = RequestString("suppliertype");
            string businessscope = RequestString("businessscope");
            string contact1 = RequestString("contact1");
            string contact2 = RequestString("contact2");
            string site = RequestString("site");
            string address = RequestString("address");
            string startdate = RequestString("startdate");
            string labelid = RequestString("labelid");
            string accountdate = RequestString("accountdate");
            string note = RequestString("note");

            string files = string.Empty;
            DataTable dt = new DataTable();

            if (code == "on")
            {
                files += "Code,";
                dt.Columns.Add("供应商代码", typeof(String));
            }

            if (name == "on")
            {
                files += "Name,";
                dt.Columns.Add("供应商名称", typeof(String));
            }
            if (supplierab == "on")
            {
                files += "SupplierAb,";
                dt.Columns.Add("供应商名称缩写", typeof(String));
            }
            if (suppliertype == "on")
            {
                files += "SupplierType,";
                dt.Columns.Add("供应商类型（RM/PM/FG/Parts/Convert)", typeof(String));
            }
            if (businessscope == "on")
            {
                files += "BusinessScope,";
                dt.Columns.Add("经营范围/供应物料", typeof(String));
            }
            if (contact1 == "on")
            {
                files += "Contact1,";
                dt.Columns.Add("联系人（1）/电话/邮箱", typeof(String));
            }
            if (contact2 == "on")
            {
                files += "Contact2,";
                dt.Columns.Add("联系人（2）/电话/邮箱", typeof(String));
            }
            if (site == "on")
            {
                files += "Site,";
                dt.Columns.Add("网址", typeof(String));
            }
            if (address == "on")
            {
                files += "Address,";
                dt.Columns.Add("地址", typeof(String));
            }
            if (startdate == "on")
            {
                files += "StartDate,";
                dt.Columns.Add("合作时间（起止）", typeof(String));
            }
            if (labelid == "on")
            {
                files += "LabelId,";
                dt.Columns.Add("合格/黑名单/潜在", typeof(String));
            }
            if (accountdate == "on")
            {
                files += "Accountdate,";
                dt.Columns.Add("账期", typeof(String));
            }
            if (note == "on")
            {
                files += "Note,";
                dt.Columns.Add("备注", typeof(String));
            }

            if (files.Length < 1)
                return LayerMsgErrorAndReturn("导出失败，请先勾选字段");

            string where = "Display!=2 ";
            where += selectLabelid > 0 ? " and LabelId=" + selectLabelid : "";
            var list = Bll.BllSupplier_List.GetList(0, files.TrimEnd(','), where, "");
            ToExcel(list, dt);

            return LayerClose();
        }

        public void ToExcel(DataTable list, DataTable dt)
        {

            if (list == null || list.Rows.Count < 1)
                return;

            int col = list.Columns.Count;
            foreach (DataRow item in list.Rows)
            {
                var row = dt.NewRow();
                for (int i = 0; i < col; i++)
                {
                    if (list.Columns[i].ToString() == "SupplierType")
                        row[i] = item[i] != null ? (Enum.Parse(typeof(Models.SupplierType), item[i].ToString())).ToString() : "未分配";
                    else if (list.Columns[i].ToString() == "LabelId")
                        row[i] = item[i] != null ? (Enum.Parse(typeof(Models.SupplierLabel), item[i].ToString())).ToString() : "未分配";
                    else
                        row[i] = item[i]?.ToString();
                }
                dt.Rows.Add(row);
            }

            string fileName = "供应商列表" + DateTime.Now.ToString("yyyyMMddHHmmss");

            Tools.Tool.ExcelHelper.ToExcelWeb(dt, "", fileName + ".xls");
        }

        private List<Supplier_List> ExcelToList(ISheet sheet, int startRow)
        {
            List<Supplier_List> supList = new List<Supplier_List>();
            if (sheet == null)
                return supList;

            //第一行为标题
            try
            {
                IRow headRow = sheet.GetRow(0);
                int cellCount = headRow.LastCellNum;
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

                    var model = new Supplier_List()
                    {
                        Code = code,
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
            }
            catch (Exception)
            {
                return supList;
            }

            return supList;
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllSupplier_List.HideModel(id, MyInfo.Id) > 0 });
        }

        [HttpPost]
        public JsonResult DeleteBatch(List<Supplier_List> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllSupplier_List.HideList(list, MyInfo.Id) > 0 });
        }

        [HttpPost]
        public JsonResult UpdateLabelBatch(List<Supplier_List> list, int labelId)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new
            {
                result = Bll.BllSupplier_List.UpdateLabel(list, labelId, MyInfo.Id) > 0
            });
        }

    }
}