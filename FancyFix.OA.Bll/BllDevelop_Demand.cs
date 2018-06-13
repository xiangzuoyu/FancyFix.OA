using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Bll.Model;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllDevelop_Demand : ServiceBase<Develop_Demand>
    {
        public static BllDevelop_Demand Instance()
        {
            return new BllDevelop_Demand();
        }

        public static List<Develop_Demand> PageList(DemandSearch demandSearch, int page, int pageSize, out long records)
        {
            var deptId = demandSearch.DeptId;
            var type = demandSearch.DemandType;
            var where = new Where<Develop_Demand>();
            if (deptId > 0)
                where.And(o => o.DeptId == deptId);
            if (type > 0)
                where.And(o => o.Type == type);
            if (!demandSearch.IsAdmin && !demandSearch.IsShow)
            {
                where.And(o => o.CreateUserId == demandSearch.LoginUserId || o.ExecutorId==demandSearch.LoginUserId);
            }
            var p = Db.Context.From<Develop_Demand>()
                .Where(where);
            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        public static bool SetStatus(int id, int status)
        {
            string sql =string.Format("UPDATE Develop_Demand SET Status={0},UpdateTime='{2}',CompleteTime='{2}' where Id={1}", status,id,DateTime.Now);
            return Db.Context.FromSql(sql).ExecuteNonQuery() > 0;

        }

        public static bool SetExecutorId(int id,int executorId)
        {
            string sql = string.Format("UPDATE Develop_Demand SET ExecutorId={0},UpdateTime='{2}',ExecutorTime='{2}' where Id={1}", executorId, id,DateTime.Now);
            return Db.Context.FromSql(sql).ExecuteNonQuery() > 0;
        }

    }
}
