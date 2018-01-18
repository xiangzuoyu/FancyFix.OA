using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllArtTaskList : ServiceBase<ArtTaskList>
    {
        public static BllArtTaskList Instance()
        {
            return new BllArtTaskList();
        }

        public static IList<ArtTaskList> GetList()
        {
            var where = new Where<ArtTaskList>();
            where.And(o => true);

            var p = Db.Context.From<ArtTaskList>()
                .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        public static bool Add(ArtTaskList model)
        {
            return Insert(model) > 0;
        }

        public static bool Update(ArtTaskList model)
        {
            return Repository<ArtTaskList>.Update(model) > 0;
        }
    }
}
