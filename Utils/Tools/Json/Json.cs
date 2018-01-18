using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyFix.Tools.Json
{
    [Serializable]
    public class ResultJson
    {
        public byte state { get; set; }
        public List<Attr> attr { get; set; }
        public string error { get; set; }
    }

    [Serializable]
    public class Attr
    {
        public int id { get; set; }
        public string name { get; set; }
        public int valueid { get; set; }
        public string value { get; set; }
        public int type { get; set; }
        public bool issearch { get; set; }
        public bool isseriers { get; set; }
        public bool isneeded { get; set; }
        public int seq { get; set; }
        public List<AttrValue> attrlist { get; set; }

    }

    [Serializable]
    public class AttrValue
    {
        public int id { get; set; }
        public string value { get; set; }
    }

    [Serializable]
    public class AttrJson
    {
        public string n { get; set; }
        public string v { get; set; }
    }

    [Serializable]
    public class AttrUrlJson
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    [Serializable]
    public class AttrListJson
    {
        public string name { get; set; }
        public List<AttrUrlJson> attrlist { get; set; }
    }

    [Serializable]
    public class FileJson
    {
        public string name { get; set; }
        public string value { get; set; }
    }


    [Serializable]
    public class ProPriceCnt
    {
        public int id { get; set; }
        public string no { get; set; }
        public int cnt { get; set; }
        public string note { get; set; }
        public decimal price { get; set; }
        public string sendday { get; set; }
    }

    public class OrderCart
    {
        public int id { get; set; }
        public int pid { get; set; }
        public byte otype { get; set; }
        public int cnt { get; set; }
        public string title { get; set; }
        public string pic { get; set; }
        public string url { get; set; }
        public decimal price { get; set; }
        public byte color { get; set; }
        public string colorval { get; set; }
    }



    [Serializable]
    public class ViewRecord
    {
        public int id { get; set; }
        public string title { get; set; }
        public string pic { get; set; }
        public DateTime date { get; set; }
        public int cnt { get; set; }
        public byte protype { get; set; }
    }

    [Serializable]
    public class Price
    {
        public int c { get; set; }
        public decimal p { get; set; }
    }

    [Serializable]
    public class WebStat
    {
        public string url { get; set; }
        public string urlreferrer { get; set; }
        public DateTime addtime { get; set; }
        public int loadtime { get; set; }
        public string callback { get; set; }
    }

    [Serializable]
    public class Completeness
    {
        public int all { get; set; }
        public int product { get; set; }
        public int transcation { get; set; }
        public int payment { get; set; }
    }

    /// <summary>
    /// 产品详细数据结构
    /// </summary>
    [Serializable]
    public class ProductDetail
    {
        public byte state { get; set; }
        public string error { get; set; }
        public int itemid { get; set; }
        public int classid { get; set; }
        public byte profrom { get; set; }
        public string title { get; set; }
        public string pics { get; set; }
        public string detail { get; set; }

        public string amazonurl { get; set; }
    }

    [Serializable]
    public class PriceRange
    {
        public int cnt { get; set; }
        public decimal price { get; set; }
        public decimal per { get; set; }

    }

    [Serializable]
    public class Color
    {
        public byte id { get; set; }
        public string value { get; set; }
        public int cnt { get; set; }
    }

    public class Answer
    {
        public int qid { get; set; }
        public string email { get; set; }
        public int country { get; set; }
        public string contact { get; set; }
        public List<Options> options { get; set; }
    }

    public class Options
    {
        public int id { get; set; }
        public string option { get; set; }
    }
}
