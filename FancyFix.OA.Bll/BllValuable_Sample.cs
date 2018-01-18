using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllValuable_Sample : ServiceBase<Valuable_Sample>
    {
        public static BllValuable_Sample Instance()
        {
            return new BllValuable_Sample();
        }

        public static IList<Valuable_Sample> GetList(int rid)
        {
            var where = new Where<Valuable_Sample>();
            where.And(o => o.Rid == rid);

            var p = Db.Context.From<Valuable_Sample>()
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        public static int Add(int rid, List<Valuable_Sample> samplelist)
        {
            int count = 0;
            if (samplelist != null && samplelist.Count > 0)
            {
                using (var trans = Db.Context.BeginTransaction())
                {
                    samplelist.ForEach(o => { o.Rid = rid; });
                    trans.Delete<Valuable_Sample>(o => o.Rid == rid);
                    count = trans.Insert(samplelist);
                    trans.Commit();
                    trans.Close();
                }
            }
            return count;
        }
    }
}
