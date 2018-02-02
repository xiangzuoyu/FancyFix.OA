using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllQuestionnaire_DISC : ServiceBase<Questionnaire_DISC>
    {
        public static BllQuestionnaire_DISC Instance()
        {
            return new BllQuestionnaire_DISC();
        }

        public static List<Questionnaire_DISC> PageList(int page, int pageSize, out long records)
        {
            var where = new Where<Questionnaire_DISC>();
            var p = Db.Context.From<Questionnaire_DISC>()
                 .Select((a) => new { a.Id, a.D, a.I, a.S, a.C, a.DISC, a.IsShow, a.Sequence })
                 .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderBy(o => o.Sequence).ToList();
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static bool SetShow(int id)
        {
            var model = FirstSelect(o => o.Id == id, o => new { o.Id, o.IsShow });
            if (model == null && model.Id == 0) return false;

            model.IsShow = !model.IsShow;
            return Update(model, o => o.Id == model.Id) > 0;
        }

        public static List<Questionnaire_DISC> GetList()
        {
            var where = new Where<Questionnaire_DISC>();
            where.And(o => o.IsShow == true);
            var p = Db.Context.From<Questionnaire_DISC>()
                 .Select((a) => new { a.Id, a.DISC })
                 .Where(where);

            return p.OrderBy(o => o.Sequence).ToList();
        }

    }
}
