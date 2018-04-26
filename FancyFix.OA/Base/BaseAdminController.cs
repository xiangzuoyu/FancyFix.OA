using FancyFix.Core.Admin;
using FancyFix.OA.Model;
using FancyFix.Tools.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Configuration;

namespace FancyFix.OA.Base
{
    public class BaseAdminController : BaseController, IAdminInfo
    {
        //设计部门Id
        protected static int DesignDepartId = ConfigurationManager.AppSettings["DesignDepartId"]?.ToInt32() ?? 10;

        private Mng_User myInfo; //当前管理员对象 

        /// <summary>
        /// 当前管理员对象
        /// </summary>
        public Mng_User MyInfo
        {
            get
            {
                if (myInfo == null)
                    myInfo = new AdminState(this.HttpContext).GetUserInfo();
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

        public bool CheckSuperAdmin(int userId)
        {
            return PermissionManager.IsSuperAdmin(userId);
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

        #region 全局Action管道

        protected override void OnActionExecuting(ActionExecutingContext executingContext)
        {
            if (myInfo == null)
                myInfo = new AdminState(this.HttpContext).GetUserInfo();
            base.OnActionExecuting(executingContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext executedContext)
        {
            base.OnActionExecuted(executedContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext executingContext)
        {
            base.OnResultExecuting(executingContext);
        }

        protected override void OnResultExecuted(ResultExecutedContext executedContext)
        {
            //在这里可以处理最后输出到浏览器的html
            //string html = RenderViewToString(this, ((ViewResult)executedContext.Result).View);
            base.OnResultExecuted(executedContext);
        }

        //protected static string RenderViewToString(Controller controller, IView view)
        //{
        //    using (StringWriter writer = new StringWriter())
        //    {
        //        ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer);
        //        viewContext.View.Render(viewContext, writer);
        //        return writer.ToString();
        //    }
        //}
        #endregion

        #region Bootstrap Datatable

        /// <summary>
        /// 重写JosnResult，实现时间格式转换
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <param name="format">输出时间格式</param>
        /// <returns></returns>
        protected JsonResult MyJson(object data, string format)
        {
            return new ToJsonResult
            {
                Data = data,
                FormateStr = format,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 返回Bootstrap DataTable约定的Json结构
        /// </summary>
        /// <param name="records"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        protected JsonResult BspTableJson(object list, object records)
        {
            var res = new { total = records, rows = list };
            return MyJson(res, "yyyy-MM-dd HH:mm:ss");
        }

        #endregion

        #region 获取图片
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="getPics">获取图片的参数名，默认：pic</param>
        /// <param name="firstPic">返回封面图</param>
        /// <returns></returns>
        protected string GetPics(string getPics, ref string firstPic)
        {
            string pics = RequestString(getPics).TrimEnd(',');
            List<string> listPic = pics.Split(',').ToList();
            if (listPic.Count > 0 && listPic[0] != string.Empty) firstPic = listPic[0];
            return pics;
        }

        protected string GetPics(string getPics = "pic")
        {
            return RequestString(getPics).TrimEnd(',');
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="getPics"></param>
        /// <returns></returns>
        protected string GetPic(string getPics = "pic")
        {
            string pics = RequestString(getPics).TrimEnd(',');
            return pics.Split(',')[0];
        }
        #endregion

        #region 部门下拉框
        protected string GetDepartOptions(int departId = 0)
        {
            return Bll.BllMng_DepartmentClass.Instance().ShowClass(0, departId, false);
            //if (IsSuperAdmin)
            //{
            //    return Bll.BllMng_DepartmentClass.Instance().ShowClass(0, departId, false);
            //}
            //StringBuilder strShowClass = new StringBuilder();

            //var myDepart = Bll.BllMng_DepartmentClass.First(o => o.Id == MyInfo.DepartId.Value);
            //if (myDepart != null)
            //{
            //    if (myDepart.ChildNum > 0)
            //    {
            //        var list = Bll.BllMng_DepartmentClass.Instance().GetListByParentId(myDepart.Id, false);
            //        GetListByParId(list, MyInfo.DepartId.Value, strShowClass, departId > 0 ? departId : MyDepartId);
            //    }
            //    else
            //    {
            //        strShowClass.AppendFormat("<option value=\"{0}\" selected>{1}</option>", myDepart.Id, myDepart.ClassName);
            //    }
            //}
            //return strShowClass.ToString();
        }

        private void GetListByParId(IEnumerable<Mng_DepartmentClass> list, int parId, StringBuilder strShowClass, int SelectedValue)
        {
            IEnumerable<Mng_DepartmentClass> childlist = list.Where(o => o.ParId == parId);
            if (childlist != null && childlist.Count() > 0)
            {
                foreach (var item in childlist)
                {
                    strShowClass.AppendFormat("<option value=\"{0}\" ", item.Id.ToString());
                    if (SelectedValue == item.Id)
                        strShowClass.Append(" selected");
                    strShowClass.Append(" >");
                    if (item.Depth == 0)
                        strShowClass.Append("├");
                    else
                    {
                        for (int j = 0; j < item.Depth; j++)
                            strShowClass.Append(" │");
                        strShowClass.Append(" ├");
                    }
                    strShowClass.Append(item.ClassName.ToString());
                    strShowClass.Append(" </option>");
                    GetListByParId(list, item.Id, strShowClass, SelectedValue);
                }
            }
        }
        #endregion

        #region 分页辅助
        /// <summary>
        /// 默认样式一的动态分页
        /// </summary>
        /// <param name="totleNum"></param>
        /// <param name="numPerPage"></param>
        /// <param name="thisPage"></param>
        /// <returns></returns>
        public string ShowPage(int totleNum, int numPerPage, int thisPage)
        {
            return ShowPage(totleNum, numPerPage, thisPage, 1, "", false, "", "", false);
        }

        /// <summary>
        /// 动态分页 
        /// </summary>
        /// <param name="totleNum">总记录数</param>
        /// <param name="numPerPage">每页条数</param>
        /// <param name="thisPage">当前页数</param>
        /// <param name="showJump">是否显示跳转</param>
        /// <param name="pageStyle">分页样式</param>
        /// <param name="postStr">URL附加参数</param>
        /// <returns>分页HTML代码</returns>
        public string ShowPage(int totleNum, int numPerPage, int thisPage, int pageStyle, string postStr, bool isNoFollow)
        {
            return ShowPage(totleNum, numPerPage, thisPage, pageStyle, postStr, false, "", "", isNoFollow);
        }

        /// <summary>
        /// .完整分页调用
        /// </summary>
        /// <param name="totleNum">记录总条数</param>
        /// <param name="numPerPage">每页条数</param>
        /// <param name="thisPage">当前页</param>
        /// <param name="pageStyle">分页样式</param>
        /// <param name="postStr">传递records数量</param>
        /// <param name="showJumpPage">是否显示跳转</param>
        /// <param name="urlFormat">(假静态)分页URL规则</param>
        /// <param name="urlFirstPage">第一页Url</param>
        /// <returns></returns>
        public string ShowPage(int totleNum, int numPerPage, int thisPage, int pageStyle, string postStr, bool showJumpPage, string urlFormat, string urlFirstPage, bool isNoFollow)
        {

            if (totleNum <= numPerPage) { return ""; }
            int AllPage = (int)Math.Ceiling((float)totleNum / numPerPage); //求总页数

            string thisurl = urlFormat == "" ? GetUrl(postStr) + "{0}" : urlFormat; //获取当前页URL地址       
            string PageHtml = "", PageHtml1 = "";

            switch (pageStyle)
            {
                case 5:
                    PageHtml = "<ul class=\"pagination pagination-md no-margin\">";
                    PageHtml += "<li " + (thisPage == 1 ? "class=\"disabled\"" : "") + "><a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + (thisPage == 1 ? "javascript:void(0)" : GetFirstRealPage(thisurl, thisPage - 1, urlFirstPage)) + "\">«</a></li>";

                    int shownum5 = 4;     //显示相邻页数量
                    int haveshownum5 = 1; //已经显示相邻页数量
                    int prenum5 = shownum5 / 2; //默认前面显示4页
                    int realPrePage5 = 0;

                    //显示前相邻页
                    for (int loop = thisPage - 1; loop >= 1 && haveshownum5 <= prenum5; loop--)
                    {
                        realPrePage5++;
                        haveshownum5++;
                        PageHtml1 = "<li><a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, loop, urlFirstPage) + "\">" + loop + "</a></li>" + PageHtml1;
                    }

                    PageHtml += PageHtml1 + "<li class=\"active\"><a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"javascript:void(0)\" >" + thisPage + "</a></li>";

                    //显示后相邻页
                    for (int loop = thisPage + 1; loop <= AllPage && haveshownum5 <= prenum5; loop++)
                    {
                        haveshownum5++;
                        PageHtml += "<li><a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, loop, urlFirstPage) + "\">" + loop + "</a></li>";
                    }

                    PageHtml += "<li " + (thisPage < AllPage ? "" : "class=\"disabled\"") + "><a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + (thisPage < AllPage ? GetFirstRealPage(thisurl, thisPage + 1, urlFirstPage) : "javascript:void(0)") + "\">»</a></li>";

                    PageHtml += "</ul>";
                    break;
                case 4:
                    #region 分页4
                    PageHtml = "<div class=\"selectpage\">";
                    if (thisPage == 1)
                    {
                        PageHtml += "<a href='javascript:void(0)'>Pre</a>";
                    }
                    if (thisPage > 1)
                    {
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, thisPage - 1, urlFirstPage) + "\" >Pre</a>";
                    }

                    int shownum4 = 8;     //显示相邻页数量
                    int haveshownum4 = 1; //已经显示相邻页数量
                    int prenum4 = shownum4 / 2; //默认前面显示4页
                    if (AllPage - thisPage < prenum4) //如果后面多余
                    {
                        prenum4 = shownum4 - (AllPage - thisPage); //重新计算前面显示页数
                    }
                    if ((thisPage - prenum4) > 1)
                    {
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, 1, urlFirstPage) + "\">1</a>";
                    }
                    if ((thisPage - prenum4) > 2)
                    {
                        PageHtml += "<em>...</em>";
                    }

                    int realPrePage4 = 0;
                    //显示前相邻页
                    for (int loop = thisPage - 1; loop >= 1 && haveshownum4 <= prenum4; loop--)
                    {
                        realPrePage4++;
                        haveshownum4++;
                        PageHtml1 = "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, loop, urlFirstPage) + "\">" + loop + "</a>" + PageHtml1;
                    }

                    PageHtml += PageHtml1 + "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " class=\"on\">" + thisPage + "</a>";//本页

                    //显示后相邻页
                    for (int loop = thisPage + 1; loop <= AllPage && haveshownum4 <= shownum4; loop++)
                    {
                        haveshownum4++;
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, loop, urlFirstPage) + "\">" + loop + "</a>";
                    }
                    if (thisPage + (shownum4 - realPrePage4) < AllPage)
                    {
                        PageHtml += "...";
                    }
                    if (thisPage < AllPage)
                    {
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, thisPage + 1, urlFirstPage) + "\"  class=\"pagenext\">Next</a>";
                    }
                    else
                    {
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + ">Next</a>";
                    }
                    if (showJumpPage)
                    {
                        PageHtml += "<span class=\"pageskip\">共<strong>" + AllPage.ToString() + "</strong>页 到第";
                        PageHtml += "<input type=\"text\" value=\"" + thisPage.ToString() + "\" size=\"3\" id=\"txtJumpPage\" name=\"jumpto\">页";
                        PageHtml += "<button type=\"button\" id=\"B_pageJump\" onclick =\"javascript:window.location.href='" + string.Format(thisurl, "'+document.getElementById('txtJumpPage').value+'") + "'\"> </button></span>";
                    }

                    PageHtml += "</div>";
                    break;

                #endregion

                case 3:
                    #region 分页3
                    PageHtml = "<div class=\"pagebox\">";
                    if (thisPage == 1)
                    {
                        PageHtml += "<a href='javascript:void(0)'>Pre</a>";
                    }
                    if (thisPage > 1)
                    {
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, thisPage - 1, urlFirstPage) + "\" >Pre</a>";
                    }

                    int shownum3 = 8;     //显示相邻页数量
                    int haveshownum3 = 1; //已经显示相邻页数量
                    int prenum3 = shownum3 / 2; //默认前面显示4页
                    if (AllPage - thisPage < prenum3) //如果后面多余
                    {
                        prenum3 = shownum3 - (AllPage - thisPage); //重新计算前面显示页数
                    }
                    if ((thisPage - prenum3) > 1)
                    {
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, 1, urlFirstPage) + "\">1</a>";
                    }
                    if ((thisPage - prenum3) > 2)
                    {
                        PageHtml += "<em>...</em>";
                    }

                    int realPrePage3 = 0;
                    //显示前相邻页
                    for (int loop = thisPage - 1; loop >= 1 && haveshownum3 <= prenum3; loop--)
                    {
                        realPrePage3++;
                        haveshownum3++;
                        PageHtml1 = "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, loop, urlFirstPage) + "\">" + loop + "</a>" + PageHtml1;
                    }

                    PageHtml += PageHtml1 + "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " class=\"on\">" + thisPage + "</a>";//本页

                    //显示后相邻页
                    for (int loop = thisPage + 1; loop <= AllPage && haveshownum3 <= shownum3; loop++)
                    {
                        haveshownum3++;
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, loop, urlFirstPage) + "\">" + loop + "</a>";
                    }
                    if (thisPage + (shownum3 - realPrePage3) < AllPage)
                    {
                        PageHtml += "...";
                    }
                    if (thisPage < AllPage)
                    {
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + " href=\"" + GetFirstRealPage(thisurl, thisPage + 1, urlFirstPage) + "\"  class=\"pagenext\">Next</a>";
                    }
                    else
                    {
                        PageHtml += "<a " + (isNoFollow ? "rel=\"nofollow\"" : "") + ">Next</a>";
                    }
                    if (showJumpPage)
                    {
                        PageHtml += "<span class=\"pageskip\">共<strong>" + AllPage.ToString() + "</strong>页 到第";
                        PageHtml += "<input type=\"text\" value=\"" + thisPage.ToString() + "\" size=\"3\" id=\"txtJumpPage\" name=\"jumpto\">页";
                        PageHtml += "<button type=\"button\" id=\"B_pageJump\" onclick =\"javascript:window.location.href='" + string.Format(thisurl, "'+document.getElementById('txtJumpPage').value+'") + "'\"> </button></span>";
                    }

                    PageHtml += "</div>";
                    break;

                #endregion

                case 2:
                    #region 分页2
                    PageHtml += "共" + totleNum + "条 ";
                    PageHtml += thisPage.ToString() + "/" + AllPage.ToString() + "页 ";


                    if (thisPage == 1)
                    {
                        PageHtml += "首页 上页";
                    }
                    else
                    {
                        PageHtml += " <a href=\"" + GetFirstRealPage(thisurl, 1, urlFirstPage) + "\">首页</a> <a href=\"" + GetFirstRealPage(thisurl, thisPage - 1, urlFirstPage) + "\">上页</a>";
                    }
                    if (thisPage < AllPage)
                    {
                        PageHtml += " <a href=\"" + GetFirstRealPage(thisurl, thisPage + 1, urlFirstPage) + "\">下页</a> <a href=\"" + GetFirstRealPage(thisurl, AllPage, urlFirstPage) + "\">尾页</a>";
                    }
                    else
                    {
                        PageHtml += " 下页 尾页";
                    }
                    if (showJumpPage)
                    {
                        PageHtml += " 第";
                        PageHtml += "<input type=\"text\" value=\"" + thisPage.ToString() + "\" size=\"3\" id=\"txtJumpPage\" name=\"jumpto\">页";
                        PageHtml += "<input type=\"button\" name=\"btnJumpPage\" value=\"跳转\" onclick =\"javascript:window.location.href='" + string.Format(thisurl, "'+document.getElementById('txtJumpPage').value+'") + "'\" />";
                    }
                    break;
                #endregion

                case 1:
                    #region 分页1
                    PageHtml += "共" + totleNum + "条 ";
                    PageHtml += thisPage.ToString() + "/" + AllPage.ToString() + "页 ";

                    //if (thisPage == 1)
                    //{
                    //    PageHtml += " [上一页] ";
                    //}

                    if (thisPage > 1)
                    {
                        PageHtml += " <a href=\"" + GetFirstRealPage(thisurl, thisPage - 1, urlFirstPage) + "\">[上一页]</a> ";
                    }

                    int shownum1 = 8;     //显示相邻页数量
                    int haveshownum1 = 1; //已经显示相邻页数量
                    int prenum1 = shownum1 / 2; //默认前面显示4页
                    if (AllPage - thisPage < prenum1) //如果后面多余
                    {
                        prenum1 = shownum1 - (AllPage - thisPage); //重新计算前面显示页数
                    }
                    if ((thisPage - prenum1) > 1)
                    {
                        PageHtml += " <a href=\"" + GetFirstRealPage(thisurl, 1, urlFirstPage) + "\">[1]</a> ";
                    }
                    if ((thisPage - prenum1) > 2)
                    {
                        PageHtml += "...";
                    }

                    int realPrePage1 = 0;
                    //显示前相邻页
                    for (int loop = thisPage - 1; loop >= 1 && haveshownum1 <= prenum1; loop--)
                    {
                        realPrePage1++;
                        haveshownum1++;
                        PageHtml1 = " <a href=\"" + GetFirstRealPage(thisurl, loop, urlFirstPage) + "\">[" + loop + "]</a> " + PageHtml1;
                    }

                    PageHtml += PageHtml1 + " <font color=red>[" + thisPage + "]</font> ";//本页

                    //显示后相邻页
                    for (int loop = thisPage + 1; loop <= AllPage && haveshownum1 <= shownum1; loop++)
                    {
                        haveshownum1++;
                        PageHtml += " <a href=\"" + GetFirstRealPage(thisurl, loop, urlFirstPage) + "\">[" + loop + "]</a> ";
                    }

                    if (thisPage + (shownum1 - realPrePage1) < AllPage)
                    {
                        PageHtml += "...";
                    }

                    if (thisPage < AllPage)
                    {
                        PageHtml += " <a href=\"" + GetFirstRealPage(thisurl, thisPage + 1, urlFirstPage) + "\">[下一页]</a> ";
                    }
                    //else
                    //{
                    //    PageHtml += " [下一页] ";
                    //} 

                    if (showJumpPage)
                    {
                        PageHtml += " 第";
                        PageHtml += "<input type=\"text\" value=\"" + thisPage.ToString() + "\" size=\"3\" id=\"txtJumpPage\" name=\"jumpto\">页";
                        PageHtml += "<input type=\"button\" name=\"btnJumpPage\" value=\"跳转\" onclick =\"javascript:window.location.href='" + string.Format(thisurl, "'+document.getElementById('txtJumpPage').value+'") + "'\" />";
                    }
                    break;

                    #endregion
            }
            return PageHtml;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlFormat"></param>
        /// <param name="thisPage"></param>
        /// <param name="firstPage"></param>
        /// <returns></returns>
        private string GetFirstRealPage(string urlFormat, int thisPage, string firstPage)
        {
            if (thisPage == 1 && firstPage != "")
            {
                return firstPage;
            }
            else
            {
                return string.Format(urlFormat, thisPage.ToString());
            }

        }

        /// <summary>
        /// 获取当前页码值
        /// </summary>
        /// <returns></returns>
        public int GetThisPage()
        {
            int thisPage = Request["page"].ToInt32();
            if (thisPage == 0) { thisPage = 1; }
            return thisPage;

        }

        /// <summary>
        /// 获取分页数
        /// </summary>
        /// <returns></returns>
        public int GetRows()
        {
            int rows = Request["rows"].ToInt32();
            if (rows == 0) { rows = 20; }
            return rows;

        }

        public string GetUrl(string postStr)
        {
            string Addressurl = Request.ServerVariables["SCRIPT_NAME"].ToString();
            Addressurl += "?";
            string ItemUrls = "";
            foreach (string queryStr in Request.QueryString)
            {
                if (postStr == "")
                {
                    if (queryStr != "page")
                    {
                        ItemUrls += queryStr + "=" + Server.UrlEncode(Request.QueryString[queryStr]) + "&";
                    }
                }
                else
                {
                    if (queryStr != "page" && queryStr != "records")
                    {
                        ItemUrls += queryStr + "=" + Server.UrlEncode(Request.QueryString[queryStr]) + "&";
                    }
                }

            }
            if (postStr != "")
            {
                Addressurl += postStr + "&";

            }
            return Addressurl + ItemUrls + "page=";
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

        #endregion

        /// <summary>
        /// 规范年月
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        protected void CheckDate(ref int year, ref int month)
        {
            if (year < StartYear) year = DateTime.Now.Year;
            if (month < 1 || month > 12) month = DateTime.Now.Month;
        }

        protected void CheckDate(ref int year, ref int fromMonth, ref int toMonth)
        {
            if (year < StartYear) year = DateTime.Now.Year;
            if (fromMonth == 0) fromMonth = DateTime.Now.Month;
            if (toMonth == 0) toMonth = DateTime.Now.Month;
        }
    }
}
