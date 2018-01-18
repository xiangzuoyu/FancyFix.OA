using Dos.DataAccess.Base;
using FancyFix.OA.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FancyFix.OA.Bll
{
    public class BllConfig_Process : ServiceBase<Config_Process>
    {
        public static BllConfig_Process Instance()
        {
            return new BllConfig_Process();
        }

        public enum ProcessType
        {
            [Description("价值观")]
            Valuable = 1,
            [Description("Kpi")]
            Kpi = 2
        }

        /// <summary>
        /// 获取年份列表
        /// </summary>
        /// <param name="processType"></param>
        /// <returns></returns>
        public static List<int?> GetYears(ProcessType processType)
        {
            return Db.Context.From<Config_Process>().Where(o => o.Type == (int)processType).OrderBy(o => o.Year).ToList().Select(o => o.Year).ToList();
        }

        /// <summary>
        /// 获取年进程
        /// </summary>
        /// <param name="processType"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static List<int> GetProcess(ProcessType processType, int year)
        {
            List<int> list = new List<int>();
            var process = Db.Context.From<Config_Process>().Where(o => o.Type == (int)processType && o.Year == year).ToFirst();
            if (process != null && !string.IsNullOrWhiteSpace(process.Process))
            {
                var array = process.Process.TrimEnd(',').Split(',');
                if (array != null && array.Length > 0)
                {
                    int month = 0;
                    foreach (var m in array)
                        if (int.TryParse(m, out month))
                            list.Add(month);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取价值观年进程
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static List<int> GetValuableProcess(int year)
        {
            return GetProcess(ProcessType.Valuable, year);
        }

        /// <summary>
        /// 获取价值观年份
        /// </summary>
        /// <returns></returns>
        public static List<int?> GetValuableYears()
        {
            return GetYears(ProcessType.Valuable);
        }

    }
}
