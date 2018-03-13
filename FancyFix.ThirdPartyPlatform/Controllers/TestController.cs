using FancyFix.ThirdPartyPlatform.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tools.Utility;

namespace FancyFix.ThirdPartyPlatform.Controllers
{
    public class TestController : BaseController
    {
        // GET: Disc
        public ActionResult Index()
        {
            var list = OA.Bll.BllQuestionnaire_Subject.GetList();
            ViewBag.list = list;
            return View();
        }

        public ActionResult Info(int id)
        {
            var model = OA.Bll.BllQuestionnaire_Subject.First(o => o.Id == id);
            if (model == null) return MessageBoxAndReturn("问卷不存在！");
            var list = OA.Bll.BllQuestionnaire_Info.GetList(id);
            ViewBag.list = list;
            return View(model);
        }

        [HttpPost]
        public JsonResult Save(List<int> id)
        {
            if (id == null || id.Count == 0)
                return Json(new { result = 0, Data = "请选择一个选项！" });

            int subjectId = RequestInt("subjectId");
            if (subjectId == 0) return Json(new { result = 0, Data = "问卷不存在！" });

            var infolist = OA.Bll.BllQuestionnaire_Info.GetList(subjectId);
            if (infolist == null || infolist.Count == 0) return Json(new { result = 0, Data = "该问卷下无试题！" });

            //验证格式
            string email = RequestString("email");
            string tel = RequestString("tel");
            if (!string.IsNullOrEmpty(email) && !Tools.Common.StringCheck.IsEmail(email))
                return Json(new { result = 0, Data = "请输入正确的邮箱地址！" });
            if (!Tools.Common.StringCheck.IsMobile(tel))
                return Json(new { result = 0, Data = "请输入正确的手机号码！" });

            int score = 0; //总分
            int correctNum = 0;//答对个数

            List<OA.Model.Questionnaire_Result> result = new List<OA.Model.Questionnaire_Result>();

            //统计结果
            foreach (int tid in id)
            {
                var model = infolist.Find(o => o.Id == tid);
                if (model != null)
                {
                    //判断类型和答案
                    bool isRight = false;
                    string answer = RequestString(tid.ToString());
                    if (model.Type == (byte)Tools.Enums.Site.InputType.CheckBox)
                    {
                        //多选题，只要其中一个对，就算得分
                        var o_answer = ("," + model.Answer.ToLower() + ",");
                        var answers = answer.ToLower().Split(',');
                        foreach (var ans in answers)
                        {
                            if (o_answer.Contains(ans))
                            {
                                isRight = true;
                                break;
                            }
                        }
                    }
                    else if (model.Type == (byte)Tools.Enums.Site.InputType.Radio)
                    {
                        isRight = model.Answer.ToLower() == answer.ToLower();
                    }
                    else if (model.Type == (byte)Tools.Enums.Site.InputType.Text)
                    {
                        isRight = model.Answer.Trim().ToLower() == answer.Trim().ToLower();
                    }
                    //答对统计数据
                    if (isRight)
                    {
                        score += model.Score.Value;
                        correctNum++;
                    }
                    //添加回答记录
                    result.Add(new OA.Model.Questionnaire_Result()
                    {
                        Question = model.Title,
                        Answer = model.Answer,
                        IsRight = isRight,
                        Result = answer,
                        Score = isRight ? model.Score.Value : 0,
                        SubjectId = subjectId
                    });
                }
            }

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

                bool isOk = OA.Bll.BllQuestionnaire_Answerer.Add(new OA.Model.Questionnaire_Answerer()
                {
                    Name = RequestString("name"),
                    Email = email,
                    Tel = tel,
                    Company = RequestString("company"),
                    StartTime = RequestString("starttime").ToDateTime(),
                    Job = RequestString("job"),
                    Department = RequestString("department"),
                    IsDISC = false,
                    DISC = "",
                    CorrectNum = correctNum,
                    Score = score,
                    SubjectId = subjectId,
                    AddTime = DateTime.Now,
                    IP = ip,
                    UserAgent = Tools.Utility.CheckClient.GetUserAgent(),
                    IsMobile = Tools.Utility.CheckClient.IsMobileDevice(),
                    Country = country,
                    Area = area
                }, result);

                if (isOk)
                    return Json(new { result = 1, data = new Result() { subjectid = subjectId, score = score, correctnum = correctNum, allnum = infolist.Count } });
                else
                    return Json(new { result = 0, data = "提交失败，请联系公司相关负责人！" });
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(DiscController), ex, 0, "");
                return Json(new { result = 0, data = "提交失败，请联系公司相关负责人！" });
            }
        }

        public class Result
        {
            public int subjectid { get; set; }
            public int score { get; set; }
            public int correctnum { get; set; }
            public int allnum { get; set; }
        }
    }
}