using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FancyFix.Tools.Enums
{
    public class ESite
    {
        /// <summary>
        /// 属性类型
        /// </summary>
        public enum AttrType : byte
        {
            [Description("文本框")]
            Text = 1,
            [Description("下拉框")]
            Dropdownlist = 2,
            [Description("多选框")]
            CheckBox = 3
        }

        /// <summary>
        /// 属性类型
        /// </summary>
        public enum AttrValueType : byte
        {
            [Description("文本")]
            String = 1,
            [Description("数字")]
            Int = 2,
            [Description("日期")]
            DateTime = 3
        }

        /// <summary>
        /// 产品类型
        /// </summary>
        public enum ProType : byte
        {
            [Description("基本产品")]
            Normal = 1,
            [Description("抓取产品")]
            Gather = 2
        }

        /// <summary>
        /// 图片类型
        /// </summary>
        public enum ImageType : byte
        {
            [Description("主图")]
            Main = 1,
            [Description("详细图")]
            Detail = 2
        }

        /// <summary>
        /// 客户来源
        /// </summary>
        public enum MBComeFrom : byte
        {
            [Description("未知")]
            Null = 0,
            /// <summary>
            /// 用户通过谷歌查找
            /// </summary>
            [Description("Google搜索")]
            Google = 1,
            [Description("邮件推广")]
            Email = 2,
            [Description("客户介绍")]
            Intro = 3,
            [Description("阿里巴巴")]
            Alibaba = 4,
            [Description("Google广告")]
            GoogleAdv = 5,
            [Description("Bing广告")]
            BingAdv = 6,
            [Description("Facebook广告")]
            FacebookAdv = 7,
            [Description("广交会")]
            CantonFair = 8,
            [Description("自主开发")]
            SelfDeveloper = 9,
            [Description("直接访问")]
            Direct = 10,
            [Description("其他")]
            Other = 100,
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        public enum OrderType : byte
        {
            //零售订单
            [Description("Stock Order")]
            Stock = 1
        }

        /// 货币种类
        /// </summary>
        public enum Currency : byte
        {
            [Description("USD")]
            USD = 1
        }

        /// <summary>
        /// 邮件类型
        /// </summary>
        public enum MailType : byte
        {
            [Description("服务邮件")]
            Sys = 1,
            [Description("推广邮件")]
            Popularize = 2
        }

        /// <summary>
        /// 友情链接类型
        /// </summary>
        public enum FriendLinkType : byte
        {
            [Description("文字链接")]
            Word = 1,
            [Description("图片链接")]
            Pic = 2
        }

        /// <summary>
        /// 友情链接显示类型
        /// </summary>
        public enum FriendLinkShowType : byte
        {
            [Description("首页显示")]
            Index = 1,
            [Description("内页显示")]
            Inner = 2
        }

        /// <summary>
        /// 产品颜色
        /// </summary>
        public enum ProColor : byte
        {
            [Description("Blue")]
            Blue = 1,
            [Description("Green")]
            Green = 2,
            [Description("Red")]
            Red = 3,
            [Description("Gray")]
            Gray = 4,
            [Description("Navy")]
            Navy = 5,
            [Description("Orange")]
            Orange = 6,
            [Description("Brown")]
            Brown = 7,
            [Description("Purple")]
            Purple = 8,
            [Description("Black")]
            Black = 9,
            [Description("Zebra Stripes")]
            ZebraStripes = 10,
            [Description("Camouflage")]
            Camouflage = 11,
            [Description("Yellow")]
            Yellow = 12
        }

        /// <summary>
        /// 询价/留言
        /// </summary>
        public enum MsgType : byte
        {
            [Description("留言")]
            Msg = 1,
            [Description("系统公告")]
            Notice = 2
        }

        /// <summary>
        /// 谷歌再营销代码
        /// </summary>
        public enum PageAdvType : byte
        {
            [Description("home")]
            Home = 1,
            [Description("searchresults")]
            Search = 2,
            [Description("offerdetail")]
            Detail = 3,
            [Description("cart")]
            Cart = 4,
            [Description("other")]
            Other = 10
        }
     
        public enum Language : byte
        {
            [Description("en")]
            English = 0,
            [Description("es")]
            Spanish = 1
        }

        /// <summary>
        /// 页面访问来源类型
        /// </summary>
        public enum AccessSource : byte
        {
            [Description("直接访问")]
            Direct = 1,

            [Description("谷歌搜索")]
            GoogleSearch = 2,

            [Description("广告")]
            Ads = 3,

            [Description("其他")]
            Other = 10,
        }
    }
}
