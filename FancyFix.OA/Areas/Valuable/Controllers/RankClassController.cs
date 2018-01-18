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
    public class RankClassController : BaseClassController<Rank_Class>
    {
        public RankClassController()
        {
            base.Title = "评分等级";
        }

        public override ActionResult List()
        {
            parId = RequestInt("parId");
            int id = RequestInt("id");
            List<Rank_Class> list = bll.GetChildrenList(parId, true).ToList();
            GetActStr(list);

            Rank_Class model = null;
            if (id > 0)
                model = Bll.BllRank_Class.First(o => o.Id == id);

            ViewBag.breadcrumb = bll.ShowClassPath(parId, "<li><a href='" + this.Path + "/list?parId={0}'>{1}</a></li>");
            ViewBag.list = list;
            ViewBag.id = id;
            ViewBag.parId = parId;
            ViewBag.model = model;
            ViewBag.path = this.Path;
            ViewBag.title = this.Title;

            return View();
        }
   
        protected override void ActHandle(Rank_Class model, int listCount, int i)
        {
            model.actStr = GetActHtmlNoNextPrev(listCount, i, model.Id, parId);
        }

        public override ActionResult Add()
        {
            string className = RequestString("className");
            int score = RequestInt("score");
            int samplenum = RequestInt("samplenum");
            parId = RequestInt("parId");

            if (className == "")
                return MessageBoxAndReturn("请填写分类名！");
          
            Rank_Class mod_RankClass = new Rank_Class();
            mod_RankClass.ClassName = className;
            mod_RankClass.ParId = parId;
            int id = bll.Add(mod_RankClass);
            if (id > 0)
            {
                mod_RankClass.Score = score;
                mod_RankClass.SampleNum = samplenum;
                Bll.BllRank_Class.Update(mod_RankClass, o => o.Id == id);
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
            int score = RequestInt("score");
            int samplenum = RequestInt("samplenum");
            parId = RequestInt("parId");

            if (className == "")
                return MessageBoxAndReturn("请填写分类名！");
            if (id == 0)
                return MessageBoxAndReturn("请选择编辑类！");

            var mod_RankClass = bll.First(id);
            if (mod_RankClass == null)
            {
                return MessageBoxAndReturn("没有获取到分类信息！");
            }
            else
            {
                mod_RankClass.ClassName = className;
                mod_RankClass.Score = score;
                mod_RankClass.SampleNum = samplenum;
                Bll.BllRank_Class.Update(mod_RankClass, o => o.Id == id);
                return MessageBoxAndJump("修改成功！", string.Format("{1}/list?parId={0}", parId, this.Path));
            }
        }
    }
}