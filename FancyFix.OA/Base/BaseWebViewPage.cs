using FancyFix.Core.Admin;
using FancyFix.OA;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FancyFix.OA.Base
{
    public abstract class BaseWebViewPage<TMmodel> : System.Web.Mvc.WebViewPage<TMmodel>, IAdminInfo
    {
        //初始化
        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            BaseAdminController baseController = (this.ViewContext.Controller) as BaseAdminController;
            if (baseController != null)
                myInfo = baseController.MyInfo;
        }

        public static string cssVersion = DateTime.Now.ToString("yyMMddhhss"); //样式版本
        public static string domain = Tools.Special.Common.GetDomain();
        public static string webUrl = Tools.Special.Common.GetWebUrl();
        public static string imgUrl = Tools.Special.Common.GetImgUrl();

        private Mng_User myInfo = null; //当前管理员对象 

        /// <summary>
        /// 当前管理员对象
        /// </summary>
        public Mng_User MyInfo
        {
            get
            {
                if (myInfo == null)
                {
                    BaseAdminController baseController = (this.ViewContext.Controller) as BaseAdminController;
                    myInfo = baseController != null && baseController.MyInfo != null ? baseController.MyInfo : new AdminState(this.Context).GetUserInfo();
                    if (myInfo == null || !(bool)myInfo.InJob)
                    {
                        Context.ClearError();
                        Context.Response.Redirect("/auth/login", true);
                        Context.Response.End();
                        return null;
                    }
                }
                return myInfo;
            }
        }

        /// <summary>
        /// 当前所在部门Id
        /// </summary>
        public int MyDepartId
        {
            get { return MyInfo?.DepartId ?? 0; }
        }

        /// <summary>
        /// 是否是超管
        /// </summary>
        public bool IsSuperAdmin
        {
            get { return PermissionManager.IsSuperAdmin(MyInfo.Id); }
        }

        /// <summary>
        /// 是否是部门管理
        /// </summary>
        public bool IsDepartAdmin
        {
            get { return Bll.BllMng_PermissionGroup.Any(o => o.IsAdmin == true && o.DepartId == MyInfo.DepartId && o.Id == MyInfo.GroupId); }
        }

        /// <summary>
        /// 获取检测权限 (通用)
        /// </summary> 
        /// <param name="permissionId">权限ID</param> 
        /// <returns>true or false</returns>
        public bool CheckPermission(int permissionId)
        {
            try
            {
                //判断权限
                return PermissionManager.CheckPermission(MyInfo, permissionId);
            }
            catch (Exception ex)
            {
                FancyFix.Tools.Tool.LogHelper.WriteLog(typeof(BaseApiController), ex, 0, "");
                return false;
            }
        }

        #region Request方法
        /// <summary>
        /// Request一个数字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int RequestInt(string name)
        {
            string v = Request[name];
            if (v == null)
            {
                return 0;
            }
            else
            {
                return v.ToInt32();
            }
        }
        /// <summary>
        /// Request一个Byte
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte RequestByte(string name)
        {
            string v = Request[name];
            if (v == null)
            {
                return 0;
            }
            else
            {
                return v.ToByte();
            }
        }

        /// <summary>
        /// Request一个Decimal
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public decimal RequestDecimal(string name)
        {
            string v = RequestString(name);
            decimal result = 0;
            if (decimal.TryParse(v, out result))
            {
                return result;
            }
            return 0;
        }
        /// <summary>
        /// Request一个布尔值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RequestBool(string name)
        {
            string v = Request[name];
            if (v == null)
            {
                return false;
            }
            else
            {
                try
                {
                    return bool.Parse(v);
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 请求一个字符串，返回null时返回空字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string RequestString(string name)
        {
            string v = Request[name];
            if (v == null)
            {
                v = string.Empty;
            }
            return v.Trim();
        }

        /// <summary>
        /// Request一个Double
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double RequestDouble(string name)
        {
            string v = RequestString(name);
            double result = 0;
            if (double.TryParse(v, out result))
            {
                return result;
            }
            return 0;
        }

        /// <summary>
        /// 请求字符串类型
        /// </summary>
        /// <param name="requestStr">请求的Request值</param>
        /// <returns>返回字符串，null则返回""</returns>
        public static string ConvertString(string requestStr)
        {
            return requestStr.ToString2();
        }

        /// <summary>
        ///   数字类型值
        /// </summary>
        /// <param name="requestStr">获取request值</param>
        /// <returns>返回数字类型,空则返回0</returns>
        public static int ConvertInt(string requestStr)
        {
            return requestStr.ToInt32();
        }

        /// <summary>
        ///   浮点类型值
        /// </summary>
        /// <param name="requestStr">获取request值</param>
        /// <returns>返回数字类型,空则返回0</returns>
        public static float ConvertFloat(string requestStr)
        {
            return requestStr.ToFloat();
        }

        /// <summary>
        ///   双精度类型值
        /// </summary>
        /// <param name="requestStr">获取request值</param>
        /// <returns>返回数字类型,空则返回0</returns>
        public static decimal ConvertDecimal(string requestStr)
        {
            return requestStr.ToDecimal();
        }

        public static Int32 ConvertObjectToInt(object requestStr)
        {
            try
            {
                return Convert.ToInt32(requestStr);
            }
            catch
            {
                return 0;
            }
        }

        public static string ConvertObjectToStr(object requestStr)
        {
            try
            {
                return Convert.ToString(requestStr);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 转换成布尔型
        /// </summary>
        /// <param name="requestStr"></param>
        /// <returns></returns>
        public static bool ConvertBoolean(string requestStr)
        {
            try
            {
                if (string.IsNullOrEmpty(requestStr))
                {
                    return false;
                }

                requestStr = requestStr.ToLower();
                if (requestStr == "1" || requestStr == "true")
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 常用

        /// <summary>
        /// (已检测)获取客户端IP
        /// </summary>
        /// <returns></returns>
        public string GetRemoteIp()
        {
            return Tools.Utility.CheckClient.GetIP();
        }

        /// <summary>
        /// 返回中文性别
        /// </summary>
        /// <param name="sex">Bool类型性别</param>
        /// <returns>男(false)或女(true)</returns>
        public static string GetChineseSex(bool sex)
        {
            return sex ? "男" : "女";
        }

        /// <summary>
        /// 获取第一张图片
        /// </summary>
        /// <param name="uptype"></param>
        /// <param name="picStr"></param>
        /// <param name="getSmallPic"></param>
        /// <returns></returns>
        public static string GetFirstPic(string picStr, bool getSmallPic)
        {
            return Tools.Utility.Web.GetFirstPic(picStr, getSmallPic);
        }

        /// <summary>
        /// 输出小图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetSmallPic(string pic)
        {
            return Tools.Usual.Utils.GetSmallPic(pic);
        }

        /// <summary>
        /// 输出大图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetBigPic(string pic)
        {
            return Tools.Usual.Utils.GetBigPic(pic);
        }
        /// <summary>
        /// 输出中图
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static string GetMiddlePic(string pic)
        {
            return Tools.Usual.Utils.GetMiddlePic(pic);
        }

        /// 返回表单下拉菜单或选框的还原项
        /// </summary>
        /// <param name="selValue">设置的值</param>
        /// <param name="thisValue">表单框值</param>
        /// <param name="isSelectBox">是否是下拉菜单，否则是选框</param>
        /// <returns>下拉菜单选中返回selected ,选框选中返回checked</returns>
        public string ReturnSel(string selValue, string thisValue, bool isSelectBox)
        {
            if (string.IsNullOrEmpty(selValue) || string.IsNullOrEmpty(thisValue))
                return "";
            if (selValue == thisValue)
            {
                if (isSelectBox)
                    return " selected";
                else
                    return " checked";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 返回前一页
        /// </summary>
        public void ReturnRefferPage()
        {
            Response.Redirect(RefferPageUrl());
            Response.End();
        }

        /// <summary>
        /// 获取前导页
        /// </summary>
        /// <returns></returns>
        public string RefferPageUrl()
        {
            try
            {
                return HttpUtility.UrlDecode(Request.UrlReferrer.AbsoluteUri, System.Text.Encoding.GetEncoding("GB2312"));
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 页面方法转换

        /// <summary>
        /// 格式化成Url
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetFormatUrl(string title)
        {
            return Tools.Usual.Utils.ConverUrl(title).ToLower();
        }

        /// <summary>
        /// 去除Html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtml(string str)
        {
            return Tools.Common.StringProcess.RemoveHtml(str);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutString(string str, int length)
        {
            return Tools.Common.StringProcess.CutString(str, length);
        }
        /// <summary>
        /// 判断文件后缀是否是图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsImg(string fileName)
        {
            return Tools.Usual.Utils.IsImg(fileName);
        }

        /// <summary>
        /// 获取文件后缀
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExt(string fileName)
        {
            return Tools.Usual.Utils.GetFileExt(fileName);
        }

        /// <summary>
        /// 格式化产品参数
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static string FormatOrderAttrDetail(string detail)
        {
            return detail.Replace(";", "\r\n");
        }

        /// <summary>
        /// Email格式化
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string FormatEmail(string email)
        {
            string[] t = email.Split('@');
            string result = string.Empty;
            result = t[0].Substring(0, 2) + "****@***.com";
            return result;
        }

        /// <summary>
        /// 文本Texteara显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ShowTextarea(string str)
        {
            return Tools.Usual.Utils.ToTexteara(str);
        }

        /// <summary>
        /// 根据条件筛选列表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected static DataTable GetSelect(DataTable dt, string where)
        {
            return Tools.Usual.Utils.GetSelect(dt, where);
        }

        /// <summary>
        /// 获取唯一数据的字段
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PrimaryKeyColumns"></param>
        /// <returns></returns>
        protected static DataTable GetDistinctPrimaryKeyColumnTable(DataTable dt, string[] PrimaryKeyColumns)
        {
            return Tools.Usual.Utils.GetDistinctPrimaryKeyColumnTable(dt, PrimaryKeyColumns);
        }

        /// <summary>
        /// 去除html内容里的图片高宽样式
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static string RemoveImageStyle(string detail)
        {
            Regex regWidth = new Regex("(<img[^>]*?)\\s+width\\s*=\\s*\\S+", RegexOptions.IgnoreCase);
            Regex regHeight = new Regex("(<img[^>]*?)\\s+height\\s*=\\s*\\S+", RegexOptions.IgnoreCase);
            Regex regStyle = new Regex("(<img[^>]*?)\\s+style\\s*=\\s*\\S+", RegexOptions.IgnoreCase);

            return regWidth.Replace(regHeight.Replace(regStyle.Replace(detail, "$1"), "$1"), "$1");
        }

        /// <summary>
        /// 获取html中所有图片的路径
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string[] GetImageUrlListFromHtml(string htmlText)
        {
            return Tools.Usual.Utils.GetImageUrlListFromHtml(htmlText);
        }

        /// <summary>
        /// 获取html中所有a标签的链接地址
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string[] GetLinkUrlListFromHtml(string htmlText)
        {
            return Tools.Usual.Utils.GetLinkUrlListFromHtml(htmlText);
        }
        #endregion

        #region 富文本内容处理
        /// <summary>
        /// 完整图片路径转换成相对路径
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected string TransferImgToLocal(string content)
        {
            //<div><img src="http://www.xxx.com/Content/images/cpa.png"></div>
            if (!string.IsNullOrEmpty(content))
            {
                var imgList = GetImageUrlListFromHtml(content);
                foreach (var img in imgList)
                    if (img.Contains(webUrl))
                        content = content.Replace(img, img.Replace(webUrl, ""));
            }
            return content;
        }

        /// <summary>
        /// 相对路径转换成完整图片路径
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected string TransferImgFromLocal(string content)
        {
            //<div><img src="/Content/images/cpa.png"></div>
            if (!string.IsNullOrEmpty(content))
            {
                var imgList = GetImageUrlListFromHtml(content).Distinct();
                foreach (var img in imgList)
                    if (!img.Contains("http://") && !img.Contains("https://"))
                        content = content.Replace(img, webUrl + img);
            }
            return content;
        }
        #endregion

        #region 价值观考核进程月份

        //static string WorkerMonth = System.Configuration.ConfigurationManager.AppSettings["WorkerMonth"].ToString2().TrimEnd(',');

        protected static int StartYear = System.Configuration.ConfigurationManager.AppSettings["StartYear"].ToString2().ToInt32();
        protected static int WorkerEndDay = System.Configuration.ConfigurationManager.AppSettings["WorkerEndDay"].ToString2().ToInt32();
        protected static int CreateEndDay = System.Configuration.ConfigurationManager.AppSettings["CreateEndDay"].ToString2().ToInt32();

        public List<int> GetWorkerMonthList(int year)
        {
            return Bll.BllConfig_Process.GetValuableProcess(year);
        }

        public int CurrentWorkerMonth
        {
            get
            {
                return GetWorkerMonth(DateTime.Now.Year, DateTime.Now.Month);
            }
        }

        /// <summary>
        /// 获取当月所在进程月
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        protected int GetWorkerMonth(int year, int month)
        {
            var workMonths = GetWorkerMonthList(year);
            for (int i = 0; i < workMonths.Count; i++)
            {
                if (month >= workMonths[workMonths.Count - 1])
                    return workMonths[workMonths.Count - 1];
                if (i > 0 && month >= workMonths[i - 1] && month < workMonths[i])
                    return workMonths[i - 1];
            }
            return 1;
        }

        /// <summary>
        /// 获取下一个进程月
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        protected int GetNextWorkerMonth(int year, int month)
        {
            var workMonths = GetWorkerMonthList(year);
            for (int i = 0; i < workMonths.Count; i++)
            {
                if (month >= workMonths[workMonths.Count - 1])
                    return workMonths[workMonths.Count - 1];
                if (i > 0 && month >= workMonths[i - 1] && month < workMonths[i])
                    return workMonths[i];
            }
            return 1;
        }

        /// <summary>
        /// 获取进程生成截止时间
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        protected static DateTime GetKpiCreateEndDay(int year, int month)
        {
            return (year + "-" + month + "-" + (CreateEndDay + 1)).ToDateTime();
        }

        /// <summary>
        /// KPI审批截止时间
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        protected static DateTime GetKpiApproveEndDay(int year, int month)
        {
            return (year + "-" + month + "-" + (WorkerEndDay + 1)).ToDateTime();
        }

        #endregion
    }
}
