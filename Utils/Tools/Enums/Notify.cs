using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Enums
{
    public class Notify
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public enum NotifyType : byte
        {
            [Description("Announce")]
            Announce = 1,
            [Description("Remind")]
            Remind = 2,
            [Description("Message")]
            Message = 3
        }

        /// <summary>
        /// 消息发送来源
        /// </summary>
        public enum UserType : byte
        {
            [Description("Welloutdoors")]
            Welloutdoors = 1,
            [Description("User")]
            User = 2
        }

        /// <summary>
        /// 操作提醒类型
        /// </summary>
        public enum TargetType : byte
        {
            /// <summary>
            /// 询盘
            /// </summary>
            [Description("Inquiry")]
            Inquiry = 1,
            /// <summary>
            /// 订单
            /// </summary>
            [Description("Order")]
            Order = 2,
            /// <summary>
            /// 用户
            /// </summary>
            [Description("User")]
            User = 3,

            /// <summary>
            /// 快速询盘
            /// </summary>
            [Description("快速询盘")]
            Require = 4,
        }

        /// <summary>
        /// 提醒动作类型
        /// </summary>
        public enum Action : byte
        {
            /// <summary>
            /// 编辑操作
            /// </summary>
            [Description("Edit")]
            Edit = 1,
            /// <summary>
            /// 确认订单操作
            /// </summary>
            [Description("Confirm")]
            Confirm = 2,
            /// <summary>
            /// 发送新回复
            /// </summary>
            [Description("Send New Message")]
            SendMsg = 3,
            /// <summary>
            /// 订单状态更新
            /// </summary>
            [Description("Status Updated")]
            UpdateStatus = 4,
            /// <summary>
            /// 新用户注册
            /// </summary>
            [Description("Register")]
            Reg = 5,
            /// <summary>
            /// 发送新询盘
            /// </summary>
            [Description("New Inquiry Send")]
            SendInquiry = 6,
        }
    }
}
