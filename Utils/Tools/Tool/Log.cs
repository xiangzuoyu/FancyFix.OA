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
        /// 写入条日志，包含http上下文
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
        /// 写入条日志，包含http上下文
        /// </summary>
        /// <param name="ext">文件夹名</param>
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

        /// <summary>
        /// 写入条纯文本日志
        /// </summary>
        /// <param name="text"></param>
        public static void WritePur(string text)
        {
            string dirPath = PathHandle.GetFilePath(logPath.TrimEnd('/') + "/");
            WritePur(text, dirPath);
        }

        /// <summary>
        /// 写入条纯文本日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="dirPath"></param>
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

        /// <summary>
        /// 插入纯净的日志行
        /// </summary>
        /// <param name="text"></param>
        public static void WritePure(string text)
        {
            string dirPath = PathHandle.GetFilePath(logPath.TrimEnd('/') + "/");
            string filePath = dirPath + "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
            {
                sw.WriteLine(text);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
