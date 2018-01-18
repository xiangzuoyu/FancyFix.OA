using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Valuable.Controllers
{
    public class ProcessController : BaseAdminController
    {
        public ActionResult Index(int id = 0)
        {
            var list = Bll.BllConfig_Process.Query(o => o.Type == (int)Bll.BllConfig_Process.ProcessType.Valuable, o => o.Year, "desc");

            Config_Process model = null;
            if (id > 0)
                model = Bll.BllConfig_Process.First(o => o.Id == id);

            ViewBag.model = model;
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            if (!IsSuperAdmin)
                return MessageBoxAndReturn("删除请超级联系管理员！");
            if (id == 0)
                return MessageBoxAndReturn("访问出错！");

            if (Bll.BllConfig_Process.Delete(o => o.Id == id) > 0)
                return MessageBoxAndJump("删除成功！", "/valuable/process");
            else
                return MessageBoxAndJump("删除失败！", "/valuable/process");
        }

        [HttpPost]
        public virtual ActionResult Add(int year, List<int> month)
        {
            if (year < 2000 || year > 3000)
                return MessageBoxAndReturn("请填写正确的年份！");

            if (month == null || month.Count <= 0)
                return MessageBoxAndReturn("请添加进程月份！");

            if (Bll.BllConfig_Process.Count(o => o.Year == year && o.Type == (int)Bll.BllConfig_Process.ProcessType.Valuable) > 0)
                return MessageBoxAndReturn("该年份已存在，请更换后重新添加！");

            Config_Process model = new Config_Process();
            model.Process = string.Join(",", month);
            model.Year = year;
            model.Type = (int)Bll.BllConfig_Process.ProcessType.Valuable;

            if (Bll.BllConfig_Process.Insert(model) > 0)
                return MessageBoxAndJump("添加成功！", "/valuable/process");
            else
                return MessageBoxAndJump("删除失败！", "/valuable/process");
        }

        [HttpPost]
        public virtual ActionResult Update(int id, int year, List<int> month)
        {
            if (id == 0)
                return MessageBoxAndReturn("访问出错！");

            if (year < 2000 || year > 3000)
                return MessageBoxAndReturn("请填写正确的年份！");

            if (month == null || month.Count <= 0)
                return MessageBoxAndReturn("请添加进程月份！");

            Config_Process model = Bll.BllConfig_Process.First(o => o.Id == id);
            if (model == null)
                return MessageBoxAndReturn("记录不存在！");

            model.Process = string.Join(",", month);
            model.Year = year;
            model.Type = (int)Bll.BllConfig_Process.ProcessType.Valuable;

            if (Bll.BllConfig_Process.Update(model, o => o.Id == id) > 0)
                return MessageBoxAndJump("添加成功！", "/valuable/process");
            else
                return MessageBoxAndJump("删除失败！", "/valuable/process");
        }
    }
}