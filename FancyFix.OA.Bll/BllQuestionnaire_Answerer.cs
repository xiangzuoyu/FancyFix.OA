using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllQuestionnaire_Answerer : ServiceBase<Questionnaire_Answerer>
    {
        public static BllQuestionnaire_Answerer Instance()
        {
            return new BllQuestionnaire_Answerer();
        }

        public static List<Questionnaire_Answerer> PageList(int subjectId, bool isDISC, int page, int pageSize, out long records)
        {
            var where = new Where<Questionnaire_Answerer>();
            where.And(o => o.IsDISC == isDISC);
            if (subjectId > 0) where.And(o => o.SubjectId == subjectId);
            var p = Db.Context.From<Questionnaire_Answerer>()
                 .Select((a) => new { a.Id, a.Name, a.SubjectId, a.Tel, a.Email, a.Company, a.WxId, a.WxName, a.Score, a.AddTime, a.StartTime, a.CorrectNum, a.DISC, a.IsDISC })
                 .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

    }
}
