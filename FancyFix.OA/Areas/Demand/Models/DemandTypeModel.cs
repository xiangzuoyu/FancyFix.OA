using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FancyFix.OA.Areas.Demand.Models
{
    public class DemandTypeModel
    {
        /// <summary>
        /// 采购情况
        /// </summary>
       public string Field1 { get; set;}
        /// <summary>
        /// 采购单价
        /// </summary>
        public string Field2 { get; set; }
        /// <summary>
        /// 频率
        /// </summary>
        public string Field3 { get; set; }


        public string Field4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Field5 { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string Field6 { get; set; }
        /// <summary>
        /// 需要解决的问题
        /// </summary>
        public string Field7 { get; set; }
        /// <summary>
        /// 预期达到的效果
        /// </summary>
        public string Field8 { get; set; }
        /// <summary>
        /// 对业绩的影响程度
        /// </summary>
        public string Field9 { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public string Field10 { get; set; }

    }


    public class ContentModel
    {
        public ContentModel()
        {
            IsRquest = true;
        }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 绑定字段
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRquest { get; set; }
        /// <summary>
        /// 文字提示
        /// </summary>
        public string Reminder { get; set; }
    }


}