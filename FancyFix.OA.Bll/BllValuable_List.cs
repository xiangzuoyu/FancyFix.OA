using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllValuable_List : ServiceBase<Valuable_List>
    {
        public static BllValuable_List Instance()
        {
            return new BllValuable_List();
        }

        public static IEnumerable<Valuable_List> PageList(int page, int pageSize, out long records)
        {
            var where = new Where<Valuable_List>();

            var p = Db.Context.From<Valuable_List>()
                 .Select((a) => new { a.Id, a.ClassId, a.ClassName, a.Content, a.Score, a.BeLock })
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

        public static IList<Valuable_List> GetList()
        {
            var where = new Where<Valuable_List>();
            where.And(o => o.BeLock == false);

            var p = Db.Context.From<Valuable_List>()
                 .Select((a) => new { a.Id, a.ClassId, a.ClassName, a.Content, a.Score, a.BeLock })
                 .Where(where);

            return p.OrderBy(o => o.Sequence).ToList();
        }

        /// <summary>
        /// 获取所有Id
        /// </summary>
        /// <returns></returns>
        public static List<int> GetIds()
        {
            using (var p = Db.Context.From<Valuable_List>().Select(o => o.Id).Where(o => o.BeLock == false).ToDataReader())
            {
                List<int> ids = new List<int>();
                while (p.Read()) ids.Add((int)p["Id"]);
                return ids;
            }
        }

        /// <summary>
        /// 更新分类名
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static bool UpdateClassName(int classId, string className)
        {
            return Db.Context.FromSql($"update Valuable_List set ClassName='{className}' where ClassId={classId}").ExecuteNonQuery() > 0;
        }
    }
}
