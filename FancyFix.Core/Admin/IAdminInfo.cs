using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Core.Admin
{
    public interface IAdminInfo
    {
        /// <summary>
        /// 当前管理员对象
        /// </summary>
        Mng_User MyInfo { get; }

        /// <summary>
        /// 当前所在部门Id
        /// </summary>
        int MyDepartId { get; }

        /// <summary>
        /// 是否是超管
        /// </summary>
        bool IsSuperAdmin { get; }

        /// <summary>
        /// 是否是部门管理员
        /// </summary>
        bool IsDepartAdmin { get; }
    }
}
