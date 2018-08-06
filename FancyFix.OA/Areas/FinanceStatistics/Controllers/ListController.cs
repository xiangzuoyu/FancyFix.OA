using FancyFix.OA.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.FinanceStatistics.Controllers
{
    public class ListController : BaseAdminController
    {
        /// <summary>
        /// 财务统计
        /// </summary>
        /// <returns></returns>
        // GET: FinanceStatistics/List
        public ActionResult List()
        {
            return View();
        }

        //public JsonResult PageList(int page = 0, int pagesize = 0, int displayid = 0)
        //{

        //}
    }
}