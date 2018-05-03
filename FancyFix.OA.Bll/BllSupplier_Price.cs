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

        public static List<Supplier_Price> PageList(int[] ids, int page, int pageSize, out long records,DateTime startdate,DateTime enddate)
        {
            //var list = Bll.BllSupplier_RawMaterialPrice.GetList(id) ?? new List<Supplier_RawMaterialPrice>();
            //var rawMaterialList = Bll.BllSupplier_RawMaterial.GetSelectList(0, "Id,SAPCode,Description", "display!=2", "") ?? new List<Supplier_RawMaterial>();
            //var supplierList = Bll.BllSupplier_List.GetSelectList(0, "Id,Code,Name", "display!=2", "") ?? new List<Supplier_List>();



            return new List<Supplier_Price>();
        }
    }
}
