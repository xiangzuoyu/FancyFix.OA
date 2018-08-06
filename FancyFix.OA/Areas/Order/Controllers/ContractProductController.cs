using FancyFix.OA.Filter;
using FancyFix.OA.Model;
using FancyFix.Tools.Config;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tools.Tool;

namespace FancyFix.OA.Areas.Order.Controllers
{
    public class ContractProductController : Base.BaseAdminController
    {
        public ActionResult List(int id)
        {
            ViewBag.contractId = id;
            return View();
        }

        //[PermissionFilter("/order/batchproduct/list")]
        [ValidateInput(false)]
        public JsonResult PageList(int page, int pagesize)
        {
            long records = 0;
            int contractId = RequestInt("contractid");

            var list = Bll.BllOrder_ContractProduct.PageList(contractId, page, pagesize, out records);
            return BspTableJson(list, list.Count);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            Order_ContractProduct model = null;
            int contractId = RequestInt("contractId");
            if (contractId == 0) return LayerAlertErrorAndClose("请选择一个合同！");
            var contract = Bll.BllOrder_Contract.First(o => o.Id == contractId);
            if (contract == null) return LayerAlertErrorAndClose("合同不存在！");

            if (id == 0)
            {
                model = new Order_ContractProduct();
            }
            else
            {
                model = Bll.BllOrder_ContractProduct.First(o => o.Id == id);
                if (model == null) return LayerAlertErrorAndClose("记录不存在！");
            }
            ViewBag.contract = contract;
            return View(model);
        }

        //[PermissionFilter("/order/batchproduct/edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(Order_ContractProduct model)
        {
            int contractId = RequestInt("contractId");
            int quantity = RequestInt("quantity");
            decimal price = RequestDecimal("price");
            string name = RequestString("name");

            if (contractId == 0) return LayerAlertErrorAndReturn("请选择一个合同！");
            var contract = Bll.BllOrder_Contract.First(o => o.Id == contractId);
            if (contract == null) return LayerAlertErrorAndClose("合同不存在！");
            if (string.IsNullOrEmpty(name)) return LayerAlertErrorAndClose("请输入产品名称！");

            if (model.Id > 0)
            {
                model = Bll.BllOrder_ContractProduct.First(o => o.Id == model.Id);
                if (model == null) return Response404();
            }
            else
            {
                model = new Order_ContractProduct();
            }

            model.ContractId = contractId;
            model.Name = name;
            model.Price = price;
            model.Quantity = quantity;
            model.TotalPrice = quantity * price;
            model.AddTime = DateTime.Now;
            model.AdminId = MyInfo.Id;

            if ((model.Id > 0 ? Bll.BllOrder_ContractProduct.Update(model) : Bll.BllOrder_ContractProduct.Insert(model)) > 0)
                return LayerAlertSuccessAndRefresh((model.Id > 0 ? "修改" : "添加") + "成功");
            else
                return LayerAlertErrorAndReturn((model.Id > 0 ? "修改" : "添加") + "失败");
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllOrder_ContractProduct.Delete(o => o.Id == id) > 0 });
        }

        /// <summary>
        /// 导入产品
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportProduct(HttpPostedFileBase file)
        {
            try
            {
                int contractId = RequestInt("contractId");
                if (contractId == 0) return LayerAlertErrorAndReturn("请选择一个合同！");
                var contract = Bll.BllOrder_Contract.First(o => o.Id == contractId);
                if (contract == null) return LayerAlertErrorAndClose("合同不存在！");

                Tools.Config.UploadConfig config = UploadProvice.Instance();
                SiteOption option = config.SiteOptions["local"];
                string filePath = option.Folder + config.Settings["file"].FilePath + DateTime.Now.ToString("yyyyMMddHHmmss")
                        + (file.FileName.IndexOf(".xlsx") > 0 ? ".xlsx" : ".xls");

                string result = FileHelper.ValicationAndSaveFileToPath(file, filePath);
                if (result != "0")
                    return MessageBoxAndReturn($"上传失败，{result}！");

                var sheet = Tools.Tool.ExcelHelper.ReadExcel(filePath);
                if (sheet == null)
                    return MessageBoxAndReturn($"Excel读取失败，请检查格式！");

                int updateCount = 0;
                List<Order_ContractProduct> list = new List<Order_ContractProduct>();
                IRow row;
                for (int i = 1; i <= sheet.LastRowNum; i++)  //从第二行开始读取
                {
                    row = sheet.GetRow(i);   //第i行数据  
                    if (row != null)
                    {
                        var name = row.GetCell(0).ToString();
                        if (!string.IsNullOrEmpty(name))
                        {
                            var price = row.GetCell(1).ToString().ToDecimal();
                            var quantity = row.GetCell(2).ToString().ToInt32();
                            var model = Bll.BllOrder_ContractProduct.First(o => o.Name == name && o.ContractId == contractId);
                            if (model != null)
                            {
                                model.Quantity += quantity;
                                model.Price += price;
                                model.TotalPrice = model.Quantity * model.Price;
                                if (Bll.BllOrder_ContractProduct.Update(model) > 0)
                                    updateCount++;
                            }
                            else
                            {
                                list.Add(new Order_ContractProduct()
                                {
                                    ContractId = contractId,
                                    Name = name,
                                    Price = price,
                                    Quantity = quantity,
                                    TotalPrice = price * quantity,
                                    AddTime = DateTime.Now,
                                    AdminId = MyInfo.Id
                                });
                            }
                        }
                    }
                }
                if (Bll.BllOrder_ContractProduct.Insert(list) > 0 || updateCount > 0)
                    return MessageBoxAndReturn("导入成功！");
                else
                    return MessageBoxAndReturn("导入失败，请联系管理员！");
            }
            catch (Exception ex)
            {
                return MessageBoxAndReturn("导入失败，请联系管理员！");
            }
        }
    }
}