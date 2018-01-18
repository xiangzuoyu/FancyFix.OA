using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Utility
{
    /// <summary>
    /// 价格信息
    /// 创建人：陈民礼 创建日期：2013-12-31
    /// </summary>
    [Serializable]
    public class JsonPrice
    {
        private int _infoId;
        private decimal _price;
        private int _sellerId;
        private int _readyDays;

        //产品Id
        public int I
        {
            set { _infoId = value; }
            get { return _infoId; }
        }

        //产品价格
        public decimal P
        {
            set { _price = value; }
            get { return _price; }
        }

        //供应商Id
        public int S
        {
            set { _sellerId = value; }
            get { return _sellerId; }
        }

        //货期
        public int R
        {
            set { _readyDays = value; }
            get { return _readyDays; }
        }
    }

    /// <summary>
    /// 价格信息
    /// 创建人：陈民礼 创建日期：2013-12-31
    /// </summary>
    [Serializable]
    public class JsonPriceList
    {
        public List<JsonPrice> List { set; get; }
        public JsonPriceList()
        {
            List = new List<JsonPrice>();
        }
    }
}
