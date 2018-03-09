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

            string filePath= UploadProvice.Instance().Settings["file"].FilePath+@"\"+DateTime.Now.ToString("yyyyMMddHHmmss")+ ".xlsx";
            var size = file.ContentLength;
            var type = file.ContentType;
            file.SaveAs(filePath);

            FileStream file1 = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            RenderToDb(file1, "ssss");

            return Redirect("List");
        }

        private int RenderToDb(Stream excelFileStream, string insertSql)
        {
            //int rowAffected = 0;

            //IWorkbook workbook = new HSSFWorkbook(excelFileStream)
            //try
            //{

            //}
            //finally
            //{
            //    if (excelFileStream != null)
            //        excelFileStream.Dispose();
               
            //}
            return rowAffected;
        }
    }
}