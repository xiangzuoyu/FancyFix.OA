using FancyFix.OA.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FancyFix.OA.Model;
using FancyFix.OA.Filter;

namespace FancyFix.OA.Areas.Supplier.Controllers
{
    public class RawMaterialController : BaseAdminController
    {
        // GET: Supplier/Rawmaterial
        #region 加载列表
        public ActionResult List()
        {
            return View();
        }

        //[PermissionFilter("/supplier/rawmaterial/list")]
        public JsonResult PageList(int page = 0, int pagesize = 0)
        {
            long records = 0;
            //Sql注入检测
            string files = Tools.Usual.Utils.CheckSqlKeyword(RequestString("files"));
            string key = Tools.Usual.Utils.CheckSqlKeyword(RequestString("key")).Trim();
            var list = Bll.BllSupplier_RawMaterial.PageList(page, pagesize, out records, files, key);
            return BspTableJson(list, records);
        }

        #region 编辑


        public ActionResult Save(int id = 0)
        {
            Supplier_RawMaterial model = null;
            if (id > 0)
            {
                model = Bll.BllSupplier_RawMaterial.First(o => o.Id == id && o.Display != 2);
                if (model == null)
                    return LayerAlertSuccessAndRefresh("加载原材料信息失败，未找到该原材料");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Supplier_RawMaterial supplierRawMaterial)
        {
            Supplier_RawMaterial model = Bll.BllSupplier_RawMaterial.First(o => o.Id == supplierRawMaterial.Id && o.Display != 2 && o.Id > 0)
                ?? new Supplier_RawMaterial();
            model.BU = supplierRawMaterial.BU;
            model.SAPCode = supplierRawMaterial.SAPCode;
            model.Description = supplierRawMaterial.Description;
            model.Category = supplierRawMaterial.Category;
            model.LeadBuyer = supplierRawMaterial.LeadBuyer;
            model.Currency = supplierRawMaterial.Currency;

            model.LastDate = DateTime.Now;
            model.LastUserId = MyInfo.Id;
            bool isok = false;
            //没有ID就新增，反之修改
            if (supplierRawMaterial.Id < 1)
            {
                model.AddDate = model.LastDate;
                model.AddUserId = model.LastUserId;
                model.Display = 1;

                //防止原材料代码重复
                var model2 = Bll.BllSupplier_RawMaterial.First(o => o.SAPCode == supplierRawMaterial.SAPCode && o.Display != 2);
                if (model2 != null)
                    return LayerAlertErrorAndReturn("添加原材料信息失败，原材料代码已存在，请修改");

                isok = Bll.BllSupplier_RawMaterial.Insert(model) > 0;
            }
            else
            {
                isok = Bll.BllSupplier_RawMaterial.Update(model) > 0;
            }

            return LayerMsgSuccessAndRefresh("保存" + (isok ? "成功" : "失败"));
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllSupplier_RawMaterial.HideModel(id, MyInfo.Id) > 0 });
        }

        //[PermissionFilter("/supplier/rawmaterial/delete")]
        [HttpPost]
        public JsonResult DeleteBatch(List<Supplier_RawMaterial> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllSupplier_RawMaterial.HideList(list, MyInfo.Id) > 0 });
        }
        #endregion

        #endregion
    }
}