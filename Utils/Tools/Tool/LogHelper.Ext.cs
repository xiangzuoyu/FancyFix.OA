using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Tool
{
    public partial class LogHelper
    {
        static string Splitter = "\r\n";
        static ConcurrentDictionary<Type, ILog> LoggerCache = new ConcurrentDictionary<Type, ILog>();

        static ILog GetLog<T>()
        {
            var type = typeof(T);

            if (type == typeof(LogHelper))
                type = GetCallerType();

            ILog log;
            if (!LoggerCache.TryGetValue(type, out log))
            {
                log = log4net.LogManager.GetLogger(type);

                LoggerCache.TryAdd(type, log);
                return log;
            }
            return log;
        }

        public static void Debug<T>(string msg, params object[] args)
        {
            if (args == null || args.Length == 0)
                GetLog<T>().Debug(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter));
            else
                GetLog<T>().DebugFormat(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), args);
        }

        public static void Debug<T>(string msg, Exception ex)
        {
            GetLog<T>().Debug(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), ex);
        }

        public static void Info<T>(string msg, params object[] args)
        {
            if (args == null || args.Length == 0)
                GetLog<T>().Info(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter));
            else
                GetLog<T>().InfoFormat(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), args);
        }

        public static void Info<T>(string msg, Exception ex)
        {
            GetLog<T>().Info(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), ex);
        }

        public static void Warn<T>(string msg, params object[] args)
        {
            if (args == null || args.Length == 0)
                GetLog<T>().Warn(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter));
            else
                GetLog<T>().WarnFormat(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), args);
        }

        public static void Warn<T>(string msg, Exception ex)
        {
            GetLog<T>().Warn(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), ex);
        }

        public static void Error<T>(string msg, params object[] args)
        {
            if (args == null || args.Length == 0)
                GetLog<T>().Error(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter));
            else
                GetLog<T>().ErrorFormat(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), args);
        }

        public static void Error<T>(string msg, Exception ex)
        {
            GetLog<T>().Error(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), ex);
        }

        public static void Fatal<T>(string msg, params object[] args)
        {
            if (args == null || args.Length == 0)
                GetLog<T>().Fatal(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter));
            else
                GetLog<T>().FatalFormat(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), args);
        }

        public static void Fatal<T>(string msg, Exception ex)
        {
            GetLog<T>().Fatal(msg + (string.IsNullOrEmpty(msg) ? string.Empty : Splitter), ex);
        }


        public static void Debug(string msg, params object[] args)
        {
            Debug<LogHelper>(msg, args);
        }

        public static void Debug(string msg, Exception ex)
        {
            Debug<LogHelper>(msg, ex);
        }

        public static void Info(string msg, params object[] args)
        {
            Info<LogHelper>(msg, args);
        }

        public static void Info(string msg, Exception ex)
        {
            Info<LogHelper>(msg, ex);
        }

        #region Compatiblity for previous methods
        public static void LogInfo(string msg, params object[] args)
        {
            Info<LogHelper>(msg, args);
        }

        public static void LogInfo(string msg, Exception ex)
        {
            Info<LogHelper>(msg, ex);
        }
        #endregion

        public static void Warn(string msg, params object[] args)
        {
            Warn<LogHelper>(msg, args);
        }

        public static void Warn(string msg, Exception ex)
        {
            Warn<LogHelper>(msg, ex);
        }

        public static void Error(string msg, params object[] args)
        {
            Error<LogHelper>(msg, args);
        }

        public static void Error(string msg, Exception ex)
        {
            Error<LogHelper>(msg, ex);
        }

        #region Compatiblity for previous methods
        public static void LogError(string msg, params object[] args)
        {
            Error<LogHelper>(msg, args);
        }

        public static void LogError(string msg, Exception ex)
        {
            Error<LogHelper>(msg, ex);
        }
        #endregion

        public static void Fatal(string msg, params object[] args)
        {
            Fatal<LogHelper>(msg, args);
        }

        public static void Fatal(string msg, Exception ex)
        {
            Fatal<LogHelper>(msg, ex);
        }




        #region Extension

        public static void Info(Exception ex)
        {
            Info<LogHelper>(null, ex);
        }
        public static void LogInfo(Exception ex)
        {
            Info<LogHelper>(null, ex);
        }
        public static void Error(Exception ex)
        {
            Error<LogHelper>(null, ex);
        }
        public static void LogError(Exception ex)
        {
            Error<LogHelper>(null, ex);
        }
        public static void Fatal(Exception ex)
        {
            Fatal<LogHelper>(null, ex);
        }
        public static void LogFatal(Exception ex)
        {
            Fatal<LogHelper>(null, ex);
        }
        #endregion

        private static Type GetCallerType(int start = 1, int maxNum = 100)
        {
            // 至少跳过当前这个
            var st = new StackTrace(0, true);

            var count = st.FrameCount;
            for (var i = start; i < count && maxNum-- > 0; i++)
            {
                var sf = st.GetFrame(i);
                var method = sf.GetMethod();

                // 跳过<>类型的匿名方法
                if (method == null || string.IsNullOrEmpty(method.Name) || method.Name[0] == '<' && method.Name.Contains(">")) continue;

                // 跳过有[DebuggerHidden]特性的方法
                if (method.GetCustomAttribute<DebuggerHiddenAttribute>() != null) continue;

                var type = method.DeclaringType ?? method.ReflectedType;

                // 跳过匿名类获取其父类
                while (type == null || string.IsNullOrEmpty(method.Name) || type.Name[0] == '<' && type.Name.Contains(">"))
                    type = type.DeclaringType ?? type.ReflectedType;

                // 跳过自动生成的中间类和运行时内部类
                if (type == null || type == typeof(LogHelper) || type.Name.Contains('`') || type.Assembly.ManifestModule?.ScopeName == "CommonLanguageRuntimeLibrary")
                    continue;

                return type;
            }
            return typeof(LogHelper);
        }
    }
}
