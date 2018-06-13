using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll.Model
{
    public class DemandSearch
    {
        /// <summary>
        ///部门Id
        /// </summary>
        public int DeptId { get; set; }
        /// <summary>
        /// 需求类型
        /// </summary>
        public int DemandType { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsShow { get; set; }

        public int LoginUserId { get; set; }

    }
}
