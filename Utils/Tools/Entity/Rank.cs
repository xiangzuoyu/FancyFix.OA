using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Entity
{
    public class KpiRank
    {
        public string id { get; set; }
        public string realname { get; set; }
        public int score { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public bool iscreated { get; set; }
        public bool isapprove { get; set; }
    }
}
