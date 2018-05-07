using Dos.DataAccess.Base;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Text;
using FancyFix.Tools.Json;

namespace FancyFix.OA.Bll
{
    public class BllProduct_AttributeSelect : ServiceBase<Product_AttributeSelect>
    {
        public static BllProduct_AttributeSelect Instance()
        {
            return new BllProduct_AttributeSelect();
        }

        /// <summary>
        /// 获取属性值列表
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        public static List<Product_AttributeSelect> ListByAttrId(int attrId)
        {
            return Query(o => o.AttributeId == attrId, o => o.Sequence, "asc");
        }

        /// <summary>
        /// 获取属性值集合
        /// </summary>
        /// <param name="attrId"></param>
        /// <returns></returns>
        public static List<AttrValue> ListAttrVale(int attrId)
        {
            List<AttrValue> list = new List<AttrValue>();
            var attrlist = ListByAttrId(attrId);
            foreach (var item in attrlist)
            {
                AttrValue attrValue = new AttrValue();
                attrValue.id = item.Id;
                attrValue.value = item.ItemName;
                list.Add(attrValue);
            }
            list.Add(new AttrValue() { id = 0, value = "自定义" });
            return list;
        }

        /// <summary>
        /// 设置属性上移
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="classid">分类ID</param>
        /// <param name="sequence">当前排序</param>
        /// <returns></returns>
        public bool SetUp(int id, int attributeId, int sequence)
        {
            try
            {
                string sql = string.Format(@"update Product_AttributeSelect set [Sequence]=isnull((select top 1 [Sequence] from Product_AttributeSelect where [Sequence]<{2} and AttributeId={1} order by [Sequence] desc),[Sequence]) where [ID]={0};update Product_AttributeSelect set [Sequence]={2} where [Sequence]=(select top 1 [Sequence] from Product_AttributeSelect  where [Sequence]<{2} and AttributeId={1} order by [Sequence] desc) and AttributeId={1} and [ID]<>{0}", id, attributeId, sequence);
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
        public bool SetDown(int id, int attributeId, int sequence)
        {
            try
            {
                string sql = string.Format(@"update Product_AttributeSelect set [Sequence]=isnull((select top 1 [Sequence] from Product_AttributeSelect where [Sequence]>{2} and AttributeId={1} order by [Sequence] asc),[Sequence]) where [ID]={0};update Product_AttributeSelect set [Sequence]={2} where [Sequence]=(select top 1 [Sequence] from Product_AttributeSelect where [Sequence]>{2} and AttributeId={1} order by [Sequence] asc) and AttributeId={1} and [ID]<>{0}", id, attributeId, sequence);
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
