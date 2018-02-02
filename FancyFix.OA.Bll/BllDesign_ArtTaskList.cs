using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyFix.OA.Bll
{
    public class BllDesign_ArtTaskList : ServiceBase<Design_ArtTaskList>
    {
        public static BllDesign_ArtTaskList Instance()
        {
            return new BllDesign_ArtTaskList();
        }

        public static IEnumerable<Design_ArtTaskList> PageList(int submitterId, int page, int pageSize, out long records, int display)
        {
            var where = new Where<Design_ArtTaskList>();
            where.And(o => o.Display != 4);
            if (submitterId > 0)
                where.And(o => o.SubmitterId == submitterId);
            if (display > 0)
            {
                if (display == 3)
                    where.And(o => o.Display == display || o.Display == 5);
                else
                    where.And(o => o.Display == display);
            }

            var p = Db.Context.From<Design_ArtTaskList>()
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
        public static IEnumerable<Design_ArtTaskList> GetList(DateTime start, DateTime end, int designerId = 0)
        {
            var list = Db.Context.From<Design_ArtTaskList>()
                .Where(o => o.Display != 1 && o.Display != 4 &&
                ((o.EstimatedStartDate >= start && o.EstimatedStartDate <= end) || (o.EstimatedEndDate >= start && o.EstimatedEndDate <= end)))
                .ToList();

            return list;
        }
    }
}
