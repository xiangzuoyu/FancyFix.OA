using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using FancyFix.Tools.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllOrder_BatchProduct : BllSys_Class<Order_BatchProduct>
    {
        public new static BllOrder_BatchProduct Instance()
        {
            return new BllOrder_BatchProduct();
        }

        public static List<Order_BatchProduct> GetListByBatchId(int batchId)
        {
            var where = new Where<Order_BatchProduct>();
            if (batchId > 0)
                where.And(o => o.BatchId == batchId);
            var p = Db.Context.From<Order_BatchProduct>().Where(where);
            return p.OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="thisId"></param>
        /// <returns></returns>
        public static bool IsExist(string tag, int thisId)
        {
            var where = new Where<Order_BatchProduct>();
            if (thisId > 0) where.And(o => o.Id != thisId);
            int count = Db.Context.From<Order_BatchProduct>().Where(where).Count();
            return count > 0;
        }

        /// <summary>
        /// 获取批次已有产品总数
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public static int GetQuantity(int batchId, int id)
        {
            string where = "BatchId=" + batchId;
            if (id > 0)
                where += " and Id<>" + id;
            return Db.Context.FromSql("select sum(quantity) from Order_BatchProduct where " + where).ToScalar<int>();
        }

        /// <summary>
        /// 修改产品信息
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public static bool EditProInfo(Order_Batch batch)
        {
            if (batch == null)
                return false;

            var list = BllOrder_BatchProduct.Query(o => o.BatchId == batch.Id);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.UnitPerCost = batch.UnitPerCost;
                    item.Unit = batch.Unit;
                    item.Cost = item.UnitValue * batch.UnitPerCost;
                    item.TotalCost = item.UnitValue * batch.UnitPerCost * item.Quantity;
                }
                int rows = Update(list);
                if (rows == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
