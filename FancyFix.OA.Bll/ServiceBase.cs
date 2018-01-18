using Dos.DataAccess.Base;
using Dos.ORM;
using System.Text.RegularExpressions;

namespace FancyFix.OA.Bll
{
    public abstract class ServiceBase<T> : Repository<T> where T : Entity
    {
        /// <summary>
        /// 检测关键词，防止SQL注入
        /// </summary>
        /// <param name="sqlValue">需要检测的关键词</param>
        /// <returns></returns>
        public static string CheckSqlValue(string sqlValue)
        {
            string reStr = sqlValue;
            if (reStr == null)
            {
                reStr = "";
            }
            reStr = reStr.Replace("'", "''");
            return reStr;
        }

        /// <summary>
        /// 检测Like字符串,防止注入和关键词匹配(替换 '_ % )
        /// </summary>
        /// <param name="keyword">需要检测的关键词</param>
        /// <returns></returns>
        public static string CheckSqlKeyword(string keyword)
        {
            string reStr = keyword;
            if (reStr == null)
            {
                reStr = "";
            }
            reStr = reStr.Replace("'", "''");
            reStr = reStr.Replace("[", "[[]");
            reStr = reStr.Replace("%", "[%]");
            reStr = reStr.Replace("_", "[_]");
            return reStr;
        }

        /// <summary>
        /// 检测数据库字段名或表名
        /// </summary>
        /// <param name="fieldName">要检测的字段名或表名</param>
        /// <returns></returns>
        public static bool CheckSqlField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(fieldName, @"^[a-zA-Z0-9_\.\,]+$");
            }
        }

        /// <summary>
        /// 检测多ID连接SQL输入字符 ( 4,2,3,27)
        /// </summary>
        /// <param name="arrayInt">输入字符</param>
        /// <returns>格式正确返回true ,错误返回false</returns>
        public static bool CheckSqlValueByArrayInt(string arrayInt)
        {
            if (string.IsNullOrEmpty(arrayInt))
            {
                return false;
            }
            else
            {
                string[] arrayIntStr = arrayInt.Split(',');
                foreach (string aryInt in arrayIntStr)
                {
                    if (!Tools.Common.StringCheck.IsIntegerByPositive(aryInt.Trim()))
                    {
                        return false;
                    }

                }
                return true;
            }
        }
    }
}
