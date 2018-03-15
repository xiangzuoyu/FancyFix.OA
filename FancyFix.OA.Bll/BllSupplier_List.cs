using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace FancyFix.OA.Bll
{
    public class BllSupplier_List : ServiceBase<Supplier_List>
    {
        public static BllSupplier_List Instance()
        {
            return new BllSupplier_List();
        }

        public static IEnumerable<Supplier_List> PageList(int page, int pageSize, out long records, int display)
        {
            var where = new Where<Supplier_List>();
            where.And(o => o.Display != 2);
            //if (submitterId > 0)
            //    where.And(o => o.SubmitterId == submitterId);
            //if (display > 0)
            //{
            //    if (display == 3)
            //        where.And(o => o.Display == display || o.Display == 5);
            //    else
            //        where.And(o => o.Display == display);
            //}

            var p = Db.Context.From<Supplier_List>()
                //.Select((a)=>new { })
                .Where(where);
            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        public static int HideModel(int id,int myuserId)
        {
            var model = First(o => o.Id == id);
            if (model == null)
                return 0;
            model.Display = 2;
            model.LastDate = DateTime.Now;
            model.LastUserId = myuserId;
            return Update(model);
        }

        public static int HideList(IEnumerable<Supplier_List> list,int myuserId)
        {
            foreach (var item in list)
                HideModel(item.Id,myuserId);

            return 1;
        }

        public static int UpdateLabel(IEnumerable<Supplier_List> list, int labelId,int myuserId)
        {
            foreach (var item in list)
            {
                var model = First(o => o.Id == item.Id);
                if (model == null)
                    continue;
                model.LabelId = labelId;
                model.LastDate = DateTime.Now;
                model.LastUserId = myuserId;
                Update(model);
            }

            return 1;
        }

        public static DataTable GetList(int top, string cols, string where, string orderBy)
        {
            string selectCols = "*";
            if (cols != "")
                selectCols = cols;

            string topStr = "";
            if (top > 0)
                topStr = "top " + top.ToString();
            string whereStr = "";
            if (where != "")
                whereStr += "where " + where;
            string orderByStr = "";
            if (orderBy != "")
                orderByStr = "order by " + orderBy;

            var sql = string.Format("select {0} {1} from {2} {3} {4}", topStr, selectCols, tableName, whereStr, orderByStr);

            var dt = Db.Context.FromSql(sql).ToDataTable();
            return dt;
        }

    }
}
