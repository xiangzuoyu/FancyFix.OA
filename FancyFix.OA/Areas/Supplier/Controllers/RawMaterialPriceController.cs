using FancyFix.OA.Base;
using FancyFix.OA.Filter;
using FancyFix.OA.Model;
using FancyFix.Tools.Config;
using FancyFix.Tools.Tool;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Tools;
using Tools.Tool;

namespace FancyFix.OA.Areas.Supplier.Controllers
{
    public class RawMaterialPriceController : BaseAdminController
    {
        // GET: Supplier/RawMaterialPrice

        #region 加载列表
        public ActionResult List()
        {
            //ViewBag.prices = Bll.BllSupplier_RawMaterialPrice.GetSelectList(0, "distinct(Years)", "display!=2", "Years");
            //ViewBag.prices = Bll.BllSupplier_Price.GetSelectList(0, "distinct(Years)", "display!=2", "Years");

            return View();
        }

        //[PermissionFilter("/supplier/rawmaterialprice/list")]
        public JsonResult PageList(int page = 0, int pagesize = 0, int priceFrequency = 0, string files = "", string key = "")
        {
            long records = 0;
            //if (years == 0)
            //    years = DateTime.Now.Year;

            var list = Bll.BllSupplier_RawMaterialPrice.PageList(page, pagesize, out records, files, key, priceFrequency);
            foreach (var item in list)
            {
                //价格频次
                item.PriceFrequencyName = item.PriceFrequency != null
                    ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.PriceFrequency), item.PriceFrequency.GetValueOrDefault().ToString().ToInt32()).ToString()
                    : "无";
            }
            return BspTableJson(list, records);
        }

        #endregion

        #region 价格列表
        public ActionResult PriceList(int[] id = null, string starttime = "", string endtime = "")
        {
            if (id == null)
                return LayerAlertErrorAndClose("请先勾选需要查看的原材料价格");

            int page = GetThisPage();
            int pageSize = 15;
            long records = 0;

            var rawMaterialPriceList = Bll.BllSupplier_RawMaterialPrice.PageList(page, pageSize, out records, "", "", 0, id.ToList())
                ?? new List<Supplier_RawMaterialPrice>();

            DateTime startdate, enddate;
            GetDate(starttime, endtime, out startdate, out enddate);
            var pricelist = Bll.BllSupplier_Price.GetList(startdate, enddate);
            var rawMaterialList = Bll.BllSupplier_RawMaterial.GetSelectList(0, "Id,SAPCode,Description,Currency", "display!=2", "") ?? new List<Supplier_RawMaterial>();
            var supplierList = Bll.BllSupplier_List.GetSelectList(0, "Id,Code,Name", "display!=2", "") ?? new List<Supplier_List>();

            string table = CreateTable(rawMaterialPriceList, pricelist, rawMaterialList, supplierList, startdate, enddate);
            string pageStr = ShowPage((int)records, pageSize, page, 5, $"starttime={starttime}&endtime={endtime}", false);
            pageStr = pageStr.Replace(HttpUtility.UrlEncode(string.Join(",", id)), string.Join("&id=", id));
            ViewBag.pricelist = table;
            ViewBag.pageStr = pageStr;
            ViewBag.Starttime = startdate.ToString("yyyy-MM");
            ViewBag.Endtime = enddate.ToString("yyyy-MM");
            ViewBag.ids = $"id=" + string.Join("&id=", id);

            return View();
        }

        private string CreateTable(IEnumerable<Supplier_RawMaterialPrice> rawMaterialPriceList, List<Supplier_Price> pricelist,
            IEnumerable<Supplier_RawMaterial> rawMaterialList, IEnumerable<Supplier_List> supplierList, DateTime start, DateTime end)
        {
            #region 添加表头
            int yearsNum = end.Year - start.Year + 1;
            int colNum = 0;
            var table = CreateHeads(start, end, out colNum);
            #endregion

            if (rawMaterialPriceList == null || rawMaterialPriceList.Count() < 1)
            {
                table.AddSpanCol("暂无数据", colNum);
                return table.GetTable();
            }

            foreach (var item in rawMaterialPriceList)
            {
                DateTime startdate = start;
                DateTime enddate = end;
                var prices = pricelist.Where(o => o.RawMaterialPriceId == item.Id).ToList() ?? new List<Supplier_Price>();
                var rawMaterial = rawMaterialList.Where(o => o.SAPCode == item.RawMaterialId).FirstOrDefault() ?? new Supplier_RawMaterial();
                var supplier = supplierList.Where(o => o.Code == item.VendorId).FirstOrDefault() ?? new Supplier_List();

                table.AddCol(supplier.Code);
                table.AddCol(supplier.Name);
                table.AddCol(rawMaterial.SAPCode);
                table.AddCol(rawMaterial.Description);
                table.AddCol(rawMaterial.Currency);

                while (startdate <= enddate)
                {
                    var price = prices.Where(o => o.Years == (startdate.Year) && o.Month == startdate.Month).Select(o => o.Price).FirstOrDefault()
                            .GetValueOrDefault();

                    table.AddCol(price.ToString("f2"), price <= 0 ? "color:#d2d2d2;" : "");
                    startdate = startdate.AddMonths(1);
                    colNum++;
                }

                table.AddRow();
            }

            return table.GetTable();
        }

        private Tables CreateHeads(DateTime startYears, DateTime endYearsa, out int colNum)
        {
            var table = new Tables();
            table.AddHeadCol("供应商代码", "min-width: 125px;");
            table.AddHeadCol("供应商名称", "min-width: 160px;");
            table.AddHeadCol("原材料代码", "min-width: 130px;");
            table.AddHeadCol("原材料名称", "min-width: 135px;");
            table.AddHeadCol("价格单位", "min-width: 135px;");
            colNum = 4;

            while (startYears <= endYearsa)
            {
                table.AddHeadCol($"{startYears.ToString("yyyy-MM")}", "min-width: 85px;");
                startYears = startYears.AddMonths(1);
                colNum++;
            }

            table.AddHeadRow();

            return table;
        }

        /// <summary>
        /// 如果日期范围为空，默认设为当年
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        private void GetDate(string starttime, string endtime, out DateTime startdate, out DateTime enddate)
        {
            DateTime currentDate = DateTime.Now;

            startdate = string.IsNullOrEmpty(starttime)
                ? ($"{currentDate.ToString("yyyy")}-01-01").ToDateTime()
                : starttime.ToDateTime();
            enddate = string.IsNullOrEmpty(endtime)
                ? ($"{currentDate.AddYears(1).ToString("yyyy")}-01-01").ToDateTime().AddDays(-1)
                : endtime.ToDateTime().AddMonths(1).AddDays(-1);
        }

        #endregion

        #region 导入Excel
        [HttpPost]
        public ActionResult List(HttpPostedFileBase file)
        {
            try
            {
                Tools.Config.UploadConfig config = UploadProvice.Instance();
                SiteOption option = config.SiteOptions["local"];
                string filePath = option.Folder + config.Settings["file"].FilePath + DateTime.Now.ToString("yyyyMMddHHmmss")
                        + (file.FileName.IndexOf(".xlsx") > 0 ? ".xlsx" : ".xls");

                string result = FileHelper.ValicationAndSaveFileToPath(file, filePath);
                if (result != "0")
                    return MessageBoxAndJump($"上传失败，{result}", "list");

                var sheet = ExcelHelper.ReadExcel(filePath);
                string msg = ExcelToList(sheet, 2);

                if (msg != "0")
                    return MessageBoxAndJump("导入失败," + GetMsg(msg), "list");
            }
            catch (Exception ex)
            {
                return MessageBoxAndJump("导入失败:" + ex.Message.ToString(), "list");
            }

            return Redirect("list");
        }

        private string GetMsg(string result)
        {
            string msg = string.Empty;
            switch (result)
            {
                case "1":
                    msg = "Excel数据为空";
                    break;
                case "2":
                    msg = "供应商Code不存在";
                    break;
                case "3":
                    msg = "原材料Code不存在";
                    break;
                case "-1":
                    msg = "导入异常";
                    break;
            }
            return msg;
        }

        private string ExcelToList(ISheet sheet, int startRow)
        {
            var logDate = DateTime.Now;
            //List<Supplier_RawMaterial> supList = new List<Supplier_RawMaterial>();
            if (sheet == null)
                return "1";

            //第一行为标题
            try
            {
                IRow headRow = sheet.GetRow(startRow);
                int cellTotal = headRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                for (int i = startRow; i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null ||
                        string.IsNullOrEmpty(row.GetCell(1)?.ToString()) ||
                        string.IsNullOrEmpty(row.GetCell(2)?.ToString()) ||
                        string.IsNullOrEmpty(row.GetCell(5)?.ToString()))
                        continue;

                    //标题行
                    if (i == startRow)
                    {
                        headRow = row;
                        continue;
                    }

                    var rawMaterialModel = CreateRawMaterialModel(row, logDate);
                    if (rawMaterialModel == null)
                        continue;

                    int id = 0;
                    string isok = Bll.BllSupplier_RawMaterial.Add(rawMaterialModel, ref id);
                    if (isok != "0")
                        return isok;

                    AddPrice(row, headRow, id, cellTotal, logDate);
                }
            }
            catch (Exception)
            {
                return "-1";
            }

            return "0";
        }

        /// <summary>
        /// 根据数据填充价格映射模型
        /// </summary>
        /// <param name="row"></param>
        /// <param name="logDate"></param>
        /// <returns></returns>
        private Supplier_RawMaterial CreateRawMaterialModel(IRow row, DateTime logDate)
        {
            if (row == null) return null;

            var model = new Supplier_RawMaterial()
            {
                BU = row.GetCell(0)?.ToString(),
                SAPCode = row.GetCell(1)?.ToString(),
                Description = row.GetCell(2)?.ToString(),
                Category = row.GetCell(3)?.ToString(),
                LeadBuyer = row.GetCell(4)?.ToString(),
                SupplierCode = row.GetCell(5)?.ToString(),
                SupplierName = row.GetCell(6)?.ToString(),
                PriceFrequency = Tools.Enums.Tools.GetValueByName(typeof(Models.PriceFrequency), row.GetCell(7)?.ToString()),
                Currency = row.GetCell(8)?.ToString(),
                AddDate = logDate,
                AddUserId = MyInfo.Id,
                LastDate = logDate,
                LastUserId = MyInfo.Id
            };

            if (string.IsNullOrEmpty(model.SAPCode) &&
                string.IsNullOrEmpty(model.Description) &&
                string.IsNullOrEmpty(model.SupplierCode))
                return null;

            return model;
        }

        private bool AddPrice(IRow row, IRow headRow, int priceId, int cellTotal, DateTime logDate)
        {
            if (row == null || headRow == null)
                return false;
            //List<Supplier_Price> list = new List<Supplier_Price>();
            //遍历个每行原材料后面的价格
            try
            {
                for (int i = 9; i < cellTotal; i++)
                {
                    string price = row.GetCell(i).ToString();
                    //如果价格为空，跳过
                    if (string.IsNullOrEmpty(price))
                        continue;

                    var date = headRow.GetCell(i).ToString().Split('-');
                    if (date.Length != 2)
                        continue;
                    var years = date[0].TrimStart('\'').ToInt32();

                    var priceModel = new Supplier_Price
                    {
                        RawMaterialPriceId = priceId,
                        Years = years,
                        Month = date[1].ToInt32(),
                        Price = price.ToDecimal(),
                        AddDate = logDate,
                        AddUserId = MyInfo.Id,
                        LastDate = logDate,
                        LastUserId = MyInfo.Id,
                        Display = 1,
                        YearsMonth = $"{years.ToString()}-{date[1].ToString()}-1".ToDateTime()
                    };

                    Bll.BllSupplier_Price.DeleteAndAdd(priceModel);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;

        }
        #endregion

        #region 导出Excel
        public ActionResult ExportExcel(string files = "", string key = "", int priceFrequency = 0, int years = 0)
        {
            ViewBag.files = files;
            ViewBag.key = key;
            ViewBag.priceFrequencyId = priceFrequency;
            ViewBag.years = years < 1 ? DateTime.Now.Year : years;

            DateTime startdate, enddate;
            GetDate("", "", out startdate, out enddate);
            ViewBag.Starttime = startdate.ToString("yyyy-MM");
            ViewBag.Endtime = enddate.ToString("yyyy-MM");

            return View();
        }

        [HttpPost]
        public ActionResult ExportExcel(int priceFrequency = 0)
        {
            string files = RequestString("files");
            string key = RequestString("key");
            string cols = RequestString("cols");
            DateTime startMonth = RequestString("startMonth").ToDateTime();
            DateTime endMonth = RequestString("endMonth").ToDateTime();
            var arr = cols.Split(',');
            if (string.IsNullOrEmpty(cols))
                return MessageBoxAndReturn("请先至少选择一个要导出的字段");

            if (!CheckSqlField(arr))
                return MessageBoxAndReturn("选择导出的字段异常");

            DataTable dt = NewExportDt(arr, ref cols, startMonth, endMonth);

            var list = Bll.BllSupplier_RawMaterialPrice.GetList(files, key, priceFrequency);
            var pricelist = Bll.BllSupplier_Price.GetList(startMonth, endMonth);
            ToExcel(list, pricelist, dt, cols, startMonth, endMonth);

            return MessageBoxAndClose("导出成功");
        }

        //填充表头
        private DataTable NewExportDt(string[] arr, ref string cols, DateTime startMonth, DateTime endMonth)
        {
            DataTable dt = new DataTable();
            DataRow rows = dt.NewRow();
            int rowIndex = 0;

            //临时字段，后面做查询条件用
            NewDtHead(ref dt, ref rows, rowIndex, "PriceId", "PriceId");
            cols += ",PriceId";
            rowIndex++;

            foreach (var item in arr)
            {
                switch (item)
                {
                    case "BU":
                        NewDtHead(ref dt, ref rows, rowIndex, "业务部门", "BU");
                        break;
                    case "SAPCode":
                        NewDtHead(ref dt, ref rows, rowIndex, "原材料代码", "SAP Code");
                        break;
                    case "Description":
                        NewDtHead(ref dt, ref rows, rowIndex, "采购产品名称", "Description");
                        break;
                    case "Category":
                        NewDtHead(ref dt, ref rows, rowIndex, "品类", "Category");
                        break;
                    case "LeadBuyer":
                        NewDtHead(ref dt, ref rows, rowIndex, "采购负责人", "Lead Buyer");
                        break;
                    case "Code":
                        NewDtHead(ref dt, ref rows, rowIndex, "供应商代码", "Vendor Code");
                        break;
                    case "Name":
                        NewDtHead(ref dt, ref rows, rowIndex, "供应商名称", "Vendor Name");
                        break;
                    case "PriceFrequency":
                        NewDtHead(ref dt, ref rows, rowIndex, "价格频次", "Price Frequency");
                        break;
                    case "Currency":
                        NewDtHead(ref dt, ref rows, rowIndex, "价格单位", "Currency");
                        break;
                }
                rowIndex++;
            }

            while (startMonth <= endMonth)
            {
                string columns = string.Format("'{0}/{1}", startMonth.Year.ToString(), startMonth.Month.ToString());
                dt.Columns.Add(columns, typeof(String));
                rows[rowIndex] = string.Format("'{0}-{1}", startMonth.Year.ToString(), startMonth.Month.ToString());
                startMonth = startMonth.AddMonths(1);
                rowIndex++;
            }

            dt.Rows.Add(rows);
            return dt;
        }

        //填充数据
        public void ToExcel(DataTable list, List<Supplier_Price> pricelist, DataTable excelDt, string cols, DateTime startMonth, DateTime endMonth)
        {
            if (list == null || list.Rows.Count < 1)
                return;

            DataTable dt = excelDt;
            try
            {
                int col = list.Columns.Count;
                foreach (DataRow item in list.Rows)
                {
                    DateTime starttiem = startMonth;
                    DateTime endtime = endMonth;

                    var row = dt.NewRow();

                    int i = 0, j = 0;
                    for (i = 0, j = 0; i < list.Columns.Count; i++, j++)
                    {
                        var colum = list.Columns[i].ToString();
                        //当前字段不在导出范围内则跳过
                        if (!cols.Contains(colum.Replace(" ", "")))
                        {
                            j--;
                            continue;
                        }

                        //价格频次
                        if (colum == "PriceFrequency")
                            row[j] = item[i] != null
                                ? Tools.Enums.Tools.GetEnumDescription(typeof(Models.PriceFrequency), item[i].ToString().ToInt32()).ToString()
                                : "无";
                        else
                            row[j] = item[i]?.ToString();

                    }

                    //循环价格
                    while (starttiem <= endtime)
                    {
                        var price = pricelist.Where(o => o.RawMaterialPriceId.ToString() == row[0].ToString() && o.Years == (starttiem.Year)
                            && o.Month == starttiem.Month).Select(o => o.Price).FirstOrDefault().GetValueOrDefault();

                        row[j] = price.ToString("f2");
                        starttiem = starttiem.AddMonths(1);
                        j++;
                    }

                    dt.Rows.Add(row);
                }
                //删除临时添加的字段
                dt.Columns.Remove("PriceId");
            }
            catch (Exception)
            {
            }

            string fileName = "供应商采购价格系统" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Tools.Tool.ExcelHelper.ToExcelWeb(dt, "凡菲供应商采购价格跟踪系统", fileName + ".xls", 2);
        }

        private void NewDtHead(ref DataTable dt, ref DataRow rows, int rowIndex, string cellName, string cellName2)
        {
            dt.Columns.Add(cellName, typeof(String));
            rows[rowIndex] = cellName2;
        }

        #endregion

        #region 编辑
        public ActionResult Save(int id = 0, int year = 0)
        {
            Supplier_RawMaterialPrice model = null;

            if (id < 1)
                return View(model);

            model = Bll.BllSupplier_RawMaterialPrice.GetModel(id);
            if (model == null)
                return LayerAlertErrorAndReturn("加载原材料价格信息失败，未找到该原材料价格");

            var list = Bll.BllSupplier_Price.GetSelectList(0, "Id,RawMaterialPriceId,Years,Month,Price,Display,YearsMonth"
                , "RawMaterialPriceId=" + model.Id, "YearsMonth");

            var selectList = list.Select(o => o.Years).Distinct();
            ViewBag.SelectList = CreateSelectList(selectList, year);

            if (list == null || list.Count() < 1)
                return View(model);

            //显示的价格年份
            int years = year < 1 ? selectList.First().GetValueOrDefault() : year;

            var priceModel = list.Where(o => o.Years == years).ToList();
            if (priceModel == null || priceModel.Count() < 1)
                return View(model);

            ViewBag.prices2 = priceModel;

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(Supplier_RawMaterialPrice rawmaterialPrice, int Years = 0)
        {
            Supplier_RawMaterialPrice model = Bll.BllSupplier_RawMaterialPrice.First(o => o.Id == rawmaterialPrice.Id && o.Display != 2) ?? new Supplier_RawMaterialPrice();

            //如果原材料不存在，返回提示
            var rawMaterialModel = Bll.BllSupplier_RawMaterial.First(o => o.SAPCode == rawmaterialPrice.SAPCode && o.Display != 2);
            if (rawMaterialModel == null)
                return LayerAlertErrorAndReturn("原材料代码不存在，请重新输入");
            //根据供应商信息，查询供应商ID，如果供应商不存在，返回提示
            var supplierModel = Bll.BllSupplier_List.First(o => o.Code == rawmaterialPrice.Code && o.Display != 2 && o.Id > 0);
            if (supplierModel == null)
                return LayerAlertErrorAndReturn("供应商代码不存在，请重新输入");

            //保存价格
            model.RawMaterialId = rawMaterialModel.SAPCode;
            model.VendorId = supplierModel.Code;
            model.PriceFrequency = rawmaterialPrice.PriceFrequency;
            model.LastUserId = MyInfo.Id;
            model.LastDate = DateTime.Now;
            model.Display = 1;
            bool isok = false;

            DateTime datetime = DateTime.Now;

            //编辑报价表
            if (rawmaterialPrice.Id < 1)
            {
                //相同的原材料和相同的供应商不能重复添加
                var rawmaterialPriceModel = Bll.BllSupplier_RawMaterialPrice.First(o => o.VendorId == model.VendorId && o.RawMaterialId == model.RawMaterialId
                && o.Display != 2);
                if (rawmaterialPriceModel != null)
                    return LayerAlertErrorAndReturn("该原材料对应的供应商已存在，请选择修改操作");

                model.AddDate = model.LastDate;
                model.AddUserId = model.LastUserId;
                model.Id = Bll.BllSupplier_RawMaterialPrice.Insert(model);
                if (model.Id < 1)
                    return LayerAlertErrorAndReturn("添加失败，请联系管理员");
            }
            else
            {
                isok = Bll.BllSupplier_RawMaterialPrice.Update(model) > 0;
                if (!isok)
                    return LayerAlertErrorAndReturn("修改失败，请联系管理员");
            }

            List<Supplier_Price> prices = new List<Supplier_Price>();
            for (int i = 1; i < 13; i++)
            {
                prices.Add(new Supplier_Price
                {
                    RawMaterialPriceId = model.Id,
                    Years = Years,
                    Month = i,
                    Price = RequestDecimal("month" + i),
                    AddDate = datetime,
                    AddUserId = MyInfo.Id,
                    LastDate = datetime,
                    LastUserId = MyInfo.Id,
                    Display = 1,
                    YearsMonth = $"{Years}-{i}-1".ToDateTime()
                });
            }

            //编辑价格表
            isok = Bll.BllSupplier_Price.InsertPrice(Years, model.Id, prices);

            return isok ? LayerAlertSuccessAndRefresh("保存成功") : LayerAlertErrorAndReturn("保存失败");
        }

        [HttpGet]
        public JsonResult GetInfoByFields(string fields = "", string value = "")
        {
            object result = new { Result = "0" };
            if (fields == "sapcode")
            {
                var rawMaterial = Bll.BllSupplier_RawMaterial.First(o => o.SAPCode == value);

                if (rawMaterial != null)
                    result = new { Result = "1", Name = rawMaterial.Description, currency = rawMaterial.Currency };
            }
            else if (fields == "code")
            {
                var supplier = Bll.BllSupplier_List.First(o => o.Code == value);
                if (supplier != null)
                    result = new { Result = "1", Name = supplier.Name };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //拼接Select下拉框
        public string CreateSelectList(IEnumerable<int?> list, int year)
        {
            string result = string.Empty;
            foreach (var item in list)
            {
                result += string.Format("<option value=\"{0}\" {2}>{1}</option>", item, item, item == year ? "selected" : "");
            }

            return result;
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { result = Bll.BllSupplier_RawMaterialPrice.HideModel(id, MyInfo.Id) > 0 });
        }

        //[PermissionFilter("/supplier/rawmaterialprice/delete")]
        [HttpPost]
        public JsonResult DeleteBatch(List<Supplier_RawMaterialPrice> list)
        {
            if (list == null || !list.Any()) return Json(new { result = false });
            return Json(new { result = Bll.BllSupplier_RawMaterialPrice.HideList(list, MyInfo.Id) > 0 });
        }
        #endregion

        #region 数据图表
        public ActionResult ShowCharts(int[] id = null, string startdate = "", string enddate = "")
        {
            if (id == null || string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate))
                return LayerAlertErrorAndClose("请先勾选需要对比的原材料价格并选择日期");

            ViewBag.ids = string.Join("&id=", id);
            ViewBag.startdate = startdate;
            ViewBag.enddate = enddate;

            return View();
        }

        //[PermissionFilter("/supplier/rawmaterialprice/showcharts")]
        public JsonResult GetChartsData(int[] id = null, string startdate = "", string enddate = "", int chartType = 1)
        {
            DateTime startMonth, endMonth;
            GetDate(startdate, enddate, out startMonth, out endMonth);
            var pricelist = Bll.BllSupplier_Price.GetList(startMonth, endMonth);
            var list = Bll.BllSupplier_RawMaterialPrice.GetList(id) ?? new List<Supplier_RawMaterialPrice>();
            var rawMaterialList = Bll.BllSupplier_RawMaterial.GetSelectList(0, "Id,SAPCode,Description,Currency", "display!=2", "")
                ?? new List<Supplier_RawMaterial>();
            var supplierList = Bll.BllSupplier_List.GetSelectList(0, "Id,Code,Name", "display!=2", "") ?? new List<Supplier_List>();

            List<string> legend = new List<string>();
            List<string> series = new List<string>();
            List<string> xAxis = new List<string>();
            StringBuilder str = new StringBuilder();
            List<string> prices = new List<string>();
            //按月统计
            if (chartType == 1)
            {
                foreach (var item in list)
                {
                    DateTime start = startMonth, end = endMonth;
                    //填充legend
                    var rawMaterial = rawMaterialList.Where(o => o.SAPCode == item.RawMaterialId).FirstOrDefault();
                    var rawMaterialName = rawMaterial?.Description;
                    var rawMaterialCurrency = rawMaterial?.Currency;
                    var supplierName = supplierList.Where(o => o.Code == item.VendorId).FirstOrDefault()?.Name;
                    legend.Add($"{rawMaterialName}_{supplierName}");

                    //循环价格
                    while (start <= end)
                    {
                        var price = pricelist.Where(o => o.RawMaterialPriceId.ToString() == item.Id.ToString() && o.Years == (start.Year)
                            && o.Month == start.Month).Select(o => o.Price).FirstOrDefault().GetValueOrDefault();

                        prices.Add(price.ToString("f2"));

                        if (str.Length < 1)
                            xAxis.Add($"{start.Year}-{start.Month}");

                        start = start.AddMonths(1);
                    }

                    if (str.Length < 1)
                        str.Append(string.Join(",", xAxis));
                    series.Add("{" + $"\"name\":\"{ rawMaterialName}_{supplierName}\",\"type\":\"bar\",\"data\":[{string.Join(",", prices)}]" + "}");

                    prices.Clear();
                }

            }
            //按季度统计
            else if (chartType == 2)
            {
                foreach (var item in list)
                {
                    DateTime start = startMonth, end = endMonth;
                    var rawMaterial = rawMaterialList.Where(o => o.SAPCode == item.RawMaterialId).FirstOrDefault();
                    var rawMaterialName = rawMaterial?.Description;
                    var rawMaterialCurrency = rawMaterial?.Currency;
                    var supplierName = supplierList.Where(o => o.Code == item.VendorId).FirstOrDefault()?.Name;
                    legend.Add($"{ rawMaterialName}_{supplierName}");

                    decimal price = 0;
                    //循环价格
                    while (start <= end)
                    {
                        switch (start.Month)
                        {
                            case 1:
                            case 2:
                            case 3:
                                if (str.Length < 1)
                                    xAxis.Add($"{start.Year}-第一季度");

                                price = pricelist.Where(o => o.RawMaterialPriceId.ToString() == item.Id.ToString() && o.Years == start.Year
                                    && o.Month >= 1 && o.Month <= 3).Select(o => o.Price).FirstOrDefault().GetValueOrDefault();

                                prices.Add(price.ToString("f2"));
                                //结束当前季度
                                start = start.AddMonths(3 - start.Month);
                                break;
                            case 4:
                            case 5:
                            case 6:
                                if (str.Length < 1)
                                    xAxis.Add($"{start.Year}-第二季度");

                                price = pricelist.Where(o => o.RawMaterialPriceId.ToString() == item.Id.ToString() && o.Years == start.Year
                                    && o.Month >= 4 && o.Month <= 6).Select(o => o.Price).FirstOrDefault().GetValueOrDefault();

                                prices.Add(price.ToString("f2"));
                                //结束当前季度
                                start = start.AddMonths(6 - start.Month);
                                break;
                            case 7:
                            case 8:
                            case 9:
                                if (str.Length < 1)
                                    xAxis.Add($"{start.Year}-第三季度");

                                price = pricelist.Where(o => o.RawMaterialPriceId.ToString() == item.Id.ToString() && o.Years == start.Year
                                    && o.Month >= 7 && o.Month <= 9).Select(o => o.Price).FirstOrDefault().GetValueOrDefault();

                                prices.Add(price.ToString("f2"));
                                //结束当前季度
                                start = start.AddMonths(9 - start.Month);
                                break;
                            case 10:
                            case 11:
                            case 12:
                                if (str.Length < 1)
                                    xAxis.Add($"{start.Year}-第四季度");

                                price = pricelist.Where(o => o.RawMaterialPriceId.ToString() == item.Id.ToString() && o.Years == start.Year
                                   && o.Month >= 10 && o.Month <= 12).Select(o => o.Price).FirstOrDefault().GetValueOrDefault();

                                prices.Add(price.ToString("f2"));
                                //结束当前季度
                                start = start.AddMonths(12 - start.Month);
                                break;
                        }

                        start = start.AddMonths(1);
                    }

                    if (str.Length < 1)
                        str.Append(string.Join(",", xAxis));
                    series.Add("{" + $"\"name\":\"{ rawMaterialName}_{supplierName}\",\"type\":\"bar\",\"data\":[{string.Join(",", prices)}]" + "}");

                    prices.Clear();
                }
            }
            //按年统计
            else if (chartType == 3)
            {
                foreach (var item in list)
                {
                    DateTime start = $"{startMonth.Year}-1-1".ToDateTime(), end = endMonth;
                    var rawMaterial = rawMaterialList.Where(o => o.SAPCode == item.RawMaterialId).FirstOrDefault();
                    var rawMaterialName = rawMaterial?.Description;
                    var rawMaterialCurrency = rawMaterial?.Currency;
                    var supplierName = supplierList.Where(o => o.Code == item.VendorId).FirstOrDefault()?.Name;
                    legend.Add($"{ rawMaterialName}_{supplierName}");

                    //循环价格
                    while (start <= end)
                    {
                        var price = pricelist.Where(o => o.RawMaterialPriceId.ToString() == item.Id.ToString()
                            && o.Years == (start.Year)).Select(o => o.Price).Sum().GetValueOrDefault();

                        prices.Add(price.ToString("f2"));

                        if (str.Length < 1)
                            xAxis.Add(start.Year.ToString());

                        start = start.AddYears(1);
                    }

                    if (str.Length < 1)
                        str.Append(string.Join(",", xAxis));
                    series.Add("{" + $"\"name\":\"{ rawMaterialName}_{supplierName}\",\"type\":\"bar\",\"data\":[{string.Join(",", prices)}]" + "}");

                    prices.Clear();
                }
            }

            return Json(new
            {
                seriesData = $"[{string.Join(",", series)}]",
                legend = string.Join(",", legend),
                xAxis = $"{str.ToString()}"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 辅助方法
        private bool CheckSqlField(string[] arr)
        {
            foreach (var item in arr)
            {
                if (!Tools.Usual.Utils.CheckSqlField(item))
                    return false;
            }

            return true;
        }


        #endregion
    }
}
