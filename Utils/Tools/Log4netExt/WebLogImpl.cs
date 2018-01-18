using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Log4netExt
{
    public class WebLogImpl : LogImpl, IWebLog
    {
        /// <summary>
        /// The fully qualified name of this declaring type not the type of any subclass.
        /// </summary>
        private readonly static Type ThisDeclaringType = typeof(WebLogImpl);

        public WebLogImpl(ILogger logger)
            : base(logger)
        {
        }

        #region Implementation of IWebLog

        public void Info(string clientIP, string requestUrl, object message, int loginId = 0, string loginUserName = "")
        {
            Info(clientIP, requestUrl, message, null, loginId, loginUserName);
        }

        public void Info(string clientIP, string requestUrl, object message, System.Exception t, int loginId = 0, string loginUserName = "")
        {
            if (this.IsInfoEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository, Logger.Name, Level.Info, message, t);
                loggingEvent.Properties["clientIP"] = clientIP;
                loggingEvent.Properties["requestUrl"] = requestUrl;
                loggingEvent.Properties["loginUserId"] = loginId;
                loggingEvent.Properties["loginUserName"] = loginUserName;
                Logger.Log(loggingEvent);
            }
        }

        public void Warn(string clientIP, string requestUrl, object message, int loginId = 0, string loginUserName = "")
        {
            Warn(clientIP, requestUrl, message, null, loginId, loginUserName);
        }

        public void Warn(string clientIP, string requestUrl, object message, System.Exception t, int loginId = 0, string loginUserName = "")
        {
            if (this.IsWarnEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository, Logger.Name, Level.Warn, message, t);
                loggingEvent.Properties["clientIP"] = clientIP;
                loggingEvent.Properties["requestUrl"] = requestUrl;
                loggingEvent.Properties["loginUserId"] = loginId;
                loggingEvent.Properties["loginUserName"] = loginUserName;
                Logger.Log(loggingEvent);
            }
        }

        public void Error(string clientIP, string requestUrl, object message, int loginId = 0, string loginUserName = "")
        {
            Error(clientIP, requestUrl, message, null, loginId, loginUserName);
        }

        public void Error(string clientIP, string requestUrl, object message, System.Exception t, int loginId = 0, string loginUserName = "")
        {
            if (this.IsErrorEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository, Logger.Name, Level.Error, message, t);
                loggingEvent.Properties["clientIP"] = clientIP;
                loggingEvent.Properties["requestUrl"] = requestUrl;
                loggingEvent.Properties["loginUserId"] = loginId;
                loggingEvent.Properties["loginUserName"] = loginUserName;
                Logger.Log(loggingEvent);
            }
        }

        public void Fatal(string clientIP, string requestUrl, object message, int loginId = 0, string loginUserName = "")
        {
            Fatal(clientIP, requestUrl, null, loginId, loginUserName);
        }

        public void Fatal(string clientIP, string requestUrl, object message, System.Exception t, int loginId = 0, string loginUserName = "")
        {
            if (this.IsFatalEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository, Logger.Name, Level.Fatal, message, t);
                loggingEvent.Properties["clientIP"] = clientIP;
                loggingEvent.Properties["requestUrl"] = requestUrl;
                loggingEvent.Properties["loginUserId"] = loginId;
                loggingEvent.Properties["loginUserName"] = loginUserName;
                Logger.Log(loggingEvent);
            }
        }

        #endregion Implementation of IWebLog
    }
}
