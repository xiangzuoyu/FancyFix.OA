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
                 .Select((a) => new { a.Id, a.Name, a.SubjectId, a.Tel, a.Email, a.Company, a.WxId, a.WxName, a.Score, a.AddTime, a.StartTime, a.CorrectNum, a.DISC, a.IsDISC, a.Job, a.Department })
                 .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        /// <summary>
        /// 添加问卷回答记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool Add(Questionnaire_Answerer model, List<Questionnaire_Result> result)
        {
            using (var trans = Db.Context.BeginTransaction())
            {
                int id = 0;
                id = trans.Insert(model);
                if (id == 0)
                {
                    trans.Rollback();
                    return false;
                }
                //结果记录绑定id
                foreach (var item in result)
                    item.AnswererId = id;
                id = trans.Insert(result);
                if (id == 0)
                {
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
                return true;
            }
        }

    }
}
