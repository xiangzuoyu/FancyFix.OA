using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyFix.Tools.Entity
{
    /// <summary>
    /// 验证注册登录Entity
    /// </summary>
    public class RegMember
    {
        public string email { get; set; }
        public string pwd { get; set; }
        public string pwd2 { get; set; }
        public string contact { get; set; }
        public string company { get; set; }
        public int countryId { get; set; }
        public string tel { get; set; }
    }

    public class BasicMember
    {
        public string email { get; set; }
        public int adminId { get; set; }
        public int id { get; set; }
    }

    public class DataResult
    {
        public bool pass { get; set; }
        public string msg { get; set; }
    }

    public class FbCheck
    {
        public string accessToken { get; set; }
    }

    public class LoginResult
    {
        /// <summary>
        /// 0失败 1未绑定 2已绑定
        /// </summary>
        public byte status { get; set; }
        public string msg { get; set; }
    }
}
