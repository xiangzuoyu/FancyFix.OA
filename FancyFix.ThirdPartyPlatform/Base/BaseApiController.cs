using FancyFix.OA.Bll;
using FancyFix.Core.Admin;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FancyFix.ThirdPartyPlatform.Base
{
    public class BaseApiController : ApiController
    {
        private static string sitePreName = "FancyFix";   //Session或Cookie前缀,区别其他站点
        public static string cssVersion = DateTime.Now.ToString("yyMMddhhss"); //样式版本
        public static string domain = Tools.Special.Common.GetDomain();
        public static string webUrl = Tools.Special.Common.GetWebUrl();

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
    }
}