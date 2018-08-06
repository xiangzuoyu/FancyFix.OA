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
    public class ContractController : Base.BaseAdminController
    {
        public ActionResult List()
        {
            return View();
        }

        //[PermissionFilter("/order/contract/list")]
        [ValidateInput(false)]
        public JsonResult PageList(int page, int pagesize)
        {
            long records = 0;
            string contractNo = RequestString("contractno");

            var list = Bll.BllOrder_Contract.PageList(contractNo, page, pagesize, out records);
            return BspTableJson(list, records);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            Order_Contract model = null;
            if (id == 0)
            {
                model = new Order_Contract();
            }
            else
            {
                model = Bll.BllOrder_Contract.First(o => o.Id == id);
                if (model == null) return LayerAlertErrorAndClose("记录不存在！");
            }
            return View(model);
        }

        //[PermissionFilter("/order/contract/edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(Order_Contract model)
        {
            string contractNo = RequestString("contractno");
            if (string.IsNullOrEmpty(contractNo)) return LayerAlertErrorAndReturn("请填写合同号！");

            if (model.Id > 0)
            {
                model = Bll.BllOrder_Contract.First(o => o.Id == model.Id);
                if (model == null) return Response404();
            }
            else
            {
                model = new Order_Contract();
            }

            if ((model.Id == 0 || model.ContractNo != contractNo) && Bll.BllOrder_Contract.CheckContractNo(contractNo))
                return LayerAlertErrorAndReturn("合同号已存在！");

            model.ContractNo = contractNo;
            model.AddTime = DateTime.Now;
            model.AdminId = MyInfo.Id;

            if ((model.Id > 0 ? Bll.BllOrder_Contract.Update(model) : Bll.BllOrder_Contract.Insert(model)) > 0)
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
            return Json(new { result = Bll.BllOrder_Contract.DeleteContract(id) });
        }

        /// <summary>
        /// 计算结果展示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowResult(int id)
        {
            var contract = Bll.BllOrder_Contract.First(o => o.Id == id);
            if (contract == null) return LayerAlertErrorAndReturn("合同不存在！");
            var dt = Bll.BllOrder_Contract.GetResult(contract.Id);
            var prolist = Bll.BllOrder_ContractProduct.GetListByContractId(contract.Id);
            foreach (DataRow dr in dt.Rows)
            {
                string name = dr["Name"].ToString();
                decimal totalCost = (decimal)dr["TotalCost"];
                decimal cost = (decimal)dr["Cost"];
                decimal quantity = (int)dr["Quantity"];
                int count = (int)dr["Count"];

                //匹配库存列表中的产品，一起核算
                var pro = prolist.Find(o => o.Name == name);
                if (pro != null)
                {
                    totalCost += pro.Quantity.Value * pro.Price.Value;
                    quantity += pro.Quantity.Value;
                    dr["Quantity"] = quantity;
                    dr["TotalCost"] = totalCost;

                    //从库存列表中移除当前匹配项
                    prolist.Remove(pro);
                }
                dr["Cost"] = (totalCost / quantity).ToString("F5");
            }
            //插入其他不在批次中的库存产品
            if (prolist != null && prolist.Count > 0)
            {
                foreach (var item in prolist)
                {
                    var newRow = dt.NewRow();
                    newRow["Name"] = item.Name;
                    newRow["Quantity"] = item.Quantity;
                    newRow["Cost"] = item.Price;
                    newRow["TotalCost"] = item.TotalPrice;
                    dt.Rows.Add(newRow);
                }
            }
            return View(dt);
        }
    }
}