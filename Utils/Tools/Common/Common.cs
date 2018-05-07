using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyFix.Tools.Common
{
    public class Common
    {
        /// <summary>
        /// 日期值的初始值
        /// </summary>
        /// <returns></returns>
        public static DateTime InitDateTime()
        {
            return DateTime.Parse("1900-01-01");
        }




        /// <summary>
        /// 格式化文件质量,传入K
        /// </summary>
        /// <param name="filesize"></param>
        /// <returns></returns>
        public static string FormatFileSize(float filesize)
        {
            float filesizeFloat = filesize / 1024;
            if (filesizeFloat < 1024)
            {
                return Math.Ceiling(filesizeFloat) + "K";
            }
            else
            {
                filesizeFloat = filesizeFloat / 1024;
                return Math.Round(filesizeFloat, 2) + "M";
            }
        }



        /// <summary>
        /// 根据ID获取放置的文件夹路径(每个文件夹最多1K个文件)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDocById(int id)
        {
            string docPath = "";
            string strId = id.ToString();
            int strLen = strId.Length;
            int strDepth = (int)Math.Floor((decimal)strLen / 3);
            for (int i = 0; i < strDepth; i++)
            {
                docPath += strId.Substring(0, 3) + "/";
                strId = strId.Substring(3);
            }
            docPath += "000/";
            return docPath;
        }


        #region 获取随机数


        /// <summary>
        /// 获得一个17位时间随机数
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDataRandom()
        {
            return System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }


        /// <summary>
        /// 获得一个9位时间随机数(小时)
        /// </summary>
        /// <returns>返回随机数</returns>
        public static string GetDataShortRandom()
        {
            return System.DateTime.Now.ToString("HHmmssfff");
        }



        /// <summary>
        /// 获取随即数.a-z,0-9
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetRadmonString(int len)
        {
            Random random = new Random();
            var _chars = new char[36];
            for (int i = 65; i <= 90; i++)
            {
                _chars[i - 65] = (char)i;
            }
            for (int i = 48; i < 58; i++)
            {

                _chars[i - 22] = (char)(i);

            }
            string str = string.Empty;
            for (int i = 0; i < len; i++)
            {
                str += _chars[random.Next(0, 35)];
            }
            return str;
        }

        #endregion
    }
}
