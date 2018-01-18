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
        /// 获取Title 或者 Description 针对详细页内容
        /// by:willian date:2016-4-29
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TOrD"></param>
        /// <returns></returns>
        public static string GetTitleAndDes4DetailHtml(int id, bool TOrD, string title, string className = "", string proNo = "")
        {
            string[] arrTitle = new string[] {
                "Wholesale {title} {classname} from China {prono} - Welfulloutdoors.com",
                "{title} {classname} Wholesaling from Hangzhou, China {prono} - Welfull Outdoors",
                "Wholesaling {title} {classname} made in China {prono} - Welfull Outdoors",
                "Purchasing {title} {classname} at an Affordable Price {prono} - Welfull Outdoors",
                "{title} {classname} Purchasing at a Reasonable Price {prono} - Welfull Outdoors ",
                "{title} {classname} Wholesale and Purchasing with a Reliable Quality {prono} - Welfull Outdoors" };
            string[] arrDes = new string[] {
                "Wholesale {title} at Welfulloutdoors.com with a lower price and better quality.",
                "Wholesale {title} at reasonable prices, buy {title} at Welfulloutdoors.com now.",
                "{title} from Welfull Outdoors deserve outdoor camping buyers having a large volume purchasing and wholesaling",
                "{title} wholesales and procurement from Welfull Outdoor served with highly competitive price and reliable quality",
                "{title} and wholesaling served by Welfull Outdoors are different in pricing, quality and quantity standards",
                "Purchasing and Wholesaling {title} from Wefull Outdoors guaranteed with an affordable price and reliable quality.",
                "Seek a {title} wholesaling and purchasing partnership in China? Welfull Outdoors is your first choice.",
                "Hard to find a satisfying {title} wholesaling and purchasing supplier? Welfull Outdoors at your service"
            };
            if (TOrD)
            {
                int index = id % arrTitle.Length;
                return arrTitle[index].Replace("{title}", title.Replace("Wholesale ", "").Replace("Wholesale", "")).Replace("{classname}", className).Replace("{prono}", proNo);
            }
            else
            {
                int index = id % arrDes.Length;
                return arrDes[index].Replace("{title}", title.Replace("Wholesale ", "").Replace("Wholesale", "")).Replace("{classname}", className).Replace("{prono}", proNo);

            }
        }

        public static string GetTitleAndDes4ListHtml(int cid, bool TOrD, string title, string className = "", string proNo = "")
        {
            string[] arrTitle = new string[] {
                "Wholesale {title} {classname} from China {prono} - Welfulloutdoors.com",
                "{title} {classname} Wholesaling from Hangzhou, China {prono} - Welfull Outdoors",
                "Wholesaling {title} {classname} made in China {prono} - Welfull Outdoors",
                "Purchasing {title} {classname} at an Affordable Price {prono} - Welfull Outdoors",
                "{title} {classname} Purchasing at a Reasonable Price {prono} - Welfull Outdoors ",
                "{title} {classname} Wholesale and Purchasing with a Reliable Quality {prono} - Welfull Outdoors" };
            string[] arrDes = new string[] {
                "Welfulloutdoors offers quality and affordable {classname} in low price. Reliable and professional China wholesaler where you can buy {classname} and drop-ship them anywhere in the world.",
                "Looking for high quality {classname} affordable prices? check out our {classname} and shop one today to start saving big!",
                "Find quality {classname} here with us. We do our best to make sure that all our {classname} are the best you can ever come across online or offline. Shop one today and start discovering your {classname} with a little bit of our help!",
                "Wholesale {classname}, we provide the {classname} of OEM / ODM, you can buy the best {classname} from China at the lowest price on Welfulloutdoors.com",
                "{classname} wholesales and purchasing, welfull outdoors serve you a series of {classname} to meet different needs of clients - Welfull Outdoors",
                "Different {classname} Wholesaling and Purchasing from Different Outdoor Demands, Welfull Outdoors at Your Service !",
                "Higher Quality and Lower Price! {classname} Wholesales and Purchasing from China Deserve Your Attention, Welfull Outdoors at Your Service"
            };
            if (TOrD)
            {
                int index = cid % arrTitle.Length;
                return arrTitle[index].Replace("{title}", title.Replace("Wholesale ", "").Replace("Wholesale", "")).Replace("{classname}", className).Replace("{prono}", proNo);
            }
            else
            {
                int index = cid % arrDes.Length;
                return arrDes[index].Replace("{classname}", className);

            }
        }

        /// <summary>
        /// 详细图片展示Alt 和title
        /// by:willian date:2016-9-11
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
            return Tool.EncryptionHelper.Des3_Encrypt(fileId.ToString());
        }

        /// <summary>
        /// 解密文件Id
        /// </summary>
        /// <param name="cryptStr"></param>
        /// <returns></returns>
        public static int DeCryptFileName(string cryptStr)
        {
            string str = Tool.EncryptionHelper.Des3_Decrypt(cryptStr);
            if (!string.IsNullOrEmpty(str))
                return str.ToInt32();
            return 0;
        }
    }
}
