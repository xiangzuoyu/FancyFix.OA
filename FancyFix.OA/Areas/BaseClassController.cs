using Dos.ORM;
using FancyFix.Core;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Controllers
{
    //通用分类列表-基类
    public abstract class BaseClassController<T> : Base.BaseAdminController where T : Entity
    {
        protected Bll.BllSys_Class<T> bll = Bll.BllSys_Class<T>.Instance();
        protected string classHtml = string.Empty;
        protected int parId = 0;

        /// <summary>
        /// 页面标题：{title}分类管理
        /// </summary>
        protected string Title { get; set; }

        protected string AreaName
        {
            get { return this.ControllerContext.RouteData.DataTokens["area"].ToString().ToLower(); }
        }
        protected string ControllerName
        {
            get { return this.ControllerContext.RouteData.Values["controller"].ToString().ToLower(); }
        }
        protected string Path
        {
            get { return (string.IsNullOrEmpty(this.AreaName) ? "" : "/" + this.AreaName) + "/" + this.ControllerName; }
        }

        //分类列表
        public virtual ActionResult List()
        {
            if (parId == 0)
                parId = RequestInt("parId");
            int id = RequestInt("id");
            classHtml = bll.ShowClass(0, 0, true);
            var list = bll.GetChildrenList(parId, true).ToList();
            GetActStr(list);

            Sys_Class model = null;
            if (id > 0)
                model = bll.GetModel(id);

            ViewBag.title = Title;
            ViewBag.breadcrumb = bll.ShowClassPath(parId, "<li><a href='" + this.Path + "/list?parId={0}'>{1}</a></li>");
            ViewBag.list = list;
            ViewBag.id = id;
            ViewBag.parId = parId;
            ViewBag.model = model;
            ViewBag.classHtml = classHtml;
            ViewBag.path = this.Path;

            //此方法不重写，则公用一个View
            return View("ClassList");
        }

        /// <summary>
        /// 获取排序Html
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected string GetActStr(List<T> list)
        {
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ActHandle(list[i], list.Count, i);
                }
            }
            return "";
        }

        /// <summary>
        /// 排序Html，循环对象处理
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listCount"></param>
        /// <param name="i"></param>
        protected abstract void ActHandle(T model, int listCount, int i);

        /// <summary>
        /// 获取排序Html
        /// </summary>
        /// <param name="listCount">列表长度</param>
        /// <param name="i">当前索引</param>
        /// <param name="id">当前对象id</param>
        /// <param name="parId">当前对象parId</param>
        protected virtual string GetActHtml(int listCount, int i, int id, int parId)
        {
            StringBuilder actStr = new StringBuilder();
            actStr.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"250\"><tr><form name=\"upform\"><td width=\"70\">");
            if (i > 0)
            {
                actStr.Append("<select name=\"step\" > ");
                for (int j = 1; j <= i; j++)
                {
                    actStr.Append("<option value=\"" + j + "\">" + j + "</option>");
                }
                actStr.Append("</select> <input type=\"button\" name=\"Submit2\" value=\"↑\" class=\"btn btn-default\" onclick=\"window.location.href='" + this.Path + "/up?parId=" + parId + "&id=" + id + "&step='+this.form.step.value\"> ");
            }

            actStr.Append("</td></form><form name=\"downform\"><td  width=\"70\">");

            if (i < listCount - 1)
            {
                actStr.Append("<select name=\"step\" > ");
                for (int j = 1; j <= listCount - i - 1; j++)
                {
                    actStr.Append("<option value=\"" + j + "\">" + j + "</option>");
                }
                actStr.Append("</select> <input type=\"button\" name=\"Submit2\" value=\"↓\" class=\"btn btn-default\" onclick=\"window.location.href='" + this.Path + "/down?parId=" + parId + "&id=" + id + "&step='+this.form.step.value\"> ");
            }
            actStr.Append("</td></form><td  width=\"30\">");
            if (parId > 0)
            {
                actStr.Append("<input type=\"button\" name=\"Submit2\" value=\" ↖ \" title=\"升一级\" class=\"btn btn-default\" onclick=\"window.location.href='" + this.Path + "/prev?parId=" + parId + "&id=" + id + "';\">");
            }
            actStr.Append("</td><td  width=\"30\">");
            if (listCount > 1 && i > 0)
            {
                actStr.Append("<input type=\"button\" name=\"Submit2\"  title=\"降一级\" value=\" ↘ \" class=\"btn btn-default\" onclick=\"window.location.href='" + this.Path + "/next?parId=" + parId + "&id=" + id + "';\">");
            }
            actStr.Append("</td></tr></table>");
            return actStr.ToString();
        }

        /// <summary>
        /// 获取排序Html(不包含父子级排序)
        /// </summary>
        /// <param name="listCount"></param>
        /// <param name="i"></param>
        /// <param name="id"></param>
        /// <param name="parId"></param>
        /// <returns></returns>
        protected virtual string GetActHtmlNoNextPrev(int listCount, int i, int id, int parId)
        {
            StringBuilder actStr = new StringBuilder();
            actStr.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"250\"><tr><form name=\"upform\"><td width=\"70\">");
            if (i > 0)
            {
                actStr.Append("<select name=\"step\" > ");
                for (int j = 1; j <= i; j++)
                {
                    actStr.Append("<option value=\"" + j + "\">" + j + "</option>");
                }
                actStr.Append("</select> <input type=\"button\" name=\"Submit2\" value=\"↑\" class=\"btn btn-default\" onclick=\"window.location.href='" + this.Path + "/up?parId=" + parId + "&id=" + id + "&step='+this.form.step.value\"> ");
            }

            actStr.Append("</td></form><form name=\"downform\"><td  width=\"70\">");

            if (i < listCount - 1)
            {
                actStr.Append("<select name=\"step\" > ");
                for (int j = 1; j <= listCount - i - 1; j++)
                {
                    actStr.Append("<option value=\"" + j + "\">" + j + "</option>");
                }
                actStr.Append("</select> <input type=\"button\" name=\"Submit2\" value=\"↓\" class=\"btn btn-default\" onclick=\"window.location.href='" + this.Path + "/down?parId=" + parId + "&id=" + id + "&step='+this.form.step.value\"> ");
            }
            actStr.Append("</td></form></tr></table>");
            return actStr.ToString();
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <returns></returns>
        public abstract ActionResult Add();

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <returns></returns>
        public abstract ActionResult Update();

        /// <summary>
        /// 下移分类
        /// </summary>
        public virtual ActionResult Down()
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
        public virtual ActionResult Up()
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

        /// <summary>
        /// 下移一级分类
        /// </summary>
        public virtual ActionResult Next()
        {
            int moveId = RequestInt("id");
            parId = RequestInt("parId");
            if (moveId == 0)
            {
                return MessageBoxAndReturn("请选择移动分类！");
            }
            if (bll.Next(moveId))
            {
                return RedirectToAction("list", ControllerName, new { area = this.AreaName, parId = parId });
            }
            else
            {
                return MessageBoxAndReturn("提交出错！");
            }
        }

        /// <summary>
        /// 上移一级分类
        /// </summary>
        public virtual ActionResult Prev()
        {
            int moveId = RequestInt("id");
            parId = RequestInt("parId");
            if (moveId == 0)
            {
                return MessageBoxAndReturn("请选择移动分类！");
            }

            if (bll.Prev(moveId))
            {
                return RedirectToAction("list", ControllerName, new { area = this.AreaName, parId = parId });
            }
            else
            {
                return MessageBoxAndReturn("提交出错！");
            }
        }

        /// <summary>
        /// 是否显示（是否权限）
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult SetBeLock()
        {
            int id = RequestInt("id");
            parId = RequestInt("parId");
            if (id == 0)
                return MessageBoxAndReturn("访问出错！");
            bll.SetBeLock(id);
            return RedirectToAction("list", ControllerName, new { area = this.AreaName, parId = parId });
        }

        /// <summary>
        /// 删除一个分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Delete()
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
    }
}