using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace FancyFix.Tools.Config
{
    public class SiteOptionCollection : ConfigurationElementCollection
    {
        public SiteOption this[int index]
        {
            get
            {
                return base.BaseGet(index) as SiteOption;
            }
        }
        public new SiteOption this[string name]
        {
            get
            {
                return base.BaseGet(name) as SiteOption;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SiteOption();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SiteOption)element).Domain;
        }
    }
}
