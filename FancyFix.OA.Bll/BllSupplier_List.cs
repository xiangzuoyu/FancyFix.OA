using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllSupplier_List:ServiceBase<Supplier_List>
    {
        public static BllSupplier_List Instance()
        {
            return new BllSupplier_List();
        }

        public static IEnumerable<Supplier_List> PageList(int page,int pageSize,out long records,int display)
        {
            var where = new Where<Supplier_List>();
            //where.And(o => o.Display != 4);
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
    }
}
