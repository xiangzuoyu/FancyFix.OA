using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Model.Business
{
    public class Rank_Valuable
    {
        public int UserId { get; set; }

        public string RealName { get; set; }

        public int Score { get; set; }

        public int DepartId { get; set; }

        public string DepartName { get; set; }

        public string GroupName { get; set; }

        public int Rank { get; set; }

        public int CountTime { get; set; }
    }
}
