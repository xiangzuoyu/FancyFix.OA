using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Core.Enum
{
    /// <summary>
    /// 新品需求类型
    /// </summary>
    public enum Demand_TypeEnum
    {
        优化现有供应链=1,
        跨境B2B询盘=2,
        跨境B2B新品=3,
        跨境B2C新品开发=4,
        设计部打样=5,
        跨境B2C项目=6,
        样品需求=7
    }
    /// <summary>
    /// 新品需求状态
    /// </summary>
    public enum Demand_StatusEnum
    {
         未完成=1,
         完成=2
    }
}
