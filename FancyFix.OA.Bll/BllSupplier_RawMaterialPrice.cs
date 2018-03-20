using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FancyFix.OA.Model;
using Dos.ORM;
using Dos.DataAccess.Base;

namespace FancyFix.OA.Bll
{
    public class BllSupplier_RawMaterialPrice:ServiceBase<Supplier_RawMaterialPrice>
    {
        public static BllSupplier_RawMaterialPrice Instance()
        {
            return new BllSupplier_RawMaterialPrice();
        }

        public static IEnumerable<Supplier_RawMaterialPrice> PageList(int page, int pageSize, out long records, string file, string key)
        {
            var where = new Where<Supplier_RawMaterialPrice>();
            where.And(o => o.Display != 2);
            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(key))
                where.And(string.Format(" '{0}'='{1}' ", file, key));
            var p = Db.Context.From<Supplier_RawMaterialPrice>()
                .Where(where);
            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }
    }
}
