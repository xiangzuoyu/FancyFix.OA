using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Enums
{
    public class Tools
    {
        /// <summary>
        /// 获取单选框列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name">元素name</param>
        /// <param name="className">classname</param>
        /// <returns></returns>
        public static string GetRadioList(Type type, string name, string className, string remark = "", byte value = 0, string selectValues = "")
        {
            string result = string.Empty;
            if (selectValues != "")
                foreach (KeyValuePair<byte, string> item in GetEnum(type))
                {
                    if (("," + selectValues + ",").Contains("," + item.Key + ","))
                        result += string.Format("<label><input type='radio'  name='{0}' id='{0}{1}' value='{1}' class='{2}' {4} {5} /><b>{3}</b></label>", name, item.Key, className, item.Value, item.Key == value ? "checked" : "", remark);
                }
            else
                foreach (KeyValuePair<byte, string> item in GetEnum(type))
                {
                    result += string.Format("<label><input type='radio'  name='{0}' id='{0}{1}' value='{1}' class='{2}' {4} {5} /><b>{3}</b></label>", name, item.Key, className, item.Value, item.Key == value ? "checked" : "", remark);
                }
            return result;
        }

        /// <summary>
        /// 获取单选框列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name">元素name</param>
        /// <param name="className">classname</param>
        /// <returns></returns>
        public static string GetRadioList(Type type, byte value, string name, string className)
        {
            string result = string.Empty;
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                result += string.Format("<input type='radio' name='{4}' value='{0}' class='{1}' {2}/>{3} ", item.Key, className, value == item.Key ? "checked" : "", item.Value, name);
            }
            return result;
        }

        public static byte GetValue(Type type, string name)
        {
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                if (item.Value == name)
                    return item.Key;
            }
            return 0;
        }

        public static byte GetValueByName(Type type, string name)
        {
            foreach (string item in GetEnumNames(type))
            {
                if (item.ToLower() == name.ToLower())
                    return (byte)Enum.Parse(type, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name));
            }
            return 0;
        }

        /// <summary>
        /// 获取多选框列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="elementName">元素的name属性值</param>
        /// <param name="className">元素Class的属性值</param>
        /// <returns></returns>
        public static string GetCheckBoxList(Type type, string elementName, string className)
        {
            string result = string.Empty;
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                result += "<input type='checkbox' name='" + elementName + "' value='" + item.Key + "' class='" + className + "' />" + item.Value + " ";
            }
            return result;
        }

        /// <summary>
        /// 获取多选框列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value">选中值</param>
        /// <param name="elementName">元素的name属性值</param>
        /// <param name="className">元素Class的属性值</param>
        /// <returns></returns>
        public static string GetCheckBoxList(Type type, string value, string elementName, string className)
        {
            string result = string.Empty;
            value = "," + value.Trim(',') + ",";
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                result += string.Format("<input type='checkbox' name='{4}' value='{0}' class='{1}' {2}/>{3} ", item.Key, className, value.Contains("," + item.Key + ",") ? "checked" : "", item.Value, elementName);
            }
            return result;
        }

        /// 获取下拉框选项
        /// </summary>
        /// <param name="type"></param>
        public static string GetOptionHtml(Type type)
        {
            string result = string.Empty;
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                result += "<option value=\"" + item.Key + "\" >" + item.Value + "</option>";
            }
            return result;
        }


        /// <summary>
        /// 获取下拉框选项
        /// </summary>
        /// <param name="type"></param>
        public static string GetOptionHtml(Type type, byte value)
        {
            string result = string.Empty;
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                result += string.Format("<option value=\"{0}\" {2}>{1}</option>", item.Key, item.Value, item.Key == value ? "selected" : "");
            }
            return result;
        }

        /// <summary>
        /// 获取下拉框选项
        /// date:2016-01-18 by Harry
        /// </summary>
        /// <param name="type"></param>
        /// <param name="values"></param>
        public static string GetOptionHtml(Type type, string values)
        {
            string result = string.Empty;
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                if (("," + values + ",").Contains("," + item.Key + ","))
                    result += string.Format("<option value=\"{0}\">{1}</option>", item.Key, item.Value);
            }
            return result;
        }

        /// <summary>
        /// 获取下拉框选项
        /// date:2016-01-18 by Harry
        /// </summary>
        /// <param name="type"></param>
        /// <param name="values"></param>
        public static string GetOptionHtml(Type type, string values, byte value)
        {
            string result = string.Empty;
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                if (("," + values + ",").Contains("," + item.Key + ","))
                    result += string.Format("<option value=\"{0}\" {2}>{1}</option>", item.Key, item.Value, item.Key == value ? "selected" : "");
            }
            return result;
        }

        /// <summary>
        /// 根据模版获取html
        /// date:2016-07-26 by Harry
        /// </summary>
        /// <param name="type"></param>
        /// <param name="template">模版 key:{0},value:{1}</param>
        public static string GetHtmlByTemp(Type type, string template)
        {
            string result = string.Empty;
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                result += string.Format(template, item.Key, item.Value);
            }
            return result;
        }

        /// <summary>
        /// 根据模版获取html
        /// date:2016-07-26 by Harry
        /// </summary>
        /// <param name="type"></param>
        /// <param name="template">模版 key:{0},value:{1},checktemp:{2}</param>
        /// <param name="checktemp">选中样式模版 比如: class="on"</param>
        /// <param name="value">选中值</param>
        public static string GetHtmlByTemp(Type type, string template, string checktemp, byte value)
        {
            string result = string.Empty;
            foreach (KeyValuePair<byte, string> item in GetEnum(type))
            {
                result += string.Format(template, item.Key, item.Value, item.Value, item.Key == value ? checktemp : "");
            }
            return result;
        }

        /// <summary>
        /// 获取所有枚举名称 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string[] GetEnumNames(Type enumType)
        {
            return System.Enum.GetNames(enumType);
        }

        /// <summary>
        /// 获取每句的描述，获取相关值,用于页面,上绑定,要继承byte
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<KeyValuePair<byte, string>> GetEnum(Type enumType)
        {
            var names = System.Enum.GetNames(enumType);
            if (names != null && names.Length > 0)
            {
                List<KeyValuePair<byte, string>> kvList = new List<KeyValuePair<byte, string>>(names.Length);
                foreach (var item in names)
                {
                    System.Reflection.FieldInfo finfo = enumType.GetField(item);
                    object[] enumAttr = finfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                    if (enumAttr != null && enumAttr.Length > 0)
                    {
                        string description = string.Empty;

                        System.ComponentModel.DescriptionAttribute desc = enumAttr[0] as System.ComponentModel.DescriptionAttribute;
                        if (desc != null)
                        {
                            description = desc.Description;
                        }
                        kvList.Add(new KeyValuePair<byte, string>((byte)finfo.GetValue(null), description));
                    }

                }
                return kvList;

            }
            return null;
        }
        /// <summary>
        /// 获取每句的描述，获取相关值,用于页面,上绑定,要继承byte,参数indexNum获取枚举的条件
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="indexNum"></param>
        /// <returns></returns>
        public static List<KeyValuePair<byte, string>> GetEnum(Type enumType, int indexFirst, int indexLast)
        {
            var names = System.Enum.GetNames(enumType);
            if (names != null && names.Length > 0)
            {
                List<KeyValuePair<byte, string>> kvList = new List<KeyValuePair<byte, string>>(names.Length);
                foreach (var item in names)
                {
                    System.Reflection.FieldInfo finfo = enumType.GetField(item);
                    if ((byte)finfo.GetValue(null) > indexFirst && (byte)finfo.GetValue(null) < indexLast)
                    {
                        object[] enumAttr = finfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                        if (enumAttr != null && enumAttr.Length > 0)
                        {
                            string description = string.Empty;

                            System.ComponentModel.DescriptionAttribute desc = enumAttr[0] as System.ComponentModel.DescriptionAttribute;
                            if (desc != null)
                            {
                                description = desc.Description;
                            }
                            kvList.Add(new KeyValuePair<byte, string>((byte)finfo.GetValue(null), description));
                        }
                    }
                }
                return kvList;
            }
            return null;
        }

        /// <summary>
        /// 获取枚举的描述 Description
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Type enumType, object val)
        {
            if (val == null) return "";
            string enumvalue = System.Enum.GetName(enumType, val);
            if (string.IsNullOrEmpty(enumvalue))
            {
                return "";
            }
            System.Reflection.FieldInfo finfo = enumType.GetField(enumvalue);
            object[] enumAttr = finfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
            if (enumAttr.Length > 0)
            {
                System.ComponentModel.DescriptionAttribute desc = enumAttr[0] as System.ComponentModel.DescriptionAttribute;
                if (desc != null)
                {
                    return desc.Description;
                }
            }
            return enumvalue;

        }
    }
}
