using FancyFix.Core.Enum;
using FancyFix.OA.Areas.Demand.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FancyFix.OA.Helper
{
    public class DemandTypeHelper
    {
        public static Dictionary<string, ContentModel> GetPageStructure(int id=0,int type=1)
        {
            Dictionary<string, ContentModel> dict = new Dictionary<string, ContentModel>();
            DemandTypeModel demandTypeModel = new DemandTypeModel();
            if (id > 0)
            {
               var demand=Bll.BllDevelop_Demand.First(p => p.Id == id);
                if (demand != null && !string.IsNullOrWhiteSpace(demand.Content) && demand.Type==type)
                {
                  demandTypeModel=JsonConvert.DeserializeObject<DemandTypeModel>(demand.Content);
                }
            }
            if (type == (int)Demand_TypeEnum.优化现有供应链)
            {
                ContentModel fileld1 = new ContentModel();
                fileld1.Value = demandTypeModel.Field1;
                fileld1.Field = "Field1";
                fileld1.IsRquest = false;
                fileld1.Reminder = "除以下信息之外，若有其他信息，可以填写在这里";
                dict.Add("现在的采购状况", fileld1);
                ContentModel fileld2 = new ContentModel();
                fileld2.Value = demandTypeModel.Field2;
                fileld2.Field = "Field2";
                dict.Add("采购单价（是否含税）", fileld2);
                ContentModel fileld3 = new ContentModel();
                fileld3.Value = demandTypeModel.Field3;
                fileld3.Field = "Field3";
                dict.Add("采购频率", fileld3);
                ContentModel fileld4 = new ContentModel();
                fileld4.Value = demandTypeModel.Field4;
                fileld4.Field = "Field4";
                dict.Add("单次采购数量", fileld4);
                ContentModel fileld5 = new ContentModel();
                fileld5.Value = demandTypeModel.Field5;
                fileld5.Field = "Field5";
                dict.Add("包装方式", fileld5);
                ContentModel fileld6 = new ContentModel();
                fileld6.Value = demandTypeModel.Field6;
                fileld6.Field = "Field6";
                dict.Add("收货地址", fileld6);
                ContentModel fileld7 = new ContentModel();
                fileld7.Value = demandTypeModel.Field7;
                fileld7.Field = "Field7";
                dict.Add("需要解决的问题", fileld7);
                ContentModel fileld8 = new ContentModel();
                fileld8.Value = demandTypeModel.Field8;
                fileld8.Field = "Field8";
                dict.Add("预期达到的效果", fileld8);
                ContentModel fileld9 = new ContentModel();
                fileld9.Value = demandTypeModel.Field9;
                fileld9.Field = "Field9";
                dict.Add("对业绩的影响程度", fileld9);
            }
            else if (type == (int)Demand_TypeEnum.跨境B2B询盘)
            {
                ContentModel fileld1 = new ContentModel();
                fileld1.Value = demandTypeModel.Field1;
                fileld1.Field = "Field1";
                fileld1.IsRquest = false;
                fileld1.Reminder = "材质，尺寸规格厚度（克重），包装方式，认证检测要求，采购数量必须体现在询价单上，如有其他的额外产品信息";
                dict.Add("产品相关内容", fileld1);
                ContentModel fileld2 = new ContentModel();
                fileld2.Value = demandTypeModel.Field2;
                fileld2.Field = "Field2";
                dict.Add("是否有客供样品", fileld2);
                ContentModel fileld3 = new ContentModel();
                fileld3.Value = demandTypeModel.Field3;
                fileld3.Field = "Field3";
                dict.Add("目标价格：最高成本价", fileld3);
                ContentModel fileld4 = new ContentModel();
                fileld4.Value = demandTypeModel.Field4;
                fileld4.Field = "Field4";
                dict.Add("出货市场", fileld4);
                ContentModel fileld5 = new ContentModel();
                fileld5.Value = demandTypeModel.Field5;
                fileld5.Field = "Field5";
                dict.Add("是否需要打样（指客户需求样品）", fileld5);
                ContentModel fileld6 = new ContentModel();
                fileld6.Value = demandTypeModel.Field6;
                fileld6.Field = "Field6";
                dict.Add("其他需求：客户的特殊需求，供应商开发数量，工厂所在区域等", fileld6);
            }
            else if (type == (int)Demand_TypeEnum.跨境B2B新品)
            {
                ContentModel fileld1 = new ContentModel();
                fileld1.Value = demandTypeModel.Field1;
                fileld1.Field = "Field1";
                fileld1.IsRquest = false;
                fileld1.Reminder = "此部分填写你关于此客户对这个产品的看法和考虑";
                dict.Add("产品相关内容", fileld1);
                ContentModel fileld2 = new ContentModel();
                fileld2.Value = demandTypeModel.Field2;
                fileld2.Field = "Field2";
                dict.Add("材质", fileld2);
                ContentModel fileld3 = new ContentModel();
                fileld3.Value = demandTypeModel.Field3;
                fileld3.Field = "Field3";
                dict.Add("尺寸规格厚度（克重）", fileld3);
                ContentModel fileld4 = new ContentModel();
                fileld4.Value = demandTypeModel.Field4;
                fileld4.Field = "Field4";
                dict.Add("使用场景", fileld4);
                ContentModel fileld5 = new ContentModel();
                fileld5.Value = demandTypeModel.Field5;
                fileld5.Field = "Field5";
                dict.Add("认证检测要求", fileld5);
                ContentModel fileld6 = new ContentModel();
                fileld6.Value = demandTypeModel.Field6;
                fileld6.Field = "Field6";
                dict.Add("是否有样品（如无产品具体描述，必须要提供样品）", fileld6);
                ContentModel fileld7 = new ContentModel();
                fileld7.Value = demandTypeModel.Field7;
                fileld7.Field = "Field7";
                dict.Add("目标价格：最高成本价", fileld7);
            }
            else if (type == (int)Demand_TypeEnum.跨境B2C新品开发)
            {
                ContentModel fileld1 = new ContentModel();
                fileld1.Value = demandTypeModel.Field1;
                fileld1.Field = "Field1";
                dict.Add("预期类目范围", fileld1);
                ContentModel fileld2 = new ContentModel();
                fileld2.Value = demandTypeModel.Field2;
                fileld2.Field = "Field2";
                dict.Add("预期新品款数", fileld2);
                ContentModel fileld3 = new ContentModel();
                fileld3.Value = demandTypeModel.Field3;
                fileld3.Field = "Field3";
                dict.Add("尺寸规格", fileld3);
                ContentModel fileld4 = new ContentModel();
                fileld4.Value = demandTypeModel.Field4;
                fileld4.Field = "Field4";
                dict.Add("风格（参考链接或图片）", fileld4);
                ContentModel fileld5 = new ContentModel();
                fileld5.Value = demandTypeModel.Field5;
                fileld5.Field = "Field5";
                dict.Add("目标价格：最高成本价", fileld5);
                ContentModel fileld6 = new ContentModel();
                fileld6.Value = demandTypeModel.Field6;
                fileld6.Field = "Field6";
                dict.Add("预期拿样时间", fileld6);
            }
            else if (type == (int)Demand_TypeEnum.设计部打样)
            {
                ContentModel fileld1 = new ContentModel();
                fileld1.Value = demandTypeModel.Field1;
                fileld1.Field = "Field1";
                dict.Add("产品类目", fileld1);
                ContentModel fileld2 = new ContentModel();
                fileld2.Value = demandTypeModel.Field2;
                fileld2.Field = "Field2";
                dict.Add("设计数量", fileld2);
                ContentModel fileld3 = new ContentModel();
                fileld3.Value = demandTypeModel.Field3;
                fileld3.Field = "Field3";
                dict.Add("打样数量： 每款几个", fileld3);
                ContentModel fileld4 = new ContentModel();
                fileld4.Value = demandTypeModel.Field4;
                fileld4.Field = "Field4";
                dict.Add("是否有指定供应商", fileld4);
                ContentModel fileld5 = new ContentModel();
                fileld5.Value = demandTypeModel.Field5;
                fileld5.Field = "Field5";
                dict.Add("材质/工艺推荐参考", fileld5);
            }
            else if (type == (int)Demand_TypeEnum.跨境B2C项目)
            {
                ContentModel fileld1 = new ContentModel();
                fileld1.Value = demandTypeModel.Field1;
                fileld1.Field = "Field1";
                dict.Add("产品相关内容", fileld1);
                ContentModel fileld2 = new ContentModel();
                fileld2.Value = demandTypeModel.Field2;
                fileld2.Field = "Field2";
                dict.Add("材质", fileld2);
                ContentModel fileld3 = new ContentModel();
                fileld3.Value = demandTypeModel.Field3;
                fileld3.Field = "Field3";
                dict.Add("尺寸规格厚度（克重）", fileld3);
                ContentModel fileld4 = new ContentModel();
                fileld4.Value = demandTypeModel.Field4;
                fileld4.Field = "Field4";
                dict.Add("包装方式", fileld4);
                ContentModel fileld5 = new ContentModel();
                fileld5.Value = demandTypeModel.Field5;
                fileld5.Field = "Field5";
                dict.Add("装箱率", fileld5);
                ContentModel fileld6 = new ContentModel();
                fileld6.Value = demandTypeModel.Field6;
                fileld6.Field = "Field6";
                dict.Add("采购数量", fileld6);
                ContentModel fileld7 = new ContentModel();
                fileld7.Value = demandTypeModel.Field7;
                fileld7.Field = "Field7";
                dict.Add("售卖平台", fileld7);
                ContentModel fileld8 = new ContentModel();
                fileld8.Value = demandTypeModel.Field8;
                fileld8.Field = "Field8";
                dict.Add("目标价格：最高成本价", fileld8);
                ContentModel fileld9 = new ContentModel();
                fileld9.Value = demandTypeModel.Field9;
                fileld9.Field = "Field9";
                dict.Add("预计完货时间", fileld9);
                ContentModel fileld10 = new ContentModel();
                fileld10.Value = demandTypeModel.Field10;
                fileld10.Field = "Field10";
                dict.Add("项目预期目标", fileld10);
            }
            return dict;

        }

        public static List<string> GetDeamndAdminUserIdList(string key)
        {
            List<string> list = new List<string>();
           var adminStr=ConfigurationManager.AppSettings[key].ToString();
            if (string.IsNullOrWhiteSpace(adminStr))
            {
                return list;
            }
            list= adminStr.Split(',').ToList();
            return list;
        }

    }
}