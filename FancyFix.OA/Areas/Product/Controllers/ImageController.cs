using FancyFix.Core;
using FancyFix.OA.Model;
using FancyFix.Tools.Json;
using FancyFix.OA.ViewModel;
using FancyFix.OA.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FancyFix.Tools.Config;

namespace FancyFix.OA.Areas.Product.Controllers
{
    public class ImageController : Base.BaseAdminController
    {
        public ActionResult List(int id)
        {
            if (id == 0) return LayerAlertErrorAndReturn("请选择一个产品！");
            Product_Info model = Bll.BllProduct_Info.First(o => o.Id == id);
            if (model == null) return Response404();
            return View(model);
        }

        [PermissionFilter("/product/image/list")]
        [ValidateInput(false)]
        public JsonResult PageList(int page, int pagesize)
        {
            long records = 0;
            int proid = RequestInt("proid");

            var list = Bll.BllProduct_Image.PageList(proid, page, pagesize, out records);
            return BspTableJson(list, records);
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            int rows = Bll.BllProduct_Image.Delete(o => o.Id == id);
            return Json(new { result = rows > 0 });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [PermissionFilter("/product/image/delete")]
        [HttpPost]
        public JsonResult DeleteBatch(List<Product_Image> list)
        {
            return Json(new { result = Bll.BllProduct_Image.Delete(list) > 0 });
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Add(int id)
        {
            if (id == 0) return LayerAlertErrorAndReturn("请选择一个产品！");
            Product_Info model = Bll.BllProduct_Info.First(o => o.Id == id);
            if (model == null) return Response404();
            return View(model);
        }

        /// <summary>
        /// 编辑图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            if (id == 0) return LayerAlertErrorAndReturn("请选择一个图片！");
            Product_Image model = Bll.BllProduct_Image.First(o => o.Id == id);
            if (model == null) return Response404();

            ViewBag.taglist = Bll.BllProduct_ImageTag.GetAll();
            return View(model);
        }

        [PermissionFilter("/product/image/save")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save()
        {
            int id = RequestInt("id");
            string tag = RequestString("tag");

            if (id == 0) return LayerAlertErrorAndReturn("请选择一个图片！");
            Product_Image model = Bll.BllProduct_Image.First(o => o.Id == id);
            if (model == null) return Response404();

            model.Tag = tag;

            int rows = Bll.BllProduct_Image.Update(model);
            if (rows > 0)
            {
                return LayerAlertSuccessAndRefresh("编辑成功");
            }
            else
            {
                return LayerAlertErrorAndReturn("编辑失败");
            }
        }
    }
}