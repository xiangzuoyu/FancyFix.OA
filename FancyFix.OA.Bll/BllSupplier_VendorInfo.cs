using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllSupplier_VendorInfo : ServiceBase<Supplier_VendorInfo>
    {
        public static string AddVendorInfo(Supplier_VendorInfo model)
        {
            if (model == null)
                return "供应商对象为空";

            DbTrans trans = DbSession.Default.BeginTransaction();
            try
            {
                //插入供应商主表
                var result = DbSession.Default.Insert(trans, new Supplier_List
                {
                    Code = model.Code,
                    Name = model.Name,

                    Site = model.CompanyWebsiteAddress,
                    Address = model.AddressOfRegisteredOffice,
                    Contact1 = $"{model.MainContactor}/{model.TelephoneNumber}",
                    Contact2 = $"{model.FinanceContact}/{model.FinanceContactPhone}/{model.FinanceContactEmail}",
                    AddDate = model.AddDate,
                    AddUserId = model.AddUserId,
                    LastDate = model.AddDate,
                    LastUserId = model.AddUserId,
                    //LabelId = 1,
                    Display = 1
                });

                if (result < 1)
                {
                    trans.Rollback();
                    return "供应商主表插入失败";
                }

                //插入供应商副表
                model.VendorId = result;
                result = Insert(model);
                if (result < 1)
                {
                    trans.Rollback();
                    return "供应商副表插入失败";
                }

                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }

            return "0";
        }

    }
}
