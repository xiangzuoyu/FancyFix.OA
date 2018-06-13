using Dos.ORM;
using FancyFix.OA.Model;
using System;

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
                    Contact1 = $"{model.MainContactor}/{model.TelephoneNumber}/{model.EmailForPO}",
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

        public static bool SaveVendorInfo(Supplier_VendorInfo vendorInfo, int userid)
        {
            Supplier_VendorInfo model = First(o => o.Id == vendorInfo.Id && o.Dispaly != 2) ?? new Supplier_VendorInfo();
            model.VendorId = vendorInfo.VendorId;
            model.Owners = vendorInfo.Owners;
            model.TableNumber = vendorInfo.TableNumber;
            model.Version = vendorInfo.Version;
            model.EffectiveDate = vendorInfo.EffectiveDate;
            model.VersionType = vendorInfo.VersionType;
            model.WriteDate = vendorInfo.WriteDate;
            model.CompanyEnglishName = vendorInfo.CompanyEnglishName;
            model.PostCode1 = vendorInfo.PostCode1;
            model.PurchaseOrderAddressedTo = vendorInfo.PurchaseOrderAddressedTo;
            model.PostCode2 = vendorInfo.PostCode2;
            model.FaxNumber = vendorInfo.FaxNumber;
            model.EmailForPO = vendorInfo.EmailForPO;
            model.TaxNumber = vendorInfo.TaxNumber;
            model.TaxRate = vendorInfo.TaxRate;
            model.OrderCurrency = vendorInfo.OrderCurrency;
            model.PaymentTerms = vendorInfo.PaymentTerms;
            model.Incoterms = vendorInfo.Incoterms;
            model.BankKey = vendorInfo.BankKey;
            model.SwiftCode = vendorInfo.SwiftCode;
            model.NameOfBank = vendorInfo.NameOfBank;
            model.ACNo = vendorInfo.ACNo;
            model.FormOfBusiness = vendorInfo.FormOfBusiness;
            model.BusinessRegistrationNumber = vendorInfo.BusinessRegistrationNumber;
            model.RegisteredCapital = vendorInfo.RegisteredCapital;
            model.CertificateOfCorporation = vendorInfo.CertificateOfCorporation;
            model.ProductsOrServiceSales = vendorInfo.ProductsOrServiceSales;
            model.MOQ = vendorInfo.MOQ;
            model.LeadTime = vendorInfo.LeadTime;
            model.ManagementMenberName1 = vendorInfo.ManagementMenberName1;
            model.ManagementMenberTitle1 = vendorInfo.ManagementMenberTitle1;
            model.ManagementMenberName2 = vendorInfo.ManagementMenberName2;
            model.ManagementMenberTitle2 = vendorInfo.ManagementMenberTitle2;
            model.ManagementMenberName3 = vendorInfo.ManagementMenberName3;
            model.ManagementMenberTitle3 = vendorInfo.ManagementMenberTitle3;
            model.ManagementMenberName4 = vendorInfo.ManagementMenberName4;
            model.ManagementMenberTitle4 = vendorInfo.ManagementMenberTitle4;
            model.ContactPersonsName1 = vendorInfo.ContactPersonsName1;
            model.ContactPersonsTitle1 = vendorInfo.ContactPersonsTitle1;
            model.ContactPersonsTel1 = vendorInfo.ContactPersonsTel1;
            model.ContactPersonsFax1 = vendorInfo.ContactPersonsFax1;
            model.ContactPersonsEmail1 = vendorInfo.ContactPersonsEmail1;
            model.ContactPersonsName2 = vendorInfo.ContactPersonsName2;
            model.ContactPersonsTitle2 = vendorInfo.ContactPersonsTitle2;
            model.ContactPersonsTel2 = vendorInfo.ContactPersonsTel2;
            model.ContactPersonsFax2 = vendorInfo.ContactPersonsFax2;
            model.ContactPersonsEmail2 = vendorInfo.ContactPersonsEmail2;
            model.LastUserId = userid;
            model.LastDate = DateTime.Now;

            bool isok = false;
            if (vendorInfo.Id < 1)
            {
                model.AddDate = model.LastDate;
                model.AddUserId = model.LastUserId;
                model.Dispaly = 1;

                isok = Insert(model) > 0;
            }
            else
                isok = Update(model) > 0;

            return isok;
        }

        public static int HideModel(int vendorId, int myuserId)
        {
            //先隐藏供应商副表
            var vendorInfo = First(o => o.VendorId == vendorId);
            if (vendorInfo == null)
                return 0;

            vendorInfo.Dispaly = 2;
            vendorInfo.LastDate = DateTime.Now;
            vendorInfo.LastUserId = myuserId;
            return Update(vendorInfo);
        }
    }
}
