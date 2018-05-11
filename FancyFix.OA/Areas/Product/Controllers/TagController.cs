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
    public class TagController : Base.BaseAdminController
    {
        public ActionResult List()
        {
            return View();
        }

        [PermissionFilter("/product/tag/list")]
        [ValidateInput(false)]
        public JsonResult PageList(int page, int pagesize)
        {
            long records = 0;
            string tag = RequestString("tag");

            var list = Bll.BllProduct_ImageTag.PageList(tag, page, pagesize, out records);
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
            int rows = Bll.BllProduct_ImageTag.Delete(o => o.Id == id);
            return Json(new { result = rows > 0 });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [PermissionFilter("/product/tag/delete")]
        [HttpPost]
        public JsonResult DeleteBatch(List<Product_ImageTag> list)
        {
            return Json(new { result = Bll.BllProduct_ImageTag.Delete(list) > 0 });
        }

        /// <summary>
        /// 编辑图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            Product_ImageTag model = null;

            if (id > 0)
            {
                model = Bll.BllProduct_ImageTag.First(o => o.Id == id);
                if (model == null) return Response404();
            }
            return View(model);
        }

        [PermissionFilter("/product/tag/edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save()
        {
            int id = RequestInt("id");
            string tag = RequestString("tag");

            Product_ImageTag model = null;
            if (id > 0)
            {
                model = Bll.BllProduct_ImageTag.First(o => o.Id == id);
                if (model == null) return Response404();
            }
            else
            {
                model = new Product_ImageTag();
            }

            bool isExist = Bll.BllProduct_ImageTag.IsExist(tag, model?.Id ?? 0);
            if (isExist)
                return LayerAlertErrorAndReturn("标签已存在！");

            model.Tag = tag;
            model.AddTime = DateTime.Now;

            int rows = id > 0 ? Bll.BllProduct_ImageTag.Update(model) : Bll.BllProduct_ImageTag.Insert(model);
            if (rows > 0)
            {
                return LayerAlertSuccessAndRefresh("操作成功！");
            }
            else
            {
                return LayerAlertErrorAndReturn("操作失败！");
            }
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public JsonResult AddTag(string tag)
        {
            tag = Tools.Usual.Utils.CheckSqlValue(tag.Replace(",", "'"));
            if (tag != "")
            {
                var model = Bll.BllProduct_ImageTag.First(o => o.Tag == tag);
                if (model != null)
                {
                    return Json(new { status = false, tag = model.Tag });
                }
                else
                {
                    int id = Bll.BllProduct_ImageTag.Insert(new Product_ImageTag()
                    {
                        Tag = tag,
                        AddTime = DateTime.Now
                    });
                    if (id > 0)
                        return Json(new { status = true, tag = tag });
                    else
                        return Json(new { status = false, tag = "" });
                }
            }
            return Json(new { status = false, tag = "" });
        }
    }
}