using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using FancyFix.OA.Model.Business;
using System;
using System.Collections.Generic;
using System.Data;

namespace FancyFix.OA.Bll
{
    public class BllValuable_Records : ServiceBase<Valuable_Records>
    {
        public static BllValuable_Records Instance()
        {
            return new BllValuable_Records();
        }

        public static IList<Valuable_Records> GetList(int userId, int year, int month)
        {
            var where = new Where<Valuable_Records>();
            where.And(o => o.UserId == userId && o.Year == year && o.Month == month);

            var p = Db.Context.From<Valuable_Records>()
                 .Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        public static IList<Valuable_Records> GetListByMonthRank(int userId, int year, int fromMonth, int toMonth)
        {
            var where = new Where<Valuable_Records>();
            where.And(o => o.UserId == userId && o.Year == year);
            if (fromMonth > 0)
                where.And(o => o.Month >= fromMonth);
            if (toMonth > 0)
                where.And(o => o.Month <= toMonth);

            var p = Db.Context.From<Valuable_Records>()
                .InnerJoin<Valuable_List>((a, b) => a.Vid == b.Id)
                .Select<Valuable_List>((a, b) => new { a.Id, a.Month, a.Year, a.Vid, a.Score, a.RankScore, a.Rank, a.ParUserName, b.ClassName, b.Content })
                 .Where(where);
            return p.OrderByDescending(o => o.Id).OrderByDescending(o => o.Month).ToList();
        }

        /// <summary>
        /// 是否自评完成
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="valueIds">价值观条目Ids</param>
        /// <returns></returns>
        public static bool IsRecorded(int userId, int year, int month, int[] valueIds)
        {
            if (userId == 0) return false;
            var where = new Where<Valuable_Records>();
            where.And(o => o.UserId == userId && o.Year == year && o.Month == month && o.Vid.In(valueIds));
            int count = Db.Context.From<Valuable_Records>().Distinct().Where(where).Count();//已提交记录总数
            return count >= valueIds.Length ? true : false;
        }

        /// <summary>
        /// 是否他评完成
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="valueIds"></param>
        /// <returns></returns>
        public static bool IsApproved(int userId, int year, int month, int[] valueIds)
        {
            if (userId == 0) return false;
            var where = new Where<Valuable_Records>();
            where.And(o => o.UserId == userId && o.Year == year && o.Month == month && o.Vid.In(valueIds) && o.IsApprove == true);
            int count = Db.Context.From<Valuable_Records>().Distinct().Where(where).Count();
            return count > 0 && count >= valueIds.Length ? true : false;
        }

        /// <summary>
        /// 获取指定进程对应总分
        /// </summary>
        /// <param name="months"></param>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static Dictionary<int, int> GetScoreByMonth(List<int> months, int userId, int year)
        {
            string sql = $"select month,sum(score) score from Valuable_Records where year = {year} and month in ({string.Join(",", months)}) and userid = {userId} and IsApprove = 1 group by month";
            using (var reader = Db.Context.FromSql(sql).ToDataReader())
            {
                Dictionary<int, int> dic = new Dictionary<int, int>();
                while (reader.Read())
                    dic.Add(reader["month"].ToString().ToInt32(), reader["score"].ToString().ToInt32());
                return dic;
            }
        }

        /// <summary>
        /// 获取排名列表
        /// </summary>
        /// <param name="fromYear"></param>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <returns></returns>
        public static List<Rank_Valuable> GetRankList(int fromYear, int fromMonth = 0, int toMonth = 0)
        {
            string where = $"IsApprove=1 and year={fromYear}";
            string whereMonth = string.Empty;

            if (fromMonth > 0 && fromMonth <= 12)
                whereMonth = $" and month >= {fromMonth} ";
            if (toMonth > 0 && toMonth <= 12 && toMonth > fromMonth)
                whereMonth = $" and month >= {fromMonth} and month <= {toMonth} ";
            if (fromMonth > 0 && fromMonth <= 12 && toMonth > 0 && toMonth <= 12 && toMonth == fromMonth)
                whereMonth = $" and month = {fromMonth} ";

            where = where + whereMonth;

            string sql = $"select UserId,RealName,sum(Score) Score," +
                $" (select count(1) from " +
                $"   (select MONTH from Valuable_Records where {where} and UserId = a.UserId group by MONTH) as tb" +
                $" ) as CountTime from Valuable_Records a " +
                $" left join Mng_User b on a.UserId = b.Id " +
                $" where {where} group by RealName,UserId order by sum(Score) desc";
            return Db.Context.FromSql(sql).ToList<Rank_Valuable>();
        }

        /// <summary>
        /// 获取排名Excel数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <returns></returns>
        public static DataTable GetExcelList(int userId, int year, int fromMonth, int toMonth)
        {
            string monthStr = string.Empty;
            if (toMonth > fromMonth)
                monthStr = $"month>={fromMonth} and month<={toMonth}";
            else
                monthStr = $"month = {fromMonth}";
            string sql = $" SELECT a.Id,c.RealName,b.ClassName,b.Content,a.Score,a.[Year],a.[Month] FROM [dbo].[Valuable_Records] a"
                       + $" left JOIN Valuable_List b on a.Vid = b.Id"
                       + $" left Join Mng_User c on a.UserId = c.Id"
                       + $" where a.UserId={userId} and IsApprove=1 and year={year} and {monthStr}"
                       + $" order by year desc,month desc";
            using (var reader = Db.Context.FromSql(sql).ToDataReader())
            {
                DataTable dt = new DataTable();
                //dt.Columns.Add("Id", typeof(System.String));
                dt.Columns.Add("进程", typeof(System.String));
                dt.Columns.Add("员工", typeof(System.String));
                dt.Columns.Add("价值观", typeof(System.String));
                dt.Columns.Add("考核内容说明", typeof(System.String));
                dt.Columns.Add("案例", typeof(System.String));
                dt.Columns.Add("得分", typeof(System.String));

                while (reader.Read())
                {
                    var row = dt.NewRow();
                    //row["Id"] = reader["Id"].ToString();
                    row["进程"] = reader["Year"].ToString() + "年" + reader["Month"].ToString() + "月";
                    row["员工"] = reader["RealName"].ToString();
                    row["价值观"] = reader["ClassName"].ToString();
                    row["考核内容说明"] = reader["Content"].ToString();

                    string sampleStr = string.Empty;
                    var samples = BllValuable_Sample.GetList(reader["Id"].ToString().ToInt32());
                    if (samples != null && samples.Count > 0)
                    {
                        for (int i = 0; i < samples.Count; i++)
                        {
                            sampleStr += "案例" + (i + 1) + "：" + samples[i].Content + (i < samples.Count - 1 ? "\n\n" : "");
                        }
                    }
                    row["案例"] = sampleStr;
                    row["得分"] = reader["Score"].ToString() + "分";
                    dt.Rows.Add(row);
                }
                return dt;
            }
        }

        /// <summary>
        /// 根据价值观获取进程
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="vid"></param>
        /// <returns></returns>
        public static Valuable_Records GetModelByVid(int userId, int year, int month, int vid)
        {
            return First(o => o.UserId == userId && o.Year == year && o.Month == month && o.Vid == vid);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="vid"></param>
        /// <returns></returns>
        public static bool IsExist(int userId, int year, int month, int vid)
        {
            return FirstSelect(o => o.UserId == userId && o.Year == year && o.Month == month && o.Vid == vid, o => o.Id)?.Id > 0;
        }
    }
}
