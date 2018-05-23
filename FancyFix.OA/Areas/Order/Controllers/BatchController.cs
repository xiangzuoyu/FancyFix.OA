using FancyFix.OA.Filter;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Order.Controllers
{
    public class BatchController : Base.BaseAdminController
    {
        public ActionResult List(int id)
        {
            ViewBag.contractId = id;
            return View();
        }

        //[PermissionFilter("/order/batch/list")]
        [ValidateInput(false)]
        public JsonResult PageList(int page, int pagesize)
        {
            long records = 0;
            int contractId = RequestInt("contractid");

            var list = Bll.BllOrder_Batch.PageList(contractId, page, pagesize, out records);
            return BspTableJson(list, records);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            Order_Batch model = null;
            int contractId = RequestInt("contractid");
            if (contractId == 0) return LayerAlertErrorAndClose("请选择一个合同！");
            if (id == 0)
            {
                model = new Order_Batch();
                model.Batch = Bll.BllOrder_Batch.GetBatch(contractId);
            }
            else
            {
                model = Bll.BllOrder_Batch.First(o => o.Id == id);
                if (model == null) return LayerAlertErrorAndClose("记录不存在！");
                model.Batch = model.Batch.Value;
            }
            ViewBag.contractId = contractId;
            return View(model);
        }

        //[PermissionFilter("/order/batch/edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(Order_Batch model)
        {
            int contractId = RequestInt("contractid");
            if (contractId == 0) return LayerAlertErrorAndReturn("请选择一个合同！");

            if (model.Id > 0)
            {
                model = Bll.BllOrder_Batch.First(o => o.Id == model.Id);
                if (model == null) return Response404();
            }
            else
            {
                model = new Order_Batch();
            }

            decimal totalcost = RequestDecimal("totalcost");
            if (totalcost == 0) return LayerAlertErrorAndReturn("请填写物流费用！");
            decimal unitvalue = RequestDecimal("unitvalue");
            if (unitvalue == 0) return LayerAlertErrorAndReturn("请填写总体积或重量！");

            model.ContractId = contractId;
            model.Batch = RequestInt("batch");
            model.TotalCost = RequestDecimal("totalcost");
            model.UnitType = RequestByte("unittype");
            model.UnitValue = RequestDecimal("unitvalue");
            model.Unit = RequestString("unit");
            model.Quantity = RequestInt("quantity");
            model.UnitPerCost = model.TotalCost / (decimal)model.UnitValue;
            model.AddTime = DateTime.Now;
            model.AdminId = MyInfo.Id;

            int rows = 0;
            if (model.Id > 0)
            {
                rows = Bll.BllOrder_Batch.Update(model);
                //相应修改产品的信息
                if (rows > 0)
                    Bll.BllOrder_BatchProduct.EditProInfo(model);
            }
            else
            {
                rows = Bll.BllOrder_Batch.Insert(model);
            }
            if (rows > 0)
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
            return Json(new { result = Bll.BllOrder_Batch.DeleteBatch(id) });
        }

        [HttpGet]
        public void ToExcel(int id)
        {
            if (id > 0)
            {
                var batch = Bll.BllOrder_Batch.First(o => o.Id == id);
                if (batch != null)
                {
                    var contract = Bll.BllOrder_Contract.First(o => o.Id == batch.ContractId)?.ContractNo ?? "";
                    var batchprolist = Bll.BllOrder_BatchProduct.GetListByBatchId(batch.Id);
                    DataTable dt = new DataTable();
                    dt.Columns.Add("合同编号", typeof(String));
                    dt.Columns.Add("批次", typeof(String));
                    dt.Columns.Add("物流总费用", typeof(String));
                    dt.Columns.Add("称重类型", typeof(String));
                    dt.Columns.Add("总体积/重量", typeof(String));
                    dt.Columns.Add("平均费用", typeof(String));
                    dt.Columns.Add("总数量", typeof(String));

                    var row = dt.NewRow();
                    row["合同编号"] = contract;
                    row["批次"] = "批次" + batch.Batch;
                    row["物流总费用"] = batch.TotalCost;
                    row["称重类型"] = "按" + (batch.UnitType == 1 ? "体积" : "重量"); ;
                    row["总体积/重量"] = batch.UnitValue + " " + batch.Unit;
                    row["平均费用"] = batch.UnitPerCost;
                    row["总数量"] = batch.Quantity;
                    dt.Rows.Add(row);

                    if (batchprolist != null && batchprolist.Count > 0)
                    {
                        dt.Rows.Add(dt.NewRow());
                        var row1 = dt.NewRow();
                        row1["合同编号"] = "";
                        row1["批次"] = "产品名称";
                        row1["物流总费用"] = "总费用";
                        row1["称重类型"] = "体积/重量";
                        row1["总体积/重量"] = "总体积/重量";
                        row1["平均费用"] = "平均费用";
                        row1["总数量"] = "数量";
                        dt.Rows.Add(row1);
                        foreach (var item in batchprolist)
                        {
                            var row2 = dt.NewRow();
                            row2["合同编号"] = "";
                            row2["批次"] = item.Name;
                            row2["物流总费用"] = item.TotalCost;
                            row2["称重类型"] = item.UnitValue + " " + item.Unit;
                            row2["总体积/重量"] = (item.UnitValue * item.Quantity).Value.ToString("F5");
                            row2["平均费用"] = item.Cost;
                            row2["总数量"] = item.Quantity;
                            dt.Rows.Add(row2);
                        }
                    }
                    string fileName = $"合同{contract}-批次" + batch.Batch;
                    Tools.Tool.ExcelHelper.ToExcelWeb(dt, fileName, fileName + ".xls");
                }
            }
        }
    }
}