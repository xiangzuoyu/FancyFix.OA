using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FancyFix.Tools.Auth
{
    public class FacebookHelper : AuthHelper
    {
        /// <summary>
        /// API获取令牌
        /// by:willian date:2016-11-18
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetTokenStr(string code, string returnUrl = FacebookConfig.ReturnUrl)
        {
            return GetTokenStr(code, FacebookConfig.TokenUrl, FacebookConfig.ClientId, FacebookConfig.ClientSecret, returnUrl);
        }

        /// <summary>
        /// 获取个人资料
        /// by:willian date:2016-11-18
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static Entity.LoginExt GetProFileEntity(string accessToken)
        {
            var profile = GetProFileStr(accessToken, FacebookConfig.ProfileResourceUrl);
            try
            {
                Tools.Entity.LoginExt entity = JsonConvert.DeserializeObject<Entity.LoginExt>(profile);
                return entity;
            }
            catch (Exception ex) { Tools.Tool.LogHelper.WriteLog(ex); }
            return null;
        }


    }
}
