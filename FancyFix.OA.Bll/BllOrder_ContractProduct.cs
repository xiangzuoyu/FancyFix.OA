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
    public class BllOrder_ContractProduct : BllSys_Class<Order_ContractProduct>
    {
        public new static BllOrder_ContractProduct Instance()
        {
            return new BllOrder_ContractProduct();
        }

        public static List<Order_ContractProduct> PageList(int contractId, int page, int pageSize, out long records)
        {
            var where = new Where<Order_ContractProduct>();
            if (contractId > 0)
                where.And(o => o.ContractId == contractId);
            var p = Db.Context.From<Order_ContractProduct>().Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderBy(o => o.Id).ToList();
        }

        public static List<Order_ContractProduct> GetListByContractId(int contractId)
        {
            var where = new Where<Order_ContractProduct>();
            if (contractId > 0)
                where.And(o => o.ContractId == contractId);
            var p = Db.Context.From<Order_ContractProduct>().Where(where);
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
            var where = new Where<Order_ContractProduct>();
            if (thisId > 0) where.And(o => o.Id != thisId);
            int count = Db.Context.From<Order_ContractProduct>().Where(where).Count();
            return count > 0;
        }
    }
}
