using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.ViewModel
{
    public class DemandModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public string ExecutorUser { get; set; }
        /// <summary>
        /// 需求对接人
        /// </summary>
        public string JoinPerson { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        public int DeptId { get; set; }

        public int CreateUserId { get; set; }

        public int ExecutorId { get; set; }

        public string StatusStr { get; set; }

        public int Type { get; set; }

        public string TypeStr { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsShow { get; set; }
        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime EstimateCompleteTime { get; set; }

    }
}
