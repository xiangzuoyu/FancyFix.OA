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
                count = Db.Context.Update(process);
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
                if (process == null) return false;

                //查询进程下其他所有指标
                var recordlist = trans.From<Kpi_Records>().Where(o => o.Pid == process.Id).ToList();
                if (recordlist == null || recordlist.Count == 0) return false;

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
            where.And(o => o.Year == year && o.Month == month && o.IsApprove == true);

            var p = Db.Context.From<Kpi_Process>()
                 .LeftJoin<Mng_User>((a, b) => a.UserId == b.Id)
                 .Select<Mng_User>((a, b) => new { a.Id, a.Score, a.Year, a.Month, a.IsApprove, b.RealName, b.DepartId })
                 .Where(where);
            return p.OrderByDescending(o => o.Score).ToList();
        }

        public static List<Kpi_Process> GetModelList(int userId)
        {
            return Db.Context.From<Kpi_Process>()
                .Where(o => o.UserId == userId && o.IsCreated == true).
                OrderBy(new Field[] { Kpi_Process._.Year, Kpi_Process._.Month }).ToList();
        }
    }
}
