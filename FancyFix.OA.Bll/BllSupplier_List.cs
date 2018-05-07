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

        public static IEnumerable<Supplier_List> PageList(int page, int pageSize, out long records, int labelId, string file, string key)
        {
            var where = new Where<Supplier_List>();
            where.And(o => o.Display != 2);
            if (labelId > 0)
                where.And(o => o.LabelId == labelId);


            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(key))
            {
                file = CheckSqlValue(file);
                key = CheckSqlKeyword(key);
                where.And(string.Format(" {0} like '%{1}%' ", file, key));
            }

            var p = Db.Context.From<Supplier_List>()
                .Where(where);
            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        public static int HideModel(int id, int myuserId)
        {
            var model = First(o => o.Id == id);
            if (model == null)
                return 0;
            model.Display = 2;
            model.LastDate = DateTime.Now;
            model.LastUserId = myuserId;
            return Update(model);
        }

        public static int HideList(IEnumerable<Supplier_List> list, int myuserId)
        {
            foreach (var item in list)
                HideModel(item.Id, myuserId);

            return 1;
        }

        public static int UpdateLabel(IEnumerable<Supplier_List> list, int labelId, int myuserId)
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

        public static IEnumerable<Supplier_List> GetList(int top, string cols, string where, string orderBy, string files, string key)
        {
            return GetSelectList(top, cols, where, orderBy);
        }

        public static Supplier_List SupplierIsRepeat(string code, string name)
        {
            var where = new Where<Supplier_List>();
            if (!string.IsNullOrEmpty(code))
                where.And(o => o.Code == code);
            if (!string.IsNullOrEmpty(name))
                where.And(o => o.Name == name);

            return Db.Context.From<Supplier_List>()
                .Where(where).First();
        }

    }
}
