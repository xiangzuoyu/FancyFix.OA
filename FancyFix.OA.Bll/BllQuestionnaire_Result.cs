using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllQuestionnaire_Result : ServiceBase<Questionnaire_Answerer>
    {
        public static BllQuestionnaire_Result Instance()
        {
            return new BllQuestionnaire_Result();
        }

        public static List<Questionnaire_Result> GetList(int answererId)
        {
            var where = new Where<Questionnaire_Result>();
            if (answererId > 0) where.And(o => o.AnswererId == answererId);
            var p = Db.Context.From<Questionnaire_Result>()
                 .Where(where);

            return p.OrderBy(o => o.Id).ToList();
        }
    }
}
