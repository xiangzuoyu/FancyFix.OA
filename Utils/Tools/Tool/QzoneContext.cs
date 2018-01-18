using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace Tools.Tool
{
    public  class QzoneContext
    {
        QQConnectConfig config = new QQConnectConfig();
        public string GetAuthorizationUrl(string state, string scope)
        {
            if (string.IsNullOrEmpty(scope))
            {
                return string.Format("{0}authorize?response_type=code&client_id={1}&redirect_uri={2}&state={3}", new object[] { this.config.GetAuthorizeURL(), this.config.GetAppKey(), this.config.GetCallBackURI().ToString(), state });
            }

            return string.Format("{0}authorize?response_type=code&client_id={1}&redirect_uri={2}&state={3}&scope={4}", new object[] { this.config.GetAuthorizeURL(), this.config.GetAppKey(), this.config.GetCallBackURI().ToString(), state, scope });
        }

        public OAuthToken GetAccessToken(string scope,string state)
        {
            Tools.Helper.HttpHelper Http = new Tools.Helper.HttpHelper("utf-8");
            string url = string.Format("{0}token?grant_type=authorization_code&client_id={1}&redirect_uri={2}&state={3}&client_secret={4}&code={5}",
                new object[] { this.config.GetAuthorizeURL(), this.config.GetAppKey(), this.config.GetCallBackURI().ToString(), state, this.config.GetAppSecret(),scope });

            Tools.Helper.HttpHelper.HttpResult hreult = Http.Get(url);
            if (hreult == null)
            {
                return null;
            }

            string accToken = this.GetUserAccessToken(hreult.ResultHtml);

            if (accToken == "")
            {
                return null;
            }

            url = string.Format("https://graph.qq.com/oauth2.0/me?access_token={0}", new object[] { accToken });
            hreult = Http.Get(url);
            if (hreult == null)
            {
                return null;
            }

            string openid = this.GetUserOpenId(hreult.ResultHtml);

            return new OAuthToken { OpenId = openid, AccessToken = accToken };
        }

        private  string GetUserOpenId(string content)
        {
            string strJson = content.Replace("callback(", "").Replace(");", "");
            return JSON.JSONHelper.Deserialize<Callback>(strJson).openid;
        }

        public class Callback
        {
            // Properties
            public string client_id { get; set; }

            public string openid { get; set; }
        }

 


        private  string GetUserAccessToken(string urlParams)
        {
            string strToken = string.Empty;
            foreach (string parameter in urlParams.Split(new char[] { '&' }))
            {
                string[] accessTokens = parameter.Split(new char[] { '=' });
                if (accessTokens[0] == "access_token")
                {
                    return accessTokens[1];
                }
            }
            return strToken;
        }
    }

    public class OAuthToken
    {
        // Properties
        public string AccessToken { get; set; }

        public string OpenId { get; set; }
    }


 


    public class QQConnectConfig
    {
        // Fields
        private NameValueCollection QzoneSection = ((NameValueCollection)ConfigurationManager.GetSection("QQSectionGroup/QzoneSection"));

        // Methods
        public string GetAppKey()
        {
            return this.QzoneSection["AppKey"];
        }

        public string GetAppSecret()
        {
            return this.QzoneSection["AppSecret"];
        }

        public string GetAuthorizeURL()
        {
            return this.QzoneSection["AuthorizeURL"];
        }

        public Uri GetCallBackURI()
        {
            return new Uri(this.QzoneSection["CallBackURI"]);
        }
    }

}
