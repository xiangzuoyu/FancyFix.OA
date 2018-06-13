using FancyFix.OA.Model;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace FancyFix.OA.Files.api
{
    public class DownloadController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetFile(string spu, string tag = "", byte type = 0)
        {
            HttpResponseMessage result = null;

            Product_Info model = Bll.BllProduct_Info.First(o => o.Spu == Tools.Usual.Utils.CheckSqlValue(spu));
            List<Product_Image> imglist = null;
            if (model != null)
                imglist = Bll.BllProduct_Image.GetList(model.Id, tag, type);

            if (imglist != null && imglist.Count > 0)
            {
                //压缩包名称
                var typeName = Tools.Enums.Tools.GetEnumDescription(typeof(Tools.Enums.ESite.ImageType), type);
                typeName = typeName != "" ? "-" + typeName : "";
                tag = tag != "" ? "-" + tag : "";
                var zipFileName = string.Format($"{spu}{typeName}{tag}.zip");

                //压缩包最终生成路径
                var downloadDir = HttpContext.Current.Server.MapPath($"~/downloads/download");
                var archive = $"{downloadDir}/{zipFileName}";

                //临时文件夹目录
                var tempDir = HttpContext.Current.Server.MapPath("~/downloads/temp");

                //创建临时目录,清空临时文件夹中的所有临时文件
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);
                else
                    Directory.EnumerateFiles(tempDir).ToList().ForEach(File.Delete);

                //遍历文件列表
                foreach (var img in imglist)
                {
                    //得到文件绝对路径
                    string filePath = ConfigurationManager.AppSettings["filePath"] + "/" + img.ImagePath.Replace(ConfigurationManager.AppSettings["ImgHttp"] + "/", "");

                    //文件存在则复制到临时目录下
                    FileInfo fi = new FileInfo(filePath.Replace("/ ", "\\"));
                    if (fi.Exists)
                    {
                        string destPath = Path.Combine(tempDir, GetFileName(filePath));
                        File.Copy(filePath, destPath, true);
                    }
                }
                //清空最终生成目录
                ClearDownloadDirectory(downloadDir);

                using (var zip = new ZipFile())
                {
                    //添加临时文件夹里的文件到zip对象
                    zip.AddDirectory(tempDir);

                    //生成zip
                    zip.Save(archive);
                }

                FileStream fs = new FileStream(archive, FileMode.Open);
                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(fs);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = zipFileName;
                result.Content.Headers.ContentLength = fs.Length;
            }
            else
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return result;
        }

        private string GetFileName(string filePath)
        {
            return filePath.Substring(filePath.LastIndexOf('/') + 1);
        }

        private void ClearDownloadDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                var files = Directory.GetFiles(directory);
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}