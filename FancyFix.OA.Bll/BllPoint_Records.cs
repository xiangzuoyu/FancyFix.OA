using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using FancyFix.OA.Model.Business;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllPoint_Records : ServiceBase<Point_Records>
    {
        public static BllPoint_Records Instance()
        {
            return new BllPoint_Records();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public static IEnumerable<Point_Records> PageList(int userId, int isSelf, int page, int pageSize, out long records)
        {
            var where = new Where<Point_Records>();
            if (isSelf > 0)
            {
                if (isSelf == 1)
                    where.And(o => o.UserId == userId);
                else
                    where.And(o => o.CreateUserId == userId && o.UserId != userId);
            }
            else
                where.And(o => o.UserId == userId || o.CreateUserId == userId);
            var p = Db.Context.From<Point_Records>()
                 .LeftJoin<Point_List>((a, b) => a.PointId == b.Id)
                 .Select<Point_List>((a, b) => new
                 {
                     a.Id,
                     a.Content,
                     a.EventTime,
                     a.UserName,
                     a.CreateTime,
                     a.IsApprove,
                     a.IsPass,
                     a.ApproveTime,
                     a.ParUserName,
                     a.Score,
                     a.PointId,
                     b.ClassName,
                     b.PointName,
                     b.PointScore
                 })
                 .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        /// <summary>
        /// 是否审批完成
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool IsApproved(int userId)
        {
            if (userId == 0) return false;
            var where = new Where<Point_Records>();
            where.And(o => o.UserId == userId && o.IsApprove == false);
            int count = Db.Context.From<Point_Records>().Where(where).Count();
            return count > 0 ? false : true;
        }

        public static IEnumerable<Point_Records> PageList(int userId, bool isApproved, int page, int pageSize, out long records)
        {
            var where = new Where<Point_Records>();
            where.And(o => o.UserId == userId && o.IsApprove == isApproved);
            var p = Db.Context.From<Point_Records>()
                 .LeftJoin<Point_List>((a, b) => a.PointId == b.Id)
                 .Select<Point_List>((a, b) => new
                 {
                     a.Id,
                     a.Content,
                     a.EventTime,
                     a.UserName,
                     a.CreateTime,
                     a.IsApprove,
                     a.IsPass,
                     a.ApproveTime,
                     a.ParUserName,
                     a.Score,
                     a.PointId,
                     b.ClassName,
                     b.PointName,
                     b.PointScore
                 })
                 .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        /// <summary>
        /// 获取已通过积分用户排名
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<Rank_Point> GetRankUserList(int year, int fromMonth, int toMonth)
        {
            string where = "IsApprove=1 and IsPass=1";
            if (year > 0) where += " and year=" + year;

            string whereMonth = string.Empty;
            if (fromMonth > 0 && fromMonth <= 12)
                whereMonth = $" and month >= {fromMonth} ";
            if (toMonth > 0 && toMonth <= 12 && toMonth > fromMonth)
                whereMonth = $" and month >= {fromMonth} and month <= {toMonth} ";
            if (fromMonth > 0 && fromMonth <= 12 && toMonth > 0 && toMonth <= 12 && toMonth == fromMonth)
                whereMonth = $" and month = {fromMonth} ";

            where = where + whereMonth;

            string sql = $"select * from (select UserId,UserName as RealName,sum(Score) as Score from Point_Records where {where} group by UserId,UserName) as tb order by score desc";
            return Db.Context.FromSql(sql).ToList<Rank_Point>();
        }

        /// <summary>
        /// 获取已通过用户积分列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<Point_Records> GetListByUser(int userId, int year, int fromMonth, int toMonth)
        {
            var where = new Where<Point_Records>();
            where.And(o => o.UserId == userId && o.IsApprove == true && o.IsPass == true);
            if (year > 0) where.And(o => o.Year == year);

            //处理月份
            var whereMonth = new Where<Point_Records>();
            if (fromMonth > 0 && fromMonth <= 12)
            {
                whereMonth.And(o => o.Month >= fromMonth);
            }
            if (toMonth > 0 && toMonth <= 12 && toMonth > fromMonth)
            {
                whereMonth = new Where<Point_Records>();
                whereMonth.And(o => o.Month >= fromMonth && o.Month <= toMonth);
            }
            if (fromMonth > 0 && fromMonth <= 12 && toMonth > 0 && toMonth <= 12 && toMonth == fromMonth)
            {
                whereMonth = new Where<Point_Records>();
                whereMonth.And(o => o.Month == fromMonth);
            }
            where.And(whereMonth.ToWhereClip());
            return Db.Context.From<Point_Records>()
                 .LeftJoin<Point_List>((a, b) => a.PointId == b.Id)
                 .Select<Point_List>((a, b) => new
                 {
                     a.Id,
                     a.Content,
                     a.EventTime,
                     a.UserName,
                     a.CreateTime,
                     a.IsApprove,
                     a.IsPass,
                     a.ApproveTime,
                     a.ParUserName,
                     a.Score,
                     a.PointId,
                     b.ClassName,
                     b.PointName,
                     b.PointScore
                 }).Where(where)
                 .OrderByDescending(o => o.Id).ToList<Point_Records>();
        }
    }
}
