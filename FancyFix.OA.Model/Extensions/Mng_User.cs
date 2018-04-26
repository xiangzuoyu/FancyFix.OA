using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Model
{
    public partial class Mng_User
    {
        public string DepartMentName { get; set; }

        public string GroupName { get; set; }

        public string ParUserName { get; set; }

        public bool IsRecorded { get; set; }

        public bool IsApproved { get; set; }

        public int Score { get; set; }

        public string ClassName { get; set; }

        public int Rank { get; set; }

        public bool IsCreated { get; set; }

        public int Count { get; set; }

        public int SelfScore { get; set; }

        public int ParScore { get; set; }
    }
}
