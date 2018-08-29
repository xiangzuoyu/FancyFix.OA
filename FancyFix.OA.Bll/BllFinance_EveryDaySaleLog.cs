using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyFix.OA.Bll
{
    public class BllFinance_EveryDaySaleLog : ServiceBase<Finance_EveryDaySaleLog>
    {
        public static BllFinance_EveryDaySaleLog Instance()
        {
            return new BllFinance_EveryDaySaleLog();
        }

        public static IEnumerable<Finance_EveryDaySaleLog> PageList(int page, int pageSize, out long records, string file, string key, DateTime? startdate, DateTime? enddate
            , string departmentName = "")
        {
            var where = new Where<Finance_EveryDaySaleLog>();
            where.And(o => o.Display != 2);
            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(key) && file != "0")
                where.And(string.Format(" {0} like '%{1}%' ", file, key));

            if (!string.IsNullOrEmpty(departmentName) && departmentName != "0")
                where.And(string.Format(" DepartmentName like '%{0}%' ", departmentName));

            if (startdate != null)
                where.And(o => o.SaleDate >= startdate);
            if (enddate != null)
                where.And(o => o.SaleDate <= enddate);

            var p = Db.Context.From<Finance_EveryDaySaleLog>()
                .Where(where);
            records = p.Count();
            return p.Page(pageSize, page).OrderByDescending(o => o.Id).ToList();
        }

        public static IEnumerable<Finance_EveryDaySaleLog> GetList(string file, string key, DateTime? startdate, DateTime? enddate, string departmentName)
        {
            var where = new Where<Finance_EveryDaySaleLog>();
            where.And(o => o.Display != 2);
            if (!string.IsNullOrEmpty(file) && !string.IsNullOrEmpty(key))
                where.And(string.Format(" {0} like '%{1}%' ", file, key));
            if (!string.IsNullOrEmpty(departmentName) && departmentName != "0")
                where.And(string.Format(" DepartmentName like '%{0}%' ", departmentName));
            if (startdate != null)
                where.And(o => o.SaleDate >= startdate);
            if (enddate != null)
                where.And(o => o.SaleDate <= enddate);

            var p = Db.Context.From<Finance_EveryDaySaleLog>()
                    .Where(where).OrderBy(o => o.SaleDate);
            return p.ToList();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="batch">为true ，</param>
        /// <returns></returns>
        public static string Add(Finance_EveryDaySaleLog model)
        {
            if (model == null)
                return "-1";

            try
            {
                //组合键（日期+销售人员+商户+产品名称+ 产品规格+合同号）如果已存在执行修改，跳过为空的字段，否则新增
                var everyDaySaleLogModel = First(o => o.SaleDate == model.SaleDate &&
                                                 o.SaleName == model.SaleName &&
                                                 o.Customer == model.Customer &&
                                                 o.ContractNumber == model.ContractNumber &&
                                                 o.ProductName == model.ProductName &&
                                                 o.ProductSKU == model.ProductSKU &&
                                                 o.ProductSpecification == model.ProductSpecification &&
                                                 o.Display != 2);

                int result = 0;
                if (everyDaySaleLogModel != null)
                    result = Update(MappingModel(everyDaySaleLogModel, model));
                else
                    result = Insert(model);

                return result > 0 ? "0" : "4";
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(Finance_EveryDaySaleLog), ex, 0, "");
                return "-1";
            }
        }

        /// <summary>
        /// 重复导入时的模型映射，字段为空时不更新
        /// </summary>
        /// <param name="oldModel"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        private static Finance_EveryDaySaleLog MappingModel(Finance_EveryDaySaleLog oldModel, Finance_EveryDaySaleLog newModel)
        {
            if (!string.IsNullOrWhiteSpace(newModel.DepartmentName))
                oldModel.DepartmentName = newModel.DepartmentName;
            if (!string.IsNullOrWhiteSpace(newModel.Supplier))
                oldModel.Supplier = newModel.Supplier;
            //if (!string.IsNullOrWhiteSpace(newModel.ProductSKU))
            //    oldModel.ProductSKU = newModel.ProductSKU;
            //if (!string.IsNullOrWhiteSpace(newModel.ProductSpecification))
            //    oldModel.ProductSpecification = newModel.ProductSpecification;
            if (newModel.SaleCount != null)
                oldModel.SaleCount = newModel.SaleCount;
            if (newModel.SalePrice != null)
                oldModel.SalePrice = newModel.SalePrice;
            if (!string.IsNullOrWhiteSpace(newModel.Currency))
                oldModel.Currency = newModel.Currency;
            if (newModel.ExchangeRate != null)
                oldModel.ExchangeRate = newModel.ExchangeRate;
            if (newModel.SaleIncome != null)
                oldModel.SaleIncome = newModel.SaleIncome;
            if (newModel.MaterialUnitPrice != null)
                oldModel.MaterialUnitPrice = newModel.MaterialUnitPrice;
            if (newModel.ProcessUnitPrice != null)
                oldModel.ProcessUnitPrice = newModel.ProcessUnitPrice;
            if (newModel.MaterialTotalPrice != null)
                oldModel.MaterialTotalPrice = newModel.MaterialTotalPrice;
            if (newModel.ProcessTotalPrice != null)
                oldModel.ProcessTotalPrice = newModel.ProcessTotalPrice;
            if (newModel.GrossProfit != null)
                oldModel.GrossProfit = newModel.GrossProfit;
            if (newModel.GrossProfitRate != null)
                oldModel.GrossProfitRate = newModel.GrossProfitRate;
            if (newModel.ChangeCostNumber != null)
                oldModel.ChangeCostNumber = newModel.ChangeCostNumber;
            if (newModel.ChangeCostMatter != null)
                oldModel.ChangeCostMatter = newModel.ChangeCostMatter;
            if (newModel.ContributionMoney != null)
                oldModel.ContributionMoney = newModel.ContributionMoney;
            if (newModel.ContributionRatio != null)
                oldModel.ContributionRatio = newModel.ContributionRatio;
            if (newModel.AvgCoatUndue != null)
                oldModel.AvgCoatUndue = newModel.AvgCoatUndue;
            if (newModel.AvgCoatCurrentdue != null)
                oldModel.AvgCoatCurrentdue = newModel.AvgCoatCurrentdue;
            if (newModel.AvgCoatOverdue != null)
                oldModel.AvgCoatOverdue = newModel.AvgCoatOverdue;
            if (newModel.CustomerContributionMoney != null)
                oldModel.CustomerContributionMoney = newModel.CustomerContributionMoney;
            if (newModel.CustomerContributionRatio != null)
                oldModel.CustomerContributionRatio = newModel.CustomerContributionRatio;
            if (newModel.Follow != null)
                oldModel.Follow = newModel.Follow;

            oldModel.AddDate = newModel.AddDate;
            oldModel.AddUserId = newModel.AddUserId;
            oldModel.LastDate = newModel.LastDate;
            oldModel.LastUserId = newModel.LastUserId;

            return oldModel;
        }

        #region 删除
        public static int Delete(int id, int myuserId)
        {
            var model = First(o => o.Id == id);
            if (model == null)
                return 0;
            model.Display = 2;
            model.LastDate = DateTime.Now;
            model.LastUserId = myuserId;

            int result = Update(model);
            //重新统计
            if (result > 0)
                BllFinance_Statistics.AgainCountData(new List<Finance_Statistics>
                {
                    new Finance_Statistics(){
                        SaleDate = model.SaleDate,
                        DepartmentName = model.DepartmentName,
                        LastUserId = myuserId,
                        LastDate = DateTime.Now
                    }
                });

            return result;
        }

        public static int DeleteList(IEnumerable<Finance_EveryDaySaleLog> list, int myuserId)
        {
            foreach (var item in list)
                Delete(item.Id, myuserId);

            return 1;
        }
        #endregion

        /// <summary>
        /// 根据查询条件获取所有部门并排序
        /// </summary>
        /// <param name="files"></param>
        /// <param name="key"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static IEnumerable<Finance_EveryDaySaleLog> GetBusinessOrder(string files, string key, string startdate, string enddate, string departmentName)
        {
            //SELECT DepartmentName FROM[dbo].[Finance_EveryDaySaleLog] order by CHARINDEX(DepartmentName, N'事业一部,事业二部,事业三部,事业四部')
            string where = " 1=1 ";

            if (!string.IsNullOrWhiteSpace(files) && !string.IsNullOrWhiteSpace(key))
                where += $" and {files} like '%{key}%' ";
            if (!string.IsNullOrEmpty(departmentName) && departmentName != "0")
                where += $" and DepartmentName like '%{departmentName}%' ";
            if (!string.IsNullOrWhiteSpace(startdate))
                where += $" and SaleDate >= '{startdate}'";
            if (!string.IsNullOrWhiteSpace(enddate))
                where += $" and SaleDate <= '{enddate}'";

            string orderby = "CHARINDEX(DepartmentName, N'事业一部,事业二部,事业三部,事业四部,事业五部,事业六部,事业七部,事业八部,事业九部,事业十部,事业十一部,事业十二部,事业十三部,事业十四部')";
            return GetSelectList(0, "DepartmentName", where, orderby);
        }
    }
}
