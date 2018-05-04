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
    public class BllSupplier_Price : ServiceBase<Supplier_Price>
    {
        /// <summary>
        /// 先删除旧的的价格，在新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool Add(Supplier_Price list)
        {
            DbTrans trans = DbSession.Default.BeginTransaction();
            try
            {
                DbSession.Default.Delete<Supplier_Price>(trans, o =>
                o.RawMaterialPriceId == list.RawMaterialPriceId &&
                o.Years == list.Years &&
                o.Month == list.Month &&
                o.Display != 2);
                bool isok = DbSession.Default.Insert(trans, list) > 0;

                if (isok)
                    trans.Commit();
                else
                    trans.Rollback();

                return isok;
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                trans.Close();
            }
        }

        public static List<Supplier_Price> GetList(DateTime startdate, DateTime enddate)
        {
            var where = new Where<Supplier_Price>();
            //where.And(o=>o.Years >= startdate.Year && o.Years <= enddate.Year &&
            //          o.Month >= startdate.Month && o.Month <= enddate.Month);
            //where.And(o => $"{o.Years}-{o.Month}-1".ToDateTime() >= startdate && $"{o.Years}-{o.Month}".ToDateTime().AddMonths(1).AddDays(-1) <= enddate);
            where.And(" CONVERT(datetime,CONVERT(VARCHAR(50),Years) +'-'+CONVERT(VARCHAR(50),Month)+'-1',101) >='" + startdate.ToString("yyyy-MM-dd")
                   + "' and DATEADD(DAY, -1, DATEADD(Month, 1, CONVERT(datetime, CONVERT(VARCHAR(50), Years) + '-' + CONVERT(VARCHAR(50), Month) + '-1', 101))) <= '"
                   + enddate.ToString("yyyy-MM-dd") + "' ");

            var p = Db.Context.From<Supplier_Price>()
                .Where(where);

            return p.ToList();
        }



    }
}
