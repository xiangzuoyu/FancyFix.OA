using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Core.Crypt
{
    public class WebKey
    {
        public static string siteKeyCode = "W===E--QR3#eFae2@&"; //网站加密字符串

        #region 网站加密验证
        /// <summary>
        /// 获取需要传递的Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string KeyGet(int id)
        {
            DateTime startDate = DateTime.Parse("2000-1-1");
            TimeSpan ts = DateTime.Now - startDate;
            int days = ts.Days;
            return id + "s" + days.ToString();
        }

        public static string KeyGet2(int id)
        {
            DateTime startDate = DateTime.Parse("2000-1-1");
            TimeSpan ts = DateTime.Now - startDate;
            int days = ts.Days;
            return id + "s" + days.ToString();
        }

        /// <summary>
        /// 获取传递的加密码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string KeyCheckCode(int id)
        {
            return Tools.Security.Md5Helper.GetMd5Hash(siteKeyCode + id).Substring(0, 8);
        }

        public static string KeyCheckCode2(int id)
        {
            return Tools.Security.Md5Helper.GetMd5Hash(siteKeyCode + id).Substring(0, 8);
        }

        /// <summary>
        /// 验证加密 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public bool KeyCheck(string key, string checkCode)
        {
            //分隔Key
            if (key == "" || checkCode == "")
            {
                return false;
            }
            string[] keys = key.Split('s');
            if (keys.Length != 2)
            {
                return false;
            }
            int id = keys[0].ToInt32();
            if (id < 0)
            {
                return false;
            }

            int days = keys[1].ToInt32();
            DateTime startDate = DateTime.Parse("2000-1-1");
            TimeSpan ts = DateTime.Now - startDate;
            if (days < ts.Days)
            {
                return false;
            }
            if (Tools.Security.Md5Helper.GetMd5Hash(siteKeyCode + id).Substring(0, 8) == checkCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证加密,返回加密的id
        /// </summary>
        /// <param name="key"></param>
        /// <param name="checkCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool KeyCheck(string key, string checkCode, ref int id)
        {
            //分隔Key
            if (key == "" || checkCode == "")
            {
                return false;
            }
            string[] keys = key.Split('s');
            if (keys.Length != 2)
            {
                return false;
            }
            id = keys[0].ToInt32();
            if (id < 0)
            {
                return false;
            }

            int days = keys[1].ToInt32();
            DateTime startDate = DateTime.Parse("2000-1-1");
            TimeSpan ts = DateTime.Now - startDate;
            if (days < ts.Days)
            {
                return false;
            }

            if (Tools.Security.Md5Helper.GetMd5Hash(siteKeyCode + id).Substring(0, 8) == checkCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool KeyCheck2(string key, string checkCode)
        {
            //分隔Key
            if (key == "" || checkCode == "")
            {
                return false;
            }
            string[] keys = key.Split('s');
            if (keys.Length != 2)
            {
                return false;
            }
            int id = keys[0].ToInt32();
            if (id < 0)
            {
                return false;
            }

            int days = keys[1].ToInt32();
            DateTime startDate = DateTime.Parse("2000-1-1");
            TimeSpan ts = DateTime.Now - startDate;
            if (days < ts.Days)
            {
                return false;
            }

            if (Tools.Security.Md5Helper.GetMd5Hash(siteKeyCode + id).Substring(0, 8) == checkCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
