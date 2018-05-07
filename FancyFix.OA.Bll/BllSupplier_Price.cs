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
        public static bool DeleteAndAdd(Supplier_Price list)
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

        public static bool InsertPrice(int years, int rawMaterialPriceId, List<Supplier_Price> list)
        {
            Db.Context.Delete<Supplier_Price>(o => o.RawMaterialPriceId == rawMaterialPriceId && o.Years == years && o.Display != 2);

            return Db.Context.Insert(list) > 0;
        }

        public static List<Supplier_Price> GetList(DateTime startdate, DateTime enddate)
        {
            var where = new Where<Supplier_Price>();
            where.And(o => o.YearsMonth >= startdate && o.YearsMonth <= enddate);

            var p = Db.Context.From<Supplier_Price>()
                .Where(where);

            return p.ToList();
        }

    }
}
