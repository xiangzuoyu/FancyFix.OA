using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FancyFix.OA.Areas.Supplier.Models
{
    public enum SupplierLabel
    {
        合格 = 1,
        潜在 = 2,
        黑名单 = 3
    }

    public enum SupplierType
    {
        RM = 1,
        PM = 2,
        FG = 3,
        Parts = 4,
        Convert = 5,
    }
}