﻿using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllProduct_Files : ServiceBase<Product_Files>
    {
        /// <summary>
        /// 根据Md5获取文件路径
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static bool GetFileUrlByMd5(string md5, ref string url)
        {
            var model = FirstSelect(o => o.Md5 == md5, o => o.FilePath, o => o.Id, "desc");
            if (model != null && !string.IsNullOrEmpty(model.FilePath))
            {
                url = model.FilePath;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="proId"></param>
        public static void DeleteFiles(int proId)
        {
            var filelist = Query(o => o.ProId == proId);
            if (filelist != null && filelist.Count > 0)
                Delete(filelist);
        }
    }
}
