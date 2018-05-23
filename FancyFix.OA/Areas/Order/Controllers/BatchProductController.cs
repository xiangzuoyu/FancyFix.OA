using FancyFix.OA.Filter;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            model.TotalCost = model.UnitValue.Value * model.Quantity.Value * batch.UnitPerCost.Value;
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

    }
}