using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Auth
{
    public class LinkinConfig
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public const string ClientId = "777lxrt9rnwxt3";

        /// <summary>
        ///  秘钥
        /// </summary>
        public const string ClientSecret = "KcLttSpSqPwtxo3j";

        /// <summary>
        /// 验证CodeURL
        /// </summary>
        public const string AuthCodeUrl = "https://www.linkedin.com/oauth/v2/authorization";
        /// <summary>
        /// 获取token URL
        /// </summary>
        public const string TokenUrl = "https://www.linkedin.com/oauth/v2/accessToken";

        /// <summary>
        /// Linkin资源URL
        /// </summary>
        public const string ProfileResourceUrl = "https://www.linkedin.com/v1/people/~";

        /// <summary>
        /// 返回URL
        /// </summary>
        public const string ReturnUrl =  "http://www.welfulloutdoors.com/auth/return_url_linkedin.aspx";

        /// <summary>
        /// 移动端返回URL
        /// </summary>
        public const string MobileReturnUrl = "http://m.welfulloutdoors.com/auth/returnlinkedin/";
    }
}
