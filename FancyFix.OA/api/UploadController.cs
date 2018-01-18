using System;
using System.Web;
using System.Web.Http;
using FancyFix.Tools.Tool;
using System.IO;
using FancyFix.Tools.Config;
using System.Configuration;

namespace FancyFix.OA.api
{
    public class UploadController : ApiController
    {
        private static Tools.Config.UploadConfig config = UploadProvice.Instance();

        [HttpPost]
        public object UploadFile()
        {
            string uptype = HttpContext.Current.Request["uptype"].Trim2();//获取上传配置  
            string checkCode = HttpContext.Current.Request["checkCode"].Trim2();  // 验证权限
            string key = HttpContext.Current.Request["key"].Trim2();  // 验证权限
            string id = HttpContext.Current.Request["id"].Trim2();        //文件Id
            string name = HttpContext.Current.Request["name"].Trim2();    //源文件名
            string title = HttpContext.Current.Request["title"].Trim2();    //文件标题

            string status = "success";

            if (HttpContext.Current.Request.Files.Count == 0)
            {
                return Json(new { code = 0, id = id, name = name, url = "", msg = "please select files" });
            }
            try
            {
                if (!Core.Crypt.WebKey.KeyCheck2(key, checkCode))
                {
                    status = "验证出错!";
                    goto result;
                }
                if (uptype == "")
                {
                    status = "上传配置丢失!";
                    goto result;
                }
                else
                {
                    //读取图片类型配置
                    Setting setting = config.Settings[uptype];
                    if (setting == null)
                    {
                        status = "没有找到当前配置!";
                        goto result;
                    }
                    else
                    {
                        //检测上传限制
                        string allowUploadExts = setting.AllowUpload.Replace(".", "");

                        HttpPostedFile fileUpload = HttpContext.Current.Request.Files[0];
                        if (fileUpload == null)
                        {
                            status = "请选择上传文件!";
                            goto result;
                        }
                        if (string.IsNullOrEmpty(fileUpload.FileName))
                        {
                            status = "请选择上传文件!";
                            goto result;
                        }
                        if (fileUpload.ContentLength <= 0)
                        {
                            status = "请选择上传文件!";
                            goto result;
                        }
                        //检测文件大小是否超过限制
                        if (fileUpload.ContentLength > setting.MaxFileSize)
                        {
                            status = "上传文件超过大小限制!";
                            goto result;
                        }
                        //检测文件类型
                        string fileExt = Path.GetExtension(fileUpload.FileName).ToLower();
                        if (string.IsNullOrEmpty(fileExt))
                        {
                            status = "文件类型出错!";
                            goto result;
                        }
                        else
                        {
                            bool allowUploadExt = false;
                            string allUploadExts = setting.AllowUpload;
                            string[] exps = allUploadExts.Split('|');
                            foreach (string exp in exps)
                            {
                                if (exp.ToLower() == fileExt)
                                {
                                    allowUploadExt = true;
                                    break;
                                }
                            }
                            if (!allowUploadExt)
                            {
                                status = "文件类型出错!";
                                goto result;
                            }
                        }

                        string saveDoc = DateTime.Now.ToString(@"yy\\MM\\");
                        string rndFileName = Tools.Usual.Common.GetDataShortRandom();

                        //如果文件需要标题，则在路径中添加标题
                        if (!string.IsNullOrEmpty(title))
                        {
                            rndFileName = Tools.Usual.Utils.ConverUrl(Tools.Common.StringProcess.CutString(title, 50)).TrimEnd('-') + "-" + rndFileName;
                        }

                        //获取绝对路径
                        string dirPath = setting.FilePath;

                        string newFileName = saveDoc + rndFileName + fileExt;
                        string newSmallFileName = saveDoc + rndFileName + "s" + fileExt;
                        if (status == "success")
                        {
                            #region 保存文件

                            if (!Directory.Exists(dirPath + saveDoc))
                            {
                                Directory.CreateDirectory(dirPath + saveDoc);
                            }

                            // 文件上传 
                            fileUpload.SaveAs(dirPath + newFileName);

                            //如果上传图片，则执行图片配置选项.
                            if (fileExt == ".jpg" || fileExt == ".gif" || fileExt == ".jpeg" || fileExt == ".bmp" || fileExt == ".png")
                            {
                                // 限制图片大小
                                Tools.Tool.ImageTools.SetImgSize(dirPath + newFileName, setting.MaxWidth, setting.MaxHeight);

                                string newBigFileName = saveDoc + rndFileName + "b" + fileExt;

                                string newMiddleFileName = saveDoc + rndFileName + "m" + fileExt;

                                //生成小图
                                if (setting.CreateSmallPic)
                                {
                                    ImageTools.CreateSmallImage(dirPath + newFileName, dirPath + newSmallFileName, setting.Width, setting.Height);
                                }
                                //生成中图
                                if (setting.CreateMiddlePic)
                                {
                                    ImageTools.CreateSmallImage(dirPath + newFileName, dirPath + newMiddleFileName, setting.MiddleWidth, setting.MiddleHeight);
                                }
                                //生成大图
                                if (setting.CreateBigPic)
                                {
                                    ImageTools.CreateSmallImage(dirPath + newFileName, dirPath + newBigFileName, setting.BigWidth, setting.BigHeight);
                                }
                                //打水印
                                if (setting.AddWaterMark)
                                {
                                    WaterMarkType wmtype;
                                    string waterMarkImgOrTxt;
                                    if (setting.WaterMarkType == "image")
                                    {
                                        wmtype = WaterMarkType.ImageMark;
                                        waterMarkImgOrTxt = HttpContext.Current.Server.MapPath(setting.WaterMarkImgOrTxt);
                                    }
                                    else
                                    {
                                        wmtype = WaterMarkType.TextMark;
                                        waterMarkImgOrTxt = setting.WaterMarkImgOrTxt;
                                    }

                                    Tools.Tool.ImageTools.AddWaterMark(dirPath + newFileName, dirPath + newFileName, waterMarkImgOrTxt, wmtype, WaterMarkPosition.Right_Bottom, setting.Transparency);
                                    Tools.Tool.ImageTools.AddWaterMark(dirPath + newBigFileName, dirPath + newBigFileName, waterMarkImgOrTxt, wmtype, WaterMarkPosition.Right_Bottom, setting.Transparency);
                                }
                            }
                            #endregion
                        }

                        // 返回url
                        string urlRealm = setting.UrlFilePath;
                        string urlFile = urlRealm + newFileName.Replace(@"\", @"/"); //相对路径
                        string urlComplete = urlFile; //完整路径

                        return Json(new FileResult() { code = 1, id = id, name = name, url = urlComplete, msg = status });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new FileResult() { code = -1, id = id, name = name, url = "", msg = ex.ToString() });
            }
            result:
            return Json(new FileResult() { code = 0, id = id, name = name, url = "", msg = status });
        }
    }

    public class FileResult
    {
        public int code { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string msg { get; set; }
        public string url { get; set; }
    }
}
