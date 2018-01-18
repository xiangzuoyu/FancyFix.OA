using System;
using System.Web;

namespace FancyFix.Tools.Redis.Session
{
    public class RedisSession
    {
        private HttpContext context;

        /// <summary>
        /// 实例化类
        /// 默认过期120分钟
        /// </summary>
        /// <param name="context"></param>
        /// <param name="IsReadOnly"></param>
        /// <param name="Timeout">单位（分）</param>
        public RedisSession(HttpContext context, bool IsReadOnly, int Timeout = 120)
        {
            this.context = context;
            this.IsReadOnly = IsReadOnly;
            this.Timeout = Timeout;

            TimeSpan ts = new TimeSpan((DateTime.Now.AddMinutes(Timeout) - DateTime.Now).Ticks);
            //更新缓存过期时间
            RedisProvider.CacheKeyExpire(SessionID, ts);
        }

        /// <summary>
        /// SessionId标识符
        /// </summary>
        public static string SessionName = "Redis_SessionId";

        //
        // 摘要:
        //     获取会话状态集合中的项数。
        //
        // 返回结果:
        //     集合中的项数。
        public long Count
        {
            get
            {
                return RedisProvider.SessionHashGetCount(SessionID);
            }
        }

        //
        // 摘要:
        //     获取一个值，该值指示会话是否为只读。
        //
        // 返回结果:
        //     如果会话为只读，则为 true；否则为 false。
        public bool IsReadOnly { get; set; }

        //
        // 摘要:
        //     获取会话的唯一标识符。
        //
        // 返回结果:
        //     唯一会话标识符。
        public string SessionID
        {
            get
            {
                return GetSessionID();
            }
        }

        //
        // 摘要:
        //     获取并设置在会话状态提供程序终止会话之前各请求之间所允许的时间（以分钟为单位）。
        //
        // 返回结果:
        //     超时期限（以分钟为单位）。
        public int Timeout { get; set; }

        /// <summary>
        /// 获取SessionID
        /// </summary>
        /// <param name="key">SessionId标识符</param>
        /// <returns>HttpCookie值</returns>
        private string GetSessionID()
        {
            HttpCookie cookie = context.Request.Cookies.Get(SessionName);
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                string newSessionID = Guid.NewGuid().ToString();
                HttpCookie newCookie = new HttpCookie(SessionName, newSessionID);
                newCookie.HttpOnly = IsReadOnly;
                newCookie.Expires = DateTime.Now.AddDays(15);
                context.Response.Cookies.Add(newCookie);
                return "Session_" + newSessionID;
            }
            else
            {
                return "Session_" + cookie.Value;
            }
        }

        //
        // 摘要:
        //     按名称获取或设置会话值。
        //
        // 参数:
        //   name:
        //     会话值的键名。
        //
        // 返回结果:
        //     具有指定名称的会话状态值；如果该项不存在，则为 null。
        public object this[string name]
        {
            get
            {
                return RedisProvider.SessionHashGetSync(SessionID, name);
            }
            set
            {
                RedisProvider.SessionHashSetSync(SessionID, name, value.ToString());
            }
        }

        // 摘要:
        //     判断会话中是否存在指定key
        //
        // 参数:
        //   name:
        //     键值
        //
        public bool IsExistKey(string name)
        {
            return RedisProvider.SessionHashExist(SessionID, name);
            //RedisBase.Hash_Exist<object>(SessionID, name);
        }

        //
        // 摘要:
        //     向会话状态集合添加一个新项。
        //
        // 参数:
        //   name:
        //     要添加到会话状态集合的项的名称。
        //
        //   value:
        //     要添加到会话状态集合的项的值。
        public void Add(string name, object value)
        {
            RedisProvider.SessionHashSetSync(SessionID, name, value.ToString());
        }

        //
        // 摘要:
        //     从会话状态集合中移除所有的键和值。
        public void Clear()
        {
            RedisProvider.SessionKeyDelSync(SessionID);
        }

        //
        // 摘要:
        //     删除会话状态集合中的项。
        //
        // 参数:
        //   name:
        //     要从会话状态集合中删除的项的名称。
        public void Remove(string name)
        {
            RedisProvider.SessionKeyDelSync(SessionID, name);
        }
        //
        // 摘要:
        //     从会话状态集合中移除所有的键和值。
        public void RemoveAll()
        {
            Clear();
        }
    }
}
