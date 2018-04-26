using Dos.DataAccess.Base;
using Dos.ORM;
using FancyFix.OA.Model;
using FancyFix.OA.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FancyFix.OA.Bll
{
    public class BllMng_DepartmentClass : BllSys_Class<Mng_DepartmentClass>
    {
        public new static BllMng_DepartmentClass Instance()
        {
            return new BllMng_DepartmentClass();
        }
    }
}
