using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Auth
{
    public class FacebookConfig
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public const string ClientId = "345446935820893";
        // "1709485219280690";

        /// <summary>
        ///  秘钥
        /// </summary>
        public const string ClientSecret = "3e97a6224a040f2bc44c3527f09f489f";
        //"268d5cb6562dd915d7084718cd75b51b";

        /// <summary>
        /// 验证CodeURL
        /// </summary>
        public const string AuthCodeUrl = "https://www.facebook.com/dialog/oauth";
        /// <summary>
        /// 获取token URL
        /// </summary>
        public const string TokenUrl = "https://graph.facebook.com/oauth/access_token";

        /// <summary>
        /// 资源URL
        /// </summary>
        public const string ProfileResourceUrl = "https://graph.facebook.com/me";

        /// <summary>
        /// 返回URL
        /// </summary>
        public const string ReturnUrl = "http://www.welfulloutdoors.com/auth/return_url_facebook.aspx";

        /// <summary>
        /// 移动端返回URL
        /// </summary>
        public const string MobileReturnUrl = "http://m.welfulloutdoors.com/auth/returnfacebook/";

    }
}
