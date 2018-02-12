using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FancyFix.OA
{
    /// <summary>
    /// 部门数据预加载
    /// </summary>
    public class AdminData
    {
        private static object locker = new object();

        private static List<Mng_User> adminlist = null;

        public AdminData()
        {
            Load();
        }

        public static Mng_User GetModel(int id)
        {
            return adminlist.Find(o => o.Id == id);
        }

        public static List<Mng_User> GetList()
        {
            return adminlist;
        }

        public static List<Mng_User> GetList(Func<Mng_User, bool> predicate)
        {
            return adminlist.Where(predicate).ToList();
        }

        public static void Load()
        {
            lock (locker)
            {
                if (adminlist == null)
                    adminlist = Bll.BllMng_User.GetAll();
            }
        }

        public static void Reload()
        {
            lock (locker)
            {
                adminlist = Bll.BllMng_User.GetAll();
            }
        }
    }
}