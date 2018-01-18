using FancyFix.OA.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FancyFix.Core.Admin
{
    /// <summary>
    /// 菜单功能权限类
    /// </summary>
    public class PermissionManager
    {
        private static string Permission_Ids = "Permission_Ids_";
        private static string Permission_Urls = "Permission_Urls_";
        private static object _lock = new object();

        //超级管理员ID集合
        private static List<string> superAdminIds = ConfigurationManager.AppSettings["SuperAdminIds"]?.ToString().TrimEnd(',').Split(',').ToList() ?? new List<string>();

        /// <summary>
        /// 获取用户所有权限Id
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public static List<int> GetPermissionIds(Mng_User admin)
        {
            lock (_lock)
            {
                if (admin != null && HttpContext.Current?.Session != null)
                {
                    var sessionName = Permission_Ids + admin.Id;

                    if (HttpContext.Current.Session[sessionName] != null)
                    {
                        return HttpContext.Current.Session[sessionName] as List<int>;
                    }
                    else
                    {
                        var perIds = OA.Bll.BllMng_User.GetPermissionIds(admin);
                        if (perIds != null && perIds.Count > 0)
                        {
                            HttpContext.Current.Session.Add(sessionName, perIds);
                            return perIds;
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 获取用户所有权限Url
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public static List<string> GetPermissionUrls(Mng_User admin)
        {
            lock (_lock)
            {
                if (admin != null && HttpContext.Current?.Session != null)
                {
                    var sessionName = Permission_Urls + admin.Id;

                    if (HttpContext.Current.Session[sessionName] != null)
                    {
                        return HttpContext.Current.Session[sessionName] as List<string>;
                    }
                    else
                    {
                        var perUrls = OA.Bll.BllMng_User.GetPermissionUrls(admin);
                        if (perUrls != null && perUrls.Count > 0)
                        {
                            HttpContext.Current.Session.Add(sessionName, perUrls);
                            return perUrls;
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public static bool CheckPermission(Mng_User admin, int permissionId)
        {
            if (admin == null) return false;
            if (IsSuperAdmin(admin.Id)) return true;
            return GetPermissionIds(admin)?.Contains(permissionId) ?? false;
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public static bool CheckPermission(Mng_User admin, string permissionUrl)
        {
            if (admin == null) return false;
            if (IsSuperAdmin(admin.Id)) return true;
            return GetPermissionUrls(admin)?.Contains(permissionUrl) ?? false;
        }

        /// <summary>
        /// 是否超管
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public static bool IsSuperAdmin(int adminId)
        {
            return superAdminIds.Contains(adminId.ToString());
        }

        /// <summary>
        /// 清除权限
        /// </summary>
        public static void ClearPermissions(int adminId)
        {
            lock (_lock)
            {
                HttpContext.Current.Session.Remove(Permission_Ids + adminId);
                HttpContext.Current.Session.Remove(Permission_Urls + adminId);
            }
        }
    }
}
