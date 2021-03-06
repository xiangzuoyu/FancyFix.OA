﻿using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using FancyFix.Tools.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllProduct_Image : ServiceBase<Product_Image>
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public static List<Product_Image> PageList(int proId, byte type, int page, int pageSize, out long records)
        {
            var where = new Where<Product_Image>();
            where.And(o => o.ProId == proId);
            if (type > 0)
                where.And(o => o.Type == type);
            var p = Db.Context.From<Product_Image>()
                 .Select<Mng_User>((a, b) => new { a.Id, a.ImagePath, a.ImageExt, a.AddTime, a.Tag, a.Type })
                 .Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        /// <summary>
        /// 根据Md5获取图片路径
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static bool GetImageUrlByMd5(string md5, ref string url, byte type)
        {
            var model = FirstSelect(o => o.Md5 == md5 && o.Type == type, o => o.ImagePath, o => o.Id, "desc");
            if (model != null && !string.IsNullOrEmpty(model.ImagePath))
            {
                url = model.ImagePath;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除所有产品图片
        /// </summary>
        public static void DeletePics(int proId)
        {
            var imagelist = Query(o => o.ProId == proId);
            if (imagelist != null && imagelist.Count > 0)
                Delete(imagelist);
        }

        /// <summary>
        /// 获取图片列表
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Product_Image> GetList(int proId, string tag, byte type)
        {
            var where = new Where<Product_Image>();
            where.And(o => o.ProId == proId);
            if (type > 0)
                where.And(o => o.Type == type);
            if (!string.IsNullOrEmpty(tag))
                where.And(o => o.Tag.Like(tag));
            var p = Db.Context.From<Product_Image>()
                 .Select<Mng_User>((a, b) => new { a.Id, a.ImagePath, a.ImageExt, a.AddTime, a.Tag, a.Type })
                 .Where(where);
            return p.OrderByDescending(o => o.Id).ToList();
        }
    }
}
