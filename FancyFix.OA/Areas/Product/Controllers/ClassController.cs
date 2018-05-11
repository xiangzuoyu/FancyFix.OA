using FancyFix.OA.Areas.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FancyFix.OA.Model;
using FancyFix.OA.Filter;

namespace FancyFix.OA.Areas.Product.Controllers
{
    public class ClassController : BaseClassController<Product_Class>
    {
        public ClassController()
        {
            base.Title = "产品";
        }

        public override ActionResult List()
        {
            parId = RequestInt("parId");
            int id = RequestInt("id");
            var list = bll.GetChildrenList(parId, true).ToList();
            GetActStr(list);

            Product_Class model = null;
            if (id > 0)
                model = bll.First(id);

            ViewBag.title = Title;
            ViewBag.breadcrumb = bll.ShowClassPath(parId, "<li><a href='" + this.Path + "/list?parId={0}'>{1}</a></li>");
            ViewBag.list = list;
            ViewBag.id = id;
            ViewBag.parId = parId;
            ViewBag.model = model;
            ViewBag.path = this.Path;

            return View();
        }

        protected override void ActHandle(Product_Class model, int listCount, int i)
        {
            model.actStr = base.GetActHtml(listCount, i, model.Id, parId);
        }

        public override ActionResult Down()
        {
            int moveId = RequestInt("id");
            int step = RequestInt("step");
            parId = RequestInt("parId");
            if (step == 0)
            {
                return MessageBoxAndReturn("请选择下移数");
            }
            if (moveId == 0)
            {
                return MessageBoxAndReturn("请选择移动分类！");
            }
            if (bll.Down(moveId, step))
            {
                return RedirectToAction("list", ControllerName, new { area = this.AreaName, parId = parId });
            }
            else
            {
                return MessageBoxAndReturn("提交出错！");
            }
        }

        /// <summary>
        /// 上移分类
        /// </summary>
        /// <returns></returns>
        public override ActionResult Up()
        {
            int moveId = RequestInt("id");
            int step = RequestInt("step");
            parId = RequestInt("parId");
            if (step == 0)
            {
                return MessageBoxAndReturn("请选择上移数");
            }
            if (moveId == 0)
            {
                return MessageBoxAndReturn("请选择移动分类！");
            }
            if (bll.Up(moveId, step))
            {
                return RedirectToAction("list", ControllerName, new { area = this.AreaName, parId = parId });
            }
            else
            {
                return MessageBoxAndReturn("提交出错！");
            }
        }

        public override ActionResult Delete()
        {
            int id = RequestInt("id");
            if (id == 0)
                return MessageBoxAndReturn("访问出错！");
            parId = RequestInt("parId");

            if (bll.Delete(id))
                return MessageBoxAndJump("删除成功！", string.Format("{1}/list?parId={0}", parId, this.Path));
            else
                return MessageBoxAndJump("删除失败！", string.Format("{1}/list?parId={0}", parId, this.Path));
        }

        public ActionResult Edit()
        {
            Product_Class model = null;
            int id = RequestInt("id");
            int parId = RequestInt("parId");
            if (id > 0)
            {
                model = Bll.BllProduct_Class.First(o => o.Id == id);
                if (model == null) return LayerAlertErrorAndClose("分类不存在！");
                if (model.ParId != parId)
                    parId = model.ParId.Value;
            }
            ViewBag.parId = parId;
            return View(model);
        }

        [PermissionFilter("/product/class/edit")]
        [ValidateInput(false)]
        public override ActionResult Add()
        {
            string className = RequestString("className");
            if (className == "")
                return LayerAlertErrorAndReturn("请填写分类名！");
            string description = RequestString("description");
            string code = RequestString("code");
            bool belock = RequestBool("belock");

            parId = RequestInt("parId");

            if (Bll.BllProduct_Class.CheckCode(code, parId))
                return LayerAlertErrorAndReturn("编码重复！");

            Product_Class mod_ProClass = new Product_Class();
            mod_ProClass.ClassName = className;
            mod_ProClass.ParId = parId;
            int id = bll.Add(mod_ProClass);
            if (id > 0)
            {
                string firstPic = string.Empty;
                mod_ProClass.Id = id;
                mod_ProClass.Description = EscapeSpace(description);
                mod_ProClass.Pics = GetPics("pic", ref firstPic);
                mod_ProClass.FirstPic = firstPic;
                mod_ProClass.UrlPath = GetFormatUrl(className);
                mod_ProClass.Code = code;
                mod_ProClass.BeLock = !belock;

                Bll.BllProduct_Class.Update(mod_ProClass, o => o.Id == id);

                return LayerMsgSuccessAndRefreshPage("提交成功！");
            }
            else
            {
                return LayerAlertErrorAndReturn("提交出错！");
            }
        }

        [PermissionFilter("/product/class/edit")]
        [ValidateInput(false)]
        public override ActionResult Update()
        {
            int id = RequestInt("id");
            string className = RequestString("className");
            string description = RequestString("description");
            string code = RequestString("code");
            bool belock = RequestBool("belock");

            parId = RequestInt("parId");

            if (className == "")
            {
                return LayerAlertErrorAndReturn("请填写分类名！");
            }
            if (id == 0)
            {
                return LayerAlertErrorAndReturn("请选择编辑类！");
            }
            var mod_ProClass = bll.First(id);
            if (mod_ProClass == null)
            {
                return LayerAlertErrorAndReturn("没有获取到分类信息！");
            }
            else
            {
                if (mod_ProClass.Code != code && Bll.BllProduct_Class.CheckCode(code, parId))
                    return LayerAlertErrorAndReturn("编码重复！");

                string firstPic = string.Empty;
                mod_ProClass.ClassName = className;
                mod_ProClass.Description = EscapeSpace(description);
                mod_ProClass.Pics = GetPics("pic", ref firstPic);
                mod_ProClass.FirstPic = firstPic;
                mod_ProClass.UrlPath = GetFormatUrl(className);
                mod_ProClass.Code = code;
                mod_ProClass.BeLock = !belock;
                Bll.BllProduct_Class.Update(mod_ProClass, o => o.Id == id);
                return LayerMsgSuccessAndRefreshPage("修改成功！");
            }
        }
    }
}