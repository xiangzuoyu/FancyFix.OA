using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllProduct_Image : ServiceBase<Product_Image>
    {
        /// <summary>
        /// 根据Md5获取图片路径
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static bool GetImageUrlByMd5(string md5, ref string url)
        {
            var model = FirstSelect(o => o.Md5 == md5, o => o.ImagePath);
            if (model != null && !string.IsNullOrEmpty(model.ImagePath))
            {
                url = model.ImagePath;
                return true;
            }
            return false;
        }
    }
}
