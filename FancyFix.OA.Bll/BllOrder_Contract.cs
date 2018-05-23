using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using FancyFix.Tools.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllOrder_Contract : BllSys_Class<Order_Contract>
    {
        public new static BllOrder_Contract Instance()
        {
            return new BllOrder_Contract();
        }

        public static List<Order_Contract> PageList(string contractNo, int page, int pageSize, out long records)
        {
            var where = new Where<Order_Contract>();
            if (!string.IsNullOrEmpty(contractNo))
                where.And(o => o.ContractNo.StartsWith(contractNo));

            var p = Db.Context.From<Order_Contract>().Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="thisId"></param>
        /// <returns></returns>
        public static bool IsExist(string tag, int thisId)
        {
            var where = new Where<Order_Contract>();
            if (thisId > 0) where.And(o => o.Id != thisId);
            int count = Db.Context.From<Order_Contract>().Where(where).Count();
            return count > 0;
        }

        /// <summary>
        /// 判断同分类下是否存在相同合同号
        /// </summary>
        /// <param name="code"></param>
        /// <param name="parId"></param>
        /// <returns></returns>
        public static bool CheckContractNo(string contractNo)
        {
            return Any(o => o.ContractNo == contractNo);
        }

        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteContract(int id)
        {
            using (var trans = Db.Context.BeginTransaction())
            {
                int rows = trans.Delete<Order_Contract>(o => o.Id == id);
                if (rows == 0)
                {
                    trans.Rollback();
                    return false;
                }
                trans.Delete<Order_Batch>(o => o.ContractId == id);
                trans.Delete<Order_BatchProduct>(o => o.ContractId == id);
                trans.Commit();
                return true;
            }
        }

        /// <summary>
        /// 获取产品费用计算结果
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public static DataTable GetResult(int contractId)
        {
            string sql = $"select Name,sum(Quantity) Quantity,sum(Cost) Cost,sum(TotalCost) TotalCost,count(*) Count from Order_BatchProduct where ContractId={contractId} group by Name";
            var dt = Db.Context.FromSql(sql).ToDataTable();
            return dt;
        }
    }
}
