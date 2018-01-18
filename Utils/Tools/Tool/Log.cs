using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;

namespace FancyFix.Tools.Tool
{
    /// <summary>
    /// 日志处理文件
    /// </summary>
    public class Log
    {
        private static readonly string logPath = ConfigurationManager.AppSettings["LogPath"];
        /// <summary>
        /// 写入条日志
        /// </summary>
        /// <param name="text"></param>
        public static void Write(string text)
        {
            string requestUrl = System.Web.HttpContext.Current.Request.RawUrl;
            string dirPath = PathHandle.GetFilePath(logPath.TrimEnd('/') + "/");
            string filePath = dirPath + "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            text = " ######################### " + DateTime.Now.ToString() + "(" + requestUrl + ") #########################\r\n" + " " + text + "\r\n\r\n";
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
            {
                sw.WriteLine(text);
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// 写入条日志
        /// </summary>
        /// <param name="text"></param>
        public static void Write(string ext, string text)
        {
            string requestUrl = System.Web.HttpContext.Current.Request.RawUrl;
            string dirPath = PathHandle.GetFilePath(logPath.TrimEnd('/') + "/");
            string filePath = dirPath + ext + "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            text = " ######################### " + DateTime.Now.ToString() + "(" + requestUrl + ") #########################\r\n" + " " + text + "\r\n\r\n";
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
            {
                sw.WriteLine(text);
                sw.Flush();
                sw.Close();
            }
        }

        public static void WritePur(string text)
        {
            string dirPath = PathHandle.GetFilePath(logPath.TrimEnd('/') + "/");
            //string dirPath = "D:\\web\\dzsc.com\\file1\\promulti\\log\\";
            string filePath = dirPath + "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            text = " ######################### " + DateTime.Now.ToString() + "#########################\r\n" + " " + text + "\r\n\r\n";
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
            {
                sw.WriteLine(text);
                sw.Flush();
                sw.Close();
            }
        }

        public static void WritePur(string text, string dirPath)
        {
            string filePath = dirPath + "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            text = " ######################### " + DateTime.Now.ToString() + "#########################\r\n" + " " + text + "\r\n\r\n";
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
            {
                sw.WriteLine(text);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
