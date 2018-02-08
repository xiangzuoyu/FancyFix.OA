using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using FancyFix.OA.Model.Business;
using System;
using System.Collections.Generic;

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

        public static List<Kpi_Records> GetListByIds(List<int> ids)
        {
            var where = new Where<Kpi_Records>();
            where.And(o => o.Id.In(ids));

            var p = Db.Context.From<Kpi_Records>()
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

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
                $"left join Mng_User b on a.UserId = b.Id " +
                $"left join Mng_DepartmentClass c on b.DepartId = c.Id " +
                $"left join Mng_PermissionGroup d on b.GroupId = d.Id " +
                $"where a.ParUserId = {parUserId} and a.year = {year} and a.month = {month}" +
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

        public static List<Kpi_Records> GetListByUserId(int userId, int year, int month)
        {
            var where = new Where<Kpi_Records>();
            where.And(o => o.UserId == userId && o.Year == year && o.Month == month);

            var p = Db.Context.From<Kpi_Records>()
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }
    }
}
