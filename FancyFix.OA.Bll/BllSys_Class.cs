using Dos.DataAccess.Base;
using Dos.ORM;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Collections;
using FancyFix.OA.Model;
using System.Data.SqlClient;

namespace FancyFix.OA.Bll
{
    public class BllSys_Class<T> : ServiceBase<T> where T : Entity
    {

        List<Sys_Class> syslist = null;

        /// <summary>
        /// 实例化一个当前逻辑操作对象
        /// </summary>
        /// <returns></returns>
        public static BllSys_Class<T> Instance()
        {
            return new BllSys_Class<T>();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        public List<Sys_Class> GetAllSys()
        {
            if (syslist == null)
                syslist = Mapping(GetAll());
            return syslist;
        }

        /// <summary>
        /// 获取默认分类
        /// </summary>
        /// <returns></returns>
        public virtual int GetDefaultId()
        {
            return Db.Context.FromSql(string.Format("select Top 1 Id from {0} where BeLock = cast(0 as bit) order by Sequence", tableName)).ToScalar()?.ToString().ToInt32() ?? 0;
        }

        public virtual int GetDefaultId(int departId)
        {
            return Db.Context.FromSql(string.Format("select Top 1 Id from {0} where BeLock = cast(0 as bit) and DepartId={1} order by Sequence", tableName, departId)).ToScalar()?.ToString().ToInt32() ?? 0;
        }

        /// <summary>
        /// 获取所有子类包括本身的ID组 (3,4,5,6...)
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="includeLock"></param>
        /// <returns></returns>
        public virtual string GetIdsByParentId(int parentId, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetListByParentId(parentId, includeLock));
            string val = parentId.ToString();
            foreach (Sys_Class model in list)
            {
                val += "," + model.Id.ToString();
            }
            return val;
        }

        public virtual string GetIdsByParentId(int parentId, int departId, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetListByParentId(parentId, departId, includeLock));
            string val = parentId.ToString();
            foreach (Sys_Class model in list)
            {
                val += "," + model.Id.ToString();
            }
            return val;
        }

        /// <summary>
        /// 分类菜单显示
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="SelectedValue">选中的值</param>
        /// <param name="includeLock">是否包含锁定项</param>
        /// <returns></returns>
        public virtual string ShowClassOld(int parentId, int SelectedValue, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetListByParentId(parentId, includeLock));
            StringBuilder strShowClass = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", list[i].Id.ToString());
                    if (SelectedValue == list[i].Id)
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.AppendFormat("attr=\"{0}\"", ShowClassPath(list[i].Id));
                    strShowClass.AppendFormat("child=\"{0}\"", list[i].ChildNum);
                    strShowClass.Append(" >");
                    if (list[i].Depth == 0)
                    {
                        strShowClass.Append("├");
                    }
                    else
                    {
                        for (int j = 0; j < list[i].Depth; j++)
                        {
                            strShowClass.Append(" │");
                        }
                        strShowClass.Append(" ├");
                    }
                    strShowClass.Append(list[i].ClassName.ToString());
                    strShowClass.Append(" </option>");
                }
            }
            return strShowClass.ToString();
        }

        /// <summary>
        /// 分类菜单显示
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="SelectedValue">选中的值</param>
        /// <param name="includeLock">是否包含锁定项</param>
        /// <returns></returns>
        public virtual string ShowClass(int parentId, int SelectedValue, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetListByParentId(parentId, includeLock));
            StringBuilder strShowClass = new StringBuilder();

            GetListByParId(list, parentId, strShowClass, SelectedValue);

            return strShowClass.ToString();
        }

        public virtual string ShowClass(int parentId, int SelectedValue, int departId, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetListByParentId(parentId, departId, includeLock));
            StringBuilder strShowClass = new StringBuilder();

            GetListByParId(list, parentId, departId, strShowClass, SelectedValue);

            return strShowClass.ToString();
        }

        /// <summary>
        /// 递归遍历
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parId"></param>
        /// <param name="strShowClass"></param>
        /// <param name="SelectedValue"></param>
        private void GetListByParId(IEnumerable<Sys_Class> list, int parId, StringBuilder strShowClass, int SelectedValue)
        {
            IEnumerable<Sys_Class> childlist = list.Where(o => o.ParId == parId);
            if (childlist != null && childlist.Count() > 0)
            {
                foreach (var item in childlist)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", item.Id.ToString());
                    if (SelectedValue == item.Id)
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.AppendFormat("attr=\"{0}\"", ShowClassPath(item.Id));
                    strShowClass.AppendFormat("child=\"{0}\"", item.ChildNum);
                    strShowClass.Append(" >");

                    if (item.Depth == 0)
                    {
                        strShowClass.Append("├");
                    }
                    else
                    {
                        for (int j = 0; j < item.Depth; j++)
                        {
                            strShowClass.Append(" │");
                        }
                        strShowClass.Append(" ├");
                    }
                    strShowClass.Append(item.ClassName.ToString());
                    strShowClass.Append(" </option>");

                    GetListByParId(list, item.Id, strShowClass, SelectedValue);
                }
            }
        }

        private void GetListByParId(IEnumerable<Sys_Class> list, int parId, int departId, StringBuilder strShowClass, int SelectedValue)
        {
            IEnumerable<Sys_Class> childlist = list.Where(o => o.ParId == parId && o.DepartId == departId);
            if (childlist != null && childlist.Count() > 0)
            {
                foreach (var item in childlist)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", item.Id.ToString());
                    if (SelectedValue == item.Id)
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.AppendFormat("attr=\"{0}\"", ShowClassPath(item.Id, departId));
                    strShowClass.AppendFormat("child=\"{0}\"", item.ChildNum);
                    strShowClass.Append(" >");

                    if (item.Depth == 0)
                    {
                        strShowClass.Append("├");
                    }
                    else
                    {
                        for (int j = 0; j < item.Depth; j++)
                        {
                            strShowClass.Append(" │");
                        }
                        strShowClass.Append(" ├");
                    }
                    strShowClass.Append(item.ClassName.ToString());
                    strShowClass.Append(" </option>");

                    GetListByParId(list, item.Id, departId, strShowClass, SelectedValue);
                }
            }
        }

        /// <summary>
        /// 分类菜单显示
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="SelectedValue">选中的值</param>
        /// <param name="includeLock">是否包含锁定项</param>
        /// <returns></returns>
        public virtual string ShowClassPath(int parentId, string selectPath, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetListByParentId(parentId, includeLock));
            StringBuilder strShowClass = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", list[i].ParPath.ToString());
                    if (selectPath == list[i].ParPath.ToString())
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.Append(" >");
                    if (list[i].Depth == 0)
                    {
                        strShowClass.Append("├");
                    }
                    else
                    {
                        for (int j = 0; j < list[i].Depth; j++)
                        {
                            strShowClass.Append(" │");
                        }
                        strShowClass.Append(" ├");
                    }
                    strShowClass.Append(list[i].ClassName.ToString());
                    strShowClass.Append(" </option>");
                }
            }
            return strShowClass.ToString();
        }

        public virtual string ShowClassPath(int parentId, string selectPath, int departId, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetListByParentId(parentId, departId, includeLock));
            StringBuilder strShowClass = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", list[i].ParPath.ToString());
                    if (selectPath == list[i].ParPath.ToString())
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.Append(" >");
                    if (list[i].Depth == 0)
                    {
                        strShowClass.Append("├");
                    }
                    else
                    {
                        for (int j = 0; j < list[i].Depth; j++)
                        {
                            strShowClass.Append(" │");
                        }
                        strShowClass.Append(" ├");
                    }
                    strShowClass.Append(list[i].ClassName.ToString());
                    strShowClass.Append(" </option>");
                }
            }
            return strShowClass.ToString();
        }

        /// <summary>
        /// 设置是否菜单项 ,如果本来是，则设置否，如果本来否，则设置是
        /// </summary>
        /// <param name="infoId">需要设置的信息ID</param>
        /// <returns></returns>
        public virtual bool SetBeLock(int infoId)
        {
            Sys_Class model = GetModel(infoId);
            if (model.BeLock.HasValue && model.BeLock.Value)
            {
                return UpdateBitField("belock", false, infoId);
            }
            else
            {
                return UpdateBitField("belock", true, infoId);
            }
        }

        /// <summary>
        /// 根据底层的ClassID 获取Class路径
        /// </summary>
        /// <param name="childClassId"></param>
        /// <param name="urlFormat"></param>
        /// <returns></returns>
        public virtual string ShowClassPath(int childClassId)
        {
            string path = "";
            Sys_Class mod_Sys_Class = GetModelByCache(childClassId);
            if (mod_Sys_Class == null)
            {
                return "";
            }
            else
            {
                path = string.Format("{0}", mod_Sys_Class.ClassName);
                if (mod_Sys_Class.ParId > 0)
                {
                    path = ShowClassPath((int)mod_Sys_Class.ParId) + " > " + path;
                }
                else if (mod_Sys_Class.ParId == 0)
                {
                    path = "<span>" + path + "</span>";
                }
            }
            return path;
        }

        public virtual string ShowClassPath(int childClassId, int departId)
        {
            string path = "";
            Sys_Class mod_Sys_Class = GetModelByCache(childClassId, departId);
            if (mod_Sys_Class == null)
            {
                return "";
            }
            else
            {
                path = string.Format("{0}", mod_Sys_Class.ClassName);
                if (mod_Sys_Class.ParId > 0)
                {
                    path = ShowClassPath((int)mod_Sys_Class.ParId, departId) + " > " + path;
                }
                else if (mod_Sys_Class.ParId == 0)
                {
                    path = "<span>" + path + "</span>";
                }
            }
            return path;
        }

        /// <summary>
        /// 根据底层的ClassID 获取Class路径（路由模版）
        /// </summary>
        /// <param name="childClassId"></param>
        /// <param name="urlFormat"></param>
        /// <returns></returns>
        public virtual string ShowClassPath(int childClassId, string urlFormat)
        {
            string path = "";
            Sys_Class model = GetModelByCache(childClassId);
            if (model == null)
            {
                return "";
            }
            else
            {
                path = string.Format(urlFormat, model.Id.ToString(), model.ClassName);
                if (model.ParId > 0)
                {
                    path = ShowClassPath((int)model.ParId, urlFormat) + path;
                }
            }
            return path;
        }

        public virtual string ShowClassPath(int childClassId, int departId, string urlFormat)
        {
            string path = "";
            Sys_Class model = GetModelByCache(childClassId, departId);
            if (model == null)
            {
                return "";
            }
            else
            {
                path = string.Format(urlFormat, model.Id.ToString(), model.ClassName);
                if (model.ParId > 0)
                {
                    path = ShowClassPath((int)model.ParId, departId, urlFormat) + path;
                }
            }
            return path;
        }

        /// <summary>
        /// 根据底层的ClassID 获取Class路径
        /// </summary>
        /// <param name="childClassId"></param>
        /// <param name="urlFormat"></param>
        /// <returns></returns>
        public virtual string ShowClassPath2(int childClassId)
        {
            string path = "";
            Sys_Class mod_Sys_Class = GetModelByCache(childClassId);
            if (mod_Sys_Class == null)
            {
                return "";
            }
            else
            {
                path = mod_Sys_Class.ClassName;
                if (mod_Sys_Class.ParId > 0)
                {
                    path += "###" + ShowClassPath((int)mod_Sys_Class.ParId);
                }
            }
            return path;
        }

        public virtual string ShowClassPath2(int childClassId, int departId)
        {
            string path = "";
            Sys_Class mod_Sys_Class = GetModelByCache(childClassId, departId);
            if (mod_Sys_Class == null)
            {
                return "";
            }
            else
            {
                path = mod_Sys_Class.ClassName;
                if (mod_Sys_Class.ParId > 0)
                {
                    path += "###" + ShowClassPath((int)mod_Sys_Class.ParId, departId);
                }
            }
            return path;
        }

        /// <summary>
        /// 根据父ID获取一级子类列表
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="includeLock">是否包含锁定项</param>
        /// <returns>一级子类列表</returns>
        public virtual IEnumerable<T> GetChildrenList(int parentId, bool includeLock)
        {
            string where = string.Format("parId = {0}", parentId.ToString());
            if (!includeLock)
            {
                where += " and belock = cast(0 as bit)";
            }
            try
            {
                return GetSelectList(0, "*", where, "sequence");
            }
            catch
            {
                return null;
            }
        }

        public virtual IEnumerable<T> GetChildrenList(int parentId, int departId, bool includeLock)
        {
            string where = string.Format("parId = {0} and departId={1}", parentId.ToString(), departId);
            if (!includeLock)
            {
                where += " and belock = cast(0 as bit)";
            }
            try
            {
                return GetSelectList(0, "*", where, "sequence");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取子集分类
        /// </summary>
        /// <param name="parId"></param>
        /// <param name="SelectedValue"></param>
        /// <param name="includeLock"></param>
        /// <returns></returns>
        public virtual string ShowClassByParId(int parId, int SelectedValue, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetChildrenList(parId, includeLock));
            StringBuilder strShowClass = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", list[i].Id.ToString());
                    if (SelectedValue == list[i].Id)
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.AppendFormat("attr=\"{0}\"", ShowClassPath(list[i].Id));
                    strShowClass.AppendFormat("child=\"{0}\"", list[i].ChildNum);
                    strShowClass.Append(" >");
                    if (list[i].Depth == 0)
                    {
                        strShowClass.Append("├");
                    }
                    else
                    {
                        for (int j = 0; j < list[i].Depth; j++)
                        {
                            strShowClass.Append(" │");
                        }
                        strShowClass.Append(" ├");
                    }
                    strShowClass.Append(list[i].ClassName.ToString());
                    strShowClass.Append(" </option>");
                }
            }
            return strShowClass.ToString();
        }

        public virtual string ShowClassByParId(int parId, int SelectedValue, int departId, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetChildrenList(parId, departId, includeLock));
            StringBuilder strShowClass = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", list[i].Id.ToString());
                    if (SelectedValue == list[i].Id)
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.AppendFormat("attr=\"{0}\"", ShowClassPath(list[i].Id, departId));
                    strShowClass.AppendFormat("child=\"{0}\"", list[i].ChildNum);
                    strShowClass.Append(" >");
                    if (list[i].Depth == 0)
                    {
                        strShowClass.Append("├");
                    }
                    else
                    {
                        for (int j = 0; j < list[i].Depth; j++)
                        {
                            strShowClass.Append(" │");
                        }
                        strShowClass.Append(" ├");
                    }
                    strShowClass.Append(list[i].ClassName.ToString());
                    strShowClass.Append(" </option>");
                }
            }
            return strShowClass.ToString();
        }

        /// <summary>
        /// 根据父ID获取一级子类列表
        /// </summary>
        /// <param name="top">列表数</param>
        /// <param name="parentId">父ID</param>
        /// <param name="includeLock">是否包含锁定项</param>
        /// <returns>一级子类列表</returns>
        public virtual IEnumerable<T> GetChildrenList(int top, int parentId, int thisid, bool includeLock)
        {
            string where = string.Format("parId = {0}", parentId.ToString());
            if (!includeLock)
            {
                where += " and belock = cast(0 as bit) and id<>" + thisid;
            }
            try
            {
                return GetSelectList(top, "*", where, "sequence");
            }
            catch
            {
                return null;
            }
        }

        public virtual IEnumerable<T> GetChildrenList(int top, int parentId, int thisid, int departId, bool includeLock)
        {
            string where = string.Format("parId = {0} and departId={1}", parentId.ToString(), departId);
            if (!includeLock)
            {
                where += " and belock = cast(0 as bit) and id<>" + thisid;
            }
            try
            {
                return GetSelectList(top, "*", where, "sequence");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据ID获取所有子类包括其本身的分类
        /// </summary>
        /// <param name="parentId">父类ID</param>
        /// <param name="includeLock">是否包含锁定项s</param>
        /// <returns>下属分类列表</returns>
        public virtual IEnumerable<T> GetListByParentId(int parentId, bool includeLock)
        {
            string where = "1=1";
            if (!includeLock)
            {
                where += " and belock = cast(0 as bit)";
            }
            if (parentId > 0)
            {
                //先获取父类信息;
                Sys_Class modParentClassInfo = GetModel(parentId);
                if (modParentClassInfo == null)
                {
                    return null;
                }
                where += string.Format(" and  parpath like '{0}%'", modParentClassInfo.ParPath);
            }
            return GetSelectList(0, "*", where, "sequence");
        }

        public virtual IEnumerable<T> GetListByParentId(int parentId, int departId, bool includeLock)
        {
            string where = "departId=" + departId;
            if (!includeLock)
            {
                where += " and belock = cast(0 as bit)";
            }
            if (parentId > 0)
            {
                //先获取父类信息;
                Sys_Class modParentClassInfo = GetModel(parentId, departId);
                if (modParentClassInfo == null)
                {
                    return null;
                }
                where += string.Format(" and  parpath like '{0}%'", modParentClassInfo.ParPath);
            }
            return GetSelectList(0, "*", where, "sequence");
        }

        /// <summary>
        /// 增加分类
        /// </summary>
        /// <param name="sys_ManageClass"></param>
        /// <returns></returns>
        public virtual int Add(T model)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            else
            {
                Sys_Class sysModel = Mapping(model);
                string flag = "Flag";
                var proc = Db.Context.FromProc(sysModel.DepartId > 0 ? "Sys_ClassAddByDepart" : "Sys_ClassAdd")
                     .AddInParameter("TableName", System.Data.DbType.String, tableName)
                     .AddInParameter("ClassName", System.Data.DbType.String, sysModel.ClassName)
                     .AddInParameter("ParId", System.Data.DbType.Int32, sysModel.ParId)
                     .AddInputOutputParameter(flag, System.Data.DbType.Int32, 0);
                if (sysModel.DepartId > 0)
                    proc.AddInParameter("DepartId", System.Data.DbType.Int32, sysModel.DepartId);
                proc.ExecuteNonQuery();
                var dic = proc.GetReturnValues();
                if (dic.ContainsKey(flag))
                    return dic[flag].ToString().ToInt32();
                return 0;
            }
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(int id)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassDel")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("DelId", System.Data.DbType.String, id)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        public virtual bool Delete(int id, int departId)
        {
            string flag = "Flag";
            var proc = Db.Context.FromProc("Sys_ClassDelByDepart")
                 .AddInParameter("TableName", System.Data.DbType.String, tableName)
                 .AddInParameter("DelId", System.Data.DbType.String, id)
                 .AddInParameter("DepartId", System.Data.DbType.Int32, departId)
                 .AddInputOutputParameter(flag, System.Data.DbType.Boolean, false);
            proc.ExecuteNonQuery();
            var dic = proc.GetReturnValues();
            if (dic.ContainsKey(flag))
                return bool.Parse(dic[flag].ToString());
            return false;
        }

        /// 上移分类
        /// </summary>
        /// <param name="MoveId">需要上移的分类ID</param>
        /// <param name="Step">上移量</param>
        /// <returns>是否执行成功</returns>
        public new bool Up(int MoveId, int Step)
        {
            return ServiceBase<T>.Up(MoveId, Step);
        }

        public new bool Up(int MoveId, int Step, int departId)
        {
            return ServiceBase<T>.Up(MoveId, Step, departId);
        }

        /// 下移分类
        /// </summary>
        /// <param name="MoveId">需要下移的分类ID</param>
        /// <param name="Step">下移量</param>
        /// <returns>是否执行成功</returns>
        public new bool Down(int MoveId, int Step)
        {
            return ServiceBase<T>.Down(MoveId, Step);
        }

        public new bool Down(int MoveId, int Step, int departId)
        {
            return ServiceBase<T>.Down(MoveId, Step, departId);
        }

        /// <summary>
        /// 上移一级分类
        /// </summary>
        /// <param name="MoveId">移动Id</param>
        /// <returns></returns>
        public new bool Prev(int MoveId)
        {
            return ServiceBase<T>.Prev(MoveId);
        }

        public new bool Prev(int MoveId, int departId)
        {
            return ServiceBase<T>.Prev(MoveId, departId);
        }

        /// <summary>
        /// 下移一级分类
        /// </summary>
        /// <param name="MoveId">移动Id</param>
        /// <returns></returns>
        public new bool Next(int MoveId)
        {
            return ServiceBase<T>.Next(MoveId);
        }

        public new bool Next(int MoveId, int departId)
        {
            return ServiceBase<T>.Next(MoveId, departId);
        }

        /// <summary>
        /// 获取指定ID的ClassName
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetClassName(int id)
        {
            return GetModel(id, "ClassName")?.ClassName ?? "";
        }

        /// <summary>
        /// 获取指定ID的ClassParPath
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetClassParPath(int id)
        {
            return GetModel(id, "ParPath")?.ParPath ?? "";
        }

        /// <summary>
        /// 获取指定分类路径导航
        /// </summary>
        /// <param name="id">分类Id</param>
        /// <param name="formatStr">如：<li><a href="/product/list?id={0}">{1}</a></li> </param>
        /// <returns></returns>
        public virtual string GetClassNavigation(int id, string formatStr)
        {
            string strClass = string.Empty;
            string parpath = GetClassParPath(id);
            if (parpath != "")
            {
                parpath += "," + id.ToString();
                var list = Db.Context.FromSql($"select Id,ParId,ClassName from {tableName} where Id in ({parpath}) order by Sequence").ToList<Sys_Class>();
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        strClass += string.Format(formatStr, item.Id, item.ClassName);
                    }
                }
            }
            return strClass;
        }

        /// <summary>
        /// 获取指定ID的ClassName
        /// </summary>
        /// <param name="ids">完整分类路径</param>
        /// <returns></returns>
        public string GetClassName(string ids)
        {
            List<Sys_Class> list = Mapping(GetSelectList(0, "ClassName", "id in(" + ids.Trim(',') + ")", "Depth"));
            if (list == null || list.Count == 0)
                return "";
            string result = "";
            foreach (Sys_Class model in list)
            {
                result += model.ClassName.ToString() + ">";
            }
            return result.Trim('>');
        }

        /// <summary>
        /// 分类菜单显示
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="SelectedValue">选中的值</param>
        /// <param name="includeLock">是否包含锁定项</param>
        /// <returns></returns>
        public virtual string ShowClass2(int parentId, int SelectedValue, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetListByParentId(parentId, includeLock));
            StringBuilder strShowClass = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\"", list[i].Id.ToString());
                    if (SelectedValue == list[i].Id)
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.Append(" >");

                    strShowClass.Append(list[i].ClassName.ToString());
                    strShowClass.Append(" </option>");
                }
            }
            return strShowClass.ToString();
        }

        public virtual string ShowClass2(int parentId, int SelectedValue, int departId, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetListByParentId(parentId, departId, includeLock));
            StringBuilder strShowClass = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\"", list[i].Id.ToString());
                    if (SelectedValue == list[i].Id)
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.Append(" >");

                    strShowClass.Append(list[i].ClassName.ToString());
                    strShowClass.Append(" </option>");
                }
            }
            return strShowClass.ToString();
        }

        /// <summary>
        /// 分类菜单显示
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="SelectedValue">选中的值</param>
        /// <param name="includeLock">是否包含锁定项</param>
        /// <returns></returns>
        public virtual string ShowClass3(int parentId, int SelectedValue, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetChildrenList(parentId, includeLock));
            System.Text.StringBuilder strShowClass = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", list[i].Id.ToString());
                    if (SelectedValue == list[i].Id)
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.AppendFormat("attr=\"{0}\"", ShowClassPath(list[i].Id));
                    strShowClass.AppendFormat("child=\"{0}\"", list[i].ChildNum);
                    strShowClass.Append(" >");
                    if (list[i].Depth == 0)
                    {
                        strShowClass.Append("├");
                    }
                    strShowClass.Append(list[i].ClassName.ToString());
                    strShowClass.Append(" </option>");
                }
            }
            return strShowClass.ToString();
        }

        public virtual string ShowClass3(int parentId, int SelectedValue, int departId, bool includeLock)
        {
            List<Sys_Class> list = Mapping(GetChildrenList(parentId, departId, includeLock));
            System.Text.StringBuilder strShowClass = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", list[i].Id.ToString());
                    if (SelectedValue == list[i].Id)
                    {
                        strShowClass.Append(" selected ");
                    }
                    strShowClass.AppendFormat("attr=\"{0}\"", ShowClassPath(list[i].Id, departId));
                    strShowClass.AppendFormat("child=\"{0}\"", list[i].ChildNum);
                    strShowClass.Append(" >");
                    if (list[i].Depth == 0)
                    {
                        strShowClass.Append("├");
                    }
                    strShowClass.Append(list[i].ClassName.ToString());
                    strShowClass.Append(" </option>");
                }
            }
            return strShowClass.ToString();
        }

        /// <summary>
        /// 获取最大排序号
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public virtual int GetMaxSequence(string strWhere)
        {
            string where = strWhere;
            if (!string.IsNullOrEmpty(strWhere))
            {
                where = " where " + strWhere;
            }
            var maxId = Db.Context.FromSql(string.Format("select Max(sequence) as maxid from {0} {1}", tableName, where)).ToScalar();
            if (maxId != null && maxId != DBNull.Value)
                return (int)maxId;
            return 0;
        }

        /// <summary>
        /// 获取最新排序号
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public virtual int GetNextSequence(string strWhere)
        {
            string where = strWhere;
            if (!string.IsNullOrEmpty(strWhere))
            {
                where = " where " + strWhere;
            }
            var maxId = Db.Context.FromSql(string.Format("select Max(sequence) as maxid from {0} {1}", tableName, where)).ToScalar();
            if (maxId != null && maxId != DBNull.Value)
                return (int)maxId + 1;
            return 1;
        }


        /// <summary>
        /// 批量更新次序
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sequence">测序号</param>
        /// <param name="isAdd">真为加1，假为减1</param>
        public void UpdateSequence(int sequence, bool isAdd)
        {
            if (isAdd)
            {
                Db.Context.FromSql(string.Format("update {0} set sequence = sequence + 1 where Sequence > {1} ", tableName, sequence)).ExecuteNonQuery();
            }
            else
            {
                Db.Context.FromSql(string.Format("update {0} set sequence = sequence - 1 where Sequence > {1} ", tableName, sequence)).ExecuteNonQuery();
            }
        }

        public void UpdateSequence(int sequence, int departId, bool isAdd)
        {
            if (isAdd)
            {
                Db.Context.FromSql(string.Format("update {0} set sequence = sequence + 1 where Sequence > {1} and departId={2} ", tableName, sequence, departId)).ExecuteNonQuery();
            }
            else
            {
                Db.Context.FromSql(string.Format("update {0} set sequence = sequence - 1 where Sequence > {1} and departId={2} ", tableName, sequence, departId)).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T First(int id)
        {
            return Db.Context.FromSql("select * from " + tableName + " where Id=@id")
                .AddInParameter("@id", System.Data.DbType.Int32, id)
                .ToFirst<T>();
        }

        public T First(int id, int departId)
        {
            return Db.Context.FromSql("select * from " + tableName + " where Id=@id and DepartId=@departId")
                .AddInParameter("@id", System.Data.DbType.Int32, id)
                .AddInParameter("@departId", System.Data.DbType.Int32, departId)
                .ToFirst<T>();
        }

        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public T First(int id, string cols)
        {
            if (cols == "") cols = "*";
            return Db.Context.FromSql("select @cols from " + tableName + " where Id=@id")
                .AddInParameter("@cols", System.Data.DbType.String, cols)
                .AddInParameter("@id", System.Data.DbType.Int32, id)
                .ToFirst<T>();
        }

        public T First(int id, int departId, string cols)
        {
            if (cols == "") cols = "*";
            return Db.Context.FromSql("select @cols from " + tableName + " where Id=@id and DepartId=@departId")
                .AddInParameter("@cols", System.Data.DbType.String, cols)
                .AddInParameter("@id", System.Data.DbType.Int32, id)
                .AddInParameter("@departId", System.Data.DbType.Int32, departId)
                .ToFirst<T>();
        }

        /// <summary>
        /// 从缓存列表从获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Sys_Class GetModelByCache(System.Int32 id)
        {
            return GetAllSys().FirstOrDefault(o => o.Id == id);
        }

        public virtual Sys_Class GetModelByCache(System.Int32 id, int departId)
        {
            return GetAllSys().FirstOrDefault(o => o.Id == id && o.DepartId == departId);
        }

        /// <summary>
        /// 根据Id取得一条记录
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>无记录或出错返回 null，有记录返回一个Model</returns>
        public virtual Sys_Class GetModel(System.Int32 id)
        {
            string sql = string.Format("select * from {0} where Id={1}", tableName, id);
            using (SqlDataReader reader = (SqlDataReader)Db.Context.FromSql(sql).ToDataReader())
            {
                Sys_Class model = null;
                try
                {
                    if (reader.Read())
                    {
                        model = new Sys_Class();

                        if (reader["Id"] != System.DBNull.Value) { model.Id = System.Int32.Parse(reader["Id"].ToString()); }
                        if (reader["ParId"] != System.DBNull.Value) { model.ParId = System.Int32.Parse(reader["ParId"].ToString()); }
                        model.ClassName = reader["ClassName"].ToString();
                        if (reader["Sequence"] != System.DBNull.Value) { model.Sequence = System.Int32.Parse(reader["Sequence"].ToString()); }
                        if (reader["Depth"] != System.DBNull.Value) { model.Depth = System.Int32.Parse(reader["Depth"].ToString()); }
                        if (reader["ChildNum"] != System.DBNull.Value) { model.ChildNum = System.Int32.Parse(reader["ChildNum"].ToString()); }
                        model.ParPath = reader["ParPath"].ToString();
                        if (reader["BeLock"] != System.DBNull.Value) { model.BeLock = System.Boolean.Parse(reader["BeLock"].ToString()); }
                    }
                }
                catch (Exception ex)
                {
                    Tools.Tool.LogHelper.WriteLog(this.GetType(), ex, 0, "");
                    return null;
                }
                return model;
            }
        }

        public virtual Sys_Class GetModel(System.Int32 id, int departId)
        {
            string sql = string.Format("select * from {0} where Id={1} and DepartId={2}", tableName, id, departId);
            using (SqlDataReader reader = (SqlDataReader)Db.Context.FromSql(sql).ToDataReader())
            {
                Sys_Class model = null;
                try
                {
                    if (reader.Read())
                    {
                        model = new Sys_Class();

                        if (reader["Id"] != System.DBNull.Value) { model.Id = System.Int32.Parse(reader["Id"].ToString()); }
                        if (reader["ParId"] != System.DBNull.Value) { model.ParId = System.Int32.Parse(reader["ParId"].ToString()); }
                        model.ClassName = reader["ClassName"].ToString();
                        if (reader["Sequence"] != System.DBNull.Value) { model.Sequence = System.Int32.Parse(reader["Sequence"].ToString()); }
                        if (reader["Depth"] != System.DBNull.Value) { model.Depth = System.Int32.Parse(reader["Depth"].ToString()); }
                        if (reader["ChildNum"] != System.DBNull.Value) { model.ChildNum = System.Int32.Parse(reader["ChildNum"].ToString()); }
                        model.ParPath = reader["ParPath"].ToString();
                        if (reader["BeLock"] != System.DBNull.Value) { model.BeLock = System.Boolean.Parse(reader["BeLock"].ToString()); }
                        if (reader["DepartId"] != System.DBNull.Value) { model.DepartId = System.Int32.Parse(reader["DepartId"].ToString()); }
                    }
                }
                catch (Exception ex)
                {
                    Tools.Tool.LogHelper.WriteLog(this.GetType(), ex, 0, "");
                    return null;
                }
                return model;
            }
        }

        /// <summary>
        /// 根据Id取得一条记录
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>无记录或出错返回 null，有记录返回一个Model</returns>
        public virtual Sys_Class GetModel(System.Int32 id, string cols)
        {
            cols = cols.ToLower();
            string sql = string.Format("select {2} from {0} where Id={1}", tableName, id, cols);
            cols = "," + cols + ",";
            using (SqlDataReader reader = (SqlDataReader)Db.Context.FromSql(sql).ToDataReader())
            {
                Sys_Class model = null;
                try
                {
                    if (reader.Read())
                    {
                        model = new Sys_Class();
                        if (cols.IndexOf(",id,".ToLower()) >= 0)
                        {
                            if (reader["Id"] != System.DBNull.Value) { model.Id = System.Int32.Parse(reader["Id"].ToString()); }
                        }
                        if (cols.IndexOf(",parid,".ToLower()) >= 0)
                        {
                            if (reader["ParId"] != System.DBNull.Value) { model.ParId = System.Int32.Parse(reader["ParId"].ToString()); }
                        }
                        if (cols.IndexOf(",classname,".ToLower()) >= 0)
                        {
                            model.ClassName = reader["ClassName"].ToString();
                        }
                        if (cols.IndexOf(",sequence,".ToLower()) >= 0)
                        {
                            if (reader["Sequence"] != System.DBNull.Value) { model.Sequence = System.Int32.Parse(reader["Sequence"].ToString()); }
                        }
                        if (cols.IndexOf(",depth,".ToLower()) >= 0)
                        {
                            if (reader["Depth"] != System.DBNull.Value) { model.Depth = System.Int32.Parse(reader["Depth"].ToString()); }
                        }
                        if (cols.IndexOf(",childnum,".ToLower()) >= 0)
                        {
                            if (reader["ChildNum"] != System.DBNull.Value) { model.ChildNum = System.Int32.Parse(reader["ChildNum"].ToString()); }
                        }
                        if (cols.IndexOf(",parpath,".ToLower()) >= 0)
                        {
                            model.ParPath = reader["ParPath"].ToString();
                        }
                        if (cols.IndexOf(",belock,".ToLower()) >= 0)
                        {
                            if (reader["BeLock"] != System.DBNull.Value) { model.BeLock = System.Boolean.Parse(reader["BeLock"].ToString()); }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tools.Tool.LogHelper.WriteLog(this.GetType(), ex, 0, "");
                    return null;
                }
                return model;
            }
        }

        public virtual Sys_Class GetModel(System.Int32 id, int departId, string cols)
        {
            cols = cols.ToLower();
            string sql = string.Format("select {2} from {0} where Id={1} and DepartId={3}", tableName, id, cols, departId);
            cols = "," + cols + ",";
            using (SqlDataReader reader = (SqlDataReader)Db.Context.FromSql(sql).ToDataReader())
            {
                Sys_Class model = null;
                try
                {
                    if (reader.Read())
                    {
                        model = new Sys_Class();
                        if (cols.IndexOf(",id,".ToLower()) >= 0)
                        {
                            if (reader["Id"] != System.DBNull.Value) { model.Id = System.Int32.Parse(reader["Id"].ToString()); }
                        }
                        if (cols.IndexOf(",parid,".ToLower()) >= 0)
                        {
                            if (reader["ParId"] != System.DBNull.Value) { model.ParId = System.Int32.Parse(reader["ParId"].ToString()); }
                        }
                        if (cols.IndexOf(",classname,".ToLower()) >= 0)
                        {
                            model.ClassName = reader["ClassName"].ToString();
                        }
                        if (cols.IndexOf(",sequence,".ToLower()) >= 0)
                        {
                            if (reader["Sequence"] != System.DBNull.Value) { model.Sequence = System.Int32.Parse(reader["Sequence"].ToString()); }
                        }
                        if (cols.IndexOf(",depth,".ToLower()) >= 0)
                        {
                            if (reader["Depth"] != System.DBNull.Value) { model.Depth = System.Int32.Parse(reader["Depth"].ToString()); }
                        }
                        if (cols.IndexOf(",childnum,".ToLower()) >= 0)
                        {
                            if (reader["ChildNum"] != System.DBNull.Value) { model.ChildNum = System.Int32.Parse(reader["ChildNum"].ToString()); }
                        }
                        if (cols.IndexOf(",parpath,".ToLower()) >= 0)
                        {
                            model.ParPath = reader["ParPath"].ToString();
                        }
                        if (cols.IndexOf(",belock,".ToLower()) >= 0)
                        {
                            if (reader["BeLock"] != System.DBNull.Value) { model.BeLock = System.Boolean.Parse(reader["BeLock"].ToString()); }
                        }
                        if (cols.IndexOf(",departid,".ToLower()) >= 0)
                        {
                            if (reader["DepartId"] != System.DBNull.Value) { model.DepartId = System.Int32.Parse(reader["DepartId"].ToString()); }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tools.Tool.LogHelper.WriteLog(this.GetType(), ex, 0, "");
                    return null;
                }
                return model;
            }
        }

        #region 辅助方法

        /// <summary>
        /// 模型映射
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected Sys_Class Mapping(T t)
        {
            if (t == null) return null;
            Sys_Class sysClass = null;
            PropertyInfo[] propertyInfo = t.GetType().GetProperties();
            if (propertyInfo != null && propertyInfo.Count() > 0)
            {
                sysClass = new Sys_Class();
                Hashtable dic = new Hashtable();
                foreach (PropertyInfo tp in propertyInfo)
                {
                    dic.Add(tp.Name.ToLower(), tp.GetValue(t, null));
                }
                sysClass.Id = (int)dic["id"];
                sysClass.ParId = (int?)dic["parid"];
                sysClass.ClassName = (string)dic["classname"];
                sysClass.ParPath = (string)dic["parpath"];
                sysClass.ChildNum = (int?)dic["childnum"];
                sysClass.Depth = (int?)dic["depth"];
                sysClass.Sequence = (int?)dic["sequence"];
                sysClass.BeLock = (bool?)dic["belock"];
                sysClass.DepartId = (int?)dic["departid"];
            }
            return sysClass;
        }

        /// <summary>
        /// 列表映射
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected List<Sys_Class> Mapping(IEnumerable<T> list)
        {
            List<Sys_Class> sysClassList = new List<Sys_Class>();
            foreach (var model in list)
            {
                sysClassList.Add(Mapping(model));
            }
            return sysClassList;
        }

        /// <summary>
        /// 列表映射
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected List<Sys_Class> Mapping(IList<T> list)
        {
            List<Sys_Class> sysClassList = new List<Sys_Class>();
            foreach (var model in list)
            {
                sysClassList.Add(Mapping(model));
            }
            return sysClassList;
        }
        #endregion
    }
}
