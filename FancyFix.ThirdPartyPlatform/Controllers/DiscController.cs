using FancyFix.ThirdPartyPlatform.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tools.Utility;

namespace FancyFix.ThirdPartyPlatform.Controllers
{
    public class DiscController : BaseController
    {
        // GET: Disc
        public ActionResult Index()
        {
            var list = OA.Bll.BllQuestionnaire_DISC.GetList();
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public JsonResult Save(List<int> id)
        {
            if (id == null || id.Count == 0)
                return Json(new { result = 0, Data = "请选择一个选项！" });

            //验证格式
            string email = RequestString("email");
            string tel = RequestString("tel");
            if (!string.IsNullOrEmpty(email) && !Tools.Common.StringCheck.IsEmail(email))
                return Json(new { result = 0, Data = "请输入正确的邮箱地址！" });
            if (!Tools.Common.StringCheck.IsMobile(tel))
                return Json(new { result = 0, Data = "请输入正确的手机号码！" });

            //所有结果加入列表
            List<string> disclist = new List<string>();
            foreach (var i in id)
                disclist.Add(RequestString(i.ToString()));

            List<Tools.Json.DISC> entity = new List<Tools.Json.DISC>();
            entity.Add(new Tools.Json.DISC() { n = "D", v = disclist.FindAll(o => o == "D").Count.ToString() });
            entity.Add(new Tools.Json.DISC() { n = "I", v = disclist.FindAll(o => o == "I").Count.ToString() });
            entity.Add(new Tools.Json.DISC() { n = "S", v = disclist.FindAll(o => o == "S").Count.ToString() });
            entity.Add(new Tools.Json.DISC() { n = "C", v = disclist.FindAll(o => o == "C").Count.ToString() });

            try
            {
                string ip = Tools.Utility.CheckClient.GetIP();
                string country = string.Empty;
                string area = string.Empty;
                if (ip != "" && IPDataPath != "")
                {
                    var ipSearch = new IPSearcher(IPDataPath).GetIPLocation(ip);
                    country = ipSearch.country;
                    area = ipSearch.area;
                }

                int count = OA.Bll.BllQuestionnaire_Answerer.Insert(new OA.Model.Questionnaire_Answerer()
                {
                    Name = RequestString("name"),
                    Email = email,
                    Tel = tel,
                    Company = RequestString("company"),
                    StartTime = RequestString("starttime").ToDateTime(),
                    Job = RequestString("job"),
                    Department = RequestString("department"),
                    IsDISC = true,
                    DISC = Tools.Tool.JsonHelper.Serialize(entity),
                    CorrectNum = 0,
                    Score = 0,
                    SubjectId = 0,
                    AddTime = DateTime.Now,
                    IP = ip,
                    UserAgent = Tools.Utility.CheckClient.GetUserAgent(),
                    IsMobile = Tools.Utility.CheckClient.IsMobileDevice(),
                    Country = country,
                    Area = area
                });

                if (count > 0)
                    return Json(new { result = 1, data = entity });
                else
                    return Json(new { result = 0, data = "提交失败，请联系公司相关负责人！" });
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(DiscController), ex, 0, "");
                return Json(new { result = 0, data = "提交失败，请联系公司相关负责人！" });
            }
        }
    }
}