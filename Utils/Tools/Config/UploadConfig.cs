using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace FancyFix.Tools.Config
{
    public class UploadConfig : ConfigurationSection
    {
        [ConfigurationProperty("Settings")]
        public SettingsCollection Settings
        {
            get
            {
                return this["Settings"] as SettingsCollection;
            }
        }

        [ConfigurationProperty("SiteOptions")]
        public SiteOptionCollection SiteOptions
        {
            get
            {
                return this["SiteOptions"] as SiteOptionCollection;
            }
        }
    }
}
