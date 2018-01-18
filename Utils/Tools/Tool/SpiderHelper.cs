using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using FancyFix.Tools.Helper;

namespace FancyFix.Tools.Tool
{
    public class SpiderHelper
    {
        private static void CreatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


        public static string GetLocalHtml(string url)
        {
            string strHtmlContent = File.ReadAllText(url, Encoding.GetEncoding("UTF-8"));
            return strHtmlContent;
        }
        /// <summary>
        /// 下载图片并保持至新地址 
        /// </summary>
        /// <param name="listPic"></param>
        /// <returns></returns>
        public static List<string> DownLoadPic(List<string> listPic, string imgPath, int id)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < listPic.Count; i++)
            {
                string t = listPic[i].Split('?')[0].ToString();
                string pic = DownLoadPic(t, imgPath, id, i);
                if (pic != "")
                    list.Add(pic);
            }
            return list;
        }

        /// <summary>
        /// 根据ID获取放置的文件夹路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDocById(int id)
        {
            string docPath = "";
            string strId = id.ToString();
            int strLen = strId.Length;
            int strDepth = (int)Math.Floor((decimal)strLen / 3);
            for (int i = 0; i < strDepth; i++)
            {
                docPath += strId.Substring(0, 3) + "/";
                strId = strId.Substring(3);
            }
            docPath += "000/";
            return docPath;

        }
        public static string DownLoadPic(string url, string imgPath, int id, int nameIndex)
        {
            try
            {
                string ext = Path.GetExtension(url).ToLower();
                string path = imgPath + GetDocById(id);
                string rndFileName = id.ToString() + nameIndex.ToString(); //by 索引值命名的文件名
                string fileName = path + rndFileName + ext;

                CreatePath(path);

                //判断文件夹下文件数量
                DirectoryInfo dir = new DirectoryInfo(path);
                int fileNum = dir.GetFiles().Length;
                //by 文件夹内已有文件数，增量命名的文件名，防止跟索引值文件名重叠，导致图片重复
                if (fileNum > 0)
                    fileName = path + id.ToString() + (fileNum + 1).ToString() + ext;

                if (File.Exists(fileName))
                {
                    return fileName.Replace(imgPath, "");
                }
                WebClient my = new WebClient();
                byte[] mybyte;
                mybyte = my.DownloadData(url);
                MemoryStream ms = new MemoryStream(mybyte);
                System.Drawing.Image img;
                img = System.Drawing.Image.FromStream(ms);
                img.Save(fileName);   //保存

                return fileName.Replace(imgPath, "");
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                // LogHelper.Error(ex.ToString());
                return "";
            }
        }


        /// <summary>
        /// 获得一个9位时间随机数(小时)
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDataShortRandom()
        {
            return System.DateTime.Now.ToString("HHmmssfff");
        }

        public static string GetHtmlDecode(string html)
        {
            return HttpUtility.UrlDecode(html);
        }
    }
}
