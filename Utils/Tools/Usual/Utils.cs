using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using FancyFix.Tools.Config;
using System.Web.UI;
using System.Web;
using System.Data;
using System.Linq;
using FancyFix.Tools.Json;

namespace FancyFix.Tools.Usual
{
    public class Utils
    {
        #region 字符串处理

        /// <summary>
        /// 剪切字符串
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public static string CutString(string strInput, int intLen)
        {
            if (String.IsNullOrEmpty(strInput))
                return strInput;
            strInput = strInput.Trim();
            byte[] buffer1 = Encoding.Default.GetBytes(strInput);
            if (buffer1.Length > intLen)
            {
                string text1 = "";
                for (int num1 = 0; num1 < strInput.Length; num1++)
                {
                    byte[] buffer2 = Encoding.Default.GetBytes(text1);
                    if (buffer2.Length >= (intLen - 4))
                    {
                        break;
                    }
                    text1 = text1 + strInput.Substring(num1, 1);
                }
                return (text1 + "...");
            }
            return strInput;
        }

        /// <summary>
        /// 获取：张***三格式的字段
        /// </summary>
        /// <param name="strInput">文件</param>
        /// <returns></returns>
        public static string GetSubContent(string strInput)
        {
            if (String.IsNullOrEmpty(strInput))
                return strInput;
            else
            {
                string text1 = "";
                text1 += strInput.Substring(0, 1);
                for (int num1 = 1; num1 < strInput.Length - 1; num1++)
                {
                    text1 += "*";
                }
                text1 += strInput.Substring(strInput.Length - 1);
                return text1;
            }
        }

        /// <summary>
        /// 文本框内容输出成一行(文本内容去回车空格,转换HTML输出)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToLineText(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = RemoveHtml(str);
            str = str.Replace("　", "");


            string text1 = "\\s+";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, " ");
            str = System.Web.HttpUtility.HtmlEncode(str);
            str = str.Replace("&#58;", ";");
            str = str.Replace("&#46;", ".");
            str = str.Replace("&#32;", " ");
            str = str.Replace("&#40;", "(");
            str = str.Replace("&#41;", ")");
            str = str.Replace("&#43;", " ");
            str = str.Replace("&#39;", "'");
            return str.Trim();
        }

        /// <summary>
        /// 剪切字符串(末尾不加点号)
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public static string GetSubString(string strInput, int intLen)
        {
            strInput = strInput.Trim();
            byte[] buffer1 = Encoding.Default.GetBytes(strInput);
            if (buffer1.Length > intLen)
            {
                string text1 = "";
                for (int num1 = 0; num1 < strInput.Length; num1++)
                {
                    byte[] buffer2 = Encoding.Default.GetBytes(text1 + strInput.Substring(num1, 1));
                    if (buffer2.Length > intLen)
                    {
                        break;
                    }
                    text1 = text1 + strInput.Substring(num1, 1);
                }
                return (text1);
            }
            return strInput;
        }

        /// <summary>
        /// 获取邮箱后缀
        /// by:willian date:2016-6-7
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetEmailSubffix(string email)
        {
            return email.Substring(email.IndexOf('@') + 1, email.Length - email.IndexOf('@') - 1);

        }

        /// <summary>
        /// 截取字符串 标点符号 空格截取为一句
        /// by:willian date:2015-11-30
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public static string GetSubSentence(string strInput, int intLen)
        {
            string subStr = GetSubString(strInput, intLen);
            return subStr.Substring(0, subStr.LastIndexOf(' ') > 0 ? subStr.LastIndexOf(' ') : subStr.Length) + "…";
        }
        /// <summary>
        /// 文本框内容输出成html显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHtmlText(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(str);
            builder1.Replace("&", "&amp;");
            builder1.Replace("<", "&lt;");
            builder1.Replace(">", "&gt;");
            builder1.Replace("\"", "&quot;");
            builder1.Replace("\r", "<br>");
            builder1.Replace(" ", "&nbsp;");
            return builder1.ToString();
        }

        /// <summary>
        /// Texteara 文本显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTexteara(string str)
        {
            if (str == null || str == "")
            {
                return ("&nbsp;");
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\r", "<br />");
                str = str.Replace("\t", "<br />");
                return (str);
            }
        }

        /// <summary>
        /// DataRow数据转DataTable
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构  
            foreach (DataRow row in rows)
                tmp.Rows.Add(row.ItemArray);  // 将DataRow添加到DataTable中  
            return tmp;
        }


        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtml(string str)
        {
            string text1 = "<.*?>";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, "");
            str = str.Replace("&nbsp;", " ");

            return str;
        }

        /// <summary>
        /// 去除html标签并截字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string RemoveHtmlAndLimit(string str, int num)
        {
            str = RemoveHtml(str);
            str = CutString(str, num);

            return str;
        }

        /// <summary>
        /// 去除空格换行
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpace(string str)
        {
            Regex reg = new Regex("\\s*");
            return reg.Replace(str, "");
        }

        /// <summary>
        /// 返回URL编码的值
        /// </summary>
        /// <param name="str">传入参数</param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return System.Web.HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// 转换大写字母 避免特殊字符转换错误
        /// 创建人:俞忠亮 创建时间:2012-5-29
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUpperNew(string str)
        {
            string ret = string.Empty;
            str = str.Trim();
            foreach (char zf in str)
            {
                if ('a' <= zf && zf <= 'z')
                {
                    ret += char.ToUpper(zf);
                }
                else
                {
                    ret += zf;
                }
            }

            return ret;
        }

        public static string Md5(string str, int code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符）  
            {
#pragma warning disable CS0618 // “FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
#pragma warning restore CS0618 // “FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            }
            if (code == 32) //32位加密  
            {
#pragma warning disable CS0618 // “FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
#pragma warning restore CS0618 // “FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            }
            return "00000000000000000000000000000000";
        }


        public static bool IsNumber(string checkStr)
        {
            if (string.IsNullOrEmpty(checkStr)) { return false; }
            return Regex.IsMatch(checkStr, @"^[-]{0,1}\d+$");

        }


        /// <summary>
        /// 格式为yyyy-MM-dd
        /// 创建人:吕海斌 创建时间:2012-3-16
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDate(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$");
            return reg.IsMatch(date);
        }

        /// <summary>
        /// 格式为yyyy-MM-dd hh:mm:ss
        /// 创建人:俞忠亮 创建时间:2012-5-1-76
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateTime(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])\s+(1[012]|0?[1-9]){1}:(0?[1-5]|[0-6][0-9]){1}:(0?[0-6]|[0-6][0-9]){1}$");
            return reg.IsMatch(date);
        }

        /// <summary>
        /// 格式为yyyy-MM-dd hh:mm
        /// 创建人:俞忠亮 创建时间:2013-2-3
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateTime2(string date)
        {
            Regex reg = new Regex(@"^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])\s+(2[0123]|1[0-9]|0?[0-9]){1}:(0?[0-9]|[1-5][0-9]|60){1}$");
            return reg.IsMatch(date);
        }

        /// <summary>
        /// 是否为URL地址
        /// 创建人：吴李辉 时间：2012-6-28
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            Regex reg = new Regex(@"^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }



        /// <summary>
        /// 获取随即数.a-z,0-9
        /// 陈艺艺
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetRadmonString(int len)
        {

            Random random = new Random();
            var _chars = new char[36];
            for (int i = 65; i <= 90; i++)
            {
                _chars[i - 65] = (char)i;
            }
            for (int i = 48; i < 58; i++)
            {

                _chars[i - 22] = (char)(i);

            }
            string str = string.Empty;
            for (int i = 0; i < len; i++)
            {
                str += _chars[random.Next(0, 35)];
            }
            return str;
        }

        /// <summary>
        /// 获取随机6位数字
        /// 俞忠亮
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetNumCode(int num)
        {
            string a = "0123456789";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取html中所有图片的路径
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string[] GetImageUrlListFromHtml(string htmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(htmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }

        /// <summary>
        /// 获取html中所有a标签的链接地址
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string[] GetLinkUrlListFromHtml(string htmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<a\b[^<>]*?\bhref[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<hyperLink>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(htmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["hyperLink"].Value;
            return sUrlList;
        }
        #endregion


        #region 文件处理


        /// <summary>
        /// 获得一个9位时间随机数(小时)
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDataShortRandom()
        {

            return System.DateTime.Now.ToString("HHmmssfff");
        }



        /// <summary>
        /// 批量删除图片
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <param name="fileName">需要删除的图片，多张删除用'|'隔开</param>
        public static void DeleteFile(string filePath, string fileName)
        {

            string[] files = fileName.Split('|');
            foreach (string file in files)
            {
                if (file.Trim() != "")
                {
                    //删除大图
                    string deleteFile = System.Web.HttpContext.Current.Server.MapPath(filePath + file.Trim());
                    if (File.Exists(deleteFile))
                    {
                        File.Delete(deleteFile);
                    }

                    //删除小图
                    int deleteExt = deleteFile.Trim().LastIndexOf('.');
                    if (deleteExt > 0)
                    {
                        string deleteSmallFile = deleteFile.Substring(0, deleteExt) + "s" + deleteFile.Substring(deleteExt);
                        if (File.Exists(deleteSmallFile))
                        {
                            File.Delete(deleteSmallFile);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 更具配置删除文件
        /// </summary>
        /// <param name="uptype">文件配置</param>
        /// <param name="fileName">文件地址</param>
        public static void DeletePic(string uptype, string fileName)
        {
            if (UploadProvice.Instance().Settings[uptype] == null)
            {
                return;
            }
            //删除大图
            string path = UploadProvice.Instance().Settings[uptype].FilePath + fileName;
            if (File.Exists(path))
            {
                File.Delete(path);
            }

        }



        /// <summary>
        /// 获得一个17位时间随机数
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDataRandom()
        {

            return System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }


        public static string GetFileSize(float filesize)
        {
            float filesizeFloat = filesize / 1024;
            if (filesizeFloat < 1024)
            {
                return Math.Ceiling(filesizeFloat) + "K";
            }
            else
            {
                filesizeFloat = filesizeFloat / 1024;
                return Math.Round(filesizeFloat, 2) + "M";
            }

        }

        #endregion


        #region 网络文件获取
        /// <summary>
        /// WebClient下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        /// <param name="referer"></param>
        /// <returns></returns>
        public static bool DownLoadFile(string url, string savePath)
        {
            try
            {
                WebClient myWebClient = new WebClient();
                if (url.ToLower().Contains("https"))
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                WebHeaderCollection headers = new WebHeaderCollection();
                headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.01; Windows NT 5.0)");

                myWebClient.DownloadFile(url, savePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// WebClient下载文件（非阻塞）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        /// <param name="referer"></param>
        /// <returns></returns>
        public static bool DownLoadFileAsync(string url, string savePath, string referer)
        {
            try
            {
                WebClient myWebClient = new WebClient();
                if (url.ToLower().Contains("https"))
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Uri downloadUrl = new Uri(url);

                WebHeaderCollection headers = new WebHeaderCollection();
                headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.01; Windows NT 5.0)");
                headers.Add(HttpRequestHeader.Referer, referer);

                myWebClient.Headers = headers;
                myWebClient.DownloadFileAsync(downloadUrl, savePath);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 下载网络文件(传递前导页，破简单的反盗链)
        /// </summary>
        /// <param name="url">网络文件地址</param>
        /// <param name="savePath">保存文件的路径</param>
        /// <param name="referer">需要传递的前导页</param>
        /// <returns>下载文件的大小(K)</returns>
        public static float DownLoadFile(string url, string savePath, string referer)
        {
            try
            {
                long ThisLength = 0;
                //https，协议需要根据实际情况改变
                if (url.ToLower().Contains("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.01; Windows NT 5.0)";
                myHttpWebRequest.Referer = referer;
                myHttpWebRequest.Timeout = 10 * 1000;
                myHttpWebRequest.Method = "GET";

                HttpWebResponse res = myHttpWebRequest.GetResponse() as HttpWebResponse;
                System.IO.Stream stream = res.GetResponseStream();


                byte[] b = new byte[1024];
                int nReadSize = 0;
                nReadSize = stream.Read(b, 0, 1024);

                System.IO.FileStream fs = System.IO.File.Create(savePath);
                try
                {

                    while (nReadSize > 0)
                    {
                        ThisLength += nReadSize;
                        fs.Write(b, 0, nReadSize);
                        nReadSize = stream.Read(b, 0, 1024);
                    }
                }
                catch
                {
                    ThisLength = 0;
                }
                finally
                {
                    fs.Close();
                }
                res.Close();
                stream.Close();
                myHttpWebRequest.Abort();
                if (ThisLength < 1024 && ThisLength > 0)
                {
                    return 1;
                }
                return (float)(ThisLength / 1024);
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据URL提取图片名称并加上随机数
        /// </summary>
        /// <param name="url">图片URL</param>
        /// <param name="bs">小图标记 _s</param>
        /// <returns></returns>
        public static string GetPictureNameFromUrl(string url, string bs)
        {
            if (string.IsNullOrEmpty(url) || !url.Contains("."))
                return string.Empty;
            string picname = bs + (url.Length > 0 ? url.Substring(url.LastIndexOf('.')) : "");
            if (string.IsNullOrEmpty(bs))
                picname = new Random().Next(100000).ToString() + picname;
            try
            {
                if (string.IsNullOrEmpty(url))
                    return picname = new Random().Next(100000).ToString() + picname;
                string name = url.Substring(url.LastIndexOf('/') + 1, url.LastIndexOf('.') - url.LastIndexOf('/') - 1);
                return string.Format("{0}{1}", name, picname);
            }
            catch
            {
                return picname;
            }
        }

        /// <summary>
        /// 下载图片通用函数
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="picName">图片名</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="folderName">文件夹名</param>
        /// <returns></returns>
        public static string DownLoadPicture(string url, string picName, string savePath, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return string.Empty;
                string dirPath = string.Empty;

                dirPath = string.Format("\\{0}\\{1}\\{2}\\{3}\\{4}\\", folderName, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour);
                if (savePath.EndsWith("\\") || savePath.EndsWith("/"))
                {
                    savePath = savePath.Substring(0, savePath.Length - 1);
                }
                string sp = string.Format(@"{1}{0}", dirPath, savePath);
                if (!Directory.Exists(sp))
                    Directory.CreateDirectory(sp);

                string path = string.Format("{0}{1}", dirPath, picName);
                DownLoadFile(url, sp + "\\" + picName, string.Empty);
                return path;
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 下载图片通用函数
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="folderName">文件夹名</param>
        /// <returns></returns>
        public static string DownLoadPicture(string url, string savePath, string folderName)
        {
            string picName = GetPictureNameFromUrl(url, string.Empty);
            return DownLoadPicture(url, picName, savePath, folderName);
        }
        #endregion


        #region 网站通用


        /// <summary>
        /// 判断文件后缀是否是图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImg(string fileName)
        {
            string imgFile = ".gif.jpg.jpeg.bmp.png";
            return imgFile.Contains(GetFileExt(fileName));
        }

        /// <summary>
        /// 获取文件后缀
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExt(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('.') + 1);
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileName(string fileName)
        {
            if (fileName.IndexOf('.') > 0)
                return fileName.Substring(0, fileName.LastIndexOf('.'));
            return fileName;
        }

        /// <summary>
        /// 图片Flash HTML代码
        /// </summary>
        /// <param name="picAddr">图片或flash上传地址</param>
        /// <param name="config">上传配置文件</param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public static string GetPicHtml(string picAddr, string url, bool isnofollow, string width, string height)
        {
            if (string.IsNullOrEmpty(picAddr)) return "";
            string postfix = picAddr.Substring(picAddr.LastIndexOf('.'));
            if (postfix == ".swf")
            {
                return "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0\" width=\"" + width + "\" height=\"" + height + "\"><param name=\"movie\" value=\"" + picAddr + "\" /><param name=\"quality\" value=\"high\" /><embed src=\"" + picAddr + "\" quality=\"high\" width=\"" + width + "\" height=\"" + height + "\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\"></embed></object>";
            }
            else
            {
                return "<a href=\"" + url + "\" target=\"_blank\" " + (isnofollow ? "rel=\"nofollow\"" : "") + "><img src=\"" + picAddr + "\" border=\"0\"/></a>";
            }
        }

        /// <summary>
        /// 生成QQ临时聊天代码
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static string GetQQ(string qq)
        {
            string[] qqsplit;
            string[] qqkey;
            string qqinx = "";
            string tempqq = " ";
            if (qq == "")
            {
                tempqq = " ";
            }
            else
            {
                qqsplit = qq.Split('/');
                if (qqsplit.Length >= 0)
                {
                    for (int i = 0; i < qqsplit.Length; i++)
                    {
                        qqkey = qqsplit[i].Split('#');
                        if (qqkey.Length > 0)
                        {
                            qqinx = qqkey[0].Trim() + "";
                        }
                        else
                        {
                            qqinx = "";
                        }
                        if (qqinx != "")
                        {
                            tempqq += "<a target=\"_blank\" href=\"http://wpa.qq.com/msgrd?v=1&uin=" + qqinx.Trim() + "&site=www.dzsc.com&menu=yes\" title=\"QQ:" + qqinx.Trim() + "\"><img border=\"0\" src=\"http://wpa.qq.com/pa?p=2:" + qqinx.Trim() + ":4\" alt=\"QQ:" + qqinx.Trim() + "\" onerror=\"this.src='http://img1.dzsc.com/img/common/qq.png'\"></a>";
                        }
                    }
                }
            }
            return tempqq;
        }

        /// <summary>
        /// 生成msn聊天代码
        /// </summary>
        /// <param name="msn"></param>
        /// <returns></returns>
        public static string GetMSN(string msn)
        {
            string temmsn = "";
            if (msn != "")
            {
                string[] temp = msn.Split('/');
                if (temp.Length >= 0)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Trim() != "")
                        {
                            temmsn += "<a href=\"msnim:chat?contact=" + temp[i].Trim() + "\"><img src=\"http://img1.dzsc.com/img/common/msn.png\" border=\"0\" alt=\"MSN:" + temp[i].Trim() + "\" ></a>";
                        }
                    }
                }
            }
            return temmsn;
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



        /// <summary>
        /// 过滤内容中的其他网站链接
        /// 创建人：陈民礼  时间：2013-5-2
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveOtherSiteLink(string content)
        {
            //<a[^>]*https?://(?!(\w+\.)*dzsc.com)[^>]*?>(.*?)</a>
            //<a[^>]*href\s*?=\s*?["'](?![\w+\.:/]+?\.dzsc\.com)[^>]*?>(?<get>.*?)</a>
            return Regex.Replace(content, @"<a[^>]*href\s*?=\s*?[""'](?![\w+\.:/\-_]+?\.1goic\.com)[^>]*?>(?<get>.*?)</a>", "$1", RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 是否输出nofollow
        /// </summary>
        /// <param name="showFlag"></param>
        /// <returns></returns>
        public static string ResponseNoFollow(bool showFlag)
        {
            if (showFlag)
            {
                return " rel=\"nofollow\"";
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 输出小图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetSmallPic(string pic)
        {
            if (string.IsNullOrEmpty(pic)) return "";

            return pic.Insert(pic.LastIndexOf('.'), "s");
        }

        /// <summary>
        /// 输出中图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetMiddlePic(string pic)
        {
            if (string.IsNullOrEmpty(pic)) return "";
            return pic.Insert(pic.LastIndexOf('.'), "m");
        }
        /// <summary>
        /// 输出大图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetBigPic(string pic)
        {
            if (string.IsNullOrEmpty(pic)) return "";
            return pic.Insert(pic.LastIndexOf('.'), "b");
        }

        /// <summary>
        /// 获取第一张图片
        /// </summary>
        /// <param name="uptype"></param>
        /// <param name="picStr"></param>
        /// <param name="getSmallPic"></param>
        /// <returns></returns>
        public static string GetFirstPic(string picStr, bool getSmallPic)
        {
            if (string.IsNullOrEmpty(picStr)) return "";

            return getSmallPic ? GetSmallPic(picStr.Split('|')[0]) : picStr.Split('|')[0];
        }

        /// <summary>
        /// 获取登录后用户ID
        /// </summary>
        /// <returns></returns>
        public static int GetMyMemberId()
        {
            if (System.Web.HttpContext.Current.Session["MB_MemberId"] == null)
            {
                return 0;
            }
            else
            {
                return int.Parse(System.Web.HttpContext.Current.Session["MB_MemberId"].ToString());
            }
        }

        #endregion


        #region sql防注入检测
        /// <summary>
        /// 检测关键词，防止SQL注入
        /// </summary>
        /// <param name="sqlValue">需要检测的关键词</param>
        /// <returns></returns>
        public static string CheckSqlValue(string sqlValue)
        {
            string reStr = sqlValue;
            if (reStr == null)
            {
                reStr = "";
            }
            reStr = reStr.Replace("'", "''");
            return reStr;
        }

        /// <summary>
        /// 检测Like字符串,防止注入和关键词匹配(替换 '_ % [ ] )
        /// </summary>
        /// <param name="keyword">需要检测的关键词</param>
        /// <returns></returns>
        public static string CheckSqlKeyword(string keyword)
        {
            string reStr = keyword;
            if (reStr == null)
            {
                reStr = "";
            }
            reStr = reStr.Replace("'", "''");
            reStr = reStr.Replace("[", "");
            reStr = reStr.Replace("]", "");
            reStr = reStr.Replace("%", "[%]");
            reStr = reStr.Replace("_", "[_]");
            return reStr;
        }



        /// <summary>
        /// 检测数据库字段名或表名
        /// </summary>
        /// <param name="fieldName">要检测的字段名或表名</param>
        /// <returns></returns>
        public static bool CheckSqlField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(fieldName, @"^[a-zA-Z0-9_\.\,]+$");
            }
        }

        public static bool CheckSqlField(string[] arr)
        {
            foreach (var item in arr)
            {
                if (!CheckSqlField(item))
                    return false;
            }

            return true;
        }



        /// <summary>
        /// 检测全文索引关键词(* " ')且长度不少于3
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string CheckSqlFullText(string keyword)
        {
            keyword = new Regex(@"\W+", RegexOptions.IgnoreCase).Replace(keyword.Trim(), "|");
            keyword = new Regex(@"\*|""|'", RegexOptions.IgnoreCase).Replace(keyword, "");
            return keyword;
        }
        #endregion


        /// <summary>
        /// 获取分割字符串后ID数组(空则返回Null)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string[] SplitStr(string target, string split)
        {
            return Regex.Split(target, split, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 根据产品名称 生成全文检索关键词
        /// </summary>
        /// <param name="proname"></param>
        /// <returns></returns>
        public static string GetKeywords(string proname)
        {
            string temp = Regex.Replace(proname, @"[^a-zA-Z0-9]*", "");
            char[] str = temp.ToCharArray();
            string newtemp = "|";
            for (int j = 3; j <= str.Length; j++)
            {
                for (int i = 0; i < str.Length - j + 1; i++)
                {
                    string newkey = temp.Substring(i, j);
                    if (newtemp.IndexOf(newkey) == -1)
                    {
                        newtemp += temp.Substring(i, j) + "|";
                    }
                }
            }
            return newtemp;
        }

        /// <summary>
        /// 获取中英文混合字符串的文字长度(1个英文占1个长度，1个汉字占2个长度)
        /// </summary>
        /// <param name="stringWithEnglishAndChinese">中英文混合的字符串</param>
        /// <returns>字符串长度(1个英文占1个长度，1个汉字占2个长度)</returns>
        public static int GetEnglishLength(string stringWithEnglishAndChinese)
        {

            int lng = 0;

            for (int i = 0; i < stringWithEnglishAndChinese.Length; i++)
            {

                byte[] b = System.Text.Encoding.Default.GetBytes(stringWithEnglishAndChinese.Substring(i, 1));

                if (b.Length > 1)

                    lng += 2;

                else

                    lng += 1;

            }

            return lng;

        }

        /// <summary>
        /// 计算时间差（分钟）
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static int DiffDateTime(DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts2.Subtract(ts1).Duration();//dt2-dt1的时间差
            return (int)(ts.TotalMinutes);
        }

        /// <summary>
        /// 获取传入日期所在星期的周一和周日
        /// </summary>
        /// <param name="date"></param>
        /// <param name="weekStart"></param>
        /// <param name="weekeEnd"></param>
        public static void GetMondayAndSunday(DateTime date, out DateTime monday, out DateTime sunday)
        {
            monday = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
            sunday = date.AddDays((int)DayOfWeek.Saturday - (int)date.DayOfWeek + 1);
        }


        /// <summary>
        /// 获取当前路径文件夹名称
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static string GetLastFolderName(string dirPath)
        {
            return dirPath.Split('/').ToList()[dirPath.Split('/').Length - 2];
        }

        /// <summary>
        /// 文件名后缀
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static bool IsSmallPic(string dirPath)
        {
            try
            {
                return Path.GetFileName(dirPath).Substring(Path.GetFileName(dirPath).IndexOf('.') - 1, 1).ToLower() == "s";
            }
#pragma warning disable CS0168 // 声明了变量“ee”，但从未使用过
            catch (Exception ee)
#pragma warning restore CS0168 // 声明了变量“ee”，但从未使用过
            {
                return false;
            }
        }


        /// <summary>
        /// 文件名后缀
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static bool IsOriginPic(string dirPath)
        {
            try
            {
                return Path.GetFileName(dirPath).Substring(Path.GetFileName(dirPath).IndexOf('.') - 1, 1).ToLower().ToString().IsInt();
            }
#pragma warning disable CS0168 // 声明了变量“ee”，但从未使用过
            catch (Exception ee)
#pragma warning restore CS0168 // 声明了变量“ee”，但从未使用过
            {
                return false;
            }
        }

        #region Web 获取Url

        /// <summary>
        /// 产品名称转换成文件名 转为 - 
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public static string GetClassNameUrl(string cName, int cId, string langCode = "")
        {
            if (langCode != "")
                langCode = "/" + langCode;
            if (cId == 0)
                return Tools.Special.Common.GetWebUrl() + langCode + "/products/";
            return Tools.Special.Common.GetWebUrl() + langCode + "/products/wholesale-" + ConverUrl(cName) + "-" + cId + "/";
        }

        /// <summary>
        /// 产品名称转换成文件名 转为 - 
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public static string GetClassAttrUrl(string cName, int cId, string attrName, string attrValue, string langCode = "")
        {
            if (langCode != "")
                langCode = "/" + langCode;
            if (cId == 0)
                return Tools.Special.Common.GetWebUrl() + langCode + "/products/";
            return Tools.Special.Common.GetWebUrl() + langCode + "/products/wholesale-" + ConverUrl(cName) + "-" + cId + "/" + attrName + "/" + attrValue;
        }

        /// <summary>
        /// 产品名称转换成文件名 转为 - 
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public static string GetProNameUrl(string productName, int pId, string langCode = "")
        {
            return GetProNameUrl(productName, pId, "", langCode);
        }


        /// <summary>
        /// 产品名称转换成文件名 转为 - 
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public static string GetProNameUrl(string productName, int pId, string type, string langCode = "")
        {
            if (pId == 0)
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            switch (type)
            {
                default:
                    return Tools.Special.Common.GetWebUrl() + langCode + "/detail/" + ConverUrl(productName) + "-" + pId.ToString() + ".html";
            }
        }

        public static string GetProNameUrl(string productName, int pId, byte type, string langCode = "")
        {
            if (pId == 0)
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            switch (type)
            {
                default:
                    return Tools.Special.Common.GetWebUrl() + langCode + "/detail/" + ConverUrl(productName) + "-" + pId.ToString() + ".html";
            }
        }

        /// <summary>
        /// 定制产品列表页URL
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string GetCustomizedListUrl(int cid, string className, string langCode = "")
        {
            if (cid == 0 || className == "")
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            return Tools.Special.Common.GetWebUrl() + langCode + string.Format("/oem/{0}-{1}.html", FancyFix.Tools.Usual.Utils.ConverUrl(className), cid);
        }

        /// <summary>
        /// 定制产品详细页URL
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetCustomizedDetailUrl(int pId, string title, string langCode = "")
        {
            if (pId == 0 || title == "")
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            return Tools.Special.Common.GetWebUrl() + langCode + string.Format("/oem/detail/{0}-{1}.html", FancyFix.Tools.Usual.Utils.ConverUrl(title), pId);
        }

        /// <summary>
        /// 产品目录URL
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="className"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public static string GetDirectoryUrl(int cid, string className, string langCode = "")
        {
            if (cid == 0 || className == "")
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            return Tools.Special.Common.GetWebUrl() + langCode + string.Format("/directory/{0}-{1}/", FancyFix.Tools.Usual.Utils.ConverUrl(className), cid);
        }

        /// <summary>
        /// 新闻列表Url
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="className"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public static string GetNewsListUrl(string className, string langCode = "")
        {
            if (className == "")
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            return Tools.Special.Common.GetWebUrl() + langCode + string.Format("/news/{0}.html", FancyFix.Tools.Usual.Utils.ConverUrl(className.ToLower()));
        }

        /// <summary>
        /// 新闻详情页Url
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="title"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public static string GetNewsDetailUrl(int cid, string title, string langCode = "")
        {
            if (cid == 0 || title == "")
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            return Tools.Special.Common.GetWebUrl() + langCode + string.Format("/news/{0}-{1}.html", FancyFix.Tools.Usual.Utils.ConverUrl(title), cid);
        }

        public static string GetTechnicalDetailUrl(int cid, string title, string langCode = "")
        {
            if (cid == 0 || title == "")
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            return Tools.Special.Common.GetWebUrl() + langCode + string.Format("/technical/{0}-{1}.html", FancyFix.Tools.Usual.Utils.ConverUrl(title), cid);
        }
        #endregion

        #region Blog 获取Url

        /// <summary>
        /// 获取分类URL 转为 - 
        /// </summary>
        /// <param name="BlogClassName"></param>
        /// <returns></returns>
        public static string GetBlogClassNameUrl(string cName, int cId)
        {
            if (cId == 0)
                return Tools.Special.Common.GetWebUrl() + "/catalog/";
            return Tools.Special.Common.GetBlogUrl() + "/catalog/" + FancyFix.Tools.Usual.Utils.ConverUrl(cName);
        }

        /// <summary>
        /// 获取标签URL 转为 - 
        /// </summary>
        /// <param name="TagName"></param>
        /// <returns></returns>
        public static string GetTagNameUrl(string tagname)
        {
            return Tools.Special.Common.GetBlogUrl() + "/tag/" + FancyFix.Tools.Usual.Utils.ConverUrl(tagname).Trim('-');
        }

        /// <summary>
        /// 获取文章URL 转为 - 
        /// </summary>
        /// <param name="BlogName"></param>
        /// <returns></returns>
        public static string GetBlogDetailUrl(string productName, int pId)
        {
            if (pId == 0)
                return "javascript:void(0)";
            return Tools.Special.Common.GetBlogUrl() + "/" + ConverUrl(productName.ToLower()) + "-" + pId.ToString() + ".html";
        }

        #endregion

        #region Outside 获取Url

        /// <summary>
        /// 获取分类URL 转为 - 
        /// </summary>
        /// <param name="BlogClassName"></param>
        /// <returns></returns>
        public static string GetOutsideClassNameUrl(string cName, int cId)
        {
            if (cId == 0)
                return Tools.Special.Common.GetWebUrl() + "/";
            return Tools.Special.Common.GetOutsideUrl() + "/" + ConverUrl(cName.ToLower()) + "/";
        }

        /// <summary>
        /// 获取标签URL 转为 - 
        /// </summary>
        /// <param name="TagName"></param>
        /// <returns></returns>
        public static string GetOutsideTagNameUrl(string tagname)
        {
            return Tools.Special.Common.GetOutsideUrl() + "/tag/" + ConverUrl(tagname.ToLower()).Trim('-');
        }

        /// <summary>
        /// 获取文章URL 转为 - 
        /// </summary>
        /// <param name="BlogName"></param>
        /// <returns></returns>
        public static string GetOutsideDetailUrl(string className, string productName, int pId)
        {
            if (pId == 0)
                return "javascript:void(0)";
            return "/" + ConverUrl(className.ToLower()) + "/" + ConverUrl(productName.ToLower()) + "-" + pId.ToString() + ".html";
        }

        #endregion

        #region Mobile 获取Url

        /// <summary>
        /// 产品名称转换成文件名 转为 - 
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public static string GetMobileProDetailUrl(string productName, int pId, string type = "", string langCode = "")
        {
            if (pId == 0)
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            switch (type)
            {
                default:
                    return Tools.Special.Common.GetMobileUrl() + langCode + "/detail/" + ConverUrl(productName) + "-" + pId.ToString() + ".html";
            }
        }

        public static string GetMobileProDetailUrl(string productName, int pId, byte proType, string langCode = "")
        {
            if (pId == 0)
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            switch (proType)
            {
                default:
                    return Tools.Special.Common.GetMobileUrl() + langCode + "/detail/" + ConverUrl(productName) + "-" + pId.ToString() + ".html";
            }
        }

        /// <summary>
        /// 获取分类URL 转为 - 
        /// </summary>
        /// <param name="MobileClassName"></param>
        /// <returns></returns>
        public static string GetMobileCategoryUrl(int cId, string langCode = "")
        {
            if (langCode != "")
                langCode = "/" + langCode;
            if (cId == 0)
                return Tools.Special.Common.GetMobileUrl() + langCode + "/category/";
            return Tools.Special.Common.GetMobileUrl() + langCode + "/category/" + cId + "/";
        }

        /// <summary>
        /// 获取分类URL 转为 - 
        /// </summary>
        /// <param name="MobileClassName"></param>
        /// <returns></returns>
        public static string GetMobileClassNameUrl(string cName, int cId, string langCode = "")
        {
            if (langCode != "")
                langCode = "/" + langCode;
            if (cId == 0)
                return Tools.Special.Common.GetMobileUrl() + langCode + "/products/";
            return Tools.Special.Common.GetMobileUrl() + langCode + "/products/wholesale-" + ConverUrl(cName) + "-" + cId + "/";
        }

        /// <summary>
        /// 定制产品列表页URL
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string GetMobileCustomizedListUrl(int cid, string className, string langCode = "")
        {
            if (cid == 0 || className == "")
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            return Tools.Special.Common.GetMobileUrl() + langCode + string.Format("/oem/{0}-{1}.html", FancyFix.Tools.Usual.Utils.ConverUrl(className), cid);
        }

        /// <summary>
        /// 定制产品详细页URL
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetMobileCustomizedDetailUrl(int pId, string title, string langCode = "")
        {
            if (pId == 0 || title == "")
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            return Tools.Special.Common.GetMobileUrl() + langCode + string.Format("/oem/detail/{0}-{1}.html", FancyFix.Tools.Usual.Utils.ConverUrl(title), pId);
        }

        /// <summary>
        /// 产品目录URL
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="className"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public static string GetMobileDirectoryUrl(int cid, string className, string langCode = "")
        {
            if (cid == 0 || className == "")
                return "javascript:void(0)";
            if (langCode != "")
                langCode = "/" + langCode;
            return Tools.Special.Common.GetMobileUrl() + langCode + string.Format("/directory/{0}-{1}/", FancyFix.Tools.Usual.Utils.ConverUrl(className), cid);
        }
        #endregion

        #region About 获取Url

        /// <summary>
        /// 获取About分类URL 
        /// </summary>
        /// <param name="AboutClassName"></param>
        /// <returns></returns>
        public static string GetAboutClassNameUrl(string cName, int id)
        {
            if (id == 0 || cName == "")
                return Tools.Special.Common.GetAboutUrl() + "/";
            return Tools.Special.Common.GetAboutUrl() + "/" + FancyFix.Tools.Usual.Utils.ConverUrl(cName.ToLower()) + "-" + id + "/";
        }

        /// <summary>
        /// 获取新闻详细页URL 
        /// </summary>
        /// <param name="AboutArticle"></param>
        /// <returns></returns>
        public static string GetAboutNewsDetailUrl(string url)
        {
            if (url == "")
                return Tools.Special.Common.GetAboutUrl() + "/";
            return Tools.Special.Common.GetAboutUrl() + "/news/" + url;
        }

        /// <summary>
        /// 设置新闻详细页URL
        /// </summary>
        /// <param name="AboutArticle"></param>
        /// <returns></returns>
        public static string SetAboutNewsDetailUrl(string cName, int aid, string title)
        {
            if (cName == "" || title == "" || aid == 0)
                return "/";
            return "/" + FancyFix.Tools.Usual.Utils.ConverUrl(title.ToLower()) + "-" + aid + ".html";
        }

        /// <summary>
        /// 获取新闻分类URL
        /// </summary>
        /// <param name="AboutArticle"></param>
        /// <returns></returns>
        public static string GetAboutNewsClassUrl(string cName, int id)
        {
            if (cName == "")
                return Tools.Special.Common.GetAboutUrl() + "/";
            return Tools.Special.Common.GetAboutUrl() + "/news/" + FancyFix.Tools.Usual.Utils.ConverUrl(cName.ToLower()) + "-" + id + "/";
        }

        #endregion

        /// <summary>
        /// 转换Url
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConverUrl(string value)
        {
            return Regex.Replace(value, @"[\W]+", "-");
        }

        /// <summary>
        /// 转换字段名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConverFieldName(string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }


        /// <summary>
        /// 根据Key获取value
        /// by；willian  date：2016-4-26
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(Dictionary<string, string> dic, string key)
        {
            return dic.FirstOrDefault(q => q.Key == key).Value;
        }
        /// <summary>
        /// 根据条件筛选列表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable GetSelect(DataTable dt, string where)
        {
            return new DataView(dt, where, "", DataViewRowState.CurrentRows).ToTable();
        }

        /// <summary>
        /// 获取唯一数据的字段
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PrimaryKeyColumns"></param>
        /// <returns></returns>
        public static DataTable GetDistinctPrimaryKeyColumnTable(DataTable dt, string[] PrimaryKeyColumns)
        {
            DataView dv = dt.DefaultView;
            DataTable dtDistinct = dv.ToTable(true, PrimaryKeyColumns);

            //第一个参数是关键,设置为 true，则返回的 System.Data.DataTable 将包含所有列都具有不同值的行。默认值为 false。
            return dtDistinct;
        }

        /// <summary>
        /// 获取国外常用邮箱登录地址
        /// by:willian  date:2015-12-16
        /// </summary>
        /// <param name="email"></param>
        /// <returns>返回登录地址，非常用返回空</returns>
        public static string GetMailSubfix(string email)
        {
            string subfix = "|" + email.ToLower().Substring(email.LastIndexOf('@') + 1) + "|";
            string emails = "|gmail.com|yahoo.com|yahoo.co|outlook.com|outlook.co|hotmail.com|live.com|live.co|msn.com|126.com|163.com|vip.163.com|aol.com|vip.sina.com|popmail.com|gmx.com|inbox.com|qq.com|";
            if (emails.Contains(subfix))
                return subfix.TrimEnd('|').TrimStart('|');
            else
                return "";
        }

        /// <summary>
        /// 获取搜索关键字列表 空格区分
        /// by:willian date:2015-12-31
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static List<string> GetListKeywords(string keyword)
        {
            List<string> listKeyword = new List<string>();
            string[] keylist = Regex.Split(keyword, @"\s+");
            for (int i = 0; i < keylist.Length; i++)
            {
                if (keylist[i] != "")
                    listKeyword.Add(keylist[i]);
            }
            return listKeyword;
        }

        /// <summary>
        /// 获取Welbuy产品价格与定金
        /// by:willian date:2016-1-16
        /// </summary>
        /// <param name="cnt"></param>
        /// <param name="price"></param>
        public static bool GetProAmount(int cnt, string price, ref PriceRange priceRange)
        {
            try
            {
                List<PriceRange> listPrice = Tools.Tool.JsonHelper.Deserialize<PriceRange>(price);
                if (listPrice == null || listPrice.Count == 0)
                    return false;
                //去除 价格为零 匹配最优方案
                listPrice = listPrice.Where(t => t.price > 0).OrderByDescending(t => t.cnt).ToList();
                //小于最小起订量 做最低起订量处理
                int minCnt = listPrice.Min(w => w.cnt);
                if (cnt < minCnt)
                    cnt = minCnt;
                priceRange = listPrice.FirstOrDefault(t => t.cnt <= cnt);
                return priceRange != null;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(Utils), ex);
                return false;
            }
        }
    }
}
