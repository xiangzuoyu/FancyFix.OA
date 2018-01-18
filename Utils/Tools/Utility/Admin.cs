using FancyFix.Tools.Redis.Session;
using System;

namespace FancyFix.Tools.Utility
{
    public class Admin
    {
        static string AdminId = "AdminId";
        static string AdminInfo = "AdminInfo";
        static bool IsRedisSession = Tools.Special.Common.GetIsRedisSession();

        /// <summary>
        ///  设置登录SESSION
        /// </summary>
        /// <param name="memberId"></param>
        public static void SetSession(int adminId, object adminInfo = null)
        {
            //Session共享缓存
            if (IsRedisSession)
            {
                RedisSession redisSession = new RedisSession(System.Web.HttpContext.Current, true, 120);
                redisSession[AdminId] = adminId;
                if (adminInfo != null)
                    redisSession[AdminInfo] = adminInfo;
            }
            else
            {
                System.Web.HttpContext.Current.Session.Timeout = 120;
                System.Web.HttpContext.Current.Session[AdminId] = adminId;
                if (adminInfo != null)
                    System.Web.HttpContext.Current.Session[AdminInfo] = adminInfo;
            }
        }


        /// <summary>
        /// 获取登录session
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public static bool GetAdminSession(ref int userId, ref object adminInfo)
        {
            bool result = true;
            //Session共享缓存
            if (IsRedisSession)
            {
                RedisSession redisSession = new RedisSession(System.Web.HttpContext.Current, true);
                var session = redisSession[AdminId];
                result = GetAdminInfo(session, ref userId);
                if (result && userId > 0)
                    adminInfo = redisSession[AdminInfo];
            }
            else if (System.Web.HttpContext.Current.Session != null)
            {
                result = GetAdminInfo(System.Web.HttpContext.Current.Session[AdminId], ref userId);
                if (result && userId > 0)
                    adminInfo = System.Web.HttpContext.Current.Session[AdminInfo];
            }
            return result;
        }

        /// <summary>
        /// 根据Session内容获取用户信息
        /// </summary>
        /// <param name="sessionValue"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool GetAdminInfo(object sessionValue, ref int userId)
        {
            if (sessionValue == null)
                return false;
            else
                userId = sessionValue.ToString().ToInt32();
            return true;
        }

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <returns></returns>
        public static int GetAdminId()
        {
            bool result = true;
            int userId = 0;
            //Session共享缓存
            if (IsRedisSession)
            {
                RedisSession redisSession = new RedisSession(System.Web.HttpContext.Current, true);
                var session = redisSession[AdminId];
                result = GetAdminInfo(session, ref userId);
            }
            else if (System.Web.HttpContext.Current.Session != null)
            {
                result = GetAdminInfo(System.Web.HttpContext.Current.Session[AdminId], ref userId);
            }
            return userId;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public static void Remove()
        {
            //Session共享缓存
            if (IsRedisSession)
            {
                RedisSession redisSession = new RedisSession(System.Web.HttpContext.Current, true, 120);
                redisSession.Remove(AdminId);
                redisSession.Remove(AdminInfo);
            }

            System.Web.HttpContext.Current.Session.Remove(AdminId);
            System.Web.HttpContext.Current.Session.Remove(AdminInfo);
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
        }

    }
}
