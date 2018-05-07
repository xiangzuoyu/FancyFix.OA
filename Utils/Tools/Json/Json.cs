using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyFix.Tools.Json
{
    /// <summary>
    /// 返回结果模型
    /// </summary>
    [Serializable]
    public class ResultJson
    {
        public byte state { get; set; }
        public List<Attr> attr { get; set; }
        public string error { get; set; }
    }

    /// <summary>
    /// 产品属性模型
    /// </summary>
    [Serializable]
    public class Attr
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public int inputtype { get; set; }
        public bool issort { get; set; }
        public bool isspecial { get; set; }
        public bool isrequired { get; set; }
        public bool isshow { get; set; }
        public int sequence { get; set; }
        public List<AttrValue> attrlist { get; set; }

    }

    /// <summary>
    /// 产品属性值模型
    /// </summary>
    [Serializable]
    public class AttrValue
    {
        public int id { get; set; }
        public string value { get; set; }
    }

    /// <summary>
    /// 产品属性键值对
    /// </summary>
    [Serializable]
    public class AttrJson
    {
        public string n { get; set; }
        public string v { get; set; }
    }

    /// <summary>
    /// 文件模型
    /// </summary>
    [Serializable]
    public class FileJson
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    /// <summary>
    /// 价格模型
    /// </summary>
    [Serializable]
    public class Price
    {
        public int c { get; set; }
        public decimal p { get; set; }
    }

    /// <summary>
    /// 流量统计模型
    /// </summary>
    [Serializable]
    public class WebStat
    {
        public string url { get; set; }
        public string urlreferrer { get; set; }
        public DateTime starttime { get; set; }
    }

    /// <summary>
    /// 产品详细数据模型
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

    /// <summary>
    /// 价格排序模型
    /// </summary>
    [Serializable]
    public class PriceRange
    {
        public int cnt { get; set; }
        public decimal price { get; set; }
        public decimal per { get; set; }

    }
}
