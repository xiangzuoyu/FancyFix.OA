using Dos.Common;
using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dos.DataAccess.Base
{
    public abstract class Repository<T> where T : Entity//: IRepository<T> 
    {
        protected static string tableName { get; }

        static Repository()
        {
            tableName = typeof(T).GetCustomAttribute<Table>().GetTableName();

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("缺少tableName，请传入模型");

            Db.Context.RegisterSqlLogger(delegate (string sql)
            {
                //在此可以记录sql日志
                //写日志会影响性能，建议开发版本记录sql以便调试，发布正式版本不要记录
                //if (System.Configuration.ConfigurationManager.AppSettings["build"] != null && bool.Parse(System.Configuration.ConfigurationManager.AppSettings["build"].ToString()) == false)
#if Debug
                    LogHelper.Debug(sql, "SQL日志");
#endif
            });
        }

        #region 查询
        /// <summary>
        /// 获取整表数据
        /// </summary>
        /// <returns></returns>
        public static List<T> GetAll()
        {
            return Db.Context.From<T>().ToList();
        }

        public static List<T> QueryNoWhere(Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc", Expression<Func<T, object>> select = null, int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.Context.From<T>();
            if (select != null)
                fs = fs.Select(select);

            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    fs.OrderBy(orderBy);
                }
                else
                {
                    fs.OrderByDescending(orderBy);
                }
            }
            return fs.ToList();
        }
        /// <summary>
        /// 通用查询    
        /// </summary>
        public static List<T> Query(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc", Expression<Func<T, object>> select = null, int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.Context.From<T>().Where(where);
            if (select != null)
                fs = fs.Select(select);

            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    fs.OrderBy(orderBy);
                }
                else
                {
                    fs.OrderByDescending(orderBy);
                }
            }
            return fs.ToList();
        }


        /// <summary>
        /// 通用查询
        /// </summary>
        public static List<T> Query(Where<T> where, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc", int? top = null, int? pageSize = null, int? pageIndex = null, Expression<Func<T, object>> select = null)
        {
            var fs = Db.Context.From<T>().Where(where);
            if (select != null)
                fs = fs.Select(select);
            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    fs.OrderBy(orderBy);
                }
                else
                {
                    fs.OrderByDescending(orderBy);
                }
            }
            return fs.ToList();
        }

        /// <summary>
        /// 通用查询
        /// </summary>
        public static List<T> Query(Where<T> where, OrderByClip orderBy = null, string ascOrDesc = "asc", int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.Context.From<T>().Where(where);
            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                fs.OrderBy(orderBy);
            }
            return fs.ToList();
        }
        /// <summary>
        /// 通用查询
        /// </summary>
        public static T First(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc", int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.Context.From<T>().Where(where);
            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    return fs.OrderBy(orderBy).First();
                }
                return fs.OrderByDescending(orderBy).First();
            }
            var model = fs.First();
            return model;
        }
        /// <summary>
        /// 通用查询
        /// </summary>
        public static T First(Where<T> where, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc", int? top = null, int? pageSize = null, int? pageIndex = null)
        {
            var fs = Db.Context.From<T>().Where(where);
            if (top != null)
            {
                fs.Top(top.Value);
            }
            else if (pageIndex != null && pageSize != null)
            {
                fs.Page(pageSize.Value, pageIndex.Value);
            }
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    return fs.OrderBy(orderBy).First();
                }
                return fs.OrderByDescending(orderBy).First();
            }
            return fs.First();
        }

        public static T FirstSelect(Expression<Func<T, bool>> where, Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc")
        {
            var fs = Db.Context.From<T>().Where(where).Select(select);
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    return fs.OrderBy(orderBy).First();
                }
                return fs.OrderByDescending(orderBy).First();
            }
            return fs.First();
        }

        public static T FirstSelect(Where<T> where, Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy = null, string ascOrDesc = "asc")
        {
            var fs = Db.Context.From<T>().Where(where).Select(select);
            if (orderBy != null)
            {
                if (ascOrDesc.ToLower() == "asc")
                {
                    return fs.OrderBy(orderBy).First();
                }
                return fs.OrderByDescending(orderBy).First();
            }
            return fs.First();
        }
        /// <summary>
        /// 根据条件判断是否存在数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool Any(Expression<Func<T, bool>> where)
        {
            return Db.Context.Exists<T>(where);
        }
        /// <summary>
        /// 取总数
        /// </summary>
        public static int Count(Expression<Func<T, bool>> where)
        {
            return Db.Context.From<T>().Where(where).Count();
        }
        /// <summary>
        /// 取总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int Count(Where<T> where)
        {
            return Db.Context.From<T>().Where(where).Count();
        }
        #endregion
        #region 插入
        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int Insert(T entity)
        {
            return Db.Context.Insert<T>(entity);
        }
        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static void Insert(DbTrans context, T entity)
        {
            Db.Context.Insert<T>(context, entity);
            //context.Set<T>().Add(entity);
        }
        /// <summary>
        /// 插入多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static int Insert(IEnumerable<T> entities)
        {
            return Db.Context.Insert<T>(entities);
        }
        public static void Insert(DbTrans context, IEnumerable<T> entities)
        {
            Db.Context.Insert<T>(context, entities.ToArray());
        }
        #endregion
        #region 更新
        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int Update(T entity)
        {
            return Db.Context.Update(entity);
        }
        /// <summary>
        /// 更新单个实体
        /// </summary>
        public static int Update(T entity, Where where)
        {
            return Db.Context.Update(entity, where);
        }
        /// <summary>
        /// 更新单个实体
        /// </summary>
        public static int Update(T entity, Expression<Func<T, bool>> lambdaWhere)
        {
            return Db.Context.Update(entity, lambdaWhere);
        }
        public static void Update(DbTrans context, T entity)
        {
            Db.Context.Update(context, entity);
        }
        /// <summary>
        /// 更新多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static int Update(IEnumerable<T> entities)
        {
            var enumerable = entities as T[] ?? entities.ToArray();
            Db.Context.Update(enumerable.ToArray());
            return 1;
        }
        public static void Update(DbTrans context, IEnumerable<T> entities)
        {
            Db.Context.Update(context, entities.ToArray());
        }
        #endregion
        #region 删除
        /// <summary>
        /// 删除单个实体
        /// </summary>
        public static int Delete(T entitie)
        {
            return Db.Context.Delete<T>(entitie);
        }
        /// <summary>
        /// 删除多个实体
        /// </summary>
        public static int Delete(IEnumerable<T> entities)
        {
            return Db.Context.Delete<T>(entities);
        }
        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete(Guid? id)
        {
            if (id == null)
            {
                return 0;
            }
            return Db.Context.Delete<T>(id.Value);
        }
        /// <summary>
        /// 删除单个实体
        /// </summary>
        public static int Delete(Expression<Func<T, bool>> where)
        {
            return Db.Context.Delete<T>(where);
        }
        /// <summary>
        /// 删除单个实体
        /// </summary>
        public static int Delete(Where<T> where)
        {
            return Db.Context.Delete<T>(where.ToWhereClip());
        }
        #endregion

        #region 拓展
        public static IEnumerable<T> GetSelectList(int top, string cols, string where, string orderBy)
        {
            string selectCols = "*";
            if (cols != "")
                selectCols = cols;

            string topStr = "";
            if (top > 0)
                topStr = "top " + top.ToString();
            string whereStr = "";
            if (where != "")
                whereStr += "where " + where;
            string orderByStr = "";
            if (orderBy != "")
                orderByStr = "order by " + orderBy;

            var sql = string.Format("select {0} {1} from {2} {3} {4}", topStr, selectCols, tableName, whereStr, orderByStr);

            IEnumerable<T> dataList = Db.Context.FromSql(sql).ToList<T>();
            return dataList;
        }

        public static IEnumerable<T> GetSelectList2(int top, string tabs, string col, string where, string orderby, string groupBy = "")
        {
            string selectTop = "";
            if (top < 0) { top = 0; }
            if (top > 0) { selectTop = " top " + top.ToString(); }
            string groupByStr = "";
            if (string.IsNullOrEmpty(col))
            {
                col = "*";
            }
            if (!string.IsNullOrEmpty(where))
            {
                where = " where " + where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                orderby = " order by " + orderby;
            }
            if (!string.IsNullOrEmpty(groupBy))
            {
                groupByStr = " group by " + groupBy;
            }
            string sql = string.Format("select {3} {0} from {4} {1} {5} {2}", col, where, orderby, selectTop, tabs, groupByStr);
            IEnumerable<T> dataList = Db.Context.FromSql(sql).ToList<T>();
            return dataList;
        }

        /// <summary>
        /// 根据指定字段为条件优先级下移，排序字段：Sequence
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="byId">排序字段值</param>
        /// <param name="byName">排序字段名</param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static bool SequenceDown(int moveId, int byId, string byName, int step)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Table_SequenceDown")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("ById", System.Data.DbType.Int32, byId)
                 .AddInParameter("ByName", System.Data.DbType.String, byName)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 根据指定字段为条件优先级下移，排序字段：Sequence
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="byId"></param>
        /// <param name="byName"></param>
        /// <param name="step"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool SequenceDown(int moveId, int byId, string byName, int step, int departId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Table_SequenceDownByDepart")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("ById", System.Data.DbType.Int32, byId)
                 .AddInParameter("ByName", System.Data.DbType.String, byName)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInParameter("DepartId", System.Data.DbType.Int32, departId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 根据指定字段为条件优先级上移，排序字段：Sequence
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="byId">排序字段值</param>
        /// <param name="byName">排序字段名</param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static bool SequenceUp(int moveId, int byId, string byName, int step)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Table_SequenceUp")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("ById", System.Data.DbType.Int32, byId)
                 .AddInParameter("ByName", System.Data.DbType.String, byName)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 根据指定字段为条件优先级上移，排序字段：Sequence
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="byId"></param>
        /// <param name="byName"></param>
        /// <param name="step"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool SequenceUp(int moveId, int byId, string byName, int step, int departId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Table_SequenceUpByDepart")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("ById", System.Data.DbType.Int32, byId)
                 .AddInParameter("ByName", System.Data.DbType.String, byName)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInParameter("DepartId", System.Data.DbType.Int32, departId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 根据自定义排序字段优先级下移
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="columnName">排序字段名</param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static bool SequenceDownSeqByColumn(int moveId, string columnName, int step)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Table_SequenceDownByColumn")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("ByName", System.Data.DbType.String, columnName)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 根据自定义排序字段优先级下移
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="columnName"></param>
        /// <param name="step"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool SequenceDownSeqByColumn(int moveId, string columnName, int step, int departId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Table_SequenceDownByColumnAndDepart")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("ByName", System.Data.DbType.String, columnName)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInParameter("DepartId", System.Data.DbType.Int32, departId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 根据自定义排序字段优先级上移
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="columnName">排序字段名</param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static bool SequenceUpSeqByColumn(int moveId, string columnName, int step)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Table_SequenceUpByColumn")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("ByName", System.Data.DbType.String, columnName)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 根据自定义排序字段优先级上移
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="columnName"></param>
        /// <param name="step"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool SequenceUpSeqByColumn(int moveId, string columnName, int step, int departId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Table_SequenceUpByColumnAndDepart")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("ByName", System.Data.DbType.String, columnName)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInParameter("DepartId", System.Data.DbType.Int32, departId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 通用分类优先级上移
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static bool Up(int moveId, int step)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassUp")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 通用分类优先级上移
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="step"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool Up(int moveId, int step, int departId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassUpByDepart")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInParameter("DepartId", System.Data.DbType.Int32, departId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 通用分类优先级下移
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static bool Down(int moveId, int step)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassDown")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 通用分类优先级下移
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="step"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool Down(int moveId, int step, int departId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassDownByDepart")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("Step", System.Data.DbType.Int32, step)
                 .AddInParameter("DepartId", System.Data.DbType.Int32, departId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 上移一级分类
        /// </summary>
        /// <param name="moveId"></param>
        /// <returns></returns>
        public static bool Prev(int moveId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassPrew")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 上移一级分类
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool Prev(int moveId, int departId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassPrewByDepart")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("DepartId", System.Data.DbType.Int32, departId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 下移一级分类
        /// </summary>
        /// <param name="moveId"></param>
        /// <returns></returns>
        public static bool Next(int moveId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassNext")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 下移一级分类
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public static bool Next(int moveId, int departId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassNextByDepart")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("MoveId", System.Data.DbType.Int32, moveId)
                 .AddInParameter("DepartId", System.Data.DbType.Int32, departId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// <summary>
        /// 根据Id，更新指定字段
        /// </summary>
        /// <param name="setField"></param>
        /// <param name="setValue"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool UpdateBitField(string setField, bool setValue, int id)
        {
            return Db.Context.FromSql($"update {tableName} set {setField}={(setValue ? 1 : 0)} where Id={id}").ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 根据自定义唯一条件，更新指定字段
        /// </summary>
        /// <param name="setField"></param>
        /// <param name="setValue"></param>
        /// <param name="indexName"></param>
        /// <param name="indexValue"></param>
        /// <returns></returns>
        public static bool UpdateBitField(string setField, bool setValue, string indexName, int indexValue)
        {
            return Db.Context.FromSql($"update {tableName} set {setField}={(setValue ? 1 : 0)} where {indexName}={indexValue}").ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 根据条件，更新指定字段
        /// </summary>
        /// <param name="setField"></param>
        /// <param name="setValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool UpdateBitField(string setField, bool setValue, string where)
        {
            if (string.IsNullOrEmpty(where))
                throw new ArgumentNullException();
            return Db.Context.FromSql($"update {tableName} set {setField}={(setValue ? 1 : 0)} where {where}").ExecuteNonQuery() > 0;
        }

        #endregion

        #region 存储过程分页

        /// <summary>
        /// 分页1 wsd_page_1： 根据唯一字段唯一值按大小排序，如ID 
        /// </summary>
        /// <param name="tb">表名</param>
        /// <param name="collist">要查询出的字段列表,*表示全部字段,注意必须包含排序字段</param>
        /// <param name="condition">查询条件 ,不带where</param>
        /// <param name="col">排序列 例：ID</param>
        /// <param name="coltype">列的类型,0-数字类型,1-字符类型</param>
        /// <param name="orderby">--排序,FALSE-顺序,TRUE-倒序</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="page">当前页</param>
        /// <param name="records">总记录数：为0则计算总记录数</param>
        /// <returns>分页记录</returns>
        public static DataSet GetPageList1(string tb, string collist, string condition, string col, int coltype, bool orderby, int pagesize, int page, ref int records)
        {
            string flag = "records";
            var proc = Db.Context.FromProc("Sys_Page1")
                 .AddInParameter("tb", DbType.String, tb)
                 .AddInParameter("collist", DbType.String, collist)
                 .AddInParameter("condition", DbType.String, condition)
                 .AddInParameter("col", DbType.String, col)
                 .AddInParameter("coltype", DbType.Byte, coltype)
                 .AddInParameter("orderby", DbType.Boolean, orderby)
                 .AddInParameter("pagesize", DbType.Int32, pagesize)
                 .AddInParameter("page", DbType.Int32, page)
                 .AddInputOutputParameter(flag, DbType.Int32, records);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                records = int.Parse(dic[flag].ToString());
            return proc.ToDataSet();
        }

        /// <summary>
        ///  分页2 wsd_page_2：单表任意排序 
        /// </summary>
        /// <param name="tb">表名  例: news</param>
        /// <param name="collist">要查询出的字段列表,*表示全部字段</param>
        /// <param name="where">查询条件，不带where 例：classid = 2</param>
        /// <param name="orderby">排序条件 例：order by tuijian desc,id desc</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="page">当前页码</param>
        /// <param name="records">总记录数：为0则重新计算</param>
        /// <returns>分页记录</returns>
        public static DataSet GetPageList2(string tb, string collist, string where, string orderby, int pagesize, int page, ref int records)
        {
            string flag = "records";
            var proc = Db.Context.FromProc("Sys_Page2")
                 .AddInParameter("tb", DbType.String, tb)
                 .AddInParameter("collist", DbType.String, collist)
                 .AddInParameter("where", DbType.String, where)
                 .AddInParameter("orderby", DbType.Boolean, orderby)
                 .AddInParameter("pagesize", DbType.Int32, pagesize)
                 .AddInParameter("page", DbType.Int32, page)
                 .AddInputOutputParameter(flag, DbType.Int32, records);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                records = int.Parse(dic[flag].ToString());
            return proc.ToDataSet();
        }

        /// <summary>
        /// 分页3： 单表/多表通用分页存储过程 wsd_page_3
        /// </summary>
        /// <param name="tb">表名 例： table1 inner join table2 on table1.xx=table2.xx </param>
        /// <param name="collist">需要获取字段 例: tabl1.xx,table2.*,注意，需要把排序列都选上</param>
        /// <param name="where">条件,不带where</param>
        /// <param name="orderby">最内层orderby(需要带上表前缀，注意asc 必须写上) 例: order by table1.xxx desc,table2.ad asc "</param>
        /// <param name="orderbyo">最外城orderby xxx.desc,ad asc</param>        
        /// <param name="pagesize">每页条数</param>
        /// <param name="page">页数</param>
        /// <param name="records">记录条数</param>
        /// <returns></returns>

        public static DataSet GetPageList3(string tb, string collist, string where, string orderby, string orderbyo, int pagesize, int page, ref int records)
        {
            string flag = "records";
            var proc = Db.Context.FromProc("Sys_Page3")
                 .AddInParameter("tb", DbType.String, tb)
                 .AddInParameter("collist", DbType.String, collist)
                 .AddInParameter("where", DbType.String, where)
                 .AddInParameter("orderby", DbType.Boolean, orderby)
                 .AddInParameter("orderbyo", DbType.Boolean, orderbyo)
                 .AddInParameter("pagesize", DbType.Int32, pagesize)
                 .AddInParameter("page", DbType.Int32, page)
                 .AddInputOutputParameter(flag, DbType.Int32, records);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                records = int.Parse(dic[flag].ToString());
            return proc.ToDataSet();
        }

        /// <summary>
        /// 分页4. 单表/多表通用分页存储过程 wsd_page_4 使用RowNumber
        /// </summary>
        /// <param name="tb">表名 例： table1 inner join table2 on table1.xx=table2.xx</param>
        /// <param name="collist">需要获取字段 例: tabl1.xx,table2.*,注意，需要把排序列都选上</param>
        /// <param name="where">条件,不带where</param>
        /// <param name="orderby">最内层orderby  例: order by table1.xxx desc,table2.ad asc "</param>
        /// <param name="orderbyo">最外城orderby xxx.desc,ad asc</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="page">页数</param>
        /// <param name="records">记录条数</param>
        /// <returns></returns>
        public static DataSet GetPageList4(string tb, string collist, string where, string orderby, string orderbyo, int pagesize, int page, ref int records)
        {
            string flag = "records";
            var proc = Db.Context.FromProc("Sys_Page4")
                 .AddInParameter("tb", DbType.String, tb)
                 .AddInParameter("collist", DbType.String, collist)
                 .AddInParameter("where", DbType.String, where)
                 .AddInParameter("orderby", DbType.Boolean, orderby)
                 .AddInParameter("orderby1", DbType.Boolean, orderbyo)
                 .AddInParameter("pagesize", DbType.Int32, pagesize)
                 .AddInParameter("page", DbType.Int32, page)
                 .AddInputOutputParameter(flag, DbType.Int32, records);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                records = int.Parse(dic[flag].ToString());
            return proc.ToDataSet();
        }
        #endregion
    }
}

