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

        /// <summary>
        /// 取消需求
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CancelTask(int id)
        {
            var model = First(o => o.Id == id);
            if (model == null)
                return false;
            model.Display = 4;
            return Update(model) > 0;
        }

        /// <summary>
        /// 设置需求完成
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CompleteTask(int id)
        {
            var model = First(o => o.Id == id);
            if (model == null)
                return false;
            model.CompletionDate = DateTime.Now;
            model.Display = 3;
            return Update(model) > 0;
        }

        /// <summary>
        /// 获取显示在日历上的需求
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ArtTaskList> GetList()
        {
            var nowdate = DateTime.Now;
            DateTime startdate = DateTime.Parse(nowdate.ToString("yyyy-MM-dd"));
            DateTime enddate = DateTime.Parse(nowdate.AddDays(30).ToString("yyyy-MM-dd"));

            var list = Db.Context.From<ArtTaskList>()
                .Where(o => o.EstimatedEndDate >= startdate && o.EstimatedEndDate <= enddate && o.Display == 2)
                .ToList();

            return list;
        }
    }
}
