using FancyFix.OA.Helper;
using FancyFix.Tools.Config;
using FancyFix.Tools.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;

namespace FancyFix.OA.Config
{
    public class ImgUploadConfig
    {
        //默认值
        private string _FileExt = "gif,jpg,jpeg,bmp,png";
        private int _MaxSize = 5 * 1024 * 1024;
        private int _UploadNum = 10;
        private string _UploadName = "pic";
        private string _Uptype = "web";
        private string _Domain = "local";
        private bool _IsProduct = false;

        //加密字段
        private static int random = new Random().Next(1000, 9999);
        private string _Key = Core.Crypt.WebKey.KeyGet2(random);
        private string _CheckCode = Core.Crypt.WebKey.KeyCheckCode2(random);

        public string FileExt { get { return _FileExt; } set { _FileExt = value; } }
        public int MaxSize { get { return _MaxSize; } set { _MaxSize = value; } }
        public int UploadNum { get { return _UploadNum; } set { _UploadNum = value; } }
        public string Uptype { get { return _Uptype; } set { _Uptype = value; } }
        public string Key { get { return _Key; } set { _Key = value; } }
        public string CheckCode { get { return _CheckCode; } set { _CheckCode = value; } }
        public string SuccessFun { get; set; }
        public string Title { get; set; }
        public string Pics { get; set; }
        public string ElementId { get; set; }
        public string Domain { get { return _Domain; } set { _Domain = value; } }
        public string WidthHeight { get; set; }
        public bool IsProduct { get { return _IsProduct; } set { _IsProduct = value; } }
        public string UploadName { get { return _UploadName; } set { _UploadName = value; } }
        public int ProId { get; set; }
        public byte Type { get; set; }
    }


    public class ImgConfig
    {
        private static Tools.Config.UploadConfig config = UploadProvice.Instance();
        public static string GetProductUrl(string imgurl)
        {
            if (string.IsNullOrWhiteSpace(imgurl))
                return string.Empty;

            imgurl = imgurl.Replace("\\", "/").TrimStart('/');
            var ext = Path.GetExtension(imgurl);
            string url = string.Empty;
            SiteOption option = config.SiteOptions["files"];
            var productConfig = config.Settings["product"];
            //图片
            string dirPath = option.Folder.TrimEnd('\\') + productConfig.FilePath + "upload\\";
            string imgPath = string.Concat(dirPath, imgurl.Replace("/", "\\"));
            if (ext == ".jpg" || ext == ".gif" || ext == ".jpeg" || ext == ".bmp" || ext == ".png")
            {
                if (option != null && productConfig != null)
                {
                    imgurl = "upload/" + imgurl;
                    url = string.Concat(option.Url, productConfig.UrlFilePath, imgurl);
                    if (File.Exists(imgPath))
                    {
                        ProductImgQueueInfo queueInfo = new ProductImgQueueInfo();
                        queueInfo.ImgPath = imgPath;
                        queueInfo.ImgUrl = imgurl;
                        ProdcutImgQueue.ProdcutImgQueueInstance.AddQueue(queueInfo);
                    }
                }
            }
            return url;
        }
    }
}