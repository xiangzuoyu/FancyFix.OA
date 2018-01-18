using FancyFix.Core;
using FancyFix.OA.Areas.Controllers;
using FancyFix.OA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.System.Controllers
{
    public class MenuClassController : BaseClassController<Mng_MenuClass>
    {
        public MenuClassController()
        {
            base.Title = "菜单";
        }

        public override ActionResult List()
        {
            parId = RequestInt("parId");
            int id = RequestInt("id");
            var list = bll.GetChildrenList(parId, true).ToList();
            GetActStr(list);

            Mng_MenuClass model = null;
            if (id > 0)
                model = Bll.BllSys_MenuClass.First(o => o.Id == id);

            ViewBag.breadcrumb = bll.ShowClassPath(parId, "<li><a href='" + this.Path + "/list?parId={0}'>{1}</a></li>");
            ViewBag.list = list;
            ViewBag.id = id;
            ViewBag.parId = parId;
            ViewBag.model = model;
            ViewBag.path = this.Path;

            return View();
        }

        protected override void ActHandle(Mng_MenuClass model, int listCount, int i)
        {
            model.actStr = base.GetActHtml(listCount, i, model.Id, parId);
        }

        /// <summary>
        /// 增加一个分类
        /// </summary>
        /// <returns></returns>
        public override ActionResult Add()
        {
            string className = RequestString("className");
            string url = RequestString("url");
            parId = RequestInt("parId");
            if (className == "")
                return MessageBoxAndReturn("请填写分类名！");
            if (!IsSuperAdmin)
                return MessageBoxAndReturn("操作失败，需要超级管理员权限！");

            Mng_MenuClass mod_ManageClass = new Mng_MenuClass();
            mod_ManageClass.ClassName = className;
            mod_ManageClass.ParId = parId;
            mod_ManageClass.Url = url;

            if (Bll.BllSys_MenuClass.Add(mod_ManageClass))
            {
                return MessageBoxAndJump("提交成功！", string.Format("{1}/list?parId={0}", parId, this.Path));
            }
            else
            {
                return MessageBoxAndReturn("提交出错！");
            }
        }

        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <returns></returns>
        public override ActionResult Update()
        {
            int id = RequestInt("id");
            string className = RequestString("className");
            parId = RequestInt("parId");
            string url = RequestString("url");

            if (className == "")
                return MessageBoxAndReturn("请填写分类名！");
            if (id == 0)
                return MessageBoxAndReturn("请选择编辑类！");
            if (!IsSuperAdmin)
                return MessageBoxAndReturn("操作失败，需要超级管理员权限！");

            Mng_MenuClass mod_ManageClass = Bll.BllSys_MenuClass.First(o => o.Id == id);
            if (mod_ManageClass == null)
            {
                return MessageBoxAndReturn("没有获取到分类信息！");
            }
            else
            {
                mod_ManageClass.ClassName = className;
                mod_ManageClass.Url = url;
                Bll.BllSys_MenuClass.Update(mod_ManageClass, o => o.Id == id);
                return MessageBoxAndJump("修改成功！", string.Format("{1}/list?parId={0}", parId, this.Path));
            }
        }

        public override ActionResult Delete()
        {
            int id = RequestInt("id");
            if (id == 0)
                return MessageBoxAndReturn("访问出错！");
            parId = RequestInt("parId");

            //菜单删除需要超管权限
            if (!IsSuperAdmin)
                return MessageBoxAndReturn("操作失败，需要超级管理员权限！");

            if (bll.Delete(id))
                return MessageBoxAndJump("删除成功！", string.Format("{1}/list?parId={0}", parId, this.Path));
            else
                return MessageBoxAndJump("删除失败！", string.Format("{1}/list?parId={0}", parId, this.Path));
        }
    }
}