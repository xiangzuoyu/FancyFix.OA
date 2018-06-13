using FancyFix.Core.Enum;
using FancyFix.OA.Base;
using FancyFix.OA.Helper;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Demand.Controllers
{
    public class DemandController : BaseAdminController
    {
        // GET: Demand/Demand
        public ActionResult Index()
        {
            ViewBag.DemandType = Tools.Enums.Tools.GetEnumList(typeof(Demand_TypeEnum));
            ViewBag.showClass = Bll.BllMng_DepartmentClass.Instance().ShowClass(0, 0, false);
            return View();
        }

        public ActionResult Add(int id=0,int isshow=0)
        {
            int deptSelectVal = 0;

            Develop_Demand demand = new Develop_Demand();
            if (id > 0)
            {
                demand=Bll.BllDevelop_Demand.First(p => p.Id == id);
                if (demand != null && demand.Id > 0)
                {
                    deptSelectVal = demand.DeptId;
                }
            }
            ViewBag.showClass = Bll.BllMng_DepartmentClass.Instance().ShowClass(0, deptSelectVal, false);
            ViewBag.DemandType = Tools.Enums.Tools.GetEnumList(typeof(Demand_TypeEnum));
            ViewBag.isshow = isshow;
            return View(demand);

        }

        public ActionResult GetDemandType(int id=0, int demandType = 1)
        {
           var model=DemandTypeHelper.GetPageStructure(id, demandType);
            return PartialView("_DemandType", model);
        }

        public ActionResult UserList(int id=0)
        {
            ViewBag.showClass = Bll.BllMng_DepartmentClass.Instance().ShowClass(0, 0, false);
            ViewBag.id = id;
            return View();
        }

    }
}