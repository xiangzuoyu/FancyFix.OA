using System;
using FancyFix.OA.Model;
namespace FancyFix.OA.Bll
{
    public class BllSupplier_PriceMapping : ServiceBase<Supplier_PriceMapping>
    {
        public static BllSupplier_PriceMapping Instance()
        {
            return new BllSupplier_PriceMapping();
        }

        /// <summary>
        /// 添加原材料映射表，返回新增数据的ID
        /// </summary>
        /// <param name="rawMaterialCode"></param>
        /// <param name="vendorCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int Add(string rawMaterialCode, string vendorCode, int userId)
        {
            return Insert(new Supplier_PriceMapping()
            {
                RawMaterialCode = rawMaterialCode,
                VendorCode = vendorCode,
                AddDate = DateTime.Now,
                AddUserId = userId,
                LastDate = DateTime.Now,
                LastUserId = userId,
                Display = 1
            });

        }

        /// <summary>
        /// 获取该原材料在映射表中的ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int GetPriceMappingId(string supplierCode, string supplierName, string SAPCode, int userId)
        {
            //如果没找到供应商，跳过
            var supplierModel = BllSupplier_List.First(o => o.Code == supplierCode
            && o.Name == supplierName
            && o.Display != 2);
            if (supplierModel == null)
                return 0;

            //添加映射表中的ID
            int AddId = 0;
            var pricemappingModel = BllSupplier_PriceMapping.First(o => o.RawMaterialCode == SAPCode
            && o.VendorCode == supplierCode
            && o.Display != 2);
            if (pricemappingModel != null)
                return pricemappingModel.Id;

            AddId = BllSupplier_PriceMapping.Add(SAPCode, supplierCode, userId);
            return AddId > 0 ? AddId : 0;
        }

    }
}
