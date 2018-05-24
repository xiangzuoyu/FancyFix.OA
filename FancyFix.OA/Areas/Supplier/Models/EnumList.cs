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
        Parts = 4,
        [Description("Convert")]
        Convert = 5
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

    public enum GetFormOfBusinessId : byte
    {
        [Description("外资公司")]
        外资公司 = 1,
        [Description("私人有限责任公司")]
        私人有限责任公司 = 2,
        [Description("有限公司")]
        有限公司 = 3,
        [Description("上市公司")]
        上市公司 = 4,
        [Description("其他")]
        其他 = 5,

    }
    public enum GetVersionTypeId : byte
    {
        [Description("原辅材料")]
        原辅材料 = 1,
        [Description("间接物料")]
        间接物料 = 2,
        [Description("服务")]
        服务 = 3,
        [Description("固定资产")]
        固定资产 = 4,
        [Description("生产性企业")]
        生产性企业 = 5,
        [Description("贸易类企业")]
        贸易类企业 = 6,
    }
}