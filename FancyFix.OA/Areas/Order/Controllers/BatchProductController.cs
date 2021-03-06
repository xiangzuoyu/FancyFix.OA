﻿using FancyFix.OA.Filter;
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
    public class BatchProductController : Base.BaseAdminController
    {

        //[PermissionFilter("/order/batchproduct/list")]
        [ValidateInput(false)]
        public JsonResult GetList(int id)
        {
            var list = Bll.BllOrder_BatchProduct.GetListByBatchId(id);
            return BspTableJson(list, list.Count);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            Order_BatchProduct model = null;
            int batchid = RequestInt("batchid");
            if (batchid == 0) return LayerAlertErrorAndClose("请选择一个批次！");
            var batch = Bll.BllOrder_Batch.First(o => o.Id == batchid);
            if (batch == null) return LayerAlertErrorAndClose("批次不存在！");

            if (id == 0)
            {
                model = new Order_BatchProduct();
            }
            else
            {
                model = Bll.BllOrder_BatchProduct.First(o => o.Id == id);
                if (model == null) return LayerAlertErrorAndClose("记录不存在！");
            }
            ViewBag.batch = batch;
            return View(model);
        }

        //[PermissionFilter("/order/batchproduct/edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(Order_BatchProduct model)
        {
            int batchId = RequestInt("batchid");
            if (batchId == 0) return LayerAlertErrorAndReturn("请选择一个批次！");
            var batch = Bll.BllOrder_Batch.First(o => o.Id == batchId);
            if (batch == null) return LayerAlertErrorAndClose("批次不存在！");
            int quantity = RequestInt("quantity");

            if (model.Id > 0)
            {
                model = Bll.BllOrder_BatchProduct.First(o => o.Id == model.Id);
                if (model == null) return Response404();
            }
            else
            {
                model = new Order_BatchProduct();
            }
            int quantityCount = Bll.BllOrder_BatchProduct.GetQuantity(batchId, model.Id);
            if (quantity + quantityCount > batch.Quantity)
                return LayerAlertErrorAndClose("超出批次设置的总数，请修改后重新提交！");

            model.ContractId = batch.ContractId;
            model.BatchId = batchId;
            model.Name = RequestString("name");
            model.UnitValue = RequestDecimal("unitvalue");
            model.Quantity = quantity;
            model.UnitPerCost = batch.UnitPerCost;
            model.Cost = model.UnitValue.Value * batch.UnitPerCost.Value;
            model.TotalCost = model.Cost * model.Quantity.Value;
            model.Unit = batch.Unit;
            model.AddTime = DateTime.Now;
            model.AdminId = MyInfo.Id;

            if ((model.Id > 0 ? Bll.BllOrder_BatchProduct.Update(model) : Bll.BllOrder_BatchProduct.Insert(model)) > 0)
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
            return Json(new { result = Bll.BllOrder_BatchProduct.Delete(o => o.Id == id) > 0 });
        }

        /// <summary>
        /// 导入批次产品
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportProduct(HttpPostedFileBase file)
        {
            try
            {
                int batchId = RequestInt("batchId");
                if (batchId == 0) return LayerAlertErrorAndReturn("请选择一个批次！");
                var batch = Bll.BllOrder_Batch.First(o => o.Id == batchId);
                if (batch == null) return LayerAlertErrorAndClose("批次不存在！");

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
                List<Order_BatchProduct> list = new List<Order_BatchProduct>();
                IRow row;
                for (int i = 1; i <= sheet.LastRowNum; i++)  //从第二行开始读取
                {
                    row = sheet.GetRow(i);   //第i行数据  
                    if (row != null)
                    {
                        var name = row.GetCell(0).ToString();
                        if (!string.IsNullOrEmpty(name))
                        {
                            var unitvalue = row.GetCell(1).ToString().ToDecimal();
                            var quantity = row.GetCell(2).ToString().ToInt32();
                            var model = Bll.BllOrder_BatchProduct.First(o => o.Name == name && o.BatchId == batchId);
                            if (model != null)
                            {
                                model.Quantity += quantity;
                                model.UnitValue += unitvalue;
                                model.Cost = model.UnitValue * batch.UnitPerCost.Value;
                                model.TotalCost = model.Cost * model.Quantity;
                                if (Bll.BllOrder_BatchProduct.Update(model) > 0)
                                    updateCount++;
                            }
                            else
                            {
                                list.Add(new Order_BatchProduct()
                                {
                                    ContractId = batch.ContractId,
                                    BatchId = batchId,
                                    Name = name,
                                    UnitValue = unitvalue,
                                    Quantity = quantity,
                                    UnitPerCost = batch.UnitPerCost,
                                    Cost = unitvalue * batch.UnitPerCost.Value,
                                    TotalCost = unitvalue * batch.UnitPerCost.Value * quantity,
                                    Unit = batch.Unit,
                                    AddTime = DateTime.Now,
                                    AdminId = MyInfo.Id
                                });
                            }
                        }
                    }
                }
                if (Bll.BllOrder_BatchProduct.Insert(list) > 0 || updateCount > 0)
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