using FancyFix.OA.Areas.Controllers;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Point.Controllers
{
    public class PointClassController : BaseClassController<Point_Class>
    {
        public PointClassController()
        {
            base.Title = "积分库";
        }

        public override ActionResult List()
        {
            parId = RequestInt("parId");
            int id = RequestInt("id");
            List<Point_Class> list = bll.GetChildrenList(parId, true).ToList();
            GetActStr(list);

            Point_Class model = null;
            if (id > 0)
                model = Bll.BllPoint_Class.First(o => o.Id == id);

            ViewBag.breadcrumb = bll.ShowClassPath(parId, "<li><a href='" + this.Path + "/list?parId={0}'>{1}</a></li>");
            ViewBag.list = list;
            ViewBag.id = id;
            ViewBag.parId = parId;
            ViewBag.model = model;
            ViewBag.path = this.Path;
            ViewBag.title = this.Title;

            return View();
        }

        protected override void ActHandle(Point_Class model, int listCount, int i)
        {
            model.actStr = GetActHtmlNoNextPrev(listCount, i, model.Id, parId);
        }

        public override ActionResult Add()
        {
            string className = RequestString("className");
            if (className == "")
                return MessageBoxAndReturn("请填写分类名！");
            parId = RequestInt("parId");

            Point_Class mod_PointClass = new Point_Class();
            mod_PointClass.ClassName = className;
            mod_PointClass.ParId = parId;
            int id = bll.Add(mod_PointClass);
            if (id > 0)
            {
                return MessageBoxAndJump("提交成功！", string.Format("{1}/list?parId={0}", parId, this.Path));
            }
            else
            {
                return MessageBoxAndReturn("提交出错！");
            }
        }

        public override ActionResult Update()
        {
            int id = RequestInt("id");
            string className = RequestString("className");
            parId = RequestInt("parId");

            if (className == "")
            {
                return MessageBoxAndReturn("请填写分类名！");
            }
            if (id == 0)
            {
                return MessageBoxAndReturn("请选择编辑类！");
            }
            var mod_PointClass = bll.First(id);
            if (mod_PointClass == null)
            {
                return MessageBoxAndReturn("没有获取到分类信息！");
            }
            else
            {
                mod_PointClass.ClassName = className;
                Bll.BllPoint_Class.Update(mod_PointClass, o => o.Id == id);
                return MessageBoxAndJump("修改成功！", string.Format("{1}/list?parId={0}", parId, this.Path));
            }
        }
    }
}