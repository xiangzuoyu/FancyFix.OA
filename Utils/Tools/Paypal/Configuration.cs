using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PayPal.Api;

namespace FancyFix.Tools.Paypal
{
    public static class Configuration
    {
        //public readonly static string ClientId;
        // public readonly static string ClientSecret;

        /// <summary>
        /// 生产/开发环境
        /// 默认开发环境
        /// </summary>
        public static bool IsTest
        {
            get
            {
                string val = GetConfig()["mode"];
                if (!string.IsNullOrEmpty(val))
                    return val == "sandbox" ? true : false;
                return true;
            }
        }
        /// <summary>
        /// 客户端Id
        /// </summary>
        public static string ClientId
        {
            get
            {
                if (IsTest)
                    return "ARPmLuufClAZXMM-QUbNzt2pNPkx34TIXgnCpwwLcvRztSRQXotxxu6R-NgcBYlh2wUDXkzSbzWsDKTn";
                return "AZsdXQRH6FjhaqGXat1Od7ZRMkKMXWVbFkNhdrWOoGZN_mQhSAEZ9oJ_XMIwGpHsV15YJRUdp-zQ85YO";
            }
        }

        /// <summary>
        ///  秘钥
        /// </summary>
        public static string ClientSecret
        {
            get
            {
                if (IsTest)
                    return "EJ_2B3YIWig8wa3md98zBPaiTatzvRKUO-T_g01Fsih-uYkxe39Wi1P35Z3wEdHHX2ltZRITSIldmr3M";
                return "EBZz7yuJNu11J3z8sumoXhJNOQXoLZw94w9gYXIng5mbA3xUaugE4UKRS260A3v5AV8YY4feah1oBA2x";
            }
        }

        /// <summary>
        /// 返回URL
        /// </summary>
        public const string ReturnUrl = "http://www.welfulloutdoors.com/auth/return_url_paypal.aspx";

        /// <summary>
        /// 移动端返回URL
        /// </summary>
        public const string MobileReturnUrl = "http://m.welfulloutdoors.com/auth/returnfacebook/";

        // Static constructor for setting the readonly static members.
        static Configuration()
        {
            //ClientId = PaypalConfig.ClientId;
            //ClientSecret = PaypalConfig.ClientSecret;
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        // Create accessToken
        private static string GetAccessToken()
        {
            // ###AccessToken
            // Retrieve the access token from
            // OAuthTokenCredential by passing in
            // ClientID and ClientSecret
            // It is not mandatory to generate Access Token on a per call basis.
            // Typically the access token can be generated once and
            // reused within the expiry window                
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        // Returns APIContext object
        public static APIContext GetAPIContext(string accessToken = "")
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken);
            apiContext.Config = GetConfig();

            // Use this variant if you want to pass in a request id  
            // that is meaningful in your application, ideally 
            // a order id.
            // String requestId = Long.toString(System.nanoTime();
            // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

            return apiContext;
        }

    }
}
