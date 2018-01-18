using Dos.DataAccess.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;

namespace FancyFix.OA.Bll
{
    public class BllMng_PermissionGroupSet : ServiceBase<Mng_PermissionGroupSet>
    {

        /// <summary>
        /// 自定义新增方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddNoReturn(Mng_PermissionGroupSet model)
        {
            if (CheckPermission((int)model.GroupId, (int)model.PermissionId))
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
        /// <param name="groupId"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public static bool CheckPermission(int groupId, int permissionId)
        {
            int id = FirstSelect(o => o.GroupId == groupId && o.PermissionId == permissionId, o => o.Id)?.Id ?? 0;
            return id > 0;
        }

        /// <summary>
        /// 保存某用户OA权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        public static bool SavePermission(int groupId, string permissionIds)
        {
            if (string.IsNullOrEmpty(permissionIds))
                return Clear(groupId); //如果没有权限，则清空。
            else
            {
                //判断权限是否符合标准
                if (CheckSqlValueByArrayInt(permissionIds))
                {
                    //先删除不属于所选范围的ID
                    string where = string.Format("GroupId = {0} and PermissionId not in({1})", groupId.ToString(), permissionIds);
                    Clear(where);

                    //再增加没有的ID
                    string[] permissionId = permissionIds.Split(',');

                    List<Mng_PermissionGroupSet> list = new List<Mng_PermissionGroupSet>();
                    for (int i = 0; i < permissionId.Length; i++)
                    {
                        //加入权限表
                        list.Add(new Mng_PermissionGroupSet()
                        {
                            GroupId = groupId,
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
            return Db.Context.FromSql(string.Format("delete from dbo.Mng_PermissionGroupSet where {0}", CheckSqlValue(where))).ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 清空某用户指定类型的权限(主要用于清除某用户的OA权限)
        /// </summary>
        /// <param name="groupId">用户ID</param> 
        /// <returns>成功或失败</returns>
        private static bool Clear(int groupId)
        {
            string where = string.Format("GroupId = {0}", groupId.ToString());
            return Clear(where);
        }

        /// <summary>
        /// 获取某用户OA权限集
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static List<Mng_PermissionGroupSet> GetPermissionList(int groupId)
        {
            return GetSelectList(0, "Id,PermissionId", string.Format("GroupId = {0}", groupId.ToString()), "") as List<Mng_PermissionGroupSet>;
        }

        /// <summary>
        /// 检测某个用户是否有个人权限
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool CheckHavePermission(int groupId)
        {
            try
            {
                List<Mng_PermissionGroupSet> list = Query(o => o.GroupId == groupId, null, "", o => o.Id, 1, null, null);
                if (list.Count >= 1)
                    return true;
                else
                    return false;
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                return false;
            }
        }
    }
}
