using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Enums
{
    public class Log
    {
        public enum LogType
        {
            //注册 1            
            //登陆 2
            //修改资料 3
            //上传证书 4
            //上传头像 5
            //上传公司logo 6
            //取回密码 7
            //删除用户 8
            //转公共用户 9
            //转入至我的用户 10

            //发布产品 11
            //修改产品 12
            //删除产品 13
            //审核产品 14
            //产品申请榜上排名 15
            //产品删除榜上排名 16
            //榜上排名审核 17
            //批量修改产品分组 18


            //发布采购 21
            //修改采购 22
            //删除采购 23
            //审核采购_通过 24
            //审核采购_不通过 25

            //发送询价 31
            //回复询价 32
            //删除询价 33
            //查看询价(buyers模块详细页) 34
            //领取询价 35
            //分配询价 36      //本地分配
            //Weiku分配询盘至Seekpart 37
            //Seekpart分配询盘至Weiku 38

            //发布新闻 41
            //修改新闻 42
            //删除新闻 43          

            //取回密码 51
            //打开邮件内链接 52
            //设置新密码 53
            //管理员设置密码 54


            //Mail邀请打开 61
            //Mail邀请点击 62
            //Mail邀请登录 63
            //询盘邮件点击 64

            //添加广告 71
            //编辑广告 72
            //删除广告 73

            //审核注册用户 81
            //用户激活 82
            //设置用户会员权限 83
            //用户合并 84

            // 用户申请提现  91

            REG = 1,
            LOGIN = 2,
            EDIT_INFO = 3,
            UPLOAD_CERTIFICATE = 4,
            UPLOAD_CONTACT_PIC = 5,
            UPLOAD_COMPANY_LOGO = 6,
            GET_PSW = 7,
            DEL_USER = 8,
            USER_INTO_PUBLIC = 9,
            USER_INTO_MY = 10,


            PRODUCT_ADD = 11,
            PRODUCT_EDIT = 12,
            PRODUCT_DELETE = 13,
            PRODUCT_CHECK = 14,
            PRODUCT_ADD_Golden_Ranking = 15,
            PRODUCT_DELETE_Golden_Ranking = 16,
            PRODUCT_CHECK_Goledn_Ranking = 17,
            PRODUCT_UpdateProductGroup = 18,

            BUY_ADD = 21,
            BUY_EDIT = 22,
            BUY_DELETE = 23,
            BUY_CHECK_TRUE = 24,
            BUY_CHECK_FALSE = 25,

            MESSAGE_ADD = 31,
            MESSAGE_REPLY = 32,
            MESSAGE_DELETE = 33,
            MESSAGE_SELECT = 34,
            MESSAGE_GET = 35,
            MESSAGE_FENPEI = 36,//本地分配
            MESSAGE_SENDTO_SEEKPART = 37,
            MESSAGE_SEEKPART_TO_WEIKU = 38,

            NEWS_ADD = 41,
            NEWS_EDIT = 42,
            NEWS_DELETE = 43,

            PWD_GET = 51,
            PWD_OPENLINK = 52,
            PWD_SETNEW = 53,
            PWD_ADMINSET = 54,

            MAIL_OPEN = 61,
            MAIL_CLICK = 62,
            MAIL_LOGIN = 63,
            MAIL_INQUIRY_CLICK = 64,
            MAIL_INQUIRY_LOGIN = 65,
            MAIL_INQUIRY_Reply = 66,

            ADV_ADD = 71,
            ADV_UPDATE = 72,
            ADV_DELETE = 73,

            USER_CHECK = 81,
            USER_ACTIVATION = 82,
            USER_SETGRADE = 83,
            USER_HEBIN = 84,

            CASHBACK_WITHDRAWAL = 91

        }
    }
}
