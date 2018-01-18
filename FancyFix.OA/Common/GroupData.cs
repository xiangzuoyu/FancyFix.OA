using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FancyFix.OA
{
    /// <summary>
    /// 岗位数据预加载
    /// </summary>
    public class GroupData
    {
        private static object locker = new object();

        private static List<Mng_PermissionGroup> groupList = null;

        public GroupData()
        {
            Load();
        }

        public static Mng_PermissionGroup GetModel(int id)
        {
            return groupList.Find(o => o.Id == id);
        }

        public static List<Mng_PermissionGroup> GetList()
        {
            return groupList;
        }

        public static void Load()
        {
            lock (locker)
            {
                if (groupList == null)
                    groupList = Bll.BllMng_PermissionGroup.GetAll();
            }
        }

        public static void Reload()
        {
            lock (locker)
            {
                groupList = Bll.BllMng_PermissionGroup.GetAll();
            }
        }
    }
}