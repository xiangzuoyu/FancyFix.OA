using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using FancyFix.OA.Model.Business;
using System;
using System.Collections.Generic;
using System.Data;

namespace FancyFix.OA.Bll
{
    public class BllKpi_Records : ServiceBase<Kpi_Records>
    {
        public static BllKpi_Records Instance()
        {
            return new BllKpi_Records();
        }

        public static List<Kpi_Records> GetList(int pid)
        {
            var where = new Where<Kpi_Records>();
            where.And(o => o.Pid == pid);

            var p = Db.Context.From<Kpi_Records>()
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// 根据Id集合获取KPI
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<Kpi_Records> GetListByIds(List<int> ids)
        {
            var where = new Where<Kpi_Records>();
            where.And(o => o.Id.In(ids));

            var p = Db.Context.From<Kpi_Records>()
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// 获取指定评分人的KPI
        /// </summary>
        /// <param name="parUserId"></param>
        /// <returns></returns>
        public static List<Kpi_Records> GetListByParUserId(int parUserId)
        {
            var where = new Where<Kpi_Records>();
            where.And(o => o.ParUserId == parUserId);

            var p = Db.Context.From<Kpi_Records>()
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// 下级员工列表
        /// </summary>
        /// <param name="parUserId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<Mng_User> GetUserList(int parUserId, int year, int month)
        {
            string sql = $"select UserId as Id,b.RealName,c.ClassName as DepartMentName,d.GroupName from Kpi_Records a " +
                $"inner join Mng_User b on a.UserId = b.Id " +
                $"left join Mng_DepartmentClass c on b.DepartId = c.Id " +
                $"left join Mng_PermissionGroup d on b.GroupId = d.Id " +
                $"where b.InJob=1 and a.ParUserId = {parUserId} and a.year = {year} and a.month = {month}" +
                $"group by UserId,b.RealName,c.ClassName,d.GroupName";
            return Db.Context.FromSql(sql).ToList<Mng_User>();
        }

        /// <summary>
        /// 获取可审批KPI的数量
        /// </summary>
        /// <param name="parUserId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetUserListCount(int parUserId)
        {
            string sql = $"select Count(1) from Kpi_Records " +
                $"where ParUserId = {parUserId}";
            return (int)Db.Context.FromSql(sql).ToScalar();
        }

        /// <summary>
        /// 指标未审批数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parUserId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetUserListUnApproveCount(int userId, int parUserId, int year, int month)
        {
            string sql = $"select Count(1) from Kpi_Records " +
                $"where UserId={userId} and ParUserId = {parUserId} and IsApprove=0 and IsCreated=1 and year={year} and month={month}";
            return (int)Db.Context.FromSql(sql).ToScalar();
        }

        /// <summary>
        /// 获取指定员工的KPI
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<Kpi_Records> GetListByUserId(int userId, int year, int month)
        {
            var where = new Where<Kpi_Records>();
            where.And(o => o.UserId == userId && o.Year == year && o.Month == month);

            var p = Db.Context.From<Kpi_Records>()
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// 上级未审批列表
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DataTable GetUnApproveList(int year, int month)
        {
            return Db.Context.FromSql($"select UserName,ParUserName from Kpi_Records where IsCreated=1 and IsApprove=0 and year={year} and month={month} group by ParUserName,UserName").ToDataTable();
        }

        /// <summary>
        /// 获取用户指标总百分比
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetUserScoreSum(int userId, int year, int month)
        {
            string sql = $"select Sum(Score) from Kpi_Records " +
                $"where UserId={userId} and year={year} and month={month}";
            return Db.Context.FromSql(sql).ToScalar<int>();
        }
    }
}
