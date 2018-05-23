using FancyFix.OA.Filter;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Product.Controllers
{
    public class PatternController : Base.BaseAdminController
    {
        public ActionResult List()
        {
            return View();
        }

        //[PermissionFilter("/product/pattern/list")]
        [ValidateInput(false)]
        public JsonResult PageList(int page, int pagesize)
        {
            long records = 0;
            string name = RequestString("name");
            string code = RequestString("code");

            var list = Bll.BllProduct_Pattern.PageList(name, code, page, pagesize, out records);
            return BspTableJson(list, records);
        }

        /// <summary>
        /// 产品编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            Product_Pattern model = null;
            if (id == 0)
            {
                model = new Product_Pattern();
            }
            else
            {
                model = Bll.BllProduct_Pattern.First(o => o.Id == id);
                if (model == null) return LayerAlertErrorAndClose("记录不存在！");
            }
            return View(model);
        }

        //[PermissionFilter("/product/pattern/edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(Product_Pattern model)
        {
            string patternName = RequestString("patternname");
            string patternCode = RequestString("patterncode");
            if (string.IsNullOrEmpty(patternName)) return LayerAlertErrorAndReturn("请填写名称！");
            if (string.IsNullOrEmpty(patternCode)) return LayerAlertErrorAndReturn("请填写编号！");
            string firstPic = string.Empty;
            string pics = GetPics("pic", ref firstPic);

            if (model.Id > 0)
            {
                model = Bll.BllProduct_Pattern.First(o => o.Id == model.Id);
                if (model == null) return Response404();
            }
            else
            {
                model = new Product_Pattern();
            }

            if ((model.Id == 0 || model.PatternCode != patternCode) && Bll.BllProduct_Pattern.CheckCode(patternCode))
                return LayerAlertErrorAndReturn("编码重复！");

            model.IsShow = RequestBool("isshow");
            model.PatternName = patternName;
            model.PatternCode = patternCode;
            model.FirstPic = firstPic;
            model.Pics = pics;
            model.AddTime = DateTime.Now;
            model.AdminId = MyInfo.Id;

            if ((model.Id > 0 ? Bll.BllProduct_Pattern.Update(model) : Bll.BllProduct_Pattern.Insert(model)) > 0)
                return LayerAlertSuccessAndRefresh((model.Id > 0 ? "修改" : "添加") + "成功");
            else
                return LayerAlertErrorAndReturn((model.Id > 0 ? "修改" : "添加") + "失败");
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllProduct_Pattern.Delete(o => o.Id == id) > 0 });
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetShow(int id)
        {
            return Json(new { result = Bll.BllProduct_Pattern.SetShow(id) });
        }
    }
}