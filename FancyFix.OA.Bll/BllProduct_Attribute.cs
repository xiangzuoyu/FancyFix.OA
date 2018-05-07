using Dos.DataAccess.Base;
using System.Collections.Generic;
using System.Data;
using FancyFix.Tools.Json;
using FancyFix.OA.Model;
using System;

namespace FancyFix.OA.Bll
{
    public class BllProduct_Attribute : ServiceBase<Product_Attribute>
    {
        public static BllProduct_Attribute Instance()
        {
            return new BllProduct_Attribute();
        }

        /// <summary>
        /// 获取属性集合
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static void LisAttr(int classId, List<Attr> attrList)
        {
            Product_Class model = BllProduct_Class.First(o => o.Id == classId);
            if (model != null && model.ParId > 0)
            {
                LisAttr((int)model.ParId, attrList);
            }
            var attrlist = Query(o => o.ClassId == classId, o => o.Sequence, "asc");
            foreach (var item in attrlist)
            {
                Attr attr = new Attr();
                attr.id = item.Id;
                attr.name = item.AttributeName;
                attr.value = item.DefaultValue;
                attr.inputtype = item.InputType.Value;
                attr.attrlist = BllProduct_AttributeSelect.ListAttrVale(attr.id);
                attr.sequence = item.Sequence.Value;
                attr.isrequired = (bool)item.IsRequired;
                attr.isshow = (bool)item.IsShow;
                attr.issort = (bool)item.IsSort;
                attr.isspecial = (bool)item.IsSpecial;
                attrList.Add(attr);
            }
        }

        /// <summary>
        /// 根据ClassId获取列表
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static IEnumerable<Product_Attribute> GetListByClassId(int classId)
        {
            return Query(o => o.ClassId == classId, o => o.Sequence, "asc");
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetShow(int id)
        {
            var model = First(o => o.Id == id);
            if (model.IsShow.HasValue && model.IsShow.Value)
                return UpdateBitField("IsShow", false, id);
            else
                return UpdateBitField("IsShow", true, id);
        }

        /// <summary>
        /// 设置必填
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetRequired(int id)
        {
            var model = First(o => o.Id == id);
            if (model.IsRequired.HasValue && model.IsRequired.Value)
                return UpdateBitField("IsRequired", false, id);
            else
                return UpdateBitField("IsRequired", true, id);
        }

        /// <summary>
        /// 设置可筛选
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetSort(int id)
        {
            var model = First(o => o.Id == id);
            if (model.IsSort.HasValue && model.IsSort.Value)
                return UpdateBitField("IsSort", false, id);
            else
                return UpdateBitField("IsSort", true, id);
        }

        /// <summary>
        /// 判断主要属性已满足个数
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static bool hasSpecial(int classId)
        {
            var list = Query(o => o.ClassId == classId && o.IsSpecial == true, o => o.Id, "desc", o => o.Id);
            if (list.Count < 2)
                return false;
            return true;
        }

        /// <summary>
        /// 设置主要属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static bool SetSpecial(int id, int classId)
        {
            var model = First(o => o.Id == id);
            if (model.IsSpecial.HasValue && model.IsSpecial.Value)
            {
                //已设置为主要属性，则可以取消
                return UpdateBitField("IsSpecial", false, id);
            }
            else
            {
                //已满足个数，则设置失败
                if (hasSpecial(classId))
                    return false;
                return UpdateBitField("IsSpecial", true, id);
            }
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="atrrId"></param>
        /// <returns></returns>
        public static bool SetDefault(int id, int attrId)
        {
            string sql = "update Product_Attribute set DefaultValue=(select ItemName from Product_AttributeSelect where ID=@id) where ID=@attrId";
            return Db.Context.FromSql(sql)
                  .AddInParameter("id", DbType.Int32, id)
                  .AddInParameter("attrId", DbType.Int32, attrId)
                  .ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 清除默认值
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        public static bool ClearDefault(int attrId)
        {
            string sql = "update Product_Attribute set DefaultValue='' where ID=@attrId";
            return Db.Context.FromSql(sql).AddInParameter("attrId", DbType.Int32, attrId).ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(Product_Attribute model)
        {
            using (var trans = Db.Context.BeginTransaction())
            {
                try
                {
                    //插入属性数据
                    int id = trans.Insert(model);
                    if (id == 0)
                    {
                        trans.Rollback();
                        return 0;
                    }
                    trans.Commit();
                    return id;
                }
                catch (Exception ex)
                {
                    Tools.Tool.LogHelper.WriteLog(typeof(BllProduct_Attribute), ex, 0, "");
                    return 0;
                }
            }
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            try
            {
                string sql = string.Format(@"declare @ClassID int;
                           select @ClassID=ClassID from Product_Attribute where [ID]={0};
                           delete from Product_Attribute where [ID]={0};
                           delete from Product_AttributeSelect where AttributeId={0};
                           update Product_Class set AttributeNum=(select count(1) from Product_Attribute where ClassId=@ClassId) where Id=@ClassID", id);
                return Db.Context.FromSql(sql).ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(this.GetType(), ex, 0, "");
                return 0;
            }
        }

        /// <summary>
        /// 设置属性上移
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="classid">分类ID</param>
        /// <param name="sequence">当前排序</param>
        /// <returns></returns>
        public bool SetUp(int id, int classid, int sequence)
        {
            try
            {
                string sql = string.Format(@"update Product_Attribute set [Sequence]=isnull((select top 1 [Sequence] from Product_Attribute where [Sequence]<{2} and ClassID={1} order by [Sequence] desc),[Sequence]) where [ID]={0};update Product_Attribute set [Sequence]={2} where [Sequence]=(select top 1 [Sequence] from Product_Attribute  where [Sequence]<{2} and ClassID={1} order by [Sequence] desc) and ClassID={1} and [ID]<>{0}", id, classid, sequence);
                return Db.Context.FromSql(sql).ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(this.GetType(), ex, 0, "");
                return false;
            }
        }
        /// <summary>
        /// 设置属性下移
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="classid">分类ID</param>
        /// <param name="sequence">当前排序</param>
        /// <returns></returns>
        public bool SetDown(int id, int classid, int sequence)
        {
            try
            {
                string sql = string.Format(@"update Product_Attribute set [Sequence]=isnull((select top 1 [Sequence] from Product_Attribute where [Sequence]>{2} and ClassID={1} order by [Sequence] asc),[Sequence]) where [ID]={0};update Product_Attribute set [Sequence]={2} where [Sequence]=(select top 1 [Sequence] from Product_Attribute  where [Sequence]>{2} and ClassID={1} order by [Sequence] asc) and ClassID={1} and [ID]<>{0}", id, classid, sequence);
                return Db.Context.FromSql(sql).ExecuteNonQuery() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(this.GetType(), ex, 0, "");
                return false;
            }
        }
    }
}
