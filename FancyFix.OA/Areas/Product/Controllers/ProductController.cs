using FancyFix.Core;
using FancyFix.OA.Model;
using FancyFix.Tools.Json;
using FancyFix.OA.ViewModel;
using FancyFix.OA.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FancyFix.Tools.Config;
using Tools.Tool;
using NPOI.SS.UserModel;
using System.Web;
using FancyFix.OA.Config;

namespace FancyFix.OA.Areas.Product.Controllers
{
    [AllowAnonymous]
    public class ProductController : Base.BaseAdminController
    {
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var tagList = Bll.BllProduct_ImageTag.GetAll();
            ViewBag.classHtml = Bll.BllProduct_Class.Instance().ShowClassPath(0, "", true);
            ViewBag.importClassHtml = Bll.BllProduct_Class.Instance().ShowClass(0, 0, true);
            ViewBag.taglist = tagList;
            return View();
        }

        //[PermissionFilter("/product/product/list")]
        [ValidateInput(false)]
        public JsonResult PageList(int page, int pagesize)
        {
            long records = 0;
            string title = RequestString("title");
            string classParPath = RequestString("classparpath");
            string spu = RequestString("spu");
            int isshow = RequestInt("isshow");
            string tag = RequestString("tag");

            var list = Bll.BllProduct_Info.PageList(title, classParPath, spu, tag, isshow, page, pagesize, out records);
            foreach (var item in list)
            {
                item.Url = GetProductUrl(item.Url, item.Id);
            }
            return BspTableJson(list, records);
        }

        /// <summary>
        /// 产品编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            Product_Info model = null;
            string parProNo = string.Empty;
            string parSize = string.Empty;
            string parColor = string.Empty;
            if (id == 0)
            {
                int similarId = RequestInt("similarId");
                if (similarId > 0)
                {
                    model = Bll.BllProduct_Info.First(o => o.Id == similarId);
                    if (model != null)
                    {
                        model.Id = 0;
                        model.ClassId = 0;
                        model.ClassId_1 = 0;
                        model.ClassId_2 = 0;
                        model.ClassName = "";
                        model.ClassParPath = "";
                        model.Spu = "";
                        model.Old_Spu = "";
                        model.PatternId = 0;
                        model.Pattern = "";
                    }
                }
                if (model == null)
                {
                    model = new Product_Info();
                }
            }
            else
            {
                model = Bll.BllProduct_Info.First(o => o.Id == id);
                if (model == null) return LayerAlertErrorAndClose("产品不存在！");
            }

            //属性列表
            var attrlist = Tools.Tool.JsonHelper.Deserialize<AttrJson>(model?.Attribute ?? "");
            var attrlistCustom = Tools.Tool.JsonHelper.Deserialize<AttrJson>(model?.AttributeCustom ?? "");

            ViewBag.attrHtml = GetAttrStr(model?.ClassId ?? 0, attrlist, attrlistCustom, "skuObj");
            ViewBag.classHtml = Bll.BllProduct_Class.Instance().ShowClass(0, (model?.ClassId ?? 0), true);
            ViewBag.patternOptions = Bll.BllProduct_Pattern.GetOptions(model?.PatternId ?? 0);
            ViewBag.parProNo = parProNo;
            return View(model);
        }

        /// <summary>
        /// 产品保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[PermissionFilter("/product/product/edit")]
        [HttpPost]
        [ValidateInput(false)]
        [ModelValidFilter]
        public ActionResult Save(ProductInfoModel model)
        {
            Product_Info modPro = null;

            //产品分类验证
            if (model.classid == 0) return LayerAlertErrorAndReturn("请选择一个产品分类！");
            Product_Class modClass = null;
            if (model.classid > 0)
            {
                modClass = Bll.BllProduct_Class.First(o => o.Id == model.classid);
                if (modClass == null) return LayerAlertErrorAndReturn("分类不存在！");
            }

            //spu编号重复验证
            if (model.id == 0 && Bll.BllProduct_Info.IsExistsSpu(model.spu))
                return LayerAlertErrorAndReturn("该SPU编号已存在，请修改！");

            //产品属性
            List<Product_AttributeSet> attrsetlist = new List<Product_AttributeSet>();//属性关联
            List<AttrJson> listAttr = GetAttrList(model.classid, ref attrsetlist);//分类配置属性
            List<AttrJson> listAttrCustom = GetCustomAttrList("myAttrId", "myAttrName", "myAttrValue");//自定义属性

            if (model.id == 0)
            {
                modPro = new Product_Info();
                //DTO转换，模型数据处理
                modPro = BindProModel(model.MapTo(modPro), modClass, listAttr, listAttrCustom);
                modPro.CreateDate = DateTime.Now;
                modPro.Url = GetFormatUrl(model.title);
                int proId = Bll.BllProduct_Info.Insert(modPro);
                if (proId > 0)
                {
                    //绑定可筛选属性
                    InsertAttrlist(attrsetlist, proId);
                    //更新编码排序
                    Bll.BllProduct_CodeSequence.UpdateSequence(modPro.ClassId, model.spu);
                    return LayerAlertSuccessAndRefresh("添加成功");
                }
                else
                {
                    return LayerAlertErrorAndReturn("添加失败");
                }
            }
            else
            {
                //获取产品信息
                modPro = Bll.BllProduct_Info.First(o => o.Id == model.id);
                if (modPro == null) return LayerAlertErrorAndReturn("产品不存在！");
                //绑定模型
                modPro = BindProModel(model.MapTo(modPro), modClass, listAttr, listAttrCustom);
                modPro.Url = GetFormatUrl(model.title);
                modPro.UpdateDate = DateTime.Now;
                if (Bll.BllProduct_Info.Update(modPro, o => o.Id == modPro.Id) > 0)
                {
                    //绑定可筛选属性
                    InsertAttrlist(attrsetlist, modPro.Id);
                    //更新编码排序
                    Bll.BllProduct_CodeSequence.UpdateSequence(modPro.ClassId, model.spu);
                    return LayerAlertSuccessAndRefresh("修改成功");
                }
                else
                {
                    return LayerAlertErrorAndReturn("修改失败");
                }
            }
        }

        //添加属性可筛选项
        private bool InsertAttrlist(List<Product_AttributeSet> attrsetlist, int proId)
        {
            if (attrsetlist == null || attrsetlist.Count == 0 || proId == 0) return false;
            return Bll.BllProduct_AttributeSet.Add(attrsetlist, proId, true);
        }

        //产品模型绑定处理
        private Product_Info BindProModel(Product_Info modPro, Product_Class modClass, List<AttrJson> listAttr, List<AttrJson> listAttrCustom)
        {
            //分类信息
            if (modClass != null)
            {
                var classIds = modClass.ParPath.TrimEnd(',').Split(',');
                modPro.ClassId_1 = classIds.Length > 0 ? classIds[0].ToInt32() : 0;
                modPro.ClassId_2 = classIds.Length > 1 ? classIds[1].ToInt32() : 0;
                modPro.ClassId = modClass.Id;
                modPro.ClassParPath = modClass.ParPath;
            }
            //产品特性
            string features = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                string temp = RequestString("feature" + i);
                if (temp != "") features += temp + "|";
            }
            modPro.Features = features.TrimEnd('|');
            //图案
            int patternid = RequestInt("patternid");
            if (patternid > 0)
            {
                modPro.PatternId = patternid;
                modPro.Pattern = Bll.BllProduct_Pattern.GetPatternName(patternid);
            }

            modPro.AdminId = MyInfo.Id;
            modPro.Attribute = Tools.Tool.JsonHelper.Serialize(listAttr);
            modPro.AttributeCustom = Tools.Tool.JsonHelper.Serialize(listAttrCustom);
            modPro.Attachment = GetFiles("attachment");
            modPro.FirstPic = GetPic("pic");
            return modPro;
        }

        /// <summary>
        /// 添加SPU
        /// </summary>
        /// <returns></returns>
        public ActionResult AddSpu()
        {
            ViewBag.classHtml = Bll.BllProduct_Class.Instance().ShowClass(0, 0, true);
            return View();
        }

        /// <summary>
        /// 保存Spu
        /// </summary>
        /// <returns></returns>
        //[PermissionFilter("/product/product/addspu")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveSpu()
        {
            Product_Info modPro = new Product_Info();
            int classId = RequestInt("classid");
            string spu = RequestString("spu");

            //产品分类验证
            Product_Class modClass = null;
            if (classId > 0)
            {
                modClass = Bll.BllProduct_Class.First(o => o.Id == classId);
                if (modClass == null) return LayerAlertErrorAndReturn("分类不存在！");
                var classIds = modClass.ParPath.TrimEnd(',').Split(',');
                modPro.ClassId_1 = classIds.Length > 0 ? classIds[0].ToInt32() : 0;
                modPro.ClassId_2 = classIds.Length > 1 ? classIds[1].ToInt32() : 0;
                modPro.ClassId = modClass.Id;
                modPro.ClassParPath = modClass.ParPath;
            }
            else
            {
                return LayerAlertErrorAndReturn("请选择一个产品分类！");
            }

            //spu编号重复验证
            if (Bll.BllProduct_Info.IsExistsSpu(spu))
                return LayerAlertErrorAndReturn("该SPU编号已存在，请修改！");

            modPro.Spu = spu;
            modPro.IsShow = false;
            modPro.CreateDate = DateTime.Now;
            modPro.AdminId = MyInfo.Id;
            int proId = Bll.BllProduct_Info.Insert(modPro);
            if (proId > 0)
            {
                return LayerAlertSuccessAndRefresh("添加成功");
            }
            else
            {
                return LayerAlertErrorAndReturn("添加失败");
            }
        }

        /// <summary>
        /// 添加产品资源
        /// </summary>
        /// <returns></returns>

        public ActionResult AddResource(int id)
        {
            Product_Info model = null;
            if (id == 0) return LayerAlertErrorAndReturn("产品不存在！");
            model = Bll.BllProduct_Info.First(o => o.Id == id);
            if (model == null) return LayerAlertErrorAndClose("产品不存在！");

            string className = string.Empty;
            if (model.ClassId > 0)
            {
                className = Bll.BllSys_Class<Product_Class>.Instance().GetClassName(model.ClassId);
            }
            ViewBag.className = className;
            return View(model);
        }

        /// <summary>
        /// 保存产品资源
        /// </summary>
        /// <returns></returns>
        //[PermissionFilter("/product/product/addresource")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveResource()
        {
            Product_Info modPro = null;
            int id = RequestInt("id");
            if (id == 0) return LayerAlertErrorAndReturn("产品不存在！");
            modPro = Bll.BllProduct_Info.First(o => o.Id == id);
            if (modPro == null) return LayerAlertErrorAndReturn("产品不存在！");

            //资源部分
            string content = RequestString("content");
            string firstPic = string.Empty;
            string pics = GetPics("pic", ref firstPic);

            modPro.Content = TransferImgToLocal(EscapeSpace(content));
            modPro.Pics = pics;
            modPro.FirstPic = firstPic;
            modPro.Videos = GetFiles("video");
            modPro.AIFile = GetFiles("aifile");
            modPro.AdminId = MyInfo.Id;
            int rows = Bll.BllProduct_Info.Update(modPro);
            if (rows > 0)
            {
                return LayerAlertSuccessAndRefresh("添加成功");
            }
            else
            {
                return LayerAlertErrorAndReturn("添加失败");
            }
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetShow(int id)
        {
            return Json(new { result = Bll.BllProduct_Info.SetShow(id) });
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            int rows = Bll.BllProduct_Info.Delete(o => o.Id == id);
            //删除图片
            if (rows > 0)
            {
                Bll.BllProduct_Image.DeletePics(id);
                Bll.BllProduct_Files.DeleteFiles(id);
            }
            return Json(new { result = rows > 0 });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //[PermissionFilter("/product/product/delete")]
        [HttpPost]
        public JsonResult DeleteBatch(List<Product_Info> list)
        {
            return Json(new { result = Bll.BllProduct_Info.DeleteAllBatch(list) });
        }

        /// <summary>
        /// 导入产品
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportProduct(HttpPostedFileBase file)
        {
            try
            {
                int classId = RequestInt("classId");
                if (classId == 0) return LayerAlertErrorAndReturn("请选择一个分类！");
                var classModel = Bll.BllProduct_Class.First(o => o.Id == classId);
                if (classModel == null) return LayerAlertErrorAndClose("分类不存在！");
                if (classModel.ChildNum > 0) return LayerAlertErrorAndReturn("请选择一个最底层分类！");

                Tools.Config.UploadConfig config = UploadProvice.Instance();
                SiteOption option = config.SiteOptions["local"];
                string filePath = option.Folder + config.Settings["file"].FilePath + DateTime.Now.ToString("yyyyMMddHHmmss")
                        + (file.FileName.IndexOf(".xlsx") > 0 ? ".xlsx" : ".xls");

                string result = FileHelper.ValicationAndSaveFileToPath(file, filePath);
                if (result != "0")
                    return MessageBoxAndReturn($"上传失败，{result}！");

                var sheet = Tools.Tool.ExcelHelper.ReadExcel(filePath);
                if (sheet == null)
                    return MessageBoxAndReturn($"Excel读取失败，请检查格式！");
                if (sheet.LastRowNum < 1)
                    return MessageBoxAndReturn($"Excel无数据！");

                int succuessCount = 0;
                List<Product_Info> list = new List<Product_Info>();
                IRow row;
                for (int i = 1; i <= sheet.LastRowNum; i++)  //从第二行开始读取
                {
                    row = sheet.GetRow(i);   //第i行数据  
                    if (row != null)
                    {
                        string title = row.GetCell(0)?.ToString();
                        if (string.IsNullOrEmpty(title)) continue; //标题为空，跳过
                        if (Bll.BllProduct_Info.Any(o => o.Title == title)) continue; //标题和老SPU已存在，跳过

                        //产品Spu
                        var classIds = classModel.ParPath.TrimEnd(',').Split(',');
                        string spu = Bll.BllProduct_Class.GetSpuCode(classId);
                        if (string.IsNullOrEmpty(spu)) continue; //spu编码生成失败，跳过
                        if (Bll.BllProduct_Info.IsExistsSpu(spu)) continue; //Spu重复，跳过

                        string titleEn = row.GetCell(1)?.ToString();
                        string oldSpu = row.GetCell(2)?.ToString();
                        string supplierCode = row.GetCell(3)?.ToString();
                        string supplierName = row.GetCell(4)?.ToString();
                        string specification = row.GetCell(5)?.ToString();
                        string priceRemark = row.GetCell(6)?.ToString();
                        string hsCode = row.GetCell(7)?.ToString();
                        string InvoiceName = row.GetCell(8)?.ToString();
                        string firstPic = row.GetCell(9)?.ToString();
                        var model = new Product_Info()
                        {
                            ClassId_1 = classIds.Length > 0 ? classIds[0].ToInt32() : 0,
                            ClassId_2 = classIds.Length > 1 ? classIds[1].ToInt32() : 0,
                            ClassId = classModel.Id,
                            ClassParPath = classModel.ParPath,
                            Title = title,
                            Title_En = titleEn,
                            IsShow = true,
                            Old_Spu = oldSpu,
                            Spu = spu,
                            SupplierProductCode = supplierCode,
                            SupplierName = supplierName,
                            Specification = specification,
                            PriceRemark = priceRemark,
                            HS_Code = hsCode,
                            InvoiceName = InvoiceName,
                            AdminId = MyInfo.Id,
                            CreateDate = DateTime.Now,
                            PriceUnit = "$",
                            Currency = "USD",
                            Price = 0,
                            TaxPrice = 0,
                            Moq = 1,
                            FirstPic = ImgConfig.GetProductUrl(firstPic)
                        };
                        if (Bll.BllProduct_Info.Insert(model) > 0)
                            succuessCount++;
                    }
                }
                if (succuessCount > 0)
                    return MessageBoxAndReturn("导入成功！");
                else
                    return MessageBoxAndReturn("导入失败，请联系管理员！");
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(ProductController), ex, MyInfo.Id, MyInfo.RealName);
                return MessageBoxAndReturn("导入失败，请联系管理员！" + ex);
            }
        }
    }
}