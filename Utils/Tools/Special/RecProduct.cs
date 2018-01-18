using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FancyFix.Tools.Special
{
    public class Category
    {
        public int type { get; set; }

        public string title { get; set; }

        public string pic { get; set; }

        public string url { get; set; }

        public int index { get; set; }
    }
}
