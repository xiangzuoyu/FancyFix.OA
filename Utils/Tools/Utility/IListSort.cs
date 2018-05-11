using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace FancyFix.Tools.Utility
{
    /// <summary>
    /// IList排序类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IListSort<T>
    {
        private string[] _propertyName;
        private bool[] _sortBy;
        private IList<T> _list;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="list">排序的Ilist</param>
        /// <param name="propertyName">排序字段属性名</param>
        /// <param name="sortBy">true升序 false 降序 不指定则为true</param>
        public IListSort(IList<T> list, string[] propertyName, bool[] sortBy)
        {
            _list = list;
            _propertyName = propertyName;
            _sortBy = sortBy;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="list">排序的Ilist</param>
        /// <param name="propertyName">排序字段属性名</param>
        /// <param name="sortBy">true升序 false 降序 不指定则为true</param>
        public IListSort(IList<T> list, string[] propertyName)
        {
            _list = list;
            _propertyName = propertyName;
            for (int i = 0; i < _propertyName.Length; i++)
            {
                _sortBy[i] = true;
            }
        }

        /// <summary>
        /// IList
        /// </summary>
        public IList<T> List
        {
            get { return _list; }
            set { _list = value; }
        }

        /// <summary>
        /// 排序字段属性名
        /// </summary>
        public string[] PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        /// <summary>
        /// true升序 false 降序
        /// </summary>
        public bool[] SortBy
        {
            get { return _sortBy; }
            set { _sortBy = value; }
        }

        /// <summary>
        /// 排序，插入排序方法
        /// </summary>
        /// <returns></returns>
        public IList<T> Sort()
        {
            if (_list.Count == 0) return _list;
            for (int i = 1; i < _list.Count; i++)
            {
                T t = _list[i];
                int j = i;
                while ((j > 0) && Compare(_list[j - 1], t) < 0)
                {
                    _list[j] = _list[j - 1];
                    --j;
                }
                _list[j] = t;
            }
            return _list;
        }

        /// <summary>
        /// 比较大小 返回值 小于零则X小于Y，等于零则X等于Y，大于零则X大于Y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int Compare(T x, T y)
        {
            int i = 0;
            //检查属性名
            for (i = 0; i < _propertyName.Length; ++i)
            {
                if (string.IsNullOrEmpty(_propertyName[i])) throw new ArgumentNullException("没有指字对象的排序字段属性名!");
            }

            //取属性的属性
            PropertyInfo[] property = new PropertyInfo[_propertyName.Length];
            for (i = 0; i < _propertyName.Length; ++i)
            {
                property[i] = typeof(T).GetProperty(_propertyName[i]);
                if (property[i] == null) throw new ArgumentNullException("在对象中没有找到指定属性!");
            }

            int compare = 0;
            for (i = 0; i < _propertyName.Length; ++i)
            {
                compare = CompareOne(x, y, property[i], _sortBy[i]);
                if (compare != 0) return compare;
            }
            return compare;
        }

        private int CompareOne(T x, T y, PropertyInfo property, bool sortBy)
        {
            switch (property.PropertyType.ToString())
            {
                case "System.Int32":
                    int int1 = 0;
                    int int2 = 0;
                    if (property.GetValue(x, null) != null)
                    {
                        int1 = Convert.ToInt32(property.GetValue(x, null));
                    }
                    if (property.GetValue(y, null) != null)
                    {
                        int2 = Convert.ToInt32(property.GetValue(y, null));
                    }
                    if (sortBy)
                    {
                        return int2.CompareTo(int1);
                    }
                    else
                    {
                        return int1.CompareTo(int2);
                    }
                case "System.Double":
                    double double1 = 0;
                    double double2 = 0;
                    if (property.GetValue(x, null) != null)
                    {
                        double1 = Convert.ToDouble(property.GetValue(x, null));
                    }
                    if (property.GetValue(y, null) != null)
                    {
                        double2 = Convert.ToDouble(property.GetValue(y, null));
                    }
                    if (sortBy)
                    {
                        return double2.CompareTo(double1);
                    }
                    else
                    {
                        return double1.CompareTo(double2);
                    }
                case "System.String":
                    string string1 = string.Empty;
                    string string2 = string.Empty;
                    if (property.GetValue(x, null) != null)
                    {
                        string1 = property.GetValue(x, null).ToString();
                    }
                    if (property.GetValue(y, null) != null)
                    {
                        string2 = property.GetValue(y, null).ToString();
                    }
                    if (sortBy)
                    {
                        return string2.CompareTo(string1);
                    }
                    else
                    {
                        return string1.CompareTo(string2);
                    }
                case "System.DateTime":
                    DateTime DateTime1 = DateTime.Now;
                    DateTime DateTime2 = DateTime.Now;
                    if (property.GetValue(x, null) != null)
                    {
                        DateTime1 = Convert.ToDateTime(property.GetValue(x, null));
                    }
                    if (property.GetValue(y, null) != null)
                    {
                        DateTime2 = Convert.ToDateTime(property.GetValue(y, null));
                    }
                    if (sortBy)
                    {
                        return DateTime2.CompareTo(DateTime1);
                    }
                    else
                    {
                        return DateTime1.CompareTo(DateTime2);
                    }
            }
            return 0;
        }
    }
}
