using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FancyFix.OA.Bll
{
    public class BllMng_User : ServiceBase<Mng_User>
    {
        /// <summary>
        /// 检测用户是否具有某个系统权限
        /// </summary>
        /// <param name="model">需要检测的管理员</param> 
        /// <param name="permissionId">检测的权限ID</param>
        /// <returns>true or false</returns>
        public static bool CheckAdminPermission(Mng_User model, int permissionId)
        {
            if (BllMng_PermissionPersonSet.CheckPermission(model.Id, permissionId) || BllMng_PermissionGroup.CheckPermission(model.GroupId.Value, permissionId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取当前用户所有权限Id
        /// </summary>
        /// <param name="model"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public static List<int> GetPermissionIds(Mng_User model)
        {
            if (model != null && model.Id > 0)
            {
                string sql = "select Distinct PermissionId from ( ";
                sql += "select PermissionId from [dbo].[Mng_PermissionPersonSet] where AdminId = @adminId ";
                if (model.GroupId != null && model.GroupId > 0)
                {
                    sql += " union select PermissionId from dbo.Mng_PermissionGroupSet where groupId in (select id from Mng_PermissionGroup where id = @groupId and belock = cast(0 as bit))";
                }
                sql += " ) as tb";
                List<int> ids = Db.Context.FromSql(sql)
                    .AddInParameter("adminId", DbType.Int32, model.Id)
                    .AddInParameter("groupId", DbType.Int32, model.GroupId ?? 0)
                    .ToList<int>();
                return ids;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前用户所有权限Url
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<string> GetPermissionUrls(Mng_User model)
        {
            if (model != null && model.Id > 0)
            {
                string sql = "select Distinct Url from ( ";
                sql += "select PermissionId from [dbo].[Mng_PermissionPersonSet] where AdminId = @adminId ";
                if (model.GroupId != null && model.GroupId > 0)
                {
                    sql += " union select PermissionId from dbo.Mng_PermissionGroupSet where groupId in (select id from Mng_PermissionGroup where id = @groupId and belock = cast(0 as bit))";
                }
                sql += " ) a join [dbo].[Mng_MenuClass] b on a.PermissionId = b.Id where Url<>''";
                List<string> urls = Db.Context.FromSql(sql)
                    .AddInParameter("adminId", DbType.Int32, model.Id)
                    .AddInParameter("groupId", DbType.Int32, model.GroupId ?? 0)
                    .ToList<string>();
                return urls;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="loginIp">登录IP</param>
        /// <returns>成功返回用户ID，失败返回0</returns>
        public static Mng_User CheckLogin(string userName, string password, string loginIp)
        {
            try
            {
                Mng_User modMng_User = null;
                int result = CheckLogin(userName, Tools.Security.Md5Helper.GetMd5Hash(password));
                if (result > 0)
                {
                    //登录成功，保存登录信息
                    modMng_User = First(o => o.Id == result);
                    if (modMng_User != null)
                    {
                        modMng_User.LoginIp = loginIp;
                        modMng_User.LoginTime = DateTime.Now;
                        modMng_User.LoginTimes = modMng_User.LoginTimes + 1;
                        Update(modMng_User, o => o.Id == result);
                    }
                }
                return modMng_User;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(ex, 0, "");
                return null;
            }
        }

        public static int CheckLogin(string userName, string password)
        {
            var result = FirstSelect(o => o.UserName == userName && o.Password == password && o.InJob == true, o => o.Id)?.Id ?? 0;
            return result;
        }

        /// <summary>
        /// 增加职员
        /// </summary>
        /// <param name="Mng_User"></param>
        /// <returns>0: 出错，-1 存在用户名 ,1 成功</returns>
        public static int AddNoReturn(Mng_User model)
        {

            if (ExistsUserName(model.UserName))
            {
                return -1;
            }
            else
            {
                if (Insert(model) > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int UpdateInfo(Mng_User model, int id)
        {
            var result = FirstSelect(o => o.UserName == model.UserName && o.Id != id, o => o.Id)?.Id ?? 0;
            if (result > 0)
            {
                return -1;
            }
            else
            {
                if (Update(model, o => o.Id == id) > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// 判断是否存在此用户名
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static bool ExistsUserName(string userName)
        {
            var result = FirstSelect(o => o.UserName == userName, o => o.Id)?.Id ?? 0;
            return result > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="deleteIds">删除的ID( ,分割)</param>
        /// <returns>删除成功条数</returns>
        public static int DeleteBatch(string deleteIds)
        {
            int delNum = 0;
            if (deleteIds.Trim() == "") { return 0; }
            string[] ids = deleteIds.Split(',');

            foreach (string idStr in ids)
            {
                if (Delete(new Mng_User() { Id = int.Parse(idStr.Trim()) }) > 0)
                {
                    delNum++;
                }
            }
            return delNum;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="searchType">搜索字段</param>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="records">返回总记录数</param>
        /// <returns>返回记录集</returns>
        public DataTable PageListManage(string searchType, string keyword, int page, int pageSize, ref int records)
        {
            System.Text.StringBuilder sql = new StringBuilder("1=1");
            if (!string.IsNullOrEmpty(searchType) && !string.IsNullOrEmpty(keyword))
            {
                sql.Append(" and " + CheckSqlValue(searchType) + " like '" + CheckSqlKeyword(keyword) + "%'");
            }
            return GetPageList2("dbo.Mng_User", "id,UserName,RealName,Sex,logintime,logintimes,injob,departid", sql.ToString(), "order by UserName", pageSize, page, ref records).Tables[0];
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="records"></param>
        /// <returns></returns>
        public static IEnumerable<Mng_User> PageList(string userName, string realName, int departId, int groupId, int page, int pageSize, ref long records)
        {
            var where = new Where<Mng_User>();
            if (!string.IsNullOrEmpty(userName))
                where.And(Mng_User._.UserName.BeginWith(userName));
            if (!string.IsNullOrEmpty(realName))
                where.And(Mng_User._.RealName.BeginWith(realName));
            if (departId > 0)
                where.And(o => o.DepartId == departId);
            if (groupId > 0)
                where.And(o => o.GroupId == groupId);

            var p = Db.Context.From<Mng_User>()
                .LeftJoin<Mng_DepartmentClass>((a, b) => a.DepartId == b.Id)
                .LeftJoin<Mng_PermissionGroup>((a, b) => a.GroupId == b.Id)
                .Select<Mng_DepartmentClass, Mng_PermissionGroup>((a, b, c) => new { a.Id, a.UserName, a.RealName, a.Sex, a.LoginTime, a.LoginTimes, a.InJob, a.DepartId, a.GroupId, a.ParUserId, b.ClassName, c.GroupName }).Where(where);

            records = p.Count();
            return p.Page(pageSize, page).OrderBy(o => o.UserName).ToList();
        }


        /// <summary>
        /// 设置是否在职
        /// </summary>
        /// <param name="id">设置ID</param>
        /// <returns>是否成功</returns>
        public static bool SetInJob(int id)
        {
            Mng_User modMng_User = First(o => o.Id == id);
            if (modMng_User != null)
            {
                Update(new Mng_User() { Id = id, InJob = !modMng_User.InJob }, o => o.Id == id);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据ID获取姓名
        /// </summary>
        /// <param name="id"></param>
        /// <returns>出错返回error</returns>
        public static string GetNameById(int id)
        {
            if (id == 0) return "";
            Mng_User model = First(o => o.Id == id);
            if (model == null)
            {
                return "";
            }
            else
            {
                return model.RealName;
            }

        }

        /// <summary>
        /// 获取所有人员
        /// </summary>
        /// <param name="onlyInjob"></param>
        /// <returns></returns>
        public static IEnumerable<Mng_User> GetAllList(bool onlyInjob, int departId = 0)
        {
            var where = new Where<Mng_User>();
            if (onlyInjob)
                where.And(o => o.InJob == true);
            if (departId > 0)
                where.And(o => o.DepartId == departId);
            return Db.Context.From<Mng_User>().Where(where).OrderBy(o => o.Id).ToList();

        }

        /// <summary>
        /// 获取子级人员
        /// </summary>
        /// <param name="parUserId"></param>
        /// <returns></returns>
        public static List<Mng_User> GetChildList(int parUserId)
        {
            if (parUserId == 0) return null;
            var where = new Where<Mng_User>();
            where.And(o => o.ParUserId == parUserId && o.Id != parUserId && o.InJob == true);
            return Db.Context.From<Mng_User>().Where(where).OrderBy(o => o.Id).ToList();

        }

        /// <summary>
        /// 指定部门下员工列表
        /// </summary>
        /// <param name="departId"></param>
        /// <param name="thisId"></param>
        /// <param name="onlyInjob"></param>
        /// <returns></returns>
        public static List<Mng_User> GetListByDepart(int departId, int thisId, bool onlyInjob)
        {
            var where = new Where<Mng_User>();
            if (onlyInjob) where.And(o => o.InJob == true);
            if (thisId > 0) where.And(o => o.Id != thisId);
            if (departId > 0) where.And(o => o.DepartId == departId);
            return Db.Context.From<Mng_User>().Where(where).OrderBy(o => o.Id).ToList();
        }

        /// <summary>
        /// 获取子级人员人数
        /// </summary>
        /// <param name="parUserId"></param>
        /// <returns></returns>
        public static int GetChildCount(int parUserId)
        {
            if (parUserId == 0) return 0;
            var where = new Where<Mng_User>();
            where.And(o => o.ParUserId == parUserId && o.Id != parUserId);
            return Db.Context.From<Mng_User>().Where(where).Count();
        }

        /// <summary>
        /// 获取所有用户，离职的人排到最后
        /// </summary>
        /// <returns></returns>
        public static List<Mng_User> GetAllUser(int departId = 0)
        {
            string where = "where 1=1";
            if (departId > 0)
                where += " and a.departId=" + departId;
            string cols = "a.Id,a.UserName,a.RealName,a.Sex,a.Email,a.InJob,a.DepartId,a.GroupId,b.ClassName as DepartMentName,c.GroupName,ParUserId";
            string join = "left join Mng_DepartmentClass b on a.DepartId = b.Id left join Mng_PermissionGroup c on a.GroupId = c.Id";
            return Db.Context.FromSql($"select {cols} from Mng_User a {join} {where} order by a.InJob desc,a.Id asc").ToList<Mng_User>();
        }
    }
}
