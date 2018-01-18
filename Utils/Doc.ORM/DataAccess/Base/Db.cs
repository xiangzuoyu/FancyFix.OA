using Dos.Common;
using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dos.DataAccess.Base
{

    public abstract class Db
    {
        public static string ConnConfig = "ConnectionString";
        public static readonly DbSession Context = new DbSession(ConnConfig);
        //public static readonly DbSession Context = new DbSession("MySqlConn");
        //public static readonly DbSession Context = new DbSession("SqliteConn");
        //public static readonly DbSession Context = new DbSession("AccessConn");
        //public static readonly DbSession Context = new DbSession("OracleConn");
        //public static readonly DbSession Context = new DbSession("PostgreSqlConn");
        static Db()
        {
            Context.RegisterSqlLogger(delegate (string sql)
            {
                //在此可以记录sql日志
                //写日志会影响性能，建议开发版本记录sql以便调试，发布正式版本不要记录
                //if (System.Configuration.ConfigurationManager.AppSettings["build"] != null && bool.Parse(System.Configuration.ConfigurationManager.AppSettings["build"].ToString()) == false)
#if Debug
                LogHelper.Debug(sql, "SQL日志");
#endif
            });
        }
    }
}
