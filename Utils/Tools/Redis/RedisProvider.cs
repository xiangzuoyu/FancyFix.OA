using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.Tools.Redis
{
    public sealed class RedisProvider
    {
        //private static ConnectionMultiplexer writeRedis = null;
        //private static ConnectionMultiplexer readRedis = null;

        //private static ConnectionMultiplexer sessionWriteRedis = null;
        //private static ConnectionMultiplexer sessionReadRedis = null;
        /// <summary>
        /// 延时加载主
        /// </summary>
        private static Lazy<ConnectionMultiplexer> lazyMaster = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(System.Configuration.ConfigurationManager.AppSettings["RedisWriteConn"].ToString());
        });

        /// <summary>
        /// 延时加载从
        /// </summary>
        private static Lazy<ConnectionMultiplexer> lazyRedis = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(System.Configuration.ConfigurationManager.AppSettings["RedisReadConn"].ToString());
        });

        /// <summary>
        /// 分布式session延时加载主
        /// </summary>
        private static Lazy<ConnectionMultiplexer> lazySessionWrite = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(System.Configuration.ConfigurationManager.AppSettings["RedisWriteConn"].ToString());
        });

        /// <summary>
        /// 分布式session延时加载从
        /// </summary>
        private static Lazy<ConnectionMultiplexer> lazySessionRead = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(System.Configuration.ConfigurationManager.AppSettings["RedisReadConn"].ToString());
        });
        /// <summary>
        /// 主写
        /// </summary>
        public static ConnectionMultiplexer writeRedis
        {
            get
            {
                return lazyMaster.Value;
            }
        }

        /// <summary>
        /// 从读
        /// </summary>
        public static ConnectionMultiplexer readRedis
        {
            get
            {
                return lazyRedis.Value;
            }
        }

        /// <summary>
        /// session主写
        /// </summary>
        public static ConnectionMultiplexer sessionWriteRedis
        {
            get
            {
                return lazySessionWrite.Value;
            }
        }

        /// <summary>
        /// session从读
        /// </summary>
        public static ConnectionMultiplexer sessionReadRedis
        {
            get
            {
                return lazySessionRead.Value;
            }
        }

        /// <summary>
        /// 前缀
        /// </summary>
        private static string Prefix = "";

        //private static int _db;


        static RedisProvider()
        {
            try
            {
                //主redis负责写
                //writeRedis = ConnectionMultiplexer.Connect(Top.Core.Configs.AppSettings["MasterRedis"]);
                //从redis负责读
                //readRedis = ConnectionMultiplexer.Connect(Top.Core.Configs.AppSettings["SlaveRedis"]);

                Prefix = System.Configuration.ConfigurationManager.AppSettings["RedisPrefix"].ToString();

                //_db = Configs.AppSettings["RedisDB"].To<int>(1);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("初始化Redis配置信息失败 异常{0}", ex.ToString()));
            }
        }

        /// <summary>
        /// 设置自定义配置
        /// </summary>
        /// <param name="master">主-写配置信息</param>
        /// <param name="slave">从-读配置信息</param>
        /// <param name="defaultDb">默认DB</param>
        public static void SetConfig(string master, string slave)
        {
            try
            {
                if (!string.IsNullOrEmpty(master))
                {

                    lazySessionWrite = new Lazy<ConnectionMultiplexer>(() =>
                    {
                        return ConnectionMultiplexer.Connect(System.Configuration.ConfigurationManager.AppSettings[master].ToString());
                    });
                }

                if (!string.IsNullOrEmpty(slave))
                {

                    lazySessionRead = new Lazy<ConnectionMultiplexer>(() =>
                    {
                        return ConnectionMultiplexer.Connect(System.Configuration.ConfigurationManager.AppSettings[slave].ToString());
                    });
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("设置Redis配置信息失败 异常{0}", ex.ToString()));
            }
        }

        /// <summary>
        /// 设置key前缀
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        private static string SetPrefix(string key, string prefix = "")
        {
            if (prefix != "")
                Prefix = prefix;
            if (!string.IsNullOrEmpty(Prefix))
            {
                key = Prefix + ":" + key;
            }
            return key;
        }

        /// <summary>
        /// 设置key前缀
        /// </summary>
        /// <param name="keys">keys</param>
        /// <returns></returns>
        private static string[] SetPrefix(string[] keys)
        {
            if (keys.Length > 0)
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    keys = keys.Select(d => Prefix + ":" + d).ToArray();
                }
            }
            return keys;
        }

        #region common

        /// <summary>
        /// 检查Redis连接是否正常
        /// </summary>
        /// <param name="db"></param>
        /// <param name="key"></param>
        public static async void CheckConnection(IDatabase db, RedisKey key)
        {
            // Workaround for StackExchange.Redis/issues/61 that sometimes Redis connection is not connected in ConnectionRestored event
            while (!db.IsConnected(key))
            {
                await Task.Delay(200);
            }
        }

        #endregion

        #region clear

        public static void Clear(int dbId)
        {
            IDatabase db = writeRedis.GetDatabase(dbId);
            IServer server = writeRedis.GetServer(System.Configuration.ConfigurationManager.AppSettings["MasterRedis"].ToString());

            server.FlushDatabase(dbId);
        }

        #endregion

        #region key

        /// <summary>
        /// 删除指定键
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<bool> KeyDel(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.KeyDeleteAsync(key);
        }

        public static bool KeyDelSync(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return db.KeyDelete(key);
        }

        public static async Task<long> KeyDel(string[] keys, int dbId = 1)
        {
            keys = SetPrefix(keys);

            RedisKey[] rk = keys.Select(d => (RedisKey)d).ToArray();
            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.KeyDeleteAsync(rk);
        }

        /// <summary>
        /// 指定的Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbId"></param>
        /// <returns>侯涛</returns>
        public static bool KeyExistSync(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return db.KeyExists(key);
        }

        /// <summary>
        /// 查找剩余失效时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static long KeyTTL(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            TimeSpan? ts = db.KeyTimeToLive(key);

            if (ts.HasValue)
            {
                return ts.Value.Ticks;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region string


        /// <summary>
        /// 设置string类型键值（异步）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<bool> StringSet(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);
            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.StringSetAsync(key, value);
        }

        /// <summary>
        /// 设置string类型键值(同步)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static bool StringSetSync(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return db.StringSet(key, value);
        }

        /// <summary>
        /// 设置string类型键值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="ts">过期时间</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<bool> StringSet(string key, string value, TimeSpan ts, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.StringSetAsync(key, value, ts);
        }

        /// <summary>
        /// 批量设置键值
        /// </summary>
        /// <param name="dict">键值对字典</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<bool> StringSet(Dictionary<string, string> dict, int dbId = 1)
        {
            Dictionary<RedisKey, RedisValue> values = new Dictionary<RedisKey, RedisValue>();

            foreach (var item in dict)
            {
                values.Add(item.Key, item.Value);
            }

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.StringSetAsync(values.ToArray());
        }

        /// <summary>
        /// 获取string类型值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<string> StringGet(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return await db.StringGetAsync(key);
        }
        /// <summary>
        /// 获取string类型值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static string StringGetSyc(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return db.StringGet(key);
        }
        /// <summary>
        /// String 累加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<long> StringIncr(string key, long value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.StringIncrementAsync(key, value);
        }

        /// <summary>
        /// String 累加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static long StringIncrementSyc(string key, long value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return db.StringIncrement(key, value);
        }
        /// <summary>
        /// String 累减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<long> StringDecr(string key, long value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.StringDecrementAsync(key, value);
        }



        /// <summary>
        /// 获取string类型值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns>add by 侯涛</returns>
        public static string StringGetSync(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return db.StringGet(key);
        }

        public static List<string> StringGetSync(string[] keys, int dbId = 1)
        {
            keys = SetPrefix(keys);

            RedisKey[] rk = keys.Select(d => (RedisKey)d).ToArray();
            IDatabase db = readRedis.GetDatabase(dbId);
            RedisValue[] rv = db.StringGet(rk);
            return rv.ToList().Select(d => (string)d).ToList<string>();
        }

        /// <summary>
        /// 设置string类型键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ts"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static bool StringSetSync(string key, string value, TimeSpan ts, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return db.StringSet(key, value, ts);
        }


        /// <summary>
        /// 获取string类型值
        /// </summary>
        /// <param name="keys">键数组</param>
        /// <param name="dbId">数据库 默认1</param>
        /// <returns></returns>
        public static async Task<List<string>> StringGet(string[] keys, int dbId = 1)
        {
            keys = SetPrefix(keys);

            RedisKey[] rk = keys.Select(d => (RedisKey)d).ToArray();
            IDatabase db = readRedis.GetDatabase(dbId);
            RedisValue[] rv = await db.StringGetAsync(rk);
            return rv.ToList().Select(d => (string)d).ToList<string>();
        }

        /// <summary>
        /// 设置指定偏移量的位值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="offset">偏移量</param>
        /// <param name="bit">位置</param>
        /// <param name="dbId">dbid</param>
        /// <returns></returns>
        public static async Task<bool> StringBitSet(string key, long offset, bool bit, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.StringSetBitAsync(key, offset, bit);
        }

        /// <summary>
        /// 计算值定key中位值为1的数量
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="start">开始位 从0开始</param>
        /// <param name="end">结束位 -1表示最后位</param>
        /// <param name="dbId">dbid</param>
        /// <returns></returns>
        public static async Task<long> StringBitCount(string key, long start = 0, long end = -1, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return await db.StringBitCountAsync(key, start, end);
        }

        /// <summary>
        /// 获取指定key指定偏移量的位值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="offset">偏移量</param>
        /// <param name="dbId">dbid</param>
        /// <returns></returns>
        public static async Task<bool> StringBitGet(string key, long offset, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return await db.StringGetBitAsync(key, offset);
        }


        #endregion

        #region list


        /// <summary>
        /// 设置list类型键值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static long ListLPushSyn(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);
            IDatabase db = writeRedis.GetDatabase(dbId);
            return db.ListLeftPush(key, value);
        }

        /// <summary>
        /// 设置list类型键值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<long> ListLPush(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.ListLeftPushAsync(key, value);
        }


        /// <summary>
        /// 批量设置list类型键值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值数组</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<long> ListLPush(string key, string[] values, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            RedisValue[] rv = values.Select(d => (RedisValue)d).ToArray();
            return await db.ListLeftPushAsync(key, rv);
        }


        /// <summary>
        /// 设置list类型键值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<long> ListRPush(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.ListRightPushAsync(key, value);
        }


        /// <summary>
        /// 批量设置list类型键值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值数组</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<long> ListRPush(string key, string[] values, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            RedisValue[] rv = values.Select(d => (RedisValue)d).ToArray();
            return await db.ListRightPushAsync(key, rv);
        }


        /// <summary>
        /// 移除list中的元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<long> ListRem(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.ListRemoveAsync(key, value);
        }

        public static long ListRemSync(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return db.ListRemove(key, value);
        }

        /// <summary>
        /// 获取list元素
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="start">开始位</param>
        /// <param name="stop">结束位</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<List<string>> ListRange(string key, long start, long stop, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            RedisValue[] rv = await db.ListRangeAsync(key, start, stop);
            return rv.ToStringArray().ToList<string>();
        }

        public static List<string> ListRangeSync(string key, long start, long stop, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            RedisValue[] rv = db.ListRange(key, start, stop);
            return rv.ToStringArray().ToList<string>();
        }

        /// <summary>
        /// 获取list长度
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dbId">数据库 默认0</param>
        /// <returns></returns>
        public static async Task<long> ListLen(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return await db.ListLengthAsync(key);
        }

        public static long ListLenSync(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return db.ListLength(key);
        }


        public static string QueueRightPop(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return db.ListRightPop(key);
        }

        public static async Task<string> ListRightPop(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.ListRightPopAsync(key);
        }

        public static async Task<string> ListLeftPop(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.ListLeftPopAsync(key);
        }

        #endregion

        #region SortedSet

        /// <summary>
        /// SET操作 Add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static bool SortSetAddSyc(string key, string member, double score, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return db.SortedSetAdd(key, member, score);
        }

        /// <summary>
        /// SET操作 增量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static bool SortedSetIncrementSyc(string key, string member, double score, int dbId = 1)
        {
            key = SetPrefix(key);
            try
            {
                IDatabase db = writeRedis.GetDatabase(dbId);
                return db.SortedSetIncrement(key, member, score) > 0;
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                return false;
            }

        }


        /// <summary>
        /// SET操作 Add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static Dictionary<string, object> SortedSetRangeByRankWithScores(string key, long start = 0, long stop = -1, Order order = Order.Ascending, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            SortedSetEntry[] result = db.SortedSetRangeByRankWithScores(key, start, stop, order);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var t in result)
            {
                dic.Add(t.Element, t.Score);
            }
            return dic;
        }


        public static long SortSetLenght(string key, int dbId = 1)
        {
            key = SetPrefix(key);
            IDatabase db = writeRedis.GetDatabase(dbId);
            return db.SortedSetLength(key);
        }

        #endregion

        #region set

        /// <summary>
        /// SET操作 Add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<bool> SetAdd(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.SetAddAsync(key, value);
        }

        public static bool SetAddSync(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return db.SetAdd(key, value);
        }

        public static async Task<long> SetAdd(string key, string[] value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            RedisValue[] rv = value.Select(d => (RedisValue)d).ToArray();
            return await db.SetAddAsync(key, rv);
        }

        /// <summary>
        /// SET操作 Remove
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<bool> SetRemove(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.SetRemoveAsync(key, value);
        }

        public static async Task<long> SetRemove(string key, long[] value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            RedisValue[] rv = value.Select(d => (RedisValue)d).ToArray();
            return await db.SetRemoveAsync(key, rv);
        }

        public static long SetRemoveSync(string key, long[] value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            RedisValue[] rv = value.Select(d => (RedisValue)d).ToArray();
            return db.SetRemove(key, rv);
        }

        /// <summary>
        /// SET操作 Members
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<List<long>> SetMembers(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            RedisValue[] rv = await db.SetMembersAsync(key);
            return rv.ToList().Select(d => (long)d).ToList<long>();
        }

        public static async Task<List<string>> SetMembersStr(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            RedisValue[] rv = await db.SetMembersAsync(key);
            return rv.ToList().Select(d => (string)d).ToList<string>();
        }

        public static List<long> SetMembersSync(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            RedisValue[] rv = db.SetMembers(key);
            return rv.ToList().Select(d => (long)d).ToList<long>();
        }

        /// <summary>
        /// SET操作 SISMEMBER
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<bool> SetContains(string key, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return await db.SetContainsAsync(key, value);
        }

        /// <summary>
        /// SET操作 Len 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<long> SetLen(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return await db.SetLengthAsync(key);
        }

        /// <summary>
        /// SET操作 SInter
        /// </summary>
        /// <param name="keys">集合keys</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<List<string>> SetInter(string[] keys, int dbId = 1)
        {
            keys = SetPrefix(keys);

            RedisKey[] rk = keys.Select(d => (RedisKey)d).ToArray();
            IDatabase db = readRedis.GetDatabase(dbId);
            RedisValue[] rv = await db.SetCombineAsync(SetOperation.Intersect, rk);
            return rv.ToList().Select(d => (string)d).ToList<string>();
        }


        public static async Task<string> SetPop(string keys, int dbId = 1)
        {
            keys = SetPrefix(keys);

            IDatabase db = readRedis.GetDatabase(dbId);
            return await db.SetPopAsync(keys);
        }
        #endregion

        #region hash

        public static async Task<bool> HashSet(string key, string hashField, string value, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = writeRedis.GetDatabase(dbId);
            return await db.HashSetAsync(key, hashField, value);
        }

        public static async Task<string> HashGet(string key, string hashField, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            return await db.HashGetAsync(key, hashField);
        }


        public static async Task<Dictionary<string, string>> HashGetAll(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            Dictionary<string, string> result = new Dictionary<string, string>();
            HashEntry[] hashEntrys = await db.HashGetAllAsync(key);
            foreach (HashEntry item in hashEntrys)
            {
                result.Add(item.Name, item.Value);
            }
            return result;
        }

        public static Dictionary<string, string> HashGetAllSync(string key, int dbId = 1)
        {
            key = SetPrefix(key);

            IDatabase db = readRedis.GetDatabase(dbId);
            Dictionary<string, string> result = new Dictionary<string, string>();
            HashEntry[] hashEntrys = db.HashGetAll(key);
            foreach (HashEntry item in hashEntrys)
            {
                result.Add(item.Name, item.Value);
            }
            return result;
        }

        /// <summary>
        /// 设置Hash结构
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="entity">对象</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task HashSet<T>(string key, T entity, int dbId = 1)
        {
            key = SetPrefix(key);
            IDatabase db = writeRedis.GetDatabase(dbId);

            PropertyInfo[] pi = entity.GetType().GetProperties();
            HashEntry[] he = new HashEntry[pi.Length];
            object objValue = null;

            for (int i = 0; i < pi.Length; i++)
            {
                objValue = pi[i].GetValue(entity) == null ? "" : pi[i].GetValue(entity);

                he[i] = new HashEntry(pi[i].Name, objValue.ToString());
            }

            await db.HashSetAsync(key, he);
        }


        /// <summary>
        /// 设置Hash结构
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="entity">对象</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static void HashSetSync<T>(string key, T entity, int dbId = 1)
        {
            key = SetPrefix(key);
            IDatabase db = writeRedis.GetDatabase(dbId);

            PropertyInfo[] pi = entity.GetType().GetProperties();
            HashEntry[] he = new HashEntry[pi.Length];
            object objValue = null;

            for (int i = 0; i < pi.Length; i++)
            {
                objValue = pi[i].GetValue(entity) == null ? "" : pi[i].GetValue(entity);

                he[i] = new HashEntry(pi[i].Name, objValue.ToString());
            }

            db.HashSet(key, he);
        }



        /// <summary>
        /// 获取Hash结构
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<T> HashGet<T>(string key, int dbId = 1) where T : new()
        {
            IDatabase db = writeRedis.GetDatabase(dbId);
            HashEntry[] he = await db.HashGetAllAsync(key);

            if (he == null || he.Length == 0)
            {
                return default(T);
            }

            T t = new T();

            for (int i = 0; i < he.Length; i++)
            {
                PropertyInfo property = t.GetType().GetProperty(he[i].Name.ToString());

                if (property != null)
                {
                    object value = ChangeRedisType(he[i].Value, property);
                    if (value != null)
                        property.SetValue(t, value);
                    else property.SetValue(t, "");
                }
            }

            return t;
        }

        /// <summary>
        /// 获取Hash结构
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">键数组</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static async Task<List<T>> HashGet<T>(string[] keys, int dbId = 1) where T : new()
        {
            List<T> ls = new List<T>();

            foreach (var key in keys)
            {
                T t = await HashGet<T>(key, dbId);

                if (t != null)
                {
                    ls.Add(t);
                }
            }

            return ls;
        }

        /// <summary>
        /// 获取Hash结构
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static T HGetSync<T>(string key, int dbId = 1) where T : new()
        {
            key = SetPrefix(key);
            IDatabase db = writeRedis.GetDatabase(dbId);
            HashEntry[] he = db.HashGetAll(key);

            if (he == null || he.Length == 0)
            {
                return default(T);
            }

            T t = new T();

            for (int i = 0; i < he.Length; i++)
            {
                PropertyInfo property = t.GetType().GetProperty(he[i].Name.ToString());

                if (property != null)
                {
                    object value = ChangeRedisType(he[i].Value, property);

                    property.SetValue(t, value);
                }
            }

            return t;
        }

        /// <summary>
        /// 获取Hash结构
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keys">键数组</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static List<T> HGetSync<T>(string[] keys, int dbId = 1) where T : new()
        {
            List<T> ls = new List<T>();

            foreach (var key in keys)
            {
                T t = HGetSync<T>(key, dbId);

                if (t != null)
                {
                    ls.Add(t);
                }
            }

            return ls;
        }

        #endregion

        #region tran

        //public static async Task TestTran(string key, string value, int dbId = 1)
        //{
        //    IDatabase db = writeRedis.GetDatabase(dbId);
        //    var p1 = db.StringGetAsync("kye1");
        //    var p2 = db.StringGetAsync("kye2");

        //    db.WaitAll(new Task[] { p1, p2 });


        //}

        #endregion

        #region Pipelining

        public static List<string[]> TestPipe(string praiseKey, string hotKey, string topKey, string talkTopKey, string officalKey, int dbId = 1)
        {
            IDatabase db = readRedis.GetDatabase(dbId);
            var a1 = db.SetMembersAsync(praiseKey);
            var a2 = db.SetMembersAsync(hotKey);
            var a3 = db.ListRangeAsync(topKey, 0, -1);
            var a4 = db.ListRangeAsync(talkTopKey, 0, -1);
            var a5 = db.ListRangeAsync(officalKey, 0, 0);

            RedisValue[] rv1 = db.Wait(a1);
            RedisValue[] rv2 = db.Wait(a2);
            RedisValue[] rv3 = db.Wait(a3);
            RedisValue[] rv4 = db.Wait(a4);
            RedisValue[] rv5 = db.Wait(a5);

            List<string[]> ls = new List<string[]>();
            ls.Add(rv1.ToStringArray());
            ls.Add(rv2.ToStringArray());
            ls.Add(rv3.ToStringArray());
            ls.Add(rv4.ToStringArray());
            ls.Add(rv5.ToStringArray());

            return ls;
        }

        #endregion

        #region 分布式session专用


        public static bool SessionKeyDelSync(string key, string hashField, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionWriteRedis.GetDatabase(dbId);

            // CheckConnection(db, key);

            return db.HashDelete(key, hashField);
        }

        public static bool SessionKeyDelSync(string key, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionWriteRedis.GetDatabase(dbId);

            // CheckConnection(db, key);

            return db.KeyDelete(key);
        }

        public static bool SessionKeyExistSync(string key, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionReadRedis.GetDatabase(dbId);

            //CheckConnection(db, key);

            return db.KeyExists(key);
        }

        public static bool SessionHashSetSync(string key, string hashField, string value, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionWriteRedis.GetDatabase(dbId);

            //CheckConnection(db, key);

            return db.HashSet(key, hashField, value);
        }

        /// <summary>
        /// Hash设置
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dict">字典</param>
        /// <param name="ts">过期时间</param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static bool SessionHashSetSync(string key, Dictionary<string, object> dict, TimeSpan ts, int dbId = 2)
        {
            HashEntry[] hashEntryArray = null;

            int i = 0;
            hashEntryArray = new HashEntry[dict.Count];

            foreach (var item in dict)
            {
                hashEntryArray[i] = new HashEntry(item.Key, Newtonsoft.Json.JsonConvert.SerializeObject(item.Value));
                i++;
            }

            key = SetPrefix(key);

            IDatabase db = sessionWriteRedis.GetDatabase(dbId);

            // CheckConnection(db, key);

            db.HashSet(key, hashEntryArray);

            return db.KeyExpire(key, ts);
        }



        public static string SessionHashGetSync(string key, string hashField, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionReadRedis.GetDatabase(dbId);

            // CheckConnection(db, key);

            return db.HashGet(key, hashField);
        }

        /// <summary>
        /// 获取key数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static long SessionHashGetCount(string key, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionReadRedis.GetDatabase(dbId);

            //CheckConnection(db, key);

            return db.HashLength(key);
        }

        public static bool SessionHashExist(string key, string hashField, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionReadRedis.GetDatabase(dbId);

            // CheckConnection(db, key);

            return db.HashExists(key, hashField);
        }
        #endregion

        #region Cache专用

        /// <summary>
        /// 缓存键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dbId">DBID</param>
        /// <returns></returns>
        public static bool CacheKeyExistSync(string key, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionReadRedis.GetDatabase(dbId);

            return db.KeyExists(key);
        }

        /// <summary>
        /// 设置缓存过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ts"></param>
        /// <param name="dbId"></param>
        /// <returns></returns>
        public static bool CacheKeyExpire(string key, TimeSpan ts, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionWriteRedis.GetDatabase(dbId);
            return db.KeyExpire(key, ts);
        }

        /// <summary>
        /// 缓存HASH结构
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireType">过期类型0不过期 1滑动 2绝对 3滑动或绝对</param>
        /// <param name="slidingExpiredTime">滑动过期时间</param>
        /// <param name="absoluteExpiredTime">绝对过期时间</param>
        /// <param name="elapsedTime">经过时间</param>
        /// <param name="dbId">DBID</param>
        /// <returns></returns>
        public static void CacheHashSet(string key, string value, int expireType, long slidingExpiredTime = 0, long absoluteExpiredTime = 0, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionWriteRedis.GetDatabase(dbId);

            HashEntry[] he = new HashEntry[5];
            he[0] = new HashEntry("Key", value);
            he[1] = new HashEntry("ExpireType", expireType);
            he[2] = new HashEntry("SlidingExpiredTime", slidingExpiredTime);
            he[3] = new HashEntry("AbsoluteExpiredTime", absoluteExpiredTime);
            he[4] = new HashEntry("ElapsedExpiredTime", 0);

            long ts = 0;

            if (expireType == 1)
            {
                ts = slidingExpiredTime;
            }
            else if (expireType == 2)
            {
                ts = absoluteExpiredTime;
            }
            else if (expireType == 3)
            {
                //过期时间设最小时间
                ts = Math.Min(slidingExpiredTime, absoluteExpiredTime);
            }

            db.HashSet(key, he);

            if (expireType != 0)
            {
                CacheKeyExpire(key, new TimeSpan(ts));
            }
        }

        /// <summary>
        /// 不存在则设置键
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="dbId">DBID</param>
        /// <returns></returns>
        public static bool CacheHashSetWhenNotExists(string key, string value, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionWriteRedis.GetDatabase(dbId);
            return db.HashSet(key, "Key", value, When.NotExists);
        }

        /// <summary>
        /// 设置Hash Field
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="dbId">DBID</param>
        /// <returns></returns>
        public static bool CacheHashSetField(string key, string hashField, string value, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionWriteRedis.GetDatabase(dbId);

            return db.HashSet(key, hashField, value);
        }

        /// <summary>
        /// 获取Hash Field
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="hashField">缓存值</param>
        /// <param name="dbId">DBID</param>
        /// <returns></returns>
        public static string CacheHashGetField(string key, string hashField, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionReadRedis.GetDatabase(dbId);

            return db.HashGet(key, hashField);
        }

        /// <summary>
        /// 根据Hash Keys获取values
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="hashFields"></param>
        /// <param name="dbId">DBID</param>
        /// <returns></returns>
        public static List<string> CacheHashGet(string[] keys, int dbId = 2)
        {
            keys = SetPrefix(keys);

            List<string> ls = new List<string>();

            for (int i = 0; i < keys.Length; i++)
            {
                string value = CacheHashGetField(keys[i], "Key");

                ls.Add(value);
            }

            return ls;
        }


        /// <summary>
        /// 键的剩余有效时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="dbId">DBID</param>
        /// <returns></returns>
        public static long CacheKeyTTL(string key, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionReadRedis.GetDatabase(dbId);
            TimeSpan? ts = db.KeyTimeToLive(key);

            if (ts.HasValue)
            {
                return ts.Value.Ticks;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="dbId">DBID</param>
        /// <returns></returns>
        public static bool CacheKeyDel(string key, int dbId = 2)
        {
            key = SetPrefix(key);

            IDatabase db = sessionWriteRedis.GetDatabase(dbId);
            return db.KeyDelete(key);
        }

        #endregion

        #region Pub/Sub

        /// <summary>
        /// 订阅频道
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="handler">回调</param>
        public static void Subscribe(string channel, Action<RedisChannel, RedisValue> handler)
        {
            ISubscriber sub = writeRedis.GetSubscriber();

            sub.SubscribeAsync(channel, handler);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static async Task<long> Publish(string channel, string message)
        {
            ISubscriber sub = writeRedis.GetSubscriber();
            return await sub.PublishAsync(channel, message);

        }

        #endregion

        #region 内部方法

        private static object ChangeRedisType(RedisValue redisValue, PropertyInfo property)
        {
            object value = null;

            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                value = Convert.ToInt32(string.IsNullOrEmpty(redisValue) ? "0" : redisValue.ToString());
            }
            else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
            {
                value = Convert.ToInt64(string.IsNullOrEmpty(redisValue) ? "0" : redisValue.ToString());
            }
            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {
                value = Convert.ToDecimal(string.IsNullOrEmpty(redisValue) ? "0" : redisValue.ToString());
            }
            else if (property.PropertyType == typeof(byte) || property.PropertyType == typeof(byte?))
            {
                value = Convert.ToByte(string.IsNullOrEmpty(redisValue) ? "0" : redisValue.ToString());
            }
            else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
            {
                value = Convert.ToDouble(string.IsNullOrEmpty(redisValue) ? "0" : redisValue.ToString());
            }
            else if (property.PropertyType == typeof(char) || property.PropertyType == typeof(char?))
            {
                value = Convert.ToChar(string.IsNullOrEmpty(redisValue) ? "0" : redisValue.ToString());
            }
            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                value = Convert.ToDateTime(string.IsNullOrEmpty(redisValue) ? DateTime.MinValue.ToString() : redisValue.ToString());
            }
            else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                value = Convert.ToBoolean(string.IsNullOrEmpty(redisValue) ? false.ToString() : redisValue.ToString());
            }
            else
            {
                value = redisValue.ToString();
            }

            return value;
        }

        #endregion
    }
}
