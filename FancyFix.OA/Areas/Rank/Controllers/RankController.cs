using FancyFix.OA.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Rank.Controllers
{
    [CheckLogin]
    public class RankController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }
    }
}