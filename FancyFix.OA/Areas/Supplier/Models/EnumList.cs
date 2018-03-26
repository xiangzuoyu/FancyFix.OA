using System.ComponentModel;

namespace FancyFix.OA.Areas.Supplier.Models
{
    public enum SupplierLabel : byte
    {
        [Description("合格")]
        合格 = 1,
        [Description("潜在")]
        潜在 = 2,
        [Description("黑名单")]
        黑名单 = 3
    }

    public enum SupplierType : byte
    {
        [Description("RM")]
        RM = 1,
        [Description("PM")]
        PM = 2,
        [Description("FG")]
        FG = 3,
        [Description("Parts")]
        Parts = 4
    }

    public enum PriceFrequency : byte
    {
        [Description("单次")]
        单次 = 1,
        [Description("月")]
        月 = 2,
        [Description("季")]
        季 = 3,
        [Description("半年")]
        半年 = 4,
        [Description("年")]
        年 = 5
    }

}