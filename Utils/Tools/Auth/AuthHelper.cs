using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Auth
{
    public abstract class AuthHelper
    {
        public static AuthToken GetToken(string code, string token_url, string cliend_id, string client_secret, string return_url)
        {
            var strResult = GetTokenStr(code, token_url, cliend_id, client_secret, return_url);
            try
            {
                var res = JsonConvert.DeserializeObject<AuthToken>(strResult);
                return res;
            }
            catch (Exception ex)
            {
                Tool.LogHelper.WriteLog(ex);
            }
            return default(AuthToken);
        }

        public static string GetTokenStr(string code, string token_url, string cliend_id, string client_secret, string return_url)
        {
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara.Add("grant_type", "authorization_code");
            dicPara.Add("code", code);
            dicPara.Add("redirect_uri", return_url);
            dicPara.Add("client_id", cliend_id);
            dicPara.Add("client_secret", client_secret);

            var token = WebApiHelper.PostResponseStr(token_url, dicPara);
            return token;
        }

        public static string GetProFileAuth(string accessToken, string profile_url)
        {
            Dictionary<string, string> dicAuth = new Dictionary<string, string>();
            dicAuth.Add("Authorization", "Bearer " + accessToken);
            var profile = WebApiHelper.GetResponseStr(profile_url, null, dicAuth);
            return profile;
        }

        public static string GetProFileStr(string accessToken, string profile_url)
        {
            Dictionary<string, string> dicQuery = new Dictionary<string, string>();
            dicQuery.Add("access_token", accessToken);
            var profile = WebApiHelper.GetResponseStr(profile_url, dicQuery, null);
            return profile;
        }
    }
}
