using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FancyFix.OA.ViewModel
{
    public class AdminInfoModel
    {
        [Display(Name = "Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "产品参数有误！")]
        public int id { get; set; }

        [Display(Name = "用户名")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请填写用户名！")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "用户名长度必须在{2}至{1}之间")]
        public string username { get; set; }

        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请填写密码！")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "产品标题长度必须在{2}至{1}之间")]
        public string password { get; set; }

        [Display(Name = "真实姓名")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请填写真实姓名！")]
        public string realname { get; set; }

        [Display(Name = "性别")]
        public bool sex { get; set; }

        [Display(Name = "部门Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请选择部门！")]
        public int departid { get; set; }

        [Display(Name = "权限组")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请选择权限组！")]
        public int groupid { get; set; }

        [Display(Name = "电话号码")]
        [StringLength(15, MinimumLength = 0, ErrorMessage = "电话号码长度必须在{2}至{1}之间")]
        public string tel { get; set; }

        [Display(Name = "邮箱")]
        public string email { get; set; }

        [Display(Name = "QQ")]
        public string qq { get; set; }

        [Display(Name = "上级")]
        public int paruserid { get; set; }
    }
}