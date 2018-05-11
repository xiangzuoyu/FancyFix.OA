using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllProduct_Pattern : ServiceBase<Product_Pattern>
    {
        public static BllProduct_Pattern Instance()
        {
            return new BllProduct_Pattern();
        }

        public static List<Product_Pattern> PageList(string name, string code, int page, int pageSize, out long records)
        {
            var where = new Where<Product_Pattern>();
            if (!string.IsNullOrEmpty(name))
                where.And(Product_Pattern._.PatternName.Like(name));
            if (!string.IsNullOrEmpty(code))
                where.And(Product_Pattern._.PatternCode.BeginWith(code));

            var p = Db.Context.From<Product_Pattern>()
                 .Select(o => new { o.Id, o.PatternName, o.PatternCode, o.FirstPic, o.AddTime, o.IsShow })
                 .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetShow(int id)
        {
            if (id == 0) return false;
            var model = FirstSelect(o => o.Id == id, o => new { o.Id, o.IsShow });
            if (model == null || model.Id == 0) return false;

            model.IsShow = !model.IsShow;
            return Update(model, o => o.Id == model.Id) > 0;
        }

        /// <summary>
        /// 获取下拉框
        /// </summary>
        /// <returns></returns>
        public static string GetOptions(int id = 0)
        {
            string options = string.Empty;
            var list = Db.Context.From<Product_Pattern>()
                .Where(o => o.IsShow == true)
                .Select(o => new { o.Id, o.PatternName }).OrderBy(o => o.Id).ToList();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    options += $"<option value='{item.Id}' {(id == item.Id ? "selected" : "")}>{item.PatternName}</option>";
                }
            }
            return options;
        }

        /// <summary>
        /// 获取图案名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetPatternName(int id)
        {
            return FirstSelect(o => o.Id == id, o => o.PatternName)?.PatternName ?? "";
        }

        /// <summary>
        /// 判断同分类下是否存在相同Code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="parId"></param>
        /// <returns></returns>
        public static bool CheckCode(string code)
        {
            return Any(o => o.PatternCode == code);
        }
    }
}
