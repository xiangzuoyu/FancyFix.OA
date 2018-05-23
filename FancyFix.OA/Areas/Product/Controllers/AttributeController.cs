using FancyFix.OA.Areas.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FancyFix.OA.Model;
using FancyFix.Core;
using System.Text;
using FancyFix.OA.Filter;

namespace FancyFix.OA.Areas.Product.Controllers
{
    public class AttributeController : Base.BaseAdminController
    {
        #region 属性列表页面
        public ActionResult List()
        {
            int classId = RequestInt("classId");
            var list = Bll.BllProduct_Attribute.GetListByClassId(classId).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].inputTypeStr = Tools.Enums.Tools.GetEnumDescription(typeof(Tools.Enums.ESite.AttrType), list[i].InputType);
                list[i].actStr = GetActHtmlAttr(list.Count, i, list[i].Id, list[i].Sequence.Value, list[i].ClassId.Value);
                list[i].defaultValueStr = GetDefaultValueString(list[i].Id, list[i].InputType.Value, list[i].DefaultValue);
            }

            ViewBag.list = list;
            ViewBag.classId = classId;
            ViewBag.className = Bll.BllProduct_Class.Instance().GetClassName(classId);
            return View();
        }

        /// <summary>
        /// 排序HTML
        /// </summary>
        /// <param name="listCount"></param>
        /// <param name="i"></param>
        /// <param name="id"></param>
        /// <param name="sequence"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        string GetActHtmlAttr(int listCount, int i, int id, int sequence, int classId)
        {
            StringBuilder actStr = new StringBuilder();
            actStr.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100\"><tr><form name=\"upform\"><td width=\"50\">");
            if (i > 0)
            {
                actStr.Append("<input type=\"button\" name=\"Submit2\" value=\"↑\" class=\"btn btn-default\" onclick=\"window.location.href='/product/attribute/up?id=" + id + "&classId=" + classId + "&sequence=" + sequence + "'\">");
            }

            actStr.Append("</td></form><form name=\"downform\"><td  width=\"50\">");

            if (i < listCount - 1)
            {
                actStr.Append("<input type=\"button\" name=\"Submit2\" value=\"↓\" class=\"btn btn-default\" onclick=\"window.location.href='/product/attribute/down?id=" + id + "&classId=" + classId + "&sequence=" + sequence + "'\">");
            }
            actStr.Append("</td></form></tr></table>");
            return actStr.ToString();
        }

        /// <summary>
        /// 取得默认值
        /// </summary>
        /// <param name="attrid">属性id</param>
        /// <param name="inputType">控件类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        string GetDefaultValueString(int attrid, int inputType, string defaultValue)
        {
            string str = string.Empty;
            if (inputType == (int)Tools.Enums.ESite.AttrType.Text)
            {
                str = "<input style='width:150px' class='form-control' type='input' readonly='readonly' value='" + defaultValue + "' />";
            }
            else if (inputType == (int)Tools.Enums.ESite.AttrType.Dropdownlist || inputType == (int)Tools.Enums.ESite.AttrType.CheckBox)
            {
                str += "<select class='form-control' style='width:150px'>";
                str += "<option value=''>==请选择==</option>";
                var list = Bll.BllProduct_AttributeSelect.ListByAttrId(attrid);
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        str += "<option value='" + item.ItemName + "' " + (item.ItemName.ToString() == defaultValue ? "selected" : "") + ">" + item.ItemName + "</option>";
                    }
                }
                str += "</select>";
            }
           
            return str;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public int Delete(int id)
        {
            return Bll.BllProduct_Attribute.Instance().Delete(id);
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        /// <returns></returns>
        public ActionResult SetBeLock()
        {
            int id = RequestInt("id");
            int classId = RequestInt("classId");
            if (id == 0)
                return LayerAlertErrorAndReturn("访问出错！");
            if (classId == 0)
                return LayerAlertErrorAndReturn("分类有误！");

            Bll.BllProduct_Attribute.SetShow(id);
            return RedirectToAction("list", "attribute", new { area = "product", classId = classId });
        }

        /// <summary>
        /// 设置必填
        /// </summary>
        /// <returns></returns>
        public ActionResult SetRequired()
        {
            int id = RequestInt("id");
            int classId = RequestInt("classId");
            if (id == 0)
                return LayerAlertErrorAndReturn("访问出错！");
            if (classId == 0)
                return LayerAlertErrorAndReturn("分类有误！");

            Bll.BllProduct_Attribute.SetRequired(id);
            return RedirectToAction("list", "attribute", new { area = "product", classId = classId });
        }

        /// <summary>
        /// 设置可筛选
        /// </summary>
        /// <returns></returns>
        public ActionResult SetSort()
        {
            int id = RequestInt("id");
            int classId = RequestInt("classId");
            if (id == 0)
                return LayerAlertErrorAndReturn("访问出错！");
            if (classId == 0)
                return LayerAlertErrorAndReturn("分类有误！");

            Bll.BllProduct_Attribute.SetSort(id);
            return RedirectToAction("list", "attribute", new { area = "product", classId = classId });
        }

        /// <summary>
        /// 设置主要属性
        /// </summary>
        /// <returns></returns>
        public ActionResult SetSpecial()
        {
            int id = RequestInt("id");
            int classId = RequestInt("classId");
            if (id == 0)
                return LayerAlertErrorAndReturn("访问出错！");
            if (classId == 0)
                return LayerAlertErrorAndReturn("分类有误！");

            if (!Bll.BllProduct_Attribute.SetSpecial(id, classId))
                return LayerAlertErrorAndReturn("设置失败！！主要属性只能设置两个，请先取消一个已设置的主要属性！");
            return RedirectToAction("list", "attribute", new { area = "product", classId = classId });
        }

        /// <summary>
        /// 下移分类
        /// </summary>
        public ActionResult Down()
        {
            int moveId = RequestInt("id");
            int classId = RequestInt("classId");
            int sequence = RequestInt("sequence");
            if (classId == 0)
                return LayerAlertErrorAndReturn("分类有误！");
            if (moveId == 0)
                return LayerAlertErrorAndReturn("请选择移动属性！");
            if (Bll.BllProduct_Attribute.Instance().SetDown(moveId, classId, sequence))
                return RedirectToAction("list", "attribute", new { area = "product", classId = classId });
            else
                return LayerAlertErrorAndReturn("设置失败！");
        }

        /// <summary>
        /// 上移分类
        /// </summary>
        /// <returns></returns>
        public ActionResult Up()
        {
            int moveId = RequestInt("id");
            int classId = RequestInt("classId");
            int sequence = RequestInt("sequence");
            if (classId == 0)
                return LayerAlertErrorAndReturn("分类有误！");
            if (moveId == 0)
                return LayerAlertErrorAndReturn("请选择移动属性！");
            if (Bll.BllProduct_Attribute.Instance().SetUp(moveId, classId, sequence))
                return RedirectToAction("list", "attribute", new { area = "product", classId = classId });
            else
                return LayerAlertErrorAndReturn("设置失败！");
        }
        #endregion

        #region 属性编辑页面
        /// <summary>
        /// 编辑新增页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            int classId = RequestInt("classId");
            if (classId == 0)
                return LayerAlertErrorAndClose("分类有误！");

            int id = RequestInt("id");
            Product_Attribute model = null;
            if (id > 0)
            {
                model = Bll.BllProduct_Attribute.First(o => o.Id == id);
                if (model == null)
                    return LayerAlertErrorAndClose("属性不存在！");
            }

            ViewBag.hasSpecail = Bll.BllProduct_Attribute.hasSpecial(classId);
            ViewBag.classId = classId;
            ViewBag.inputTypeSelect = Tools.Enums.Tools.GetOptionHtml(typeof(Tools.Enums.ESite.AttrType), (byte)(model?.InputType ?? 0));
            ViewBag.fieldTypeSelect = Tools.Enums.Tools.GetOptionHtml(typeof(Tools.Enums.ESite.AttrValueType), (byte)(model?.FieldType ?? 0));
            return View(model);
        }


        /// <summary>
        /// 保存属性
        /// </summary>
        /// <returns></returns>
        //[PermissionFilter("/product/attribute/edit")]
        [HttpPost]
        public ActionResult Save(Product_Attribute model)
        {
            Product_Attribute mod = null;
            bool IsRequired = RequestBool("isneeded");
            bool isSpecial = RequestBool("isspecial");

            if (model.Id > 0)
            {
                mod = Bll.BllProduct_Attribute.First(o => o.Id == model.Id);
                if (mod == null)
                    return LayerAlertErrorAndClose("属性不存在！");
                mod.InputType = model.InputType;
                mod.IsRequired = IsRequired;
                mod.Remark = model.Remark;
                mod.AttributeName = model.AttributeName;
                mod.FieldType = model.FieldType;
                if (mod.IsSpecial.HasValue && mod.IsSpecial.Value)
                    mod.IsSpecial = isSpecial;
                else
                    mod.IsSpecial = Bll.BllProduct_Attribute.hasSpecial(model.ClassId.Value) ? false : isSpecial;
                if (Bll.BllProduct_Attribute.Update(mod, o => o.Id == model.Id) > 0)
                    return LayerAlertSuccessAndRefreshPage("修改成功！");
                else
                    return LayerAlertErrorAndClose("修改失败！");
            }
            else
            {
                mod = new Product_Attribute();
                mod.InputType = model.InputType;
                mod.IsRequired = IsRequired;
                mod.Remark = model.Remark;
                mod.AttributeName = model.AttributeName;
                mod.FieldType = model.FieldType;
                mod.ClassId = model.ClassId;
                mod.DefaultValue = "";
                mod.IsShow = false;
                mod.IsSort = false;
                mod.Unit = "";
                mod.IsSpecial = Bll.BllProduct_Attribute.hasSpecial(model.ClassId.Value) ? false : isSpecial;
                mod.Sequence = Bll.BllSys_Class<Product_Attribute>.Instance().GetMaxSequence("ClassID=" + model.ClassId);
                if (Bll.BllProduct_Attribute.Add(mod) > 0)
                    return LayerAlertSuccessAndRefreshPage("添加成功！");
                else
                    return LayerAlertErrorAndClose("添加失败！");
            }
        }
        #endregion

        #region 属性值列表页面

        public ActionResult ValueList()
        {
            int attrid = RequestInt("attrid");
            if (attrid == 0)
                LayerAlertErrorAndClose("属性有误！");
            var model = Bll.BllProduct_Attribute.First(o => o.Id == attrid);
            if (model == null)
                LayerAlertErrorAndClose("属性有误！");

            if (model.InputType == (int)Tools.Enums.ESite.AttrType.Text)
            {
                return RedirectToAction("ValueEdit", new { attrId = attrid });
            }

            var list = Bll.BllProduct_AttributeSelect.ListByAttrId(attrid);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].actStr = GetActHtmlAttrValue(list.Count, i, list[i].Id, list[i].Sequence.Value, list[i].AttributeId.Value);
            }

            ViewBag.modAttr = model;
            ViewBag.list = list;
            ViewBag.attrId = attrid;
            return View();
        }

        /// <summary>
        /// 排序HTML
        /// </summary>
        /// <param name="listCount"></param>
        /// <param name="i"></param>
        /// <param name="id"></param>
        /// <param name="sequence"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        string GetActHtmlAttrValue(int listCount, int i, int id, int sequence, int attrId)
        {
            StringBuilder actStr = new StringBuilder();
            actStr.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100\"><tr><form name=\"upform\"><td width=\"50\">");
            if (i > 0)
            {
                actStr.Append("<input type=\"button\" name=\"Submit2\" value=\"↑\" class=\"btn btn-default\" onclick=\"window.location.href='/product/attribute/valueup?id=" + id + "&attrid=" + attrId + "&sequence=" + sequence + "'\">");
            }

            actStr.Append("</td></form><form name=\"downform\"><td  width=\"50\">");

            if (i < listCount - 1)
            {
                actStr.Append("<input type=\"button\" name=\"Submit2\" value=\"↓\" class=\"btn btn-default\" onclick=\"window.location.href='/product/attribute/valuedown?id=" + id + "&attrid=" + attrId + "&sequence=" + sequence + "'\">");
            }
            actStr.Append("</td></form></tr></table>");
            return actStr.ToString();
        }

        [HttpPost]
        public int ValueDelete(int id)
        {
            return Bll.BllProduct_AttributeSelect.Delete(o => o.Id == id);
        }

        /// <summary>
        /// 下移分类
        /// </summary>
        public ActionResult ValueDown()
        {
            int moveId = RequestInt("id");
            int attrid = RequestInt("attrid");
            int sequence = RequestInt("sequence");
            if (attrid == 0)
                return LayerAlertErrorAndReturn("属性有误！");
            if (moveId == 0)
                return LayerAlertErrorAndReturn("请选择移动属性值！");
            if (Bll.BllProduct_AttributeSelect.Instance().SetDown(moveId, attrid, sequence))
                return RedirectToAction("valuelist", "attribute", new { area = "product", attrid = attrid });
            else
                return LayerAlertErrorAndReturn("设置失败！");
        }

        /// <summary>
        /// 上移分类
        /// </summary>
        /// <returns></returns>
        public ActionResult ValueUp()
        {
            int moveId = RequestInt("id");
            int attrid = RequestInt("attrid");
            int sequence = RequestInt("sequence");
            if (attrid == 0)
                return LayerAlertErrorAndReturn("属性有误！");
            if (moveId == 0)
                return LayerAlertErrorAndReturn("请选择移动属性值！");
            if (Bll.BllProduct_AttributeSelect.Instance().SetUp(moveId, attrid, sequence))
                return RedirectToAction("valuelist", "attribute", new { area = "product", attrid = attrid });
            else
                return LayerAlertErrorAndReturn("设置失败！");
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <returns></returns>
        public ActionResult SetDefault()
        {
            int id = RequestInt("id");
            int attrId = RequestInt("attrId");
            if (id == 0)
                return LayerAlertErrorAndReturn("访问出错！");
            if (attrId == 0)
                return LayerAlertErrorAndReturn("属性有误！");

            if (Bll.BllProduct_Attribute.SetDefault(id, attrId))
                return RedirectToAction("valuelist", "attribute", new { area = "product", attrid = attrId });
            else
                return LayerAlertErrorAndReturn("设置失败！");
        }

        /// <summary>
        /// 清除默认值
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearDefault()
        {
            int attrId = RequestInt("attrId");
            if (attrId == 0)
                return LayerAlertErrorAndReturn("属性有误！");

            if (Bll.BllProduct_Attribute.ClearDefault(attrId))
                return RedirectToAction("valuelist", "attribute", new { area = "product", attrid = attrId });
            else
                return LayerAlertErrorAndReturn("设置失败！");
        }

        #endregion

        #region 属性值编辑页面
        /// <summary>
        /// 编辑新增页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ValueEdit()
        {
            int attrId = RequestInt("attrId");
            if (attrId == 0)
                return LayerAlertErrorAndClose("属性有误！");
            Product_Attribute modAttr = Bll.BllProduct_Attribute.First(o => o.Id == attrId);
            if (modAttr == null)
                LayerAlertErrorAndClose("属性有误！");

            //判断是否是文本
            bool isString = false;
            if (modAttr.InputType == (int)Tools.Enums.ESite.AttrType.Text)
            {
                isString = true;
            }

            int id = RequestInt("id");
            Product_AttributeSelect model = null;
            if (id > 0)
            {
                model = Bll.BllProduct_AttributeSelect.First(o => o.Id == id);
                if (model == null)
                    return LayerAlertErrorAndClose("属性不存在！");
            }

            ViewBag.isString = isString;
            ViewBag.modAttr = modAttr;
            ViewBag.attrId = attrId;
            return View(model);
        }

        /// <summary>
        /// 保存属性
        /// </summary>
        /// <returns></returns>
        //[PermissionFilter("/product/attribute/valueedit")]
        [HttpPost]
        public ActionResult ValueSave()
        {
            int id = RequestInt("id");
            int attrId = RequestInt("attrId");
            string itemName = RequestString("ItemName");
            if (attrId == 0)
                return LayerAlertErrorAndClose("属性有误！");

            Product_Attribute modAttr = Bll.BllProduct_Attribute.First(o => o.Id == attrId);
            if (modAttr == null)
                LayerAlertErrorAndClose("属性有误！");

            //判断是否是文本
            if (modAttr.InputType == (int)Tools.Enums.ESite.AttrType.Text)
            {
                modAttr.DefaultValue = itemName;
                if (Bll.BllProduct_Attribute.Update(modAttr, o => o.Id == modAttr.Id) > 0)
                    return LayerAlertSuccessAndRefreshPage("添加成功！");
                else
                    return LayerAlertErrorAndClose("添加失败！");
            }
            else
            {
                Product_AttributeSelect mod = null;
                if (id > 0)
                {
                    mod = Bll.BllProduct_AttributeSelect.First(o => o.Id == id);
                    if (mod == null)
                        return LayerAlertErrorAndClose("属性不存在！");
                    mod.ItemName = itemName;
                    if (Bll.BllProduct_AttributeSelect.Update(mod, o => o.Id == id) > 0)
                        return LayerAlertSuccessAndRefreshPage("修改成功！");
                    else
                        return LayerAlertErrorAndClose("修改失败！");
                }
                else
                {
                    mod = new Product_AttributeSelect();
                    mod.ItemName = itemName;
                    mod.AttributeId = attrId;
                    mod.Sequence = Bll.BllSys_Class<Product_AttributeSelect>.Instance().GetMaxSequence("AttributeID=" + attrId);
                    if (Bll.BllProduct_AttributeSelect.Insert(mod) > 0)
                        return LayerAlertSuccessAndRefreshPage("添加成功！");
                    else
                        return LayerAlertErrorAndClose("添加失败！");
                }
            }
        }
        #endregion
    }
}