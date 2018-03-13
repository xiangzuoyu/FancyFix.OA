using FancyFix.OA.Base;
using FancyFix.Tools.Config;
using System;
using FancyFix.OA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

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

                var strList = Tools.Tool.ExcelHelper.RenderToSql<Supplier_List>(filePath, 1);

                var modelList = StrToList(strList);

                if (modelList == null || modelList.Count < 1)
                    LayerAlertSuccessAndRefresh("数据为空，导出失败！");

                if (Bll.BllSupplier_List.Insert(modelList) < 1)
                    return LayerAlertErrorAndReturn("导入失败");
            }
            catch (Exception)
            {
                return LayerAlertErrorAndReturn("导入失败");
            }

            return Redirect("list");
        }

        //('A','B','C','D','E','F','G','H','2015-8-10','2017-3-10','J','K'),('1','2','3','4','5','6','7','8','2015-8-10','2017-3-10','10','11')

        private List<Supplier_List> StrToList(List<string> strList)
        {
            List<Supplier_List> list = new List<Supplier_List>();

            if (strList == null || strList.Count() < 1)
                return list;

            try
            {
                foreach (var item in strList)
                {
                    var model = new Supplier_List();
                    Regex reg = new Regex(@"%%%@@@");
                    var arr = reg.Split(item);
                    if (arr.Length < 11)
                        continue;

                    model.Code = arr[0];
                    model.Name = arr[1];

                    //model.SupplierType = arr[1];

                    list.Add(model);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return list;
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllSupplier_List.Delete(o => o.Id == id) > 0 });
        }

        [HttpPost]
        public JsonResult DeleteBatch(List<Supplier_List> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllSupplier_List.Delete(list) > 0 });
        }
    }
}