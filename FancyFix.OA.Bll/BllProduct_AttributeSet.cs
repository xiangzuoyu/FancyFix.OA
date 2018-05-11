using Dos.DataAccess.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Text;
using FancyFix.Tools.Json;

namespace FancyFix.OA.Bll
{
    public class BllProduct_AttributeSet : ServiceBase<Product_AttributeSet>
    {
        public static BllProduct_AttributeSet Instance()
        {
            return new BllProduct_AttributeSet();
        }

        /// <summary>
        /// 添加可筛选属性
        /// </summary>
        /// <param name="list"></param>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static bool Add(List<Product_AttributeSet> list, int proId, bool isSpu)
        {
            using (var trans = Db.Context.BeginTransaction())
            {
                trans.Delete<Product_AttributeSet>(o => o.ProductId == proId && o.IsSpu == isSpu);
                foreach (var item in list)
                {
                    item.ProductId = proId;
                    item.IsSpu = isSpu;
                }
                int rows = trans.Insert(list);
                if (rows == 0)
                {
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
                return true;
            }
        }
    }
}
