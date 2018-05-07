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

        /// <summary>
        /// 添加价格映射表，返回ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Add(Supplier_RawMaterial model, ref int AddId)
        {
            if (model == null)
                return "-1";

            try
            {
                //获取供应商ID，如果供应商不存在跳过
                var supplierModel = BllSupplier_List.First(o => o.Code == model.SupplierCode && o.Name == model.SupplierName && o.Display != 2);
                if (supplierModel == null)
                    return "2";

                //如果原材料代码不存在执行添加
                var rawmaterialModel = First(o => o.SAPCode == model.SAPCode && o.Display != 2);

                string sapcode = string.Empty;
                if (rawmaterialModel == null)
                {
                    AddId = Insert(model);
                    sapcode = model.SAPCode;
                }
                else
                {
                    AddId = rawmaterialModel.Id;
                    sapcode = rawmaterialModel.SAPCode;
                }

                //原材料Code不存在
                if (AddId < 1)
                    return "3";

                //添加价格表，如果原材料代码相同 && 供应商代码相同 && 年份相同 && 为可用状态，就跳过
                var rawmaterialpriceModel = BllSupplier_RawMaterialPrice.First(o => o.RawMaterialId == sapcode && o.VendorId == supplierModel.Code
                    && o.Display != 2);
                //已添加就直接返回映射表ID
                if (rawmaterialpriceModel != null)
                {
                    AddId = rawmaterialpriceModel.Id;
                    return "0";
                }

                AddId = BllSupplier_RawMaterialPrice.Insert(new Supplier_RawMaterialPrice()
                {
                    RawMaterialId = sapcode,
                    VendorId = supplierModel.Code,
                    PriceFrequency = model.PriceFrequency,
                    AddDate = model.AddDate,
                    AddUserId = model.AddUserId,
                    LastDate = model.AddDate,
                    LastUserId = model.AddUserId
                });

                if (AddId < 1)
                    return "-1";
                return "0";
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(BllSupplier_RawMaterial), ex, 0, "");
                return "-1";
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
