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
    public class BllProduct_CodeSequence : BllSys_Class<Product_CodeSequence>
    {
        public new static BllProduct_CodeSequence Instance()
        {
            return new BllProduct_CodeSequence();
        }

        /// <summary>
        /// 获取最近一个编号
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static string GetMaxId(int classId)
        {
            int maxId = (First(o => o.ClassId == classId)?.Sequence ?? 0) + 1;
            if (maxId < 10)
                return "00" + maxId;
            else if (maxId < 100)
                return "0" + maxId;
            else
                return maxId.ToString();
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        /// <param name="classId">分类Id</param>
        /// <param name="spuCode">产品编码</param>
        /// <returns></returns>
        public static int UpdateSequence(int classId, string spuCode)
        {
            try
            {
                string classCode = Bll.BllProduct_Class.GetCode(classId);//eg: 102
                if (spuCode.Length <= classCode.Length) return 0;
                int sequence = spuCode.Substring(classCode.Length).TrimStart('0').ToInt32();
                Product_CodeSequence model = First(o => o.ClassId == classId);
                if (model != null)
                {
                    model.Sequence = sequence;
                    return Update(model) > 0 ? model.Id : 0;
                }
                else
                {
                    model = new Product_CodeSequence();
                    model.ClassId = classId;
                    model.Sequence = sequence;
                    return Insert(model);
                }
            }
            catch(Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(ex);
                return 0;
            }
        }
    }
}
