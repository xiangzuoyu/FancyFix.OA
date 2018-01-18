using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Log4netExt
{
    public interface IWebLog : ILog
    {
        void Info(string clientIP, string requestUrl, object message, int loginId = 0, string loginUserName = "");
        void Info(string clientIP, string requestUrl, object message, Exception t, int loginId = 0, string loginUserName = "");

        void Warn(string clientIP, string requestUrl, object message, int loginId = 0, string loginUserName = "");
        void Warn(string clientIP, string requestUrl, object message, Exception t, int loginId = 0, string loginUserName = "");

        void Error(string clientIP, string requestUrl, object message, int loginId = 0, string loginUserName = "");
        void Error(string clientIP, string requestUrl, object message, Exception t, int loginId = 0, string loginUserName = "");

        void Fatal(string clientIP, string requestUrl, object message, int loginId = 0, string loginUserName = "");
        void Fatal(string clientIP, string requestUrl, object message, Exception t, int loginId = 0, string loginUserName = "");
    }
}
