using Dos.DataAccess.Base;
using FancyFix.OA.Model;

namespace FancyFix.OA.Bll
{
    public class BllSys_MenuClass : BllSys_Class<Mng_MenuClass>
    {
        public new static BllSys_MenuClass Instance()
        {
            return new BllSys_MenuClass();
        }

        /// <summary>
        /// 增加分类
        /// </summary>
        /// <param name="sys_ManageClass"></param>
        /// <returns></returns>
        public new static bool Add(Mng_MenuClass model)
        {
            if (string.IsNullOrEmpty(model.ClassName))
            {
                return false;
            }
            else
            {
                string flag = "Flag";
                var proc = Db.Context.FromProc("Sys_MenuClassAdd")
                     .AddInParameter("TableName", System.Data.DbType.String, tableName)
                     .AddInParameter("ClassName", System.Data.DbType.String, model.ClassName)
                     .AddInParameter("ParId", System.Data.DbType.Int32, model.ParId)
                     .AddInParameter("Url", System.Data.DbType.String, model.Url)
                     .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
                proc.ExecuteNonQuery();
                var dic = proc.GetReturnValues();
                if (dic.ContainsKey(flag))
                    return bool.Parse(dic[flag].ToString());
                return false;
            }
        }
    }
}
