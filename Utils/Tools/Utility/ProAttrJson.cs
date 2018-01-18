using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Utility
{
    public class ProAttrJson
    {
        [Serializable]
        public class Attr
        {
            private string _name = string.Empty;
            private string _value = string.Empty;
            private string _unit = string.Empty;
            private string _pcsMin = string.Empty;
            private string _price = string.Empty;
            //属性名称
            public string Name
            {
                set { _name = value.Replace("\"", "&quot;"); }
                get { return _name; }
            }

            //属性值
            public string Value
            {
                set { _value = value.Replace("\"", "&quot;"); }
                get { return _value; }
            }

            //单位
            public string Unit
            {
                set { _unit = value.Replace("\"", "&quot;"); }
                get { return _unit; }
            }

            public string PcsMin
            {
                set { _pcsMin = value.Replace("\"", "&quot;"); }
                get { return _pcsMin; }
            }

            public string Price
            {
                set { _price = value.Replace("\"", "&quot;"); }
                get { return _price; }
            }
        }

        /// <summary>
        /// 电子市场产品属性
        /// </summary>
        [Serializable]
        public class AttrList
        {
            public List<Attr> List { set; get; }
            public AttrList()
            {
                List = new List<Attr>();
            }
        }

        /// <summary>
        /// 产品价格
        /// 创建人：陈民礼 创建日期：2013-4-28
        /// </summary>
        [Serializable]
        public class AttrPrice
        {
            private string _pcsmin = string.Empty;
            private string _price = string.Empty;

            //产品起订量
            public string PcsMin
            {
                set { _pcsmin = value.Replace("\"", "&quot;"); }
                get { return _pcsmin; }
            }

            //产品价格
            public string Price
            {
                set { _price = value.Replace("\"", "&quot;"); }
                get { return _price; }
            }

        }
        /// <summary>
        /// 产品属性集合（新）
        /// 创建人：陈民礼 创建日期：2012-1-25
        /// </summary>
        [Serializable]
        public class AttrPriceList
        {
            public List<AttrPrice> List { set; get; }
            public AttrPriceList()
            {
                List = new List<AttrPrice>();
            }
        }
    }
}
