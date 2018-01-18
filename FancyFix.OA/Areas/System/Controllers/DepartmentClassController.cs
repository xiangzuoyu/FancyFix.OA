using FancyFix.OA.Areas.Controllers;
using FancyFix.OA.Model;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace FancyFix.OA.Areas.System.Controllers
{
    public class DepartmentClassController : BaseClassController<Mng_DepartmentClass>
    {
        public DepartmentClassController()
        {
            base.Title = "部门";
        }

        public override ActionResult List()
        {
            parId = RequestInt("parId");
            int id = RequestInt("id");
            List<Mng_DepartmentClass> list = bll.GetChildrenList(parId, true).ToList();
            GetActStr(list);

            Mng_DepartmentClass model = null;
            if (id > 0)
                model = Bll.BllMng_DepartmentClass.First(o => o.Id == id);

            ViewBag.breadcrumb = bll.ShowClassPath(parId, "<li><a href='" + this.Path + "/list?parId={0}'>{1}</a></li>");
            ViewBag.list = list;
            ViewBag.id = id;
            ViewBag.parId = parId;
            ViewBag.model = model;
            ViewBag.path = this.Path;
            return View();
        }

        protected override void ActHandle(Mng_DepartmentClass model, int listCount, int i)
        {
            model.actStr = base.GetActHtml(listCount, i, model.Id, parId);
        }

        public override ActionResult Add()
        {
            string className = RequestString("className");
            if (className == "")
                return MessageBoxAndReturn("请填写分类名！");
            parId = RequestInt("parId");
            string domain = RequestString("domain");

            Mng_DepartmentClass mod_ManageClass = new Mng_DepartmentClass();
            mod_ManageClass.ClassName = className;
            mod_ManageClass.ParId = parId;
            int id = bll.Add(mod_ManageClass);
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
            string domain = RequestString("domain");

            if (className == "")
            {
                return MessageBoxAndReturn("请填写分类名！");
            }
            if (id == 0)
            {
                return MessageBoxAndReturn("请选择编辑类！");
            }
            var mod_ManageClass = bll.First(id);
            if (mod_ManageClass == null)
            {
                return MessageBoxAndReturn("没有获取到分类信息！");
            }
            else
            {
                mod_ManageClass.ClassName = className;
                Bll.BllMng_DepartmentClass.Update(mod_ManageClass, o => o.Id == id);
                return MessageBoxAndJump("修改成功！", string.Format("{1}/list?parId={0}", parId, this.Path));
            }
        }
    }
}