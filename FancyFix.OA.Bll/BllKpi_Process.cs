using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using FancyFix.OA.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FancyFix.OA.Bll
{
    public class BllKpi_Process : ServiceBase<Kpi_Process>
    {
        public static BllKpi_Records Instance()
        {
            return new BllKpi_Records();
        }

        public static Kpi_Process GetModel(int userId, int year, int month)
        {
            var where = new Where<Kpi_Process>();
            where.And(o => o.UserId == userId && o.Year == year && o.Month == month);

            var p = Db.Context.From<Kpi_Process>()
                 .Where(where);
            return p.OrderByDescending(o => o.Id).ToFirst();
        }

        /// <summary>
        /// 进程生成
        /// </summary>
        /// <param name="process"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public static bool CreateProcess(Kpi_Process process, List<Kpi_Records> records)
        {
            using (var trans = Db.Context.BeginTransaction())
            {
                //指标生成
                records.ForEach(o => o.IsCreated = true);
                int count = trans.Update(records);
                if (count == 0)
                {
                    trans.Rollback();
                    return false;
                }
                //进程生成
                process.IsCreated = true;
                count = trans.Update(process);
                if (count == 0)
                {
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
                return true;
            }
        }

        /// <summary>
        /// 更新进程状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool UpdateProcessStatus(Kpi_Records record)
        {
            using (var trans = Db.Context.BeginTransaction())
            {
                int id = trans.Update(record, o => o.Id == record.Id);
                if (id == 0)
                {
                    trans.Rollback();
                    return false;
                }
                //查询进程是否存在
                var process = trans.From<Kpi_Process>().Where(o => o.UserId == record.UserId && o.Year == record.Year && o.Month == record.Month).ToFirst();
                if (process == null)
                {
                    trans.Rollback();
                    return false;
                }

                //查询进程下其他所有指标
                var recordlist = trans.From<Kpi_Records>().Where(o => o.Pid == process.Id).ToList();
                if (recordlist == null || recordlist.Count == 0)
                {
                    trans.Rollback();
                    return false;
                }

                //更新状态和总分，指标
                process.IsApprove = recordlist.Count(o => o.IsApprove == false) == 0;
                process.Score = recordlist.Where(o => o.IsApprove == true).Sum(o => o.FinishScore).Value;
                id = trans.Update(process, o => o.Id == process.Id);
                if (id == 0)
                {
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
                return true;
            }
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<Kpi_Process> GetRankUserList(int year, int month)
        {
            var where = new Where<Kpi_Process>();
            where.And<Mng_User>((a, b) => a.Year == year && a.Month == month && a.IsApprove == true && b.InJob == true);

            var p = Db.Context.From<Kpi_Process>()
                 .InnerJoin<Mng_User>((a, b) => a.UserId == b.Id)
                 .Select<Mng_User>((a, b) => new { a.Id, a.Score, a.Year, a.Month, a.IsApprove, b.RealName, b.DepartId })
                 .Where(where);
            return p.OrderByDescending(o => o.Score).ToList();
        }

        /// <summary>
        /// 获取所有进程列表
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<Kpi_Process> GetList(int year, int month)
        {
            var where = new Where<Kpi_Process>();
            where.And(o => o.Year == year && o.Month == month);

            var p = Db.Context.From<Kpi_Process>()
                 .Select(o => new { o.Id, o.UserId, o.Score, o.Year, o.Month, o.IsApprove, o.IsCreated, o.SelfScore })
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// 获取进程列表
        /// </summary>
        /// <param name="year"></param>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<Kpi_Process> GetList(int year, int fromMonth, int toMonth, int userId = 0)
        {
            var where = new Where<Kpi_Process>();
            where.And(o => o.Year == year);
            if (userId > 0)
                where.And(o => o.UserId == userId);

            if (fromMonth > 0 && fromMonth <= 12 && toMonth > 0 && toMonth <= 12 && toMonth == fromMonth)
                where.And(o => o.Year == year && o.Month == fromMonth);
            else if (toMonth > 0 && toMonth <= 12 && toMonth > fromMonth)
                where.And(o => o.Year == year && o.Month >= fromMonth && o.Month <= toMonth);
            else
                where.And(o => o.Year == year && o.Month >= fromMonth);

            var p = Db.Context.From<Kpi_Process>()
                 .Select(o => new { o.Id, o.UserId, o.Score, o.Year, o.Month, o.IsApprove, o.IsCreated, o.SelfScore })
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// 获取历史模版列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<Kpi_Process> GetModelList(int userId)
        {
            return Db.Context.From<Kpi_Process>()
                .Where(o => o.UserId == userId && o.IsCreated == true).
                OrderBy(new Field[] { Kpi_Process._.Year, Kpi_Process._.Month }).ToList();
        }

        /// <summary>
        /// 获取所有用户的排名
        /// </summary>
        /// <param name="year"></param>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="realname"></param>
        /// <returns></returns>
        public static List<Mng_User> GetUserRankList(int year, int fromMonth = 0, int toMonth = 0, int departId = 0, string realname = "", int injob = 1)
        {
            string where = $"year={year}";

            string whereMonth = string.Empty;
            if (fromMonth > 0 && fromMonth <= 12 && toMonth > 0 && toMonth <= 12 && toMonth == fromMonth)
                whereMonth = $" and month = {fromMonth} ";
            else if (toMonth > 0 && toMonth <= 12 && toMonth > fromMonth)
                whereMonth = $" and month >= {fromMonth} and month <= {toMonth} ";
            else if (fromMonth > 0 && fromMonth <= 12)
                whereMonth = $" and month >= {fromMonth} ";

            where = where + whereMonth;

            string where1 = " where 1=1";
            if (realname != "")
                where1 += $" and RealName like '{CheckSqlValue(realname)}%'";
            if (departId > 0)
            {
                where1 += " and a.DepartId=" + departId;
            }
            if (injob > 0)
            {
                if (injob == 1)
                {
                    where1 += " and a.InJob=" + 1;
                }
                else
                {
                    where1 += " and a.InJob=" + 0;
                }
            }

            string cols = "a.Id,UserName,RealName,Sex,Email,InJob,b.ClassName as DepartMentName,c.GroupName,ParUserId,";
            cols += $"(select sum(Score) from Kpi_Process where {where} and UserId = a.Id) as Score,";
            cols += $"(select sum(SelfScore) from Kpi_Process where {where} and UserId = a.Id) as SelfScore,";
            cols += $"(select count(1) from (select Month from Kpi_Process where {where} and IsCreated=1 and UserId = a.Id group by Month) as tb) as Count";

            string sql = $"select * from (" +
                 $" select top 100 percent {cols} from Mng_User a " +
                 $" left join Mng_DepartmentClass b on a.DepartId = b.Id" +
                 $" left join Mng_PermissionGroup c on a.GroupId = c.Id" +
                 where1 +
                 $" order by InJob desc,a.Id asc" +
                 $") as tb order by Score desc";
            return Db.Context.FromSql(sql).ToList<Mng_User>();
        }

        /// <summary>
        /// 取消生成
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool CancelCreate(List<int> userIds, int year, int month)
        {
            using (var trans = Db.Context.BeginTransaction())
            {
                var ids = string.Join(",", userIds);
                //指标生成
                int count = trans.FromSql($"update Kpi_Process set iscreated = 0 where iscreated = 1 and year = {year} and month = {month} and userid in ({ids})").ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return false;
                }
                //进程生成
                count = trans.FromSql($"update Kpi_Records set iscreated = 0 where iscreated = 1 and year = {year} and month = {month} and userid in ({ids})").ExecuteNonQuery();
                if (count == 0)
                {
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
                return true;
            }
        }

    }
}
