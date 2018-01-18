using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace FancyFix.Tools.Tool
{
    public class Uploader
    {
        string state = "SUCCESS";

        string URL = null;
        string currentType = null;
        string uploadpath = null;
        string filename = null;
        string originalName = null;
        HttpPostedFile uploadFile = null;

        /**
          * 上传文件的主处理方法
          * @param HttpContext
          * @param string
          * @param  string[]
          *@param int
          * @return Hashtable
        */
        public Hashtable upFile(HttpContext cxt, string filePath, string urlFilePath, string[] filetype, int size)
        {
            //保存文件
            string saveDoc = DateTime.Now.ToString(@"yy\\MM\\dd\\");
            string rndFileName = FancyFix.Tools.Usual.Common.GetDataShortRandom();
            string dirPath = filePath;
            string urlRealm = System.Configuration.ConfigurationManager.AppSettings["ImgHttp"].ToString() + urlFilePath;
            
            try
            {
                uploadpath = dirPath + saveDoc;
                uploadFile = cxt.Request.Files[0];
                originalName = uploadFile.FileName;
                string fileExt = Path.GetExtension(uploadFile.FileName).ToLower();
                filename = rndFileName + fileExt;

                //string newSmallFileName = saveDoc + rndFileName + "s" + fileExt;
                string urlFile = urlRealm + saveDoc.Replace(@"\", @"/") + filename;
                //目录创建
                createFolder();

                //格式验证
                if (checkType(filetype))
                {
                    state = "不允许的文件类型";
                }
                //大小验证
                if (checkSize(size))
                {
                    state = "文件大小超出网站限制";
                }
                //保存图片
                if (state == "SUCCESS")
                {
                    uploadFile.SaveAs(uploadpath + filename);
                    URL = urlFile;
                }
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            {
                state = "未知错误";
                URL = "";
            }
            return getUploadInfo();
        }

        public Hashtable upFile(HttpContext cxt, string[] filetype, int size)
        {
            try
            {
                string pathBase = getTmpFilePath(cxt);

                uploadFile = cxt.Request.Files[0];
                originalName = uploadFile.FileName;
                string tempPath = DateTime.Now.ToString("yyyy-MM-dd");
                uploadpath = uploadpath + tempPath;
                filename = reName();
                string savePath = uploadpath + @"\" + filename;
                string urlPath = pathBase + tempPath + "/" + filename;

                //目录创建
                createFolder();

                //格式验证
                if (checkType(filetype))
                {
                    state = "不允许的文件类型";
                }

                //大小验证
                if (checkSize(size))
                {
                    state = "文件大小超出网站限制";
                }

                //保存图片
                if (state == "SUCCESS")
                {
                    uploadFile.SaveAs(savePath);
                    URL = urlPath;
                }
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            {
                state = "未知错误";
                URL = "";
            }
            return getUploadInfo();
        }

        /**
         * 上传涂鸦的主处理方法
          * @param HttpContext
          * @param string
          * @param  string[]
          *@param string
          * @return Hashtable
         */
        public Hashtable upScrawl(HttpContext cxt, string filePath, string urlFilePath, string base64Data)
        {
            //保存文件
            string saveDoc = DateTime.Now.ToString(@"yy\\MM\\dd\\");
            string rndFileName = FancyFix.Tools.Usual.Common.GetDataShortRandom();
            string dirPath = filePath;
            string urlRealm = System.Configuration.ConfigurationManager.AppSettings["ImgHttp"].ToString() + urlFilePath;

            FileStream fs = null;
            try
            {
                uploadpath = dirPath + saveDoc;
                //创建目录
                createFolder();
                string fileExt = cxt.Request["fileext"];
                if (string.IsNullOrEmpty(fileExt))
                    fileExt = ".png";
                //生成图片
                filename = rndFileName + fileExt;
                //filename = System.Guid.NewGuid() + ".png";
                string urlFile = urlRealm + saveDoc.Replace(@"\", @"/") + filename;
                fs = File.Create(uploadpath + filename);
                byte[] bytes = Convert.FromBase64String(base64Data);
                fs.Write(bytes, 0, bytes.Length);

                URL = urlFile;
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            {
                state = "未知错误";
                URL = "";
            }
            finally
            {
                fs.Close();
                //deleteFolder(cxt.Server.MapPath(tmppath));
            }
            return getUploadInfo();
        }

        /**
        * 获取文件信息
        * @param context
        * @param string
        * @return string
        */
        public string getOtherInfo(HttpContext cxt, string field)
        {
            string info = null;
            if (cxt.Request.Form[field] != null && !String.IsNullOrEmpty(cxt.Request.Form[field]))
            {
                info = field == "fileName" ? cxt.Request.Form[field].Split(',')[1] : cxt.Request.Form[field];
            }
            return info;
        }

        /**
         * 获取上传信息
         * @return Hashtable
         */
        private Hashtable getUploadInfo()
        {
            Hashtable infoList = new Hashtable();

            infoList.Add("state", state);
            infoList.Add("url", URL);

            if (currentType != null)
                infoList.Add("currentType", currentType);
            if (originalName != null)
                infoList.Add("originalName", originalName);
            return infoList;
        }

        /**
         * 重命名文件
         * @return string
         */
        private string reName()
        {
            return System.Guid.NewGuid() + getFileExt();
        }


        /**
         * 文件类型检测
         * @return bool
         */
        private bool checkType(string[] filetype)
        {
            currentType = getFileExt();
            return Array.IndexOf(filetype, currentType) == -1;
        }

        /**
         * 文件大小检测
         * @param int
         * @return bool
         */
        private bool checkSize(int size)
        {
            //return uploadFile.ContentLength >= (size * 1024 * 1024);
            return uploadFile.ContentLength >= size;
        }

        /**
         * 获取文件扩展名
         * @return string
         */
        private string getFileExt()
        {
            string[] temp = uploadFile.FileName.Split('.');
            return "."+temp[temp.Length - 1].ToLower();
        }

        /**
         * 按照日期自动创建存储文件夹
         */
        private void createFolder()
        {
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }
        }

        /**
         * 删除存储文件夹
         * @param string
         */
        public void deleteFolder(string path)
        {
            //if (Directory.Exists(path))
            //{
            //    Directory.Delete(path, true);
            //}
        }

        /**
          * 删除临时文件
          * @param string
          */
        public void deleteTemFile(HttpContext cxt, string fileName)
        {
            //  filename = DateTime.Now.ToString("yyyy-MM-dd") + "/" + reName();
            Regex rg = new Regex(@"\d{4}-\d{2}-\d{2}/.{1,}",RegexOptions.IgnoreCase);
            if (rg.IsMatch(fileName))
            {
                filename = rg.Match(fileName).Value.Trim();
                getTmpFilePath(cxt);
                if (File.Exists(uploadpath + filename))
                    File.Delete(uploadpath + filename);
            }
        }

        /**
        * 统一获取临时文件存放地址
        * @param string
        */
        private string getTmpFilePath(HttpContext cxt)
        {
            uploadpath = cxt.Server.MapPath("/siteadmin/ueditor/tmp/"); //获取文件上传路径

            return "/siteadmin/ueditor/tmp/";
        }
    }
}
