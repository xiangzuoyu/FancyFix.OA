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
            //先隐藏供应商副表
            var vendorInfo = BllSupplier_VendorInfo.First(o => o.VendorId == id);
            if (vendorInfo != null)
            {
                vendorInfo.Dispaly = 2;
                vendorInfo.LastDate = DateTime.Now;
                vendorInfo.LastUserId = myuserId;
                BllSupplier_VendorInfo.Update(vendorInfo);
            }

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

        public static Supplier_List SupplierIsRepeat(int Id, string code, string name)
        {
            var where = new Where<Supplier_List>();

            if (Id > 0)
                where.And(o => o.Id != Id);
            if (!string.IsNullOrEmpty(code))
                where.And(o => o.Code == code);
            if (!string.IsNullOrEmpty(name))
                where.And(o => o.Name == name);
            where.And(o => o.Display != 2);

            return Db.Context.From<Supplier_List>()
                .Where(where).First();
        }

        public static DataTable NewExportDt(string[] arr)
        {
            DataTable dt = new DataTable();
            foreach (var item in arr)
            {
                switch (item)
                {
                    case "Code":
                        dt.Columns.Add("供应商代码", typeof(String));
                        break;
                    case "Name":
                        dt.Columns.Add("供应商名称", typeof(String));
                        break;
                    case "SupplierAb":
                        dt.Columns.Add("供应商名称缩写", typeof(String));
                        break;
                    case "SupplierType":
                        dt.Columns.Add("供应商类型（RM/PM/FG/Parts/Convert)", typeof(String));
                        break;
                    case "BusinessScope":
                        dt.Columns.Add("经营范围/供应物料", typeof(String));
                        break;
                    case "Contact1":
                        dt.Columns.Add("联系人（1）/电话/邮箱", typeof(String));
                        break;
                    case "Contact2":
                        dt.Columns.Add("联系人（2）/电话/邮箱", typeof(String));
                        break;
                    case "Site":
                        dt.Columns.Add("网址", typeof(String));
                        break;
                    case "Address":
                        dt.Columns.Add("地址", typeof(String));
                        break;
                    case "StartDate":
                        dt.Columns.Add("合作时间（起止）", typeof(String));
                        break;
                    case "LabelId":
                        dt.Columns.Add("合格/黑名单/潜在", typeof(String));
                        break;
                    case "Accountdate":
                        dt.Columns.Add("账期", typeof(String));
                        break;
                    case "Note":
                        dt.Columns.Add("备注", typeof(String));
                        break;
                }
            }

            return dt;
        }

    }
}
