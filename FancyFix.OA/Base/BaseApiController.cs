using FancyFix.OA.Bll;
using FancyFix.Core.Admin;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FancyFix.OA.Base
{
    public class BaseApiController : ApiController, IAdminInfo
    {
        private static string sitePreName = "FancyFix";   //Session或Cookie前缀,区别其他站点
        public static string cssVersion = DateTime.Now.ToString("yyMMddhhss"); //样式版本
        public static string domain = Tools.Special.Common.GetDomain();
        public static string webUrl = Tools.Special.Common.GetWebUrl();
        public static string imgUrl = Tools.Special.Common.GetImgUrl();

        public static string SitePreName
        {
            get { return sitePreName; }
        }

        public object FormatOutput(bool status, string remark, object List = null, long records = 0)
        {
            return new { Status = status, Remark = remark, Records = records, List = List };
        }

        public object FormatOutput(bool status, object List = null, long records = 0)
        {
            return FormatOutput(status, "", List, records);
        }

        private Mng_User myInfo = null;

        /// <summary>
        /// 返回当前管理员对象
        /// </summary>
        public Mng_User MyInfo
        {
            get
            {
                if (myInfo == null)
                    myInfo = new AdminState((HttpContextBase)Request.Properties["MS_HttpContext"]).GetUserInfo();
                return myInfo;
            }
        }

        /// <summary>
        /// 当前所在部门Id
        /// </summary>
        public int MyDepartId
        {
            get { return MyInfo?.DepartId ?? 0; }
        }

        /// <summary>
        /// 是否是超管
        /// </summary>
        public bool IsSuperAdmin
        {
            get { return PermissionManager.IsSuperAdmin(MyInfo.Id); }
        }

        /// <summary>
        /// 是否是部门管理
        /// </summary>
        public bool IsDepartAdmin
        {
            get { return Bll.BllMng_PermissionGroup.Any(o => o.IsAdmin == true && o.DepartId == MyInfo.DepartId && o.Id == MyInfo.GroupId); }
        }

        /// <summary>
        /// 获取检测权限 (通用)
        /// </summary> 
        /// <param name="permissionId">权限ID</param> 
        /// <returns>true or false</returns>
        public bool CheckPermission(int permissionId)
        {
            try
            {
                //判断权限
                return PermissionManager.CheckPermission(MyInfo, permissionId);
            }
            catch (Exception ex)
            {
                FancyFix.Tools.Tool.LogHelper.WriteLog(typeof(BaseApiController), ex, 0, "");
                return false;
            }
        }

        public IHttpActionResult ReturnResult(bool success=true,string msg="操作成功")
        {
            return Ok(new { success = success, msg = msg });
        }
    }
}