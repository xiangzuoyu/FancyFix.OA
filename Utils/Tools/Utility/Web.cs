using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using FancyFix.Tools.Json;
using System.Web;

namespace FancyFix.Tools.Utility
{
    public class Web
    {

        private static string defaultImg = "/Content/img/adminlte/img/default-50x50.gif";

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
        /// 输出小图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetSmallPic(string pic)
        {
            if (string.IsNullOrEmpty(pic))
            {
                return defaultImg;
            }
            return pic.Insert(pic.LastIndexOf('.'), "s");
        }
        /// <summary>
        /// 输出中图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetMiddlePic(string pic)
        {
            if (string.IsNullOrEmpty(pic))
            {
                return defaultImg;
            }
            return pic.Insert(pic.LastIndexOf('.'), "m");
        }
        /// <summary>
        /// 输出大图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetBigPic(string pic)
        {
            if (string.IsNullOrEmpty(pic))
            {
                return defaultImg;
            }
            return pic.Insert(pic.LastIndexOf('.'), "b");
        }

        /// <summary>
        /// 转换属性格式 (例:属性名:属性值;)
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string ConvertAttr(string attribute)
        {
            if (attribute == "")
                return "";
            List<Attr> listAttr = Tools.Tool.JsonHelper.Deserialize<Attr>(attribute);
            string result = string.Empty;
            foreach (Attr attr in listAttr)
                result += string.Format("{0}:{1};", attr.name, attr.value);
            return result;
        }

        /// <summary>
        /// 转换属性格式 (例:属性名:属性值;)
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string ConvertAttr2(string attribute)
        {
            if (attribute == "")
                return "";
            List<AttrJson> listAttr = Tools.Tool.JsonHelper.Deserialize<AttrJson>(attribute);
            string result = string.Empty;
            foreach (AttrJson attr in listAttr)
                result += string.Format("{0}:{1};", attr.n, attr.v);
            return result;
        }

        public static string UrlEncode(string parm)
        {
            return HttpUtility.UrlEncode(parm, Encoding.UTF8);
        }


        public static void AddLoginLog(IDictionary<string, object> list, int result, string msg)
        {
            string log = string.Empty;
            log += "msg:" + msg;
            log += "\r\nresult:" + result;

            foreach (var item in list)
            {
                log += string.Format("\r\n{0}:{1}", item.Key, item.Value);
            }
            Tools.Tool.Log.WritePur(log);
        }

        /// <summary>
        /// 获取价格
        /// </summary>
        /// <param name="cnt"></param>
        /// <param name="price"></param>
        /// <returns>-1价格错误 0 询价 >0 价格</returns>
        public static decimal GetPrice(int cnt, string price)
        {
            List<Price> listPrice = Tools.Tool.JsonHelper.Deserialize<Price>(price);
            if (listPrice == null)
                return -1;
            decimal result = 0;
            listPrice.OrderByDescending(x => x.c);
            foreach (Price p in listPrice)
            {
                if (cnt >= p.c)
                {
                    result = p.p;
                    break;
                }
            }

            //超出无价格的部分 按照低一档价格确定
            if (result == 0 && listPrice.Count > 1)
            {
                result = listPrice[listPrice.Count - 2].p;
            }
            return result;
        }

        /// <summary>
        /// 详细图片展示Alt 和title
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="alt"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string ShowImgAltOrTitle(string detail, string alt = "", string title = "")
        {
            string reuslt = string.Empty;
            RegexOptions ops = RegexOptions.Multiline;
            Regex r = new Regex(@"(<img.*?(src="".*?"").*?>)", ops);
            foreach (Match MatchData in r.Matches(detail))
            {
                string img = MatchData.Groups[1].Value;
                string src = MatchData.Groups[2].Value;
                Regex rAlt = new Regex(@"alt=""(.*?)""", RegexOptions.Singleline);
                Regex rTitle = new Regex(@"title=""(.*?)""", RegexOptions.Singleline);
                //try
                //{
                if (alt != "")
                    //替换titleAlt
                    if (rAlt.IsMatch(img))
                    {
                        Match matchALt = rAlt.Match(img);
                        detail = detail.Replace(matchALt.Groups[0].Value, "alt=\"" + alt + "\"");
                    }
                    else
                    {
                        detail = detail.Replace(src, src + " alt=\"" + alt + "\"");
                    }
                if (title != "")
                    if (rTitle.IsMatch(img))
                    {
                        Match matchTitle = rTitle.Match(img);
                        detail = detail.Replace(matchTitle.Groups[0].Value, "title=\"" + title + "\"");
                    }
                //}
                //catch (Exception ee)
                //{
                //}
            }
            //return reuslt;
            return detail;
        }

        /// <summary>
        /// 解析HTML中的图片
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<string> GetHtmlPic(string html)
        {
            List<string> list = new List<string>();
            RegexOptions ops = RegexOptions.Multiline;
            Regex r = new Regex(@"(<img.*?(src=""(.*?)"").*?>)", ops);
            foreach (Match MatchData in r.Matches(html))
            {
                string img = MatchData.Groups[1].Value;
                string src = MatchData.Groups[3].Value;
                if (src != "")
                    list.Add(src);
            }
            return list;
        }

        /// <summary>
        /// 加密文件Id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static string EnCryptFileName(int fileId)
        {
            return Security.EncryptionHelper.Des3_Encrypt(fileId.ToString());
        }

        /// <summary>
        /// 解密文件Id
        /// </summary>
        /// <param name="cryptStr"></param>
        /// <returns></returns>
        public static int DeCryptFileName(string cryptStr)
        {
            string str = Security.EncryptionHelper.Des3_Decrypt(cryptStr);
            if (!string.IsNullOrEmpty(str))
                return str.ToInt32();
            return 0;
        }
    }
}
