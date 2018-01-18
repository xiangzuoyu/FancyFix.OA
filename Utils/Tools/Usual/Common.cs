using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;

namespace FancyFix.Tools.Usual
{
    public class Common
    {
        /// <summary>
        /// 日期值的初始值
        /// </summary>
        /// <returns></returns>
        public static DateTime InitDateTime()
        {
            return DateTime.Parse("1900-01-01");
        }

        public static string[] ShortUrl(string url)
        {
            //可以自定义生成MD5加密字符传前的混合KEY
            string key = "Leejor";
            //要使用生成URL的字符
            string[] chars = new string[]{
         "a","b","c","d","e","f","g","h",
         "i","j","k","l","m","n","o","p",
        "q","r","s","t","u","v","w","x",
           "y","z","0","1","2","3","4","5",
         "6","7","8","9","A","B","C","D",
          "E","F","G","H","I","J","K","L",
          "M","N","O","P","Q","R","S","T",
        "U","V","W","X","Y","Z"
        };

            //对传入网址进行MD5加密
#pragma warning disable CS0618 // “FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            string hex = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key + url, "md5");
#pragma warning restore CS0618 // “FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”

            string[] resUrl = new string[4];

            for (int i = 0; i < 4; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
                string outChars = string.Empty;
                for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加
                    outChars += chars[index];
                    //每次循环按位右移5位
                    hexint = hexint >> 5;
                }
                //把字符串存入对应索引的输出数组
                resUrl[i] = outChars;
            }
            return resUrl;
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
        /// 创建人：吴李辉  时间：2012-5-3
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveOtherSiteLink(string content)
        {
            //<a[^>]*https?://(?!(\w+\.)*dzsc.com)[^>]*?>(.*?)</a>
            //<a[^>]*href\s*?=\s*?["'](?![\w+\.:/]+?\.dzsc\.com)[^>]*?>(?<get>.*?)</a>
            return Regex.Replace(content, @"<a[^>]*href\s*?=\s*?[""'](?![\w+\.:/\-_]+?\.dzsc\.com)[^>]*?>(?<get>.*?)</a>", "$1", RegexOptions.IgnoreCase);
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

        /// <summary>
        /// 获得一个17位时间随机数
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDataRandom()
        {
            return System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// 获得一个9位时间随机数(精确到毫秒)
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDataShortRandom()
        {
            return System.DateTime.Now.ToString("HHmmssfff");
        }

        /// <summary>
        /// 获得一个6位时间随机数(精确到秒)
        /// </summary>
        /// <returns></returns>
        public static string GetDataShortRandom2()
        {
            return System.DateTime.Now.ToString("HHmmss");
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
                    byte[] buffer2 = Encoding.Default.GetBytes(text1 + strInput.Substring(num1, 1));
                    if (buffer2.Length > intLen)
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
        /// 剪切字符串(末尾不加点号)
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public static string GetSubString(string strInput, int intLen)
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
        /// 去除html标签 不换行
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
            return str.Trim();
        }

        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtml(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string text1 = "<.*?>";
                Regex regex1 = new Regex(text1);
                str = regex1.Replace(str, "");
                str = str.Replace("&nbsp;", " ");
            }
            return str;
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

        public static string OutHtmlKeyword(string str)
        {
            string text1 = "<!--.*?-->";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, "");
            return str;
        }

        public static string OutHtmlByDiv(string str)
        {
            string text1 = "<div.*?>|<DIV.*?>|</div.*?>|</DIV.*?>|<script.*?>|<SCRIPT.*?>|</script.*?>|</SCRIPT.*?>";
            Regex reg = new Regex(text1);
            str = reg.Replace(str, "");
            return str;
        }
        public static string OutHtmlByDivAndScriptAndHref(string str)
        {
            string text1 = "<div.*?>|</div>|<a.*?>|</a>|(<script.*?>(.*?)</script>)";
            Regex reg = new Regex(text1, RegexOptions.IgnoreCase);
            str = reg.Replace(str, "");
            return str;
        }

        /// <summary>
        /// 替换文本中不带Nofollow标签的链接加上nofollow
        /// </summary>
        /// <param name="str">需要替换的HTML</param>
        /// <returns></returns>
        public static string ReplaceLinkAddNofollow(string str)
        {
            string exp = @"<[A|a](?![^<>]*rel=[^<>]*)[^<>]*>";
            MatchEvaluator me = new MatchEvaluator(replaceA);
            return System.Text.RegularExpressions.Regex.Replace(str, exp, me);
        }

        private static string replaceA(Match mth)
        {
            string v = mth.Value;
            string rev = string.Empty;
            if (!string.IsNullOrEmpty(v))
            {
                try
                {
                    rev = v.Insert(v.IndexOf(' ') + 1, @"rel=""nofollow"" ");
                }
                catch
                {
                    rev = v;
                }
            }
            return rev;
        }

        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string OutHtmlText(string str)
        {
            string text1 = "<.*?>";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, "");
            str = str.Replace("&nbsp;", " ");
            return str;
        }

        /// <summary>
        /// 获得一个16位时间随机数
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDateRandom()
        {

            string strData = System.DateTime.Now.Year.ToString() +
            System.DateTime.Now.Month.ToString() +
            System.DateTime.Now.Day.ToString() +
            System.DateTime.Now.Hour.ToString() +
            System.DateTime.Now.Minute.ToString() +
            System.DateTime.Now.Second.ToString() +
            System.DateTime.Now.Millisecond.ToString();

            //Random r = new Random();
            //strData = strData + r.Next(1000);
            return strData;
        }

        /// <summary>
        /// 返回int型列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns>用逗号分割的数字字符串/空字符串</returns>
        public static List<int> GetListByComma(string str)
        {
            List<int> list = new List<int>();
            string[] strArr = str.Split(',');
            foreach (var i in strArr)
            {
                if (i.ToInt32() > 0)
                    list.Add(i.ToInt32());
            }

            return list;
        }

        /// <summary>
        /// 用逗号分割的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns>用逗号分割的数字字符串/空字符串</returns>
        public static string GetCutStringByComma(string str)
        {
            List<string> list = new List<string>();
            string[] strArr = str.Split(',');
            foreach (var i in strArr)
            {
                if (i.ToInt32() > 0)
                    list.Add(i);
            }

            return string.Join(",", list.ToArray());
        }


        //获取随机数(解决随机数BUG,实例放到ThreadLocal中,保证每个线程一个Randonm实例)
        private static readonly ThreadLocal<Random> appRandom = new ThreadLocal<Random>(() => new Random());
        public static int GetRandomNumber()
        {
            return appRandom.Value.Next();
        }

        public static int GetRandomNumber(int num)
        {
            return appRandom.Value.Next(num);
        }

        public static int GetRandomNumber(int minNum, int maxNum)
        {
            return appRandom.Value.Next(minNum, maxNum);
        }

        #region 网络文件获取
        /// <summary>
        /// 下载网络文件
        /// </summary>
        /// <param name="url">下载文件地址</param>
        /// <param name="savePath">保存路径</param>
        /// <returns></returns>
        public static bool DownLoadFile(string url, string savePath)
        {
            try
            {
                WebClient myWebClient = new WebClient();
                myWebClient.DownloadFile(url, savePath);
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
            catch
            {
                return 0;
            }
        }



        #endregion

        #region  将A~Z转换为数字
        /// <summary>
        /// 将A~Z转换为数字
        /// </summary>
        /// <param name="titleIndex"></param>
        /// <returns></returns>
        public static byte TransformTitleIndex(string titleIndex)
        {
            switch (titleIndex)
            {
                case "A":
                    return 10;
                case "B":
                    return 11;
                case "C":
                    return 12;
                case "D":
                    return 13;
                case "E":
                    return 14;
                case "F":
                    return 15;
                case "G":
                    return 16;
                case "H":
                    return 17;
                case "I":
                    return 18;
                case "J":
                    return 19;
                case "K":
                    return 20;
                case "L":
                    return 21;
                case "M":
                    return 22;
                case "N":
                    return 23;
                case "O":
                    return 24;
                case "P":
                    return 25;
                case "Q":
                    return 26;
                case "R":
                    return 27;
                case "S":
                    return 28;
                case "T":
                    return 29;
                case "U":
                    return 30;
                case "V":
                    return 31;
                case "W":
                    return 32;
                case "X":
                    return 33;
                case "Y":
                    return 34;
                case "Z":
                    return 35;
                default:
                    return 0;
            };
        }
        /// <summary>
        /// 是否全角
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsFullWidth(char c)
        {
            if (System.Text.Encoding.Default.GetByteCount(c.ToString()) == 2)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 将时间转换为INT
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetDateInt(string time)
        {
            if (!string.IsNullOrEmpty(time))
            {
                return Convert.ToInt32(time.Replace("-", ""));
            }
            return 0;
        }
        /// <summary>
        /// 获取首字母
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetTitleIndex(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                string firstChar = Chinese.Get(title.Replace("供应", "").Substring(0, 1)).ToUpper().Trim();
                if (!string.IsNullOrEmpty(firstChar))
                {
                    char index = Convert.ToChar(firstChar);
                    if (IsFullWidth(index))
                    {
                        char result = (char)(index - 65248);
                        return result.ToString();
                    }
                    return firstChar;
                }
            }
            return "";
        }
        /// <summary>
        /// 获取转换后的字母INT
        /// </summary>
        /// <param name="tilteIndex"></param>
        /// <returns></returns>
        public static byte GetTitleIndexInt(string title)
        {
            string titleIndex = GetTitleIndex(title);
            if (!string.IsNullOrEmpty(titleIndex))
            {
                if (FancyFix.Tools.Usual.StringCheck.IsNumber(titleIndex))
                {
                    return Convert.ToByte(titleIndex);
                }
                return TransformTitleIndex(titleIndex);
            }
            return 100;
        }

        /// <summary>
        /// 获取相差时间数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type">D 天数 M 月 H 小时</param>
        /// <returns></returns>
        public static int GetDateTimeDiff(string start, string end, string type)
        {
            DateTime dtEnd = end.ToDateTime();
            DateTime dtStart = start.ToDateTime();
            if (type == "D")
                return (dtEnd - dtStart).Days + 1;
            else if (type == "M")
                return dtEnd.Year * 12 + dtEnd.Month - (dtStart.Year * 12 + dtStart.Month) + 1;
            else if (type == "H")
            {
                TimeSpan ts = dtEnd - dtStart;
                return ts.Days * 12 + ts.Hours + 1;
            }
            return 0;
        }
        #endregion
    }
}
