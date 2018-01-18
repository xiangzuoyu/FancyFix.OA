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
            CheckBox = 3,
            [Description("固定值")]
            FixedValue = 4,
            [Description("国家地区")]
            Area = 5
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
        /// 客户类型
        /// </summary>
        public enum MbType : byte
        {
            [Description("批发商")]
            Wholesalers = 1,
            [Description("零售商")]
            Retailers = 2,
            [Description("品牌商")]
            Brands = 3,
            [Description("其他")]
            Other = 100
        }

        /// <summary>
        /// 产品来源
        /// </summary>
        public enum ProFrom : byte
        {
            [Description("自营产品")]
            WelfullOutdoors = 1,
            [Description("Amazon采集")]
            Amazon = 2
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        public enum OrderType : byte
        {
            /// <summary>
            /// 样品订单
            /// </summary>
            [Description("Sample Order")]
            Special = 1,
            /// <summary>
            /// 大货订单
            /// </summary>
            [Description("Mass Order")]
            Standard = 2,
            //[Description("Addition Order")]
            //Customization = 3,
            //[Description("Transferred From Inquiry")]
            //Require = 4,
            //[Description("Group Buy")]
            //GroupBuy = 5,
            /// <summary>
            /// Welbuy订单
            /// </summary>
            [Description("Welbuy Order")]
            Addtional = 6,
            [Description("Stock Order")]
            Stock = 7
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public enum OrderStatus : byte
        {
            [Description("Approval pending")]
            CheckIng = 11,
            [Description("Check failed")]
            CheckFailed = 2,
            [Description("Order approved")]
            CheckSuccess = 12,
            [Description("Samples in preparation")]
            Sample = 13,
            [Description("Order placed successfully")]
            Confirm = 21,
            [Description("Under production")]
            Production = 31,
            [Description("Production finished")]
            ProductionFinish = 32,
            [Description("Order delivered")]
            Delivered = 41,
            [Description("Order finished")]
            Finished = 101,
            [Description("Order canceled")]
            Canceled = 102,
            [Description("Order reback")]
            Reback = 200
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public enum Order_InquiryStatus : byte
        {
            [Description("Negotiation")]
            Negotiating = 1,

            [Description("Waiting for confirmation")]
            Waiting4Confirmation = 2,

            [Description("Order has been placed")]
            OrderGenerated = 3

        }


        /// <summary>
        /// 订单状态
        /// </summary>
        public enum OrderStatusGlobal : byte
        {
            [Description("Send Order Request")]
            SendOrderRequest = 10,
            [Description("Place Order Success")]
            PlaceOrderSuccess = 20,
            [Description("Order Production")]
            OrderProduction = 30,
            [Description("Shipment")]
            OrderShipment = 40,
            [Description("OrderComplete")]
            OrderComplete = 100
        }

        /// <summary>
        /// 询盘状态（后台显示）
        /// </summary>
        public enum RequireSource : byte
        {
            [Description("Sourcing consultancy")]
            Source_consultancy = 1,
            [Description("OEM Sourcing consultancy")]
            OEM_Source_consultancy = 2,
            [Description("Order control")]
            Order_control = 3,
            [Description("Logistics")]
            Logistics = 4,
            [Description("Give Us Your Advice")]
            Give_Us_Your_Advice = 10,
        }

        /// <summary>
        /// 货币种类
        /// </summary>
        public enum Currency : byte
        {

            [Description("Dollar")]
            Dollar = 1
            //[Description("EUR")]
            //EUR = 2,
            //[Description("HK")]
            //HK = 3,
            //[Description("RMB")]
            //RMB = 4,
        }

        /// <summary>
        /// 询盘状态（后台显示）
        /// </summary>
        public enum RequireAdminStatus : byte
        {

            [Description("待跟进")]
            New = 1,
            [Description("已跟进")]
            Followed = 2,
            [Description("新回复")]
            NewReply = 3,
            [Description("已生成订单")]
            CreateOrder = 4,
        }


        /// <summary>
        /// 询盘会员显示状态
        /// </summary>
        public enum RequireMemberStatus : byte
        {
            [Description("New Inquiry")]
            New = 1,
            [Description("Replied")]
            Followed = 2,
            [Description("New Reply")]
            NewRequire = 3,
            [Description("Has Created Order")]
            CreateOrder = 4,
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        public enum PayMethod : byte
        {
            [Description("PayPal")]
            PayPal = 1,

            [Description("Bank")]
            Bank = 2,

            [Description("Western Union")]
            WesternUnion = 3,
        }

        /// <summary>
        /// 付款方式
        /// </summary>
        public enum PayMent : byte
        {
            [Description("To be negotiated")]
            Undefind = 0,
            /// <summary>
            /// 电汇
            /// </summary>
            [Description("TT")]
            TT = 2,

            /// <summary>
            /// 信用证
            /// </summary>
            [Description("L/C")]
            LC = 3,

            /// <summary>
            /// 付款交单
            /// </summary>
            [Description("DP")]
            DP = 4,

            /// <summary>
            /// 放账
            /// </summary>
            [Description("OA")]
            OA = 5,

            /// <summary>
            /// 其他
            /// </summary>
            [Description("Other")]
            Other = 255
        }
        /// <summary>
        /// 付款类型
        /// </summary>
        public enum PayMentType : byte
        {
            /// <summary>
            /// 30%定金
            /// </summary>
            [Description("30% Deposit")]
            Deposit = 1,

            /// <summary>
            /// 
            /// </summary>
            [Description("100% full payment")]
            Full = 2
        }

        /// <summary>
        /// 支付状态
        /// </summary>
        public enum PayStatus : byte
        {
            /// <summary>
            /// 未支付
            /// </summary>
            [Description("UnPayed")]
            Unpayed = 0,

            /// <summary>
            /// 30%定金
            /// </summary>
            [Description("30% Deposit Payed")]
            DepositOK = 1,

            /// <summary>
            /// 全款已付
            /// </summary>
            [Description("100% full payment Payed")]
            FullOK = 2
        }

        /// <summary>
        /// 交易方式
        /// </summary>
        public enum TradeMethod : byte
        {
            [Description("To be negotiated")]
            Undefind = 0,
            [Description("FOB")]
            FOB = 2,
            [Description("EXW")]
            EXW = 1,

            [Description("CFR")]
            CFR = 3,

            [Description("CIF")]
            CIF = 4,

            [Description("DDP")]
            DDP = 5,

            [Description("DDU")]
            DDU = 6

        }

        /// <summary>
        /// 物流方式
        /// </summary>
        public enum ShipMethod : byte
        {
            [Description("To be negotiated")]
            Undefind = 0,
            [Description("By Sea")]
            Sea = 1,

            [Description("By Air")]
            Air = 2,

            [Description("By Express")]
            Express = 3
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
        /// 邮件发送步骤
        /// </summary>
        public enum MailProcess : byte
        {
            [Description("客户提交采购询盘")]
            UserRequiry = 1,
            [Description("客户提交留言")]
            UserMsg = 2,
            [Description("采购回复")]
            UserRequiryReply = 3,
            [Description("留言回复")]
            UserMsgReply = 4,
            [Description("客户提交询盘")]
            OrderSub = 5,
            [Description("订单审核通过")]
            OrderApproved = 6,
            [Description("订单打样中")]
            OrderSample = 7,
            [Description("订单已确认")]
            OrderConfirm = 8,
            [Description("订单正在生产中")]
            OrderProduction = 9,
            //[Description("订单生成完成")]
            //OrderProductionFinish = 7,
            [Description("订单已发货")]
            OrderDelivered = 11,
            [Description("订单已完成")]
            OrderFinished = 12,
            [Description("询盘/订单回复（后台）")]
            UserInquiryReply = 13,
            [Description("新客户注册(指定业务员)")]
            UserNew = 14,
            [Description("询盘/订单回复（会员）")]
            UserInquiryReply2 = 15,

            [Description("询盘信息-客户提交")]
            InquiryUserSend = 16,

            [Description("询盘信息-客户修改")]
            InquiryUserEdit = 17,

            [Description("询盘信息-平台修改")]
            InquirySupplyEdit = 18,

            [Description("询盘信息-客户确认")]
            InquiryUserConfirm = 19,

            [Description("询盘信息-平台生成订单信息")]
            InquiryCreateOrder = 20,

            [Description("用户激活")]
            UserActivation = 50,

            [Description("Welbuy订阅产品开售提醒")]
            WelbuyRemindMe = 51,

            [Description("Welbuy产品预售发送模版")]
            WelbuyNewProRemind = 52,

            [Description("用户后台注册提示内容")]
            UserCRMReg = 60,

            [Description("订单状态详细更新内容")]
            OrderUpdtes = 70,
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
        /// 会员菜单
        /// </summary>
        public enum MemberLeft : byte
        {
            [Description("Home")]
            Home = 1,
            [Description("PostRequire")]
            PostRequire = 2,
            [Description("Order")]
            Order = 3,
            [Description("Msg")]
            Msg = 4,
            [Description("Account")]
            Account = 5,
            [Description("Freight")]
            Freight = 6,
            [Description("Pwd")]
            Pwd = 7,
            [Description("Inquiry")]
            Inquiry = 8,
            [Description("SetRecMail")]
            SetRecMail = 9,
            [Description("SetRecMail")]
            WishList = 10
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
        /// 会员来源网站
        /// </summary>
        public enum FromWebSite : byte
        {
            [Description("FaceBook")]
            FaceBook = 1,

            [Description("领英")]
            LinkedIn = 2
        }

        /// <summary>
        /// 提醒信息类型
        /// </summary>
        public enum RemindType : byte
        {
            [Description("新会员注册")]
            NewUser = 1,

            [Description("新询盘/询盘留言")]
            NewInquiry = 2,

            [Description("新快速询盘/快速询盘留言")]
            NewQuickInquiry = 3,

            [Description("新站内信")]
            NewMsg = 4,

            [Description("新拼单")]
            NewPingDan = 5,
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

        /// <summary>
        /// 报关途径
        /// </summary>
        public enum OrderCustomType : byte
        {

            [Description("综试区")]
            ZSQ = 1,

            [Description("一达通")]
            YDT = 2
        }

        /// <summary>
        /// 激活类型
        /// </summary>
        public enum ActivationType : byte
        {

            [Description("未激活")]
            NewUserInactivated = 1,

            [Description("老客户未激活")]
            OldUserInactivated = 2,

            [Description("激活成功")]
            Activated = 3,

            [Description("业务员代注册")]
            FromCRM = 4,

            [Description("免注册用户")]
            NoRegUser = 10
        }

        /// <summary>
        /// 推荐产品引用位置
        /// </summary>
        public enum ProRecPositon : byte
        {

            [Description("顶部搜索关键字")]
            TopKeyword = 1,

            [Description("产品列表页推荐")]
            ProListRec = 2,

            [Description("产品详细页推荐")]
            ProDetailRec = 3,

            [Description("谷歌着陆页推荐")]
            LandingPageRec = 4,

            [Description("谷歌着陆页感兴趣")]
            LandingPageInterest = 5,

            [Description("首页welbuy产品推荐")]
            IndexWelbuyList = 6,

            [Description("首页产品-帐篷")]
            IndexTent = 7,

            [Description("首页产品-椅子")]
            IndexChair = 8,

            [Description("首页产品-睡袋")]
            IndexSleepingBag = 9,

            [Description("移动端首页产品-帐篷")]
            MobileIndexTent = 10,

            [Description("移动端首页产品-椅子")]
            MobileIndexChair = 11,

            [Description("移动端首页产品-睡袋")]
            MobileIndexSleepingBag = 12,

            [Description("批发产品专题页产品")]
            Sp_Wholesale = 13
        }

        /// <summary>
        /// 拼单产品状态
        /// </summary>
        public enum WelBuyStatus : byte
        {

            [Description("Start Soon")]
            StartSoon = 1,

            [Description("Ongoing")]
            Ongoing = 2,

            [Description("Closure")]
            Closure = 3
        }

        public enum WelbuyMailRemind : byte
        {

            [Description("产品开售提醒.(来源：welbuy产品RemindMe)")]
            ProOnsale = 1,

            [Description("新产品上架提醒.(来源：welbuy产品列表)")]
            NewProOnsale = 2,

            [Description("任意信息推送.(来源：首页)")]
            GetAllNews = 3,

            [Description("资讯信息推送.(来源：资讯站右侧)")]
            GetNewArticle = 4
        }

        public enum BannerType : byte
        {

            [Description("Web首页Banner")]
            WebIndexBanner = 1,

            [Description("Mobile首页Banner")]
            MobileIndexBanner = 2,

            [Description("Blog首页Banner")]
            BlogIndexBanner = 3,

            [Description("About首页Banner")]
            AboutIndexBanner = 3
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

        /// <summary>
        /// Facebook页面访问来源类型
        /// </summary>

        public enum FbaType : byte
        {
            [Description("CompleteRegistration")]
            CompleteRegistration = 1,

            [Description("Lead")]
            Lead = 2,

            [Description("AddToCart")]
            AddToCart = 3,

            [Description("ViewContent")]
            ViewContent = 4,

            [Description("Search")]
            Search = 5,

            [Description("InitiateCheckout")]
            InitiateCheckout = 6
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        public enum RegType : byte
        {
            [Description("免注册")]
            NoUserReg = 1,

            [Description("主动注册")]
            UserReg = 2,

            [Description("代注册[LiveChat]")]
            LiveChat = 3,

            [Description("代注册[Service邮箱]")]
            Service = 4,
            [Description("代注册[阿里巴巴]")]
            Alibaba = 5,
            [Description("代注册[邮件推广]")]
            Email = 6,
            [Description("代注册[其他]")]
            Other = 10,
        }

        /// <summary>
        /// 询盘提交类型
        /// </summary>
        public enum InquiryType : byte
        {
            [Description("用户网站提交")]
            UserSub = 1,

            [Description("LiveChat")]
            LiveChat = 2,

            [Description("Service邮箱")]
            Service = 3,

            [Description("Require")]
            Require = 4,

            [Description("阿里巴巴")]
            Alibaba = 5,

            [Description("邮件推广")]
            Email = 6,

            [Description("其他")]
            Other = 10,
        }

        /// <summary>
        ///  询盘标签分类
        /// </summary>
        public enum InquiryTag : byte
        {
            [Description("常规采购")]
            TagNormal = 1,

            [Description("样品采购")]
            TagSample = 2,

            [Description("个人采购")]
            TagCustomer = 3,

            [Description("垃圾采购")]
            TagInvalid = 4,

            [Description("定制采购")]
            TagCustomize = 5,
        }

        public enum SortType : byte
        {
            [Description("Most Popular")]
            MostPopular = 1,

            [Description("Recommended")]
            Recommended = 2,

            [Description("New Arrival")]
            NewArrival = 3,

        }
        public enum SortTypeSearch : byte
        {
            [Description("Most Popular")]
            MostPopular = 1,

            [Description("Recommended")]
            Recommended = 2
        }


        public enum SampleType : byte
        {
            [Description("In Stock")]
            InStock = 1,
            [Description("Proofing")]
            Proof = 2
        }

        public enum ExpressType : byte
        {
            [Description("文件")]
            Folder = 1,
            [Description("小包裹")]
            Package = 2,
            [Description("大货")]
            Lager = 3
        }
        public enum Language : byte
        {
            [Description("en")]
            English = 0,
            [Description("es")]
            Spanish = 1,
            //[Description("fr-FR")]
            //French = 2
        }

        public enum IndustryType : byte
        {
            [Description("纯文件")]
            Files = 1,

            [Description("资讯")]
            News = 2
        }
    }
}
