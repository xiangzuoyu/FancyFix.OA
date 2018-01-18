using Dos.DataAccess.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllMng_PermissionPersonSet : ServiceBase<Mng_PermissionPersonSet>
    {
        public static bool AddNoReturn(Mng_PermissionPersonSet model)
        {
            if (CheckPermission((int)model.AdminId, (int)model.PermissionId))
            {
                return false;
            }
            else
            {
                return Insert(model) > 0;
            }
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public static bool CheckPermission(int adminId, int permissionId)
        {
            try
            {
                int id = FirstSelect(o => o.AdminId == adminId && o.PermissionId == permissionId, o => o.Id)?.Id ?? 0;
                return id > 0;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(BllMng_PermissionPersonSet), ex, 0, "");
                return false;
            }
        }

        /// <summary>
        /// 保存某用户OA权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        public static bool SavePermission(int adminId, string permissionIds)
        {
            if (string.IsNullOrEmpty(permissionIds))
                return Clear(adminId); //如果没有权限，则清空。
            else
            {
                //判断权限是否符合标准
                if (CheckSqlValueByArrayInt(permissionIds))
                {
                    //先删除不属于所选范围的ID
                    string where = string.Format("AdminId = {0} and PermissionId not in({1})", adminId.ToString(), permissionIds);
                    Clear(where);

                    //再增加没有的ID
                    string[] permissionId = permissionIds.Split(',');
                    List<Mng_PermissionPersonSet> list = new List<Mng_PermissionPersonSet>();
                    for (int i = 0; i < permissionId.Length; i++)
                    {
                        //加入权限表
                        list.Add(new Mng_PermissionPersonSet()
                        {
                            AdminId = adminId,
                            PermissionId = int.Parse(permissionId[i].ToString())
                        });
                    }
                    return Insert(list) > 0;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="string">where</param>
        /// <returns>True or False</returns>
        public static bool Clear(string where)
        {
            if (where == "") return false;
            return Db.Context.FromSql(string.Format("delete from dbo.Mng_PermissionPersonSet where {0}", CheckSqlValue(where))).ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 清空某用户指定类型的权限(主要用于清除某用户的OA权限)
        /// </summary>
        /// <param name="adminId">用户ID</param> 
        /// <returns>成功或失败</returns>
        private static bool Clear(int adminId)
        {
            string where = string.Format("AdminId = {0}", adminId.ToString());
            return Clear(where);
        }

        /// <summary>
        /// 获取某用户OA权限集
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public static List<Mng_PermissionPersonSet> GetPermissionList(int adminId)
        {
            return GetSelectList(0, "Id,PermissionId", string.Format("AdminId = {0}", adminId.ToString()), "") as List<Mng_PermissionPersonSet>;
        }

        /// <summary>
        /// 检测某个用户是否有个人权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public static bool CheckHavePermission(int adminId)
        {
            try
            {
                List<Mng_PermissionPersonSet> list = Query(o => o.AdminId == adminId, null, "", o => o.Id, 1, null, null);
                if (list.Count >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(BllMng_PermissionPersonSet), ex, 0, "");
                return false;
            }
        }
    }
}
