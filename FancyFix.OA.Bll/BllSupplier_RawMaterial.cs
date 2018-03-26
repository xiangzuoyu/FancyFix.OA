using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FancyFix.OA.Model;
using Dos.DataAccess.Base;
using Dos.ORM;
using System.Data;

namespace FancyFix.OA.Bll
{
    public class BllSupplier_RawMaterial : ServiceBase<Supplier_RawMaterial>
    {
        public static BllSupplier_RawMaterial Instance()
        {
            return new BllSupplier_RawMaterial();
        }

        public static IEnumerable<Supplier_RawMaterial> PageList(int page, int pageSize, out long records, string file, string key)
        {
            var where = new Where<Supplier_RawMaterial>();
            where.And(o => o.Display != 2);
            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(key))
                where.And(string.Format(" {0} like '%{1}%' ", file, key));

            var p = Db.Context.From<Supplier_RawMaterial>()
                .Where(where);
            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        public static bool Add(List<Supplier_RawMaterial> list)
        {
            if (list == null || list.Count() < 1)
                return false;

            int AddId = 0;
            try
            {
                foreach (var item in list)
                {
                    //获取供应商ID，如果供应商不存在跳过
                    var supplierModel = BllSupplier_List.First(o => o.Code == item.SupplierCode && o.Name == item.SupplierName && o.Display != 2);
                    if (supplierModel == null)
                        continue;

                    //如果原材料代码不存在执行添加
                    var rawmaterialModel = First(o => o.SAPCode == item.SAPCode && o.Display != 2);

                    AddId = rawmaterialModel == null ? Insert(item) : rawmaterialModel.Id;

                    if (AddId < 1)
                        continue;

                    //添加价格表
                    var rawmaterialpriceModel = BllSupplier_RawMaterialPrice.First(o => o.RawMaterialId == AddId && o.VendorId == supplierModel.Id
                        && o.Years == item.Years && o.Display != 2);

                    if (rawmaterialpriceModel != null)
                        continue;

                    AddId = BllSupplier_RawMaterialPrice.Insert(new Supplier_RawMaterialPrice()
                    {
                        RawMaterialId = AddId,
                        VendorId = supplierModel.Id,
                        Years = item.Years,
                        PriceFrequency = item.PriceFrequency,
                        Month1 = item.Month1,
                        Month2 = item.Month2,
                        Month3 = item.Month3,
                        Month4 = item.Month4,
                        Month5 = item.Month5,
                        Month6 = item.Month6,
                        Month7 = item.Month7,
                        Month8 = item.Month8,
                        Month9 = item.Month9,
                        Month10 = item.Month10,
                        Month11 = item.Month11,
                        Month12 = item.Month12,
                        AddDate = item.AddDate,
                        AddUserId = item.AddUserId,
                        LastDate = item.AddDate,
                        LastUserId = item.AddUserId
                    });

                    if (AddId < 1)
                        continue;
                }

                return true;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(BllSupplier_RawMaterial), ex, 0, "");
                return false;
            }
        }

        public static int HideModel(int id, int myuserId)
        {
            var model = First(o => o.Id == id);
            if (model == null)
                return 0;
            model.Display = 2;
            model.LastDate = DateTime.Now;
            model.LastUserId = myuserId;
            return Update(model);
        }

        public static int HideList(IEnumerable<Supplier_RawMaterial> list, int myuserId)
        {
            foreach (var item in list)
                HideModel(item.Id, myuserId);

            return 1;
        }

    }
}
