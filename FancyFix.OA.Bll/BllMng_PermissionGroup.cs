using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace FancyFix.OA.Bll
{
    public class BllMng_PermissionGroup : ServiceBase<Mng_PermissionGroup>
    {
        public static BllMng_PermissionGroup Instance()
        {
            return new BllMng_PermissionGroup();
        }
        /// <summary>
        /// 检测组权限,检测组权限(权限自动过滤锁定组)
        /// </summary>
        /// <param name="groups">组字符串(1,3,4,5)</param>
        /// <param name="permissionId">权限ID</param>
        /// <returns></returns>
        public static bool CheckPermission(string groups, int permissionId)
        {
            try
            {
                if (string.IsNullOrEmpty(groups))
                    return false;
                if (CheckSqlValueByArrayInt(groups))
                {
                    string sql = "select top 1 id from dbo.Mng_PermissionGroupSet where groupId in (select id from Mng_PermissionGroup where id in(@groups) and belock = cast(0 as bit)) and  PermissionId = @permissionId";
                    object result = Db.Context.FromSql(sql)
                        .AddInParameter("groups", DbType.String, groups)
                        .AddInParameter("permissionId", DbType.Int32, permissionId)
                        .ToScalar();
                    if (result == null)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(BllMng_PermissionGroup), ex, 0, "");
                return false;
            }
        }

        public static bool CheckPermission(int groupId, int permissionId)
        {
            try
            {
                if (groupId == 0)
                    return false;
                string sql = "select top 1 id from dbo.Mng_PermissionGroupSet where groupId in (select id from Mng_PermissionGroup where id = @groupId and belock = cast(0 as bit)) and  PermissionId = @permissionId";
                object result = Db.Context.FromSql(sql)
                    .AddInParameter("groupId", DbType.Int32, groupId)
                    .AddInParameter("permissionId", DbType.Int32, permissionId)
                    .ToScalar();
                if (result == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(BllMng_PermissionGroup), ex, 0, "");
                return false;
            }
        }

        /// <summary>
        /// 返回权限组记录集
        /// </summary>
        /// <param name="groupIds">权限组ID字符串(1,3,4)</param>
        /// <returns>记录集包含字段(ID,OAPermission)</returns>
        public static List<Mng_PermissionGroup> GetPermissionGroup(string groupIds)
        {
            if (groupIds == "")
                return null;
            else
            {
                try
                {
                    return GetSelectList(0, "id", "BeLock = cast(0 as bit) and id in(" + groupIds + ")", "seuqence asc") as List<Mng_PermissionGroup>;
                }
                catch (Exception ex)
                {
                    Tools.Tool.LogHelper.WriteLog(typeof(BllMng_PermissionGroup), ex, 0, "");
                    return null;
                }
            }
        }

        /// <summary>
        /// 上移排序
        /// </summary>
        /// <param name="id"></param>
        public new static void Up(int id, int departId)
        {
            Mng_PermissionGroup model = First(o => o.Id == id);
            if (model != null)
            {
                List<Mng_PermissionGroup> list = Query(o => o.Sequence < model.Sequence && o.DepartId == departId, o => o.Sequence, "desc", o => o.Id, 1);
                if (list.Count == 1)
                {
                    Mng_PermissionGroup model1 = First(o => o.Id == list[0].Id);
                    int tempSequence = (int)model.Sequence;
                    model.Sequence = model1.Sequence;
                    Update(model, o => o.Id == id);
                    model1.Sequence = tempSequence;
                    Update(model1, o => o.Id == list[0].Id);
                }
            }
        }

        /// <summary>
        /// 下移排序
        /// </summary>
        /// <param name="id"></param>
        public new static void Down(int id, int departId)
        {
            Mng_PermissionGroup model = First(o => o.Id == id);
            if (model != null)
            {
                List<Mng_PermissionGroup> list = Query(o => o.Sequence > model.Sequence && o.DepartId == departId, o => o.Sequence, "asc", o => o.Id, 1);
                if (list.Count == 1)
                {
                    Mng_PermissionGroup model1 = First(o => o.Id == list[0].Id);
                    int tempSequence = (int)model.Sequence;
                    model.Sequence = model1.Sequence;
                    Update(model, o => o.Id == id);
                    model1.Sequence = tempSequence;
                    Update(model1, o => o.Id == list[0].Id);
                }
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="deleteIds">删除的ID( ,分割)</param>
        /// <returns>删除成功条数</returns>
        public static int DeleteBatch(string deleteIds, int departId)
        {
            int delNum = 0;
            if (deleteIds.Trim() == "") { return 0; }
            string[] ids = deleteIds.Split(',');
            foreach (string idStr in ids)
            {
                if (Delete(int.Parse(idStr.Trim()), departId))
                {
                    delNum++;
                }
            }
            return delNum;
        }

        public static bool Delete(int id, int departId)
        {
            bool result = false;
            Mng_PermissionGroup model = First(o => o.Id == id);
            result = Delete(o => o.Id == id) > 0;
            if (result)
            {
                Db.Context.FromSql("update dbo.Mng_PermissionGroup set Sequence = Sequence - 1 where Sequence > @Sequence and DepartId=@DepartId")
                    .AddInParameter("Sequence", DbType.Int32, model.Sequence)
                    .AddInParameter("DepartId", DbType.Int32, departId)
                    .ExecuteNonQuery();
            }
            return result;
        }

        /// <summary>
        /// 设置是否锁定
        /// </summary>
        /// <param name="id">需要设置的信息ID</param>
        /// <returns>true:已设置 false:错误</returns>
        public static bool SetBeLock(int id)
        {
            Mng_PermissionGroup modMng_PermissionGroup = First(o => o.Id == id);
            if (modMng_PermissionGroup != null)
            {
                Update(new Mng_PermissionGroup() { BeLock = !modMng_PermissionGroup.BeLock }, o => o.Id == id);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取所有记录列表
        /// </summary>
        /// <returns>返回列表记录集</returns>
        public static List<Mng_PermissionGroup> GetAllList(int departId, bool includelock = false)
        {
            var where = new Where<Mng_PermissionGroup>();
            if (!includelock)
                where.And(o => o.BeLock == false);
            if (departId > 0)
                where.And(o => o.DepartId == departId);
            return Db.Context.From<Mng_PermissionGroup>().Where(where).OrderBy(o => o.Sequence).ToList();
        }

        /// <summary>
        /// 增加组
        /// </summary>
        /// <param name="modMng_PermissionGroup">组对象</param>
        /// <returns>-2:出错 , -1 存在相同组名的记录, 0 发布成功</returns>
        public static int AddNoReturn(Mng_PermissionGroup modMng_PermissionGroup, int departId)
        {
            if (ExistsGruopName(modMng_PermissionGroup.GroupName, departId))
            {
                return -1;
            }
            else
            {
                //设置排序字段
                modMng_PermissionGroup.Sequence = GetNextSequence(departId);
                if (Insert(modMng_PermissionGroup) > 0)
                {
                    return 0;
                }
                else
                {
                    return -2;
                }
            }
        }

        /// <summary>
        /// 判断是否存在此组名
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static bool ExistsGruopName(string groupName, int departId)
        {
            int id = FirstSelect(o => o.GroupName == groupName && o.DepartId == departId, o => o.Id)?.Id ?? 0;
            return id > 0;
        }

        /// <summary>
        /// 获取下一个Seq
        /// </summary>
        /// <returns></returns>
        public static int GetNextSequence()
        {
            object result = Db.Context.FromSql("select max(sequence) from dbo.Mng_PermissionGroup").ToScalar();
            if (result == DBNull.Value)
                return 1;
            else
                return int.Parse(result.ToString()) + 1;
        }

        public static int GetNextSequence(int departId)
        {
            object result = Db.Context.FromSql("select max(sequence) from dbo.Mng_PermissionGroup where departId=" + departId).ToScalar();
            if (result == DBNull.Value)
                return 1;
            else
                return int.Parse(result.ToString()) + 1;
        }

        /// <summary>
        /// 获取权限组名
        /// </summary>
        /// <param name="groupIds"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static List<string> GetGruopNames(string groupIds, int departId)
        {
            return Db.Context.FromSql($"select GroupName from dbo.Mng_PermissionGroup where Id in ({groupIds.TrimEnd(',')})").ToList<string>();

        }

        /// <summary>
        /// 删除部门下所有主管配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool RemoveAdmin(int departId)
        {
            return (int)Db.Context.FromSql(
                $"update dbo.Mng_PermissionGroup set isadmin=0 where departid={departId}")
                .ToScalar() > 0;
        }

        /// <summary>
        /// 设置为主管
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool SetAdmin(int id, int departId)
        {
            return (int)Db.Context.FromSql(
                $"update dbo.Mng_PermissionGroup set isadmin=0 where departid={departId} " +
                $"update dbo.Mng_PermissionGroup set isadmin=1 where id={id} and departid={departId}")
                .ToScalar() > 0;
        }

    }
}
