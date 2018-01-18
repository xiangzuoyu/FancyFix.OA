using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Collections;

namespace FancyFix.Tools.Tool
{
    public class JsonHelper
    {

        public static string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static List<T> Deserialize<T>(string obj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(obj);
        }

        public static T DeserializeModel<T>(string obj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(obj);
        }

        public static string DataTableToJson(DataTable dt, int records)
        {

            DataColumnToLower(dt);
            if (records <= 0) records = dt.Rows.Count;

            string json = "{\"total\":\"" + records + "\",\"rows\":" + DataTaableToJson(dt) + "}";
            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected static string DataTaableToJson(DataTable dt)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            //string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dt, timeConverter);
            //return jsonStr;
            DBNullCreationConverter nullConverter = new DBNullCreationConverter();
            return JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented, timeConverter, nullConverter);

        }




        public static void DataColumnToLower(DataTable dt)
        {
            foreach (DataColumn item in dt.Columns)
            {
                item.ColumnName = item.ColumnName.ToLower();
            }
        }



    }




    /// <summary>
    /// 对DBNull的转换处理，此处只写了转换成JSON字符串的处理，JSON字符串转对象的未处理
    /// </summary>
    public class DBNullCreationConverter : JsonConverter
    {
        /// <summary>
        /// 是否允许转换
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            bool canConvert = false;
            switch (objectType.FullName)
            {
                case "System.DBNull":

                    canConvert = true;
                    break;
            }
            return canConvert;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(string.Empty);
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 是否允许转换JSON字符串时调用
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
    }
}
