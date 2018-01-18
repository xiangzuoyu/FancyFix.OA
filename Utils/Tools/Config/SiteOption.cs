using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace FancyFix.Tools.Config
{
    public class SiteOption : ConfigurationElement
    {
        /// <summary>
        /// 上传域 eg: cnxfg.com
        /// </summary>
        [ConfigurationProperty("domain", IsRequired = true)]
        public string Domain
        {
            get
            {
                return this["domain"] as string;
            }
        }

        /// <summary>
        /// 获取网站目录名
        /// </summary>
        [ConfigurationProperty("folder", IsRequired = true)]
        public string Folder
        {
            get
            {
                return this["folder"] as string;
            }
        }

        /// <summary>
        /// 获取网站完整URL
        /// </summary>
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return this["url"] as string;
            }
        }

    }
}
