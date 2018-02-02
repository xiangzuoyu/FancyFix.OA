using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllQuestionnaire_Subject : ServiceBase<Questionnaire_Subject>
    {
        public static BllQuestionnaire_Subject Instance()
        {
            return new BllQuestionnaire_Subject();
        }

        public static List<Questionnaire_Subject> PageList(int page, int pageSize, out long records)
        {
            var where = new Where<Questionnaire_Subject>();

            var p = Db.Context.From<Questionnaire_Subject>()
                 .Select((a) => new { a.Id, a.Title, a.Timelimit, a.Remark, a.Score, a.Sequence })
                 .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderBy(o => o.Sequence).ToList();
        }
      
    }
}
