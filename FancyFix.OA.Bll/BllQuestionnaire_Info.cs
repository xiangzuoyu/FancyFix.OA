using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllQuestionnaire_Info : ServiceBase<Questionnaire_Info>
    {
        public static BllQuestionnaire_Info Instance()
        {
            return new BllQuestionnaire_Info();
        }

        public static List<Questionnaire_Info> PageList(int subjectId, int page, int pageSize, out long records)
        {
            var where = new Where<Questionnaire_Info>();
            if (subjectId > 0) where.And(o => o.SubjectId == subjectId);
            var p = Db.Context.From<Questionnaire_Info>()
                 .Select((a) => new { a.Id, a.Title, a.SubjectId, a.Options, a.Type, a.Answer, a.Remark, a.Score, a.Sequence, a.IsShow })
                 .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderBy(o => o.Sequence).ToList();
        }

        public static List<Questionnaire_Info> GetList(int subjectId)
        {
            var where = new Where<Questionnaire_Info>();
            if (subjectId > 0) where.And(o => o.SubjectId == subjectId);
            var p = Db.Context.From<Questionnaire_Info>()
                 .Select((a) => new { a.Id, a.Title, a.SubjectId, a.Options, a.Type, a.Answer, a.Remark, a.Score, a.Sequence, a.IsShow })
                 .Where(where);

            return p.OrderBy(o => o.Sequence).ToList();
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetShow(int id)
        {
            var model = FirstSelect(o => o.Id == id, o => new { o.Id, o.IsShow });
            if (model == null && model.Id == 0) return false;

            model.IsShow = !model.IsShow;
            return Update(model, o => o.Id == model.Id) > 0;
        }

    }
}
