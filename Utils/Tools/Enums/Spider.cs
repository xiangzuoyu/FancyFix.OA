using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FancyFix.Tools.Enums
{
    public class Spider
    {

        public enum CompanyStatus : int
        {
            [Description("无效")]
            Invalid = -1,
            [Description("正常")]
            Normal = 0,
        }

        public enum CheckStatus : int
        {
            [Description("处理失败")]
            Fail = -1,
            [Description("未处理")]
            Wait = 0,
            [Description("处理成功")]
            Success = 1,
            [Description("图片未下载")]
            UnDownLoad = 2
        }

        public enum CompanyImg : byte
        {
            [Description("公司图")]
            CompanyImg = 1,
            [Description("生产流程图")]
            ProductionFlowImg = 2,
            [Description("公司Logo")]
            CompanyLogo = 3,
            [Description("联系人Logo")]
            ContactLogo = 4
        }

        public enum GatherResult : int
        {
            [Description("数据不完整")]
            Incomplete = -2,
            [Description("发生错误")]
            Error = -1,
            [Description("添加失败")]
            InsertFailed = 0,
            [Description("添加成功")]
            InsertSuccess = 1,
            [Description("更新成功")]
            UpdateSuccess = 2,
            [Description("更新成功")]
            UpdateFailed = 3
        }

        public enum ProductImg : byte
        {
            [Description("产品图片")]
            ProductImg = 1,
            [Description("产品详细页内容图片")]
            DetailImg = 2
        }

        public enum ImgType : byte
        {
            [Description("原图")]
            Img = 1,
            [Description("图片50*50")]
            Img50 = 2,
            [Description("图片200*200")]
            Img200 = 3,
            [Description("图片350*350")]
            Img350 = 4
        }

        public enum ProListGatherTag : int
        {
            [Description("未采集")]
            Begin = 0,
            [Description("采集中")]
            Working = 1,
            [Description("采集完成")]
            Finish = 2,
        }

        /// <summary>
        /// 综试区代码类型
        /// </summary>
        public enum SpiderProTag : byte
        {
            [Description("下载Html等待中")]
            DownLoadWait = 1,

            [Description("下载Html中")]
            DownloadIng = 2,

            [Description("下载Html完成")]
            DownloadOk = 3,

            [Description("下载报错")]
            DownloadError = 4,

            [Description("分析Html中")]
            AnalysisIng = 5,

            [Description("分析Html成功")]
            AnalysisOk = 6,

            [Description("分析Html报错")]
            AnalysisError = 7,

            [Description("发布成功")]
            PublishOk = 10,

            [Description("图片待处理")]
            ProWaitPicHandel = 11,

            [Description("产品不可用")]
            ProUnEnable = 12,

            [Description("产品待发布")]
            ProWaitPub = 13,

            [Description("发布至待审核")]
            ProToCheck = 20,

            [Description("图片不符合")]
            PicNoPass = 101,

            [Description("未知错误")]
            Error = 102,
            [Description("标题重复")]
            Repeat = 103,
        }

        public enum SpiderTagClass : byte
        {
            [Description("分类信息待处理")]
            WaitUrlMax = 1,

            [Description("分类处理中2")]
            Handeling = 2,

            [Description("Url处理成3")]
            UrlOk = 3,

            [Description("处理异常4")]
            Error4 = 4,

            [Description("采集产品详细中5")]
            SpiderDetailIng = 5,

            [Description("采集产品详细成功6")]
            SpiderDetailOk = 6,

            [Description("分析页面出错")]
            Error7 = 7,


            [Description("分类不可用")]
            Unabled = 10
        }

        public enum SpiderClassCode : int
        {
            [Description("ZP")]
            Tent = 1,

            [Description("SD")]
            SleepingBag = 2,

            [Description("YZ")]
            OutdoorChair = 3,

            [Description("TC")]
            OutdoorBed = 4,

            [Description("ZZ")]
            OutdoorTable = 5,

            [Description("LZ")]
            PicnicBasket = 6,

            [Description("DC")]
            Hammock = 7,

            [Description("BB")]
            Backpack = 48,

            [Description("SZ")]
            OutdoorSticks = 60,

            [Description("YYD")]
            OutdoorMat = 56,

            [Description("PR")]
            OutdoorCooking = 67,

            [Description("CQ")]
            AirBedsPads = 72,

            [Description("DJ")]
            LightingLanterns = 76,

            [Description("JJ")]
            FirstAidSurvival = 85,

            [Description("PJ")]
            OutdoorAccessories = 86,

            [Description("QT")]
            Other = 66
        }
    }
}
