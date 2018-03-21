using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FancyFix.OA.Model;
using Dos.ORM;
using Dos.DataAccess.Base;
using System.Data;

namespace FancyFix.OA.Bll
{
    public class BllSupplier_RawMaterialPrice : ServiceBase<Supplier_RawMaterialPrice>
    {
        public static BllSupplier_RawMaterialPrice Instance()
        {
            return new BllSupplier_RawMaterialPrice();
        }

        public static IEnumerable<Supplier_RawMaterialPrice> PageList(int page, int pageSize, out long records, string file, string key, int years, int priceFrequency)
        {
            var where = new Where<Supplier_RawMaterialPrice>();

            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(key))
                where.And(string.Format(" {0} like '%{1}%' ", ("SupplierCode,SupplierName".Contains(file) ? file.Replace("Supplier", "") : file), key));

            where.And(string.Format(" Years={0} ", years));
            if (priceFrequency > 0)
                where.And(string.Format(" PriceFrequency={0} ", priceFrequency));

            var p = Db.Context.From<Supplier_RawMaterialPrice>()
                .Select<Supplier_RawMaterial, Supplier_List>((a, b, c) => new
                {
                    a.Id,
                    b.BU,
                    b.SAPCode,
                    b.Description,
                    b.Category,
                    b.LeadBuyer,
                    c.Code,
                    c.Name,
                    b.PriceFrequency,
                    b.Currency,
                    a.Month1,
                    a.Month2,
                    a.Month3,
                    a.Month4,
                    a.Month5,
                    a.Month6,
                    a.Month7,
                    a.Month8,
                    a.Month9,
                    a.Month10,
                    a.Month11,
                    a.Month12
                })
                .InnerJoin<Supplier_RawMaterial>((a, b) => a.RawMaterialId == b.Id && a.Display != 2 && b.Display != 2)
                .InnerJoin<Supplier_List>((b, c) => b.VendorId == c.Id && c.Display != 2)
                .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        public static DataTable GetList(string file, string key, int years, int priceFrequency)
        {
            var where = new Where<Supplier_RawMaterialPrice>();

            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(key))
                where.And(string.Format(" {0} like '%{1}%' ", ("SupplierCode,SupplierName".Contains(file) ? file.Replace("Supplier", "") : file), key));

            where.And(string.Format(" Years={0} ", years));
            if (priceFrequency > 0)
                where.And(string.Format(" PriceFrequency={0} ", priceFrequency));

            var p = Db.Context.From<Supplier_RawMaterialPrice>()
                .Select<Supplier_RawMaterial, Supplier_List>((a, b, c) => new
                {
                    b.BU,
                    b.SAPCode,
                    b.Description,
                    b.Category,
                    b.LeadBuyer,
                    c.Code,
                    c.Name,
                    b.PriceFrequency,
                    b.Currency,
                    a.Month1,
                    a.Month2,
                    a.Month3,
                    a.Month4,
                    a.Month5,
                    a.Month6,
                    a.Month7,
                    a.Month8,
                    a.Month9,
                    a.Month10,
                    a.Month11,
                    a.Month12
                })
                .InnerJoin<Supplier_RawMaterial>((a, b) => a.RawMaterialId == b.Id && a.Display != 2 && b.Display != 2)
                .InnerJoin<Supplier_List>((b, c) => b.VendorId == c.Id && c.Display != 2)
                .Where(where);

            return p.OrderByDescending(o => o.Id).ToDataTable();
        }
    }
}
