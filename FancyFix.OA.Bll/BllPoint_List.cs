using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllPoint_List : ServiceBase<Point_List>
    {
        public static BllPoint_List Instance()
        {
            return new BllPoint_List();
        }

        public static IEnumerable<Point_List> PageList(int classid, int page, int pageSize, out long records)
        {
            var where = new Where<Point_List>();

            if (classid > 0)
                where.And(o => o.ClassId == classid);

            var p = Db.Context.From<Point_List>()
                 .Select((a) => new { a.Id, a.ClassId, a.ClassName, a.PointScore, a.ScoreRemark, a.PointName, a.CountTime, a.CountBySelf, a.BeLock, a.Remark })
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
            var model = FirstSelect(o => o.Id == id, o => new { o.Id, o.BeLock });
            if (model == null && model.Id == 0) return false;

            model.BeLock = !model.BeLock;
            return Update(model, o => o.Id == model.Id) > 0;
        }

        /// <summary>
        /// 设置申请人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetSelf(int id)
        {
            var model = FirstSelect(o => o.Id == id, o => new { o.Id, o.CountBySelf });
            if (model == null && model.Id == 0) return false;

            model.CountBySelf = !model.CountBySelf;
            return Update(model, o => o.Id == model.Id) > 0;
        }
    }
}
