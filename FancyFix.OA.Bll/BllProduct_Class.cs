using Dos.DataAccess.Base;
using FancyFix.OA.Model;
using FancyFix.Tools.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllProduct_Class : BllSys_Class<Product_Class>
    {
        public new static BllProduct_Class Instance()
        {
            return new BllProduct_Class();
        }

        #region 后台方法

        /// <summary>
        /// 获取分类编号
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static string GetCode(int classId)
        {
            string code = string.Empty;
            var classModel = First(o => o.Id == classId);
            string[] pars = classModel?.ParPath.TrimEnd(',').Split(',');
            if (pars != null && pars.Length > 0)
            {
                foreach (var cid in pars)
                {
                    if (int.TryParse(cid, out classId) && classId > 0)
                    {
                        code += First(o => o.Id == classId).Code;
                    }
                }
            }
            return code;
        }

        #endregion

        #region 前台方法

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <returns></returns>
        public static List<Product_Class> GetList()
        {
            return Query(o => o.BeLock == false, o => o.Sequence, "asc");
        }

        /// <summary>
        /// 获取顶级Class列表
        /// </summary>
        /// <param name="departId"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static List<Product_Class> GetListTopClass(int top = 0)
        {
            var p = Db.Context.From<Product_Class>().Where(o => o.BeLock == false && o.ParId == 0);
            if (top > 0) p.Top(top);
            return p.OrderBy(o => o.Sequence).ToList();
        }


        /// <summary>
        /// 获取顶级ClassId
        /// </summary>
        /// <param name="classlist"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static int GetTopClassId(List<Product_Class> classlist, int classId)
        {
            var classModel = classlist.Find(o => o.Id == classId);
            if (classModel != null)
            {
                if (classModel.ParId > 0)
                    return GetTopClassId(classlist, classModel.ParId.Value);
                else
                    return classModel.Id;
            }
            return classId;
        }

        /// <summary>
        /// 获取顶级Class信息
        /// </summary>
        /// <param name="classlist"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static Product_Class GetTopClass(List<Product_Class> classlist, int classId)
        {
            var classModel = classlist.Find(o => o.Id == classId);
            if (classModel != null)
            {
                if (classModel.ParId > 0)
                    return GetTopClass(classlist, classModel.ParId.Value);
                else
                    return classModel;
            }
            return classModel;
        }

        /// <summary>
        /// 获取全部列表（Redis缓存）
        /// </summary>
        /// <returns></returns>
        public static List<Product_Class> GetListCache()
        {
            try
            {
                string key = "ProClass";
                if (RedisProvider.KeyExistSync(key))
                {
                    string str = RedisProvider.StringGetSync(key);
                    return Tools.Tool.JsonHelper.Deserialize<Product_Class>(str);
                }
                else
                {
                    var list = GetList();
                    if (list != null && list.Count > 0)
                    {
                        //缓存过期时间-1天更新一次
                        TimeSpan ts = new TimeSpan(DateTime.Now.AddDays(1).Ticks - DateTime.Now.Ticks);
                        RedisProvider.StringSetSync(key, Tools.Tool.JsonHelper.Serialize(list), ts);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(BllProduct_Class), ex, Tools.Utility.User.GetMyMemberId(), Tools.Utility.User.GetMyMemberName());
                return null;
            }
        }

        #endregion
    }
}
