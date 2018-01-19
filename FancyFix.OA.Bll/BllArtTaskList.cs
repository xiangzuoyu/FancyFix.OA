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

        public static IEnumerable<ArtTaskList> PageList(int page, int pageSize, out long records, int display)
        {
            var where = new Where<ArtTaskList>();
            where.And(o => o.Display != 4);

            if (display > 0)
                where.And(o => o.Display == display);

            var p = Db.Context.From<ArtTaskList>()
                //.Select((a)=>new { })
                .Where(where);
            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.SubmittedDate).ToList();
        }

        public static bool CancelTask(int id)
        {
            var model = First(o => o.Id == id);
            if (model == null)
                return false;
            model.Display = 4;
            return Update(model) > 0;
        }

        public static bool CompleteTask(int id)
        {
            var model = First(o => o.Id == id);
            if (model == null)
                return false;
            model.CompletionDate = DateTime.Now;
            model.Display = 3;
            return Update(model) > 0;
        }
    }
}
