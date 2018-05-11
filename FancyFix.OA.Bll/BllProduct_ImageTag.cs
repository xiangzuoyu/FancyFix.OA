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
    public class BllProduct_ImageTag : BllSys_Class<Product_ImageTag>
    {
        public new static BllProduct_ImageTag Instance()
        {
            return new BllProduct_ImageTag();
        }

        public static List<Product_ImageTag> PageList(string tag, int page, int pageSize, out long records)
        {
            var where = new Where<Product_ImageTag>();
            if (!string.IsNullOrEmpty(tag))
                where.And(o => o.Tag.Like(tag));

            var p = Db.Context.From<Product_ImageTag>()
                 .Select(o => new { o.Id, o.AddTime, o.Tag })
                 .Where(where);

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
            var where = new Where<Product_ImageTag>();
            if (thisId > 0) where.And(o => o.Id != thisId);
            where.And(o => o.Tag == tag);
            int count = Db.Context.From<Product_ImageTag>().Where(where).Count();
            return count > 0;
        }
    }
}
