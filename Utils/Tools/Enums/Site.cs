using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Enums
{
    public class Site
    {
        public enum BannerType : byte
        {
            [Description("首页Banner")]
            IndexBanner = 1,

            [Description("展会Banner")]
            Exhibition = 2
        }

        public enum QuoteType : byte
        {
            [Description("ContactUs")]
            ContactUs = 1,

            [Description("Inquiry")]
            Inquiry = 2,

            [Description("FeedBack")]
            FeedBack = 3,

            [Description("RequestBrochure")]
            RequestBrochure = 4,

            [Description("BecomeDistributor")]
            BecomeDistributor = 5,

            [Description("Download")]
            Download = 6,

            [Description("Subscribe")]
            Subscribe = 7,

            [Description("GetLatestPrice")]
            GetLatestPrice = 8
        }

        public enum QuoteFrom : byte
        {
            [Description("ContactUs页面")]
            ContactUs = 1,

            [Description("产品详情页")]
            Product = 2,

            [Description("CotactUs弹窗")]
            FloatDialog = 3,

            [Description("底部表单")]
            Bottom = 4,

            [Description("FeedBack页面")]
            FeedBack = 5,

            [Description("RequestBrochure页面")]
            RequestBrochure = 6,

            [Description("BecomeDistributor页面")]
            BecomeDistributor = 7,

            [Description("产品列表页")]
            ProductList = 8,

            [Description("文档下载页")]
            Download = 9,

            [Description("案例详情页")]
            CaseDetail = 10,

            [Description("首页")]
            Index = 11,

            [Description("LandingPage")]
            LandingPage = 12
        }
    }
}
