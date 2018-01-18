using FancyFix.OA.Model;
using System.Collections.Generic;

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
