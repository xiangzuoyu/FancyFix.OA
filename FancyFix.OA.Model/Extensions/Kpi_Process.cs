using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Model
{
    public partial class Kpi_Process
    {
        public string RealName { get; set; }

        public int DepartId { get; set; }

        public string DepartmentName { get; set; }

        public int Rank { get; set; }
    }
}
