﻿using FancyFix.Core;
using FancyFix.OA.Model;
using FancyFix.Tools.Json;
using FancyFix.OA.ViewModel;
using FancyFix.OA.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Product.Controllers
{
    public class ProductSkuController : Base.BaseAdminController
    {
        /// <summary>
        /// 产品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            ViewBag.classHtml = Bll.BllProduct_Class.Instance().ShowClassPath(0, "", true);
            return View();
        }

        //[PermissionFilter("/product/productsku/list")]
        [ValidateInput(false)]
        public JsonResult PageList(int page, int pagesize)
        {
            long records = 0;
            string title = RequestString("title");
            string classParPath = RequestString("classparpath");
            string spu = RequestString("spu");
            int isshow = RequestInt("isshow");

            var list = Bll.BllProduct_Info.PageList(title, classParPath, spu, "", isshow, page, pagesize, out records);
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
                model = new Product_Info();
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
        //[PermissionFilter("/product/productsku/edit")]
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

            model.content = TransferImgToLocal(EscapeSpace(model.content ?? ""));

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
            //产品图片
            string firstPic = string.Empty;
            string pics = GetPics("pic", ref firstPic);

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
            //上架时间
            modPro.FirstPic = firstPic;
            modPro.Pics = pics;
            modPro.AdminId = MyInfo.Id;
            modPro.Attribute = Tools.Tool.JsonHelper.Serialize(listAttr);
            modPro.AttributeCustom = Tools.Tool.JsonHelper.Serialize(listAttrCustom);
            modPro.Attachment = GetFile("attachment");
            modPro.Videos = GetFile("video");
            return modPro;
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
            return Json(new { result = Bll.BllProduct_Info.Delete(o => o.Id == id) > 0 });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //[PermissionFilter("/product/productsku/delete")]
        [HttpPost]
        public JsonResult DeleteBatch(List<Product_Info> list)
        {
            return Json(new { result = Bll.BllProduct_Info.DeleteAllBatch(list) });
        }
    }
}