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
    public class BllFinance_Statistics : ServiceBase<Finance_Statistics>
    {
        public static BllFinance_Statistics Instance()
        {
            return new BllFinance_Statistics();
        }

        public static IEnumerable<Finance_Statistics> PageList(int page, int pageSize, out long records, string file, string key, DateTime? startdate, DateTime? enddate)
        {
            var where = new Where<Finance_Statistics>();
            where.And(o => o.Display != 2);
            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(key))
                where.And(string.Format(" {0} like '%{1}%' ", file, key));
            if (startdate != null)
                where.And(o => o.SaleDate >= startdate);
            if (enddate != null)
                where.And(o => o.SaleDate <= enddate);

            var p = Db.Context.From<Finance_Statistics>()
                    .Where(where);
            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        public static IEnumerable<Finance_Statistics> GetList(string file, string key, DateTime? startdate, DateTime? enddate)
        {
            var where = new Where<Finance_Statistics>();
            where.And(o => o.Display != 2);
            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(key))
                where.And(string.Format(" {0} like '%{1}%' ", file, key));
            if (startdate != null)
                where.And(o => o.SaleDate >= startdate);
            if (enddate != null)
                where.And(o => o.SaleDate <= enddate);

            var p = Db.Context.From<Finance_Statistics>()
                    .Where(where);
            return p.ToList();
        }

        #region 删除
        public static int Delete(int id, int myuserId)
        {
            var model = First(o => o.Id == id);
            if (model == null)
                return 0;
            model.Display = 2;
            model.LastDate = DateTime.Now;
            model.LastUserId = myuserId;
            return Update(model);
        }

        public static int DeleteList(IEnumerable<Finance_Statistics> list, int myuserId)
        {
            foreach (var item in list)
                Delete(item.Id, myuserId);

            return 1;
        }
        #endregion

        /// <summary>
        /// 重新统计更新过的数据
        /// </summary>
        /// <param name="list"></param>
        public static void AgainCountData(List<Finance_Statistics> list)
        {
            if (list == null || list.Count < 1)
                return;

            //查询统计时需要用到的销售记录
            List<DateTime?> dateList = (from o in list orderby o.SaleDate ascending select o.SaleDate).ToList();
            DateTime start = dateList.First().GetValueOrDefault().AddDays(-1);
            DateTime end = dateList.Last().GetValueOrDefault().AddDays(1);

            var everyDaySaleLog = BllFinance_EveryDaySaleLog.Query(o => o.SaleDate > start && o.SaleDate < end && o.Display != 2).ToList();
            if (everyDaySaleLog == null)
                return;

            foreach (var item in list)
            {
                decimal? totalIncome = (from o in everyDaySaleLog where o.SaleDate == item.SaleDate && o.DepartmentName == item.DepartmentName select o.SaleIncome).Sum();

                Edit(item, totalIncome);
            }
        }

        //更新统计
        public static void Edit(Finance_Statistics statistics, decimal? businessIncome)
        {
            var model = First(o => o.SaleDate == statistics.SaleDate && o.DepartmentName == statistics.DepartmentName && o.Display != 2);
            if (model == null)
            {
                statistics.BusinessIncome = businessIncome;
                statistics.Year = statistics.SaleDate.GetValueOrDefault().Year;
                statistics.Month = statistics.SaleDate.GetValueOrDefault().Month;
                statistics.Day = statistics.SaleDate.GetValueOrDefault().Day;
                statistics.AddDate = statistics.LastDate;
                statistics.AddUserId = statistics.LastUserId;
                statistics.Display = 1;
                Insert(statistics);
            }
            else
            {
                model.BusinessIncome = businessIncome;
                model.LastUserId = statistics.LastUserId;
                model.LastDate = statistics.LastDate;
                Update(model);
            }
        }

        public static decimal? GetBusinessRate(decimal? dividend, decimal? divisor)
        {
            if (dividend != null && divisor != null && divisor != 0)
                return ((dividend / divisor) * 100).GetValueOrDefault().ToString("f2").ToDecimal();
            else if (divisor == null)
                return null;
            else
                return 0;
        }
    }
}
