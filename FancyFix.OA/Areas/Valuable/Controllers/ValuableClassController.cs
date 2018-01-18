using FancyFix.OA.Areas.Controllers;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Valuable.Controllers
{
    public class ValuableClassController : BaseClassController<Valuable_Class>
    {
        public ValuableClassController()
        {
            base.Title = "价值观";
        }

        public override ActionResult List()
        {
            parId = RequestInt("parId");
            int id = RequestInt("id");
            List<Valuable_Class> list = bll.GetChildrenList(parId, true).ToList();
            GetActStr(list);

            Valuable_Class model = null;
            if (id > 0)
                model = Bll.BllValuable_Class.First(o => o.Id == id);

            ViewBag.breadcrumb = bll.ShowClassPath(parId, "<li><a href='" + this.Path + "/list?parId={0}'>{1}</a></li>");
            ViewBag.list = list;
            ViewBag.id = id;
            ViewBag.parId = parId;
            ViewBag.model = model;
            ViewBag.path = this.Path;
            ViewBag.title = this.Title;

            return View();
        }

        protected override void ActHandle(Valuable_Class model, int listCount, int i)
        {
            model.actStr = GetActHtmlNoNextPrev(listCount, i, model.Id, parId);
        }

        public override ActionResult Add()
        {
            string className = RequestString("className");
            if (className == "")
                return MessageBoxAndReturn("请填写分类名！");
            parId = RequestInt("parId");

            Valuable_Class mod_ValuableClass = new Valuable_Class();
            mod_ValuableClass.ClassName = className;
            mod_ValuableClass.ParId = parId;
            int id = bll.Add(mod_ValuableClass);
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
            var mod_ValuableClass = bll.First(id);
            if (mod_ValuableClass == null)
            {
                return MessageBoxAndReturn("没有获取到分类信息！");
            }
            else
            {
                mod_ValuableClass.ClassName = className;
                Bll.BllValuable_Class.Update(mod_ValuableClass, o => o.Id == id);
                return MessageBoxAndJump("修改成功！", string.Format("{1}/list?parId={0}", parId, this.Path));
            }
        }
    }
}