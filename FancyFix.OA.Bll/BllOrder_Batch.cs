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
    public class BllOrder_Batch : BllSys_Class<Order_Batch>
    {
        public new static BllOrder_Batch Instance()
        {
            return new BllOrder_Batch();
        }

        public static List<Order_Batch> PageList(int contractId, int page, int pageSize, out long records)
        {
            var where = new Where<Order_Batch>();
            if (contractId > 0)
                where.And(o => o.ContractId == contractId);
            var p = Db.Context.From<Order_Batch>().Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="thisId"></param>
        /// <returns></returns>
        public static bool IsExist(string tag, int thisId)
        {
            var where = new Where<Order_Batch>();
            if (thisId > 0) where.And(o => o.Id != thisId);
            int count = Db.Context.From<Order_Batch>().Where(where).Count();
            return count > 0;
        }

        /// <summary>
        /// 删除批次
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteBatch(int id)
        {
            using (var trans = Db.Context.BeginTransaction())
            {
                int rows = trans.Delete<Order_Batch>(o => o.Id == id);
                if (rows == 0)
                {
                    trans.Rollback();
                    return false;
                }
                trans.Delete<Order_BatchProduct>(o => o.BatchId == id);
                trans.Commit();
                return true;
            }
        }

        /// <summary>
        /// 获取批次
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public static int GetBatch(int contractId)
        {
            return Db.Context.FromSql("select max(batch) from Order_Batch where ContractId=" + contractId).ToScalar<int>() + 1;
        }

        /// <summary>
        /// 根据Ids获取列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<Order_Batch> GetListByIds(List<int> ids)
        {
            var where = new Where<Order_Batch>();
            where.And(o => o.Id.In(ids));
            return Db.Context.From<Order_Batch>().Where(where).OrderBy(o => o.Id).ToList();
        }
    }
}
