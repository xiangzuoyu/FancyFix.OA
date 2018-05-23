using FancyFix.OA.Model;
using FancyFix.Tools.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Product.Controllers
{
    public class SearchController : Base.BaseAdminController
    {
        // GET: Product/Search
        public ActionResult Index()
        {
            string spu = RequestString("spu");
            string tag = RequestString("tag");
            byte type = RequestByte("type");

            //标签列表
            var taglist = Bll.BllProduct_ImageTag.GetAll();

            Product_Info model = Bll.BllProduct_Info.First(o => o.Spu == Tools.Usual.Utils.CheckSqlValue(spu));

            string className = string.Empty;
            List<Product_Image> imglist = null;
            if (model != null)
            {
                imglist = Bll.BllProduct_Image.GetList(model.Id, tag, type);
                className = Bll.BllProduct_Class.Instance().GetClassName(model.ClassId);
            }

            ViewBag.typeHtml = Tools.Enums.Tools.GetOptionHtml(typeof(Tools.Enums.ESite.ImageType), type);
            ViewBag.className = className;
            ViewBag.tag = tag;
            ViewBag.spu = spu;
            ViewBag.type = type;
            ViewBag.taglist = taglist;
            ViewBag.imglist = imglist;
            return View(model);
        }

        public FileResult DownloadFiles()
        {
            string spu = RequestString("spu");
            string tag = RequestString("tag");
            byte type = RequestByte("type");

            Product_Info model = Bll.BllProduct_Info.First(o => o.Spu == Tools.Usual.Utils.CheckSqlValue(spu));
            if (model == null)
            {

            }

            List<Product_Image> imglist = null;
            if (model != null)
            {
                imglist = Bll.BllProduct_Image.GetList(model.Id, tag, type);
            }

            string root = Server.MapPath("~/App_Data");
            string fileName = "test.jpg";
            string filePath = Path.Combine(root, fileName);
            string s = MimeMapping.GetMimeMapping(fileName);

            return File(filePath, s, Path.GetFileName(filePath));
        }
    }
}