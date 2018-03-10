using FancyFix.OA.Base;
using FancyFix.Tools.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Supplier.Controllers
{
    public class SupplierManagerController : BaseAdminController
    {
        // GET: Supplier/SupplierManager
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(HttpPostedFileBase file)
        {
            if (file == null)
                Redirect("List");

            string filePath = UploadProvice.Instance().Settings["file"].FilePath + DateTime.Now.ToString("yyyyMMddHHmmss")
                + (file.FileName.IndexOf(".xlsx") > 0 ? ".xlsx" : "xls");
            var size = file.ContentLength;
            var type = file.ContentType;
            file.SaveAs(filePath);

            string insert = "insert into Supplier_List(code,name,SupplierType,BusinessScope,Contact1,Contact2,Site,Address,StartDate,EndDate,LabelId,Note)";
            string insertSql = Tools.Tool.ExcelHelper.RenderToSql(filePath, insert);

            return Redirect("List");
        }
 
    }
}