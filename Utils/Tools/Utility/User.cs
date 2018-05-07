using FancyFix.Tools.Redis.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FancyFix.Tools.Utility
{
    public class User
    {
        public string UserLoginTicket
        {
            get
            {
                // return GetMyMemberName();
                if (HttpContext.Current.Session["USER_LOGON_TICKET"] != null)
                    return HttpContext.Current.Session["USER_LOGON_TICKET"].ToString();
                else
                {
                    var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (cookie != null)
                    {
                        //Cookie未过期时，读取cookie，重新写Session
                        HttpContext.Current.Session["USER_LOGON_TICKET"] = cookie.Value;
                        return cookie.Value;
                    }
                    else
                    {
                        //Session, Cookie都过期，重新登录
                        return string.Empty;
                    }
                }
            }
        }

        public static User Current
        {
            get
            {
                //通过用户登录标识，获取用户跟权限有关的对象
                //var user = (new AuthoirzedUserManager()).GetByUserName();
                return new User();
            }
        }

        /// <summary>
        /// 保存GUID至COOKIE并获取值
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            string guid = Tools.Utility.CookieHelper.GetValue("USERGUID");
            if (guid == null || guid == "")
            {
                guid = System.Guid.NewGuid().ToString();
                Tools.Utility.CookieHelper.SetObj("USERGUID", 1, guid, Tools.Special.Common.GetDomain());
            }
            return guid;
        }


        /// <summary>
        /// 获取登录后用户名
        /// </summary>
        /// <returns></returns>
        public static string GetMyUserName()
        {
            if (System.Web.HttpContext.Current.Session["Mb_UserName"] == null)
            {
                return "";
            }
            else
            {
                return System.Web.HttpContext.Current.Session["Mb_UserName"].ToString();
            }
        }

        /// <summary>
        /// 获取登录后用户名
        /// </summary>
        /// <returns></returns>
        public static string GetMyMemberName()
        {
            int result = 0;
            string userName = string.Empty;
            if (!GetMemberSession(ref result, ref userName))
            {
                result = GetRemember(ref userName);
                if (result > 0)
                    SetMemberSession(result, userName);
            }
            return userName;
        }

        /// <summary>
        /// 获取登录后用户ID
        /// </summary>
        /// <returns></returns>
        public static int GetMyMemberId()
        {
            int result = 0;
            string userName = string.Empty;
            GetMemberSession(ref result, ref userName);
            //result = GetRemember(ref userName);
            //if (result > 0)
            //  SetMemberSession(result, userName);
            return result;
        }

        /// <summary>
        /// 设置用户编号
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static string FormatUserCode(int userId, string adminCode)
        {
            string code = userId.ToString();
            if (code.Length < 5)
            {
                code = code.PadLeft(5, '0');
            }
            return "U" + code + adminCode.ToUpper();
        }

        /// <summary>
        ///  设置登录SESSION
        /// </summary>
        /// <param name="memberId"></param>
        public static void SetMemberSession(int memberId, string userName)
        {
            var sessionValue = memberId + "|" + userName;
        
            //Session共享缓存
            if (Tools.Special.Common.GetIsRedisSession())
            {
                RedisSession redisSession = new RedisSession(System.Web.HttpContext.Current, true);
                redisSession["MB_Member"] = sessionValue;
            }
            else
            {
                System.Web.HttpContext.Current.Session["MB_Member"] = sessionValue;
                System.Web.HttpContext.Current.Session.Timeout = 120;
            }
        }

        /// <summary>
        /// 获取登录session
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public static bool GetMemberSession(ref int userId, ref string userName)
        {
            //Session共享缓存
            if (Tools.Special.Common.GetIsRedisSession())
            {
                RedisSession redisSession = new RedisSession(System.Web.HttpContext.Current, true);
                var session = redisSession["MB_Member"];
                return GetMemberInfo(session, ref userId, ref userName);
            }
            else if (System.Web.HttpContext.Current.Session != null)
            {
                return GetMemberInfo(System.Web.HttpContext.Current.Session["MB_Member"], ref userId, ref userName);
            }

            return true;
        }

        /// <summary>
        /// 根据Session内容获取用户信息
        /// by:willian date:2016-8-9
        /// </summary>
        /// <param name="sessionValue"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool GetMemberInfo(object sessionValue, ref int userId, ref string userName)
        {
            if (sessionValue == null)
            {
                return false;
            }
            else
            {
                string[] tmp = sessionValue.ToString().Split('|');
                if (!(tmp.Length > 0 && int.TryParse(tmp[0], out userId)))
                {
                    userId = 0;
                }
                if (tmp.Length > 1)
                    userName = tmp[1];
            }
            return true;
        }
        /// <summary>
        /// 设置记住密码
        /// </summary>
        /// <param name="memberId"></param>
        public static void SetRemember(int userId, string userName, bool remember)
        {
            //生成验证字符串cookie
            string authStr = userId + "^" + userName + "^" + DateTime.Now.AddDays(2);
            authStr = Tools.Security.EncryptionHelper.Des3_Encrypt(authStr);
            //添加Cookie
            Tools.Utility.CookieHelper.SetObj("Auth", authStr);
            //SetMemberSession(userId, userName);
        }

        /// <summary>
        /// 获取会员Id COOKIE
        /// </summary>
        /// <param name="memberId"></param>
        public static int GetRemember(ref string userName)
        {
            int userId = 0;
            string authCookie = Tools.Utility.CookieHelper.GetValue("Auth");
            if (!CheckAuthInfo(authCookie, ref userId, ref userName))
            {
                return 0;
            }
            return userId;
        }

        /// <summary>
        /// 根据验证字符串获取登录信息
        /// </summary>
        /// <param name="authStr">登录验证字符串(cookie)</param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool CheckAuthInfo(string authStr, ref int userId, ref string userName)
        {
            //解密验证字符串
            string decrypt = Tools.Security.EncryptionHelper.Des3_Decrypt(authStr);
            if (string.IsNullOrEmpty(decrypt))
                return false;

            string[] infoTmp = decrypt.Split('^');
            if (infoTmp.Length < 3)
                return false;
            try
            {
                userId = Convert.ToInt32(infoTmp[0]);
                userName = infoTmp[1];
                DateTime expireTime = Convert.ToDateTime(infoTmp[2]);
                //判断该验证字符串是否过期
                if (expireTime.CompareTo(DateTime.Now) < 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public static void RemoveMember()
        {
            Tools.Utility.CookieHelper.Del("Remember");
            Tools.Utility.CookieHelper.Del("Auth");
            System.Web.HttpContext.Current.Session.Remove("MB_MemberId");
            //Session共享缓存
            if (Tools.Special.Common.GetIsRedisSession())
            {
                RedisSession redisSession = new RedisSession(System.Web.HttpContext.Current, true, 120);
                redisSession.Remove("MB_MemberId");
            }
        }


        /// <summary>
        /// 设置Session 外站登录值
        /// </summary>
        /// <param name="outId"></param>
        public static void SetOutLoginId(int outId)
        {
            System.Web.HttpContext.Current.Session["OutLoginId"] = outId;
        }


        /// <summary>
        /// 移除外站登录值Session
        /// </summary>
        /// <param name="outId"></param>
        public static void RemoveOutLoginId(int outId)
        {
            System.Web.HttpContext.Current.Session.Remove("OutLoginId");
        }
        /// <summary>
        /// 获取外站登录Session
        /// </summary>
        /// <param name="outId"></param>
        public static int GetOutLoginId()
        {
            if (System.Web.HttpContext.Current.Session["OutLoginId"] == null)
                return 0;
            return System.Web.HttpContext.Current.Session["OutLoginId"].ToString().ToInt32();
            //System.Web.HttpContext.Current.Session["OutLoginId"] = outId;
        }
    }
}
