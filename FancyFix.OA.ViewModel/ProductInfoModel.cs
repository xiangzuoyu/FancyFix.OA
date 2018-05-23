using FancyFix.Tools.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FancyFix.OA.ViewModel
{
    public class ProductInfoModel
    {
        [Display(Name = "Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "产品参数有误！")]
        public int id { get; set; }

        [Display(Name = "产品名称")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请填写{0}！")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "{0}长度必须在{2}至{1}之间")]
        public string title { get; set; }

        [Display(Name = "英文名称")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请填写{0}！")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "{0}长度必须在{2}至{1}之间")]
        public string title_en { get; set; }

        [Display(Name = "产品分类")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请选择{0}！")]
        public int classid { get; set; }

        [Display(Name = "SPU编号")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string spu { get; set; }

        [Display(Name = "老SPU编号")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string old_spu { get; set; }

        [Display(Name = "产品描述")]
        public string description { get; set; }

        [Display(Name = "产品封面图")]
        [StringLength(200, ErrorMessage = "{0}必须小于{1}个字符")]
        public string firstpic { get; set; }

        [Display(Name = "产品图片")]
        public string pics { get; set; }

        [Display(Name = "产品库存")]
        public int stock { get; set; }

        [Display(Name = "最小起订量")]
        public int moq { get; set; }

        [Display(Name = "最小起订量单位")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string moqunit { get; set; }

        [Display(Name = "产品计量单位")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string quantityunit { get; set; }

        [Display(Name = "货币单位")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string currency { get; set; }

        [Display(Name = "最低价")]
        [Range(typeof(decimal), "0.00", "999999.99")]
        public decimal minprice { get; set; }

        [Display(Name = "最高价")]
        [Range(typeof(decimal), "0.00", "999999.99")]
        public decimal maxprice { get; set; }

        [Display(Name = "不含税价")]
        [Range(typeof(decimal), "0.00", "999999.99")]
        public decimal price { get; set; }

        [Display(Name = "含税价")]
        [Range(typeof(decimal), "0.00", "999999.99")]
        public decimal taxprice { get; set; }

        [Display(Name = "价格单位")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string priceunit { get; set; }

        [Display(Name = "价格备注")]
        [StringLength(100, ErrorMessage = "{0}必须小于{1}个字符")]
        public string priceremark { get; set; }

        [Display(Name = "是否显示")]
        public bool isshow { get; set; }

        [Display(Name = "产品详细")]
        public string content { get; set; }

        [Display(Name = "管理员Id")]
        public int adminid { get; set; }

        [Display(Name = "产品属性")]
        public string attribute { get; set; }

        [Display(Name = "产品规格")]
        [StringLength(255, ErrorMessage = "{0}必须小于{1}个字符")]
        public string specification { get; set; }

        [Display(Name = "产品颜色")]
        [StringLength(20, ErrorMessage = "{0}必须小于{1}个字符")]
        public string color { get; set; }

        [Display(Name = "产品厚度")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string thickness { get; set; }

        [Display(Name = "产品重量")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string weight { get; set; }

        [Display(Name = "法律法规")]
        [StringLength(500, ErrorMessage = "{0}必须小于{1}个字符")]
        public string regulation { get; set; }

        [Display(Name = "产品成本")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string cost { get; set; }

        [Display(Name = "HS编码")]
        [StringLength(50, ErrorMessage = "{0}必须小于{1}个字符")]
        public string hs_code { get; set; }

        [Display(Name = "开票名")]
        [StringLength(200, ErrorMessage = "{0}必须小于{1}个字符")]
        public string invoicename { get; set; }

        [Display(Name = "图案Id")]
        public int patternid { get; set; }

        [Display(Name = "供应商Id")]
        public int supplierid { get; set; }

        [Display(Name = "供应商名称")]
        [StringLength(500, ErrorMessage = "{0}必须小于{1}个字符")]
        public string suppliername { get; set; }

        [Display(Name = "供应商产品代码")]
        [StringLength(30, ErrorMessage = "{0}必须小于{1}个字符")]
        public string supplierproductcode { get; set; }

    }
}