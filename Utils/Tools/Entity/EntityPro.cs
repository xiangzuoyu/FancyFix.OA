using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FancyFix.Tools.Json;

namespace FancyFix.Tools.Entity
{
    /// <summary>
    /// WebApi 产品公开字段
    /// </summary>
    public class EntityPro
    {
        public string ProNo;
        public string Title;
        public string Pic;
        public string Attr;
        public string AttrValue;
        public int ClassId;
        public int Id;
        public string AttrName;
        public byte ProType;
    }

    /// <summary>
    /// 产品通用配置
    /// </summary>
    public class RecProConfig
    {
        public string prono { get; set; }
        public string title { get; set; }
        public string pic { get; set; }
        public int classid { get; set; }
        public int id { get; set; }
        public string url { get; set; }
        public string murl { get; set; }
        public byte protype { get; set; }
        public int moq { get; set; }
        public decimal maxprice { get; set; }
        public decimal minprice { get; set; }
    }


    public class ProClassSearch
    {
        public string className { get; set; }
        public int classId { get; set; }
        /// <summary>
        /// 产品数
        /// </summary>
        public int cnt { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public List<ProClassSearch> child { get; set; }
    }

    public class EntityProRedis
    {
        public string ProNo { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Id { get; set; }
        //浏览量
        public double Cnt { get; set; }
        public string Pic { get; set; }
        public int ClassId { get; set; }
        public byte ProType { get; set; }
        public int Moq { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public class EntityProListRedis
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstPic { get; set; }
        public string Attribute { get; set; }
        public string Detail { get; set; }
        public int Moq { get; set; }
        public int Class_1 { get; set; }
        public int Class_2 { get; set; }
    }


    public class EntityKeyword
    {
        public string Keyword { get; set; }

        public double Cnt { get; set; }
    }

    public class EntityStock
    {
        public int Id { get; set; }
        public int ProId { get; set; }
        public int Cnt { get; set; }
        public byte Color { get; set; }
        public string ColorVal { get; set; }
        public string Price { get; set; }
        public int Moq { get; set; }
        public string TotalPrice { get; set; }
        public int OrderCnt { get; set; }
        public int RemainCnt { get; set; }
        public int ExceedCnt { get; set; }
    }

    public class EntityCart
    {
        public string title { get; set; }
        public string firstpic { get; set; }
        public int id { get; set; }
        public string url { get; set; }
        public int proid { get; set; }
        public byte protype { get; set; }
        public int cnt { get; set; }
        public decimal price { get; set; }
        public byte color { get; set; }
        public string colorval { get; set; }
    }
}
