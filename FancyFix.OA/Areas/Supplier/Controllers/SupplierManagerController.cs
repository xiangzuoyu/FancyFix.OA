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
        #region 加载列表
        public ActionResult List()
        {
            return View();
        }

        public JsonResult PageList(int page = 0, int pagesize = 0, int selectLabelid = 0)
        {
            long records = 0;
            var list = Bll.BllSupplier_List.PageList(page, pagesize, out records, selectLabelid);
            foreach (var item in list)
            {
                item.SupplierTypeName = item.SupplierType != null
                    ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.SupplierType), item.SupplierType.GetValueOrDefault().ToString().ToInt32()).ToString()
                    : "未分配";
                item.LabelName = item.LabelId != null
                    ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.SupplierLabel), item.LabelId.GetValueOrDefault().ToString().ToInt32()).ToString()
                    : "未分配";
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
                var modelList = ExcelToList(sheet, 1);

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
                        //SupplierType = Enum.IsDefined(typeof(Models.SupplierType), row.GetCell(3)?.ToString())
                        //? (int)Enum.Parse(typeof(Models.SupplierType), row.GetCell(3)?.ToString())
                        //: 1,
                        SupplierType = Tools.Enums.Tools.GetValueByName(typeof(Models.SupplierType), row.GetCell(3)?.ToString()),
                        BusinessScope = row.GetCell(4)?.ToString(),
                        Contact1 = row.GetCell(5)?.ToString(),
                        Contact2 = row.GetCell(6)?.ToString(),
                        Site = row.GetCell(7)?.ToString(),
                        Address = row.GetCell(8)?.ToString(),
                        StartDate = row.GetCell(9)?.ToString().ToDateTime(),
                        //LabelId = Enum.IsDefined(typeof(Models.SupplierLabel), row.GetCell(10)?.ToString())
                        //? (int)Enum.Parse(typeof(Models.SupplierLabel), row.GetCell(10)?.ToString())
                        //: 1,
                        LabelId = Tools.Enums.Tools.GetValueByName(typeof(Models.SupplierLabel), row.GetCell(10)?.ToString()),
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
        #endregion

        #region 导出Excel
        public ActionResult ExportExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExportExcel(int selectLabelid = 0)
        {
            string cols = RequestString("cols");
            var arr = cols.Split(',');
            if (!CheckSqlField(arr))
                MessageBox("选择字段异常");

            if (arr.Length < 1)
                return LayerMsgErrorAndReturn("导出失败，请先勾选字段");

            DataTable dt = NewExportDt(arr);

            string where = "Display!=2 ";
            where += selectLabelid > 0 ? " and LabelId=" + selectLabelid : "";
            var list = Bll.BllSupplier_List.GetList(0, cols, where, "");
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
                                ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.SupplierType), item[i].ToString().ToInt32()).ToString()
                                : "未分配";
                        else if (list.Columns[i].ToString() == "LabelId")
                            row[i] = item[i] != null
                                ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.SupplierLabel), item[i].ToString().ToInt32()).ToString()
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
            Tools.Tool.ExcelHelper.ToExcelWeb(dt, "", fileName + ".xls");
        }

        private DataTable NewExportDt(string[] arr)
        {
            DataTable dt = new DataTable();
            foreach (var item in arr)
            {
                switch (item)
                {
                    case "Code":
                        dt.Columns.Add("供应商代码", typeof(String));
                        break;
                    case "Name":
                        dt.Columns.Add("供应商名称", typeof(String));
                        break;
                    case "SupplierAb":
                        dt.Columns.Add("供应商名称缩写", typeof(String));
                        break;
                    case "SupplierType":
                        dt.Columns.Add("供应商类型（RM/PM/FG/Parts/Convert)", typeof(String));
                        break;
                    case "BusinessScope":
                        dt.Columns.Add("经营范围/供应物料", typeof(String));
                        break;
                    case "Contact1":
                        dt.Columns.Add("联系人（1）/电话/邮箱", typeof(String));
                        break;
                    case "Contact2":
                        dt.Columns.Add("联系人（2）/电话/邮箱", typeof(String));
                        break;
                    case "Site":
                        dt.Columns.Add("网址", typeof(String));
                        break;
                    case "Address":
                        dt.Columns.Add("地址", typeof(String));
                        break;
                    case "StartDate":
                        dt.Columns.Add("合作时间（起止）", typeof(String));
                        break;
                    case "LabelId":
                        dt.Columns.Add("合格/黑名单/潜在", typeof(String));
                        break;
                    case "Accountdate":
                        dt.Columns.Add("账期", typeof(String));
                        break;
                    case "Note":
                        dt.Columns.Add("备注", typeof(String));
                        break;
                }
            }

            return dt;
        }
        #endregion

        #region 删除
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

        #region 编辑
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
            Supplier_List model = Bll.BllSupplier_List.First(o => o.Id == supplierList.Id && o.Display != 2 && o.Id > 0) ?? new Supplier_List();
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