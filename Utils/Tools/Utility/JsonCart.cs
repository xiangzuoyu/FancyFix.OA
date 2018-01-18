using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Utility
{
    /// <summary>
    /// 购物车信息
    /// 创建人：陈民礼 创建日期：2013-5-13
    /// </summary>
    [Serializable]
    public class Cart
    {
        private string _proId = string.Empty;
        private string _cnt = string.Empty;
        private string _pic = string.Empty;
        private string _proName = string.Empty;
        //产品Id
        public string P
        {
            set { _proId = value.Replace("\"", "&quot;"); }
            get { return _proId; }
        }

        //产品数量
        public string C
        {
            set { _cnt = value.Replace("\"", "&quot;"); }
            get { return _cnt; }
        }

        //产品图片
        public string Pic
        {
            set { _pic = value.Replace("\"", "&quot;"); }
            get { return _pic; }
        }

        //产品名称
        public string PN
        {
            set { _proName = value.Replace("\"", "&quot;"); }
            get { return _proName; }
        }

        //产品名称
        public string Class_2
        {
            get;
            set;
        }

        //产品名称
        public string MaxVF
        {
            get;
            set;
        }

        //产品名称
        public string Attr20
        {
            get;
            set;
        }

        //产品名称
        public string Attr4
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 购物车信息
    /// 创建人：陈民礼 创建日期：2013-5-13
    /// </summary>
    [Serializable]
    public class CartList
    {
        public List<Cart> List { set; get; }
        public CartList()
        {
            List = new List<Cart>();
        }
    }
    /// <summary>
    /// 优惠规则
    /// 创建人：陈民礼 创建日期：2013-8-8
    /// </summary>
    [Serializable]
    public class Preferential
    {
        private int _acount;      //购买金额
        private bool _isFreeShip;// 是否包邮 1为包邮 0为不包邮
        private decimal _minus;      //减少金额

        //购买金额
        public int Acount
        {
            set { _acount = value; }
            get { return _acount; }
        }

        //是否包邮
        public bool IsFreeShip
        {
            set { _isFreeShip = value; }
            get { return _isFreeShip; }
        }

        //减少金额
        public decimal Minus
        {
            set { _minus = value; }
            get { return _minus; }
        }
    }

    /// <summary>
    /// 优惠规则列表
    /// 创建人：陈民礼 创建日期：2013-8-8
    /// </summary>
    [Serializable]
    public class PreferentialList
    {
        public List<Preferential> List { set; get; }
        public PreferentialList()
        {
            List = new List<Preferential>();
        }
    }
}
