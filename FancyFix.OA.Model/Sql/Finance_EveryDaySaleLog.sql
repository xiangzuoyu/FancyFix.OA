/*
Navicat SQL Server Data Transfer

Source Server         : YZX-PC
Source Server Version : 110000
Source Host           : 127.0.0.1:1433
Source Database       : OA
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 110000
File Encoding         : 65001

Date: 2018-09-20 09:43:21
*/


-- ----------------------------
-- Table structure for Finance_EveryDaySaleLog
-- ----------------------------
DROP TABLE [dbo].[Finance_EveryDaySaleLog]
GO
CREATE TABLE [dbo].[Finance_EveryDaySaleLog] (
[Id] int NOT NULL IDENTITY(1,1) ,
[SaleDate] datetime NULL ,
[Year] int NULL ,
[Month] int NULL ,
[Day] int NULL ,
[DepartmentName] nvarchar(128) NULL ,
[SaleName] nvarchar(128) NULL ,
[Customer] nvarchar(256) NULL ,
[ContractNumber] nvarchar(64) NULL ,
[ProductName] nvarchar(256) NULL ,
[ProductSKU] nvarchar(256) NULL ,
[SaleCount] money NULL ,
[SalePrice] money NULL ,
[Currency] nvarchar(64) NULL ,
[ExchangeRate] money NULL ,
[SaleIncome] money NULL ,
[MaterialUnitPrice] money NULL ,
[ProcessUnitPrice] money NULL ,
[MaterialTotalPrice] money NULL ,
[ProcessTotalPrice] money NULL ,
[GrossProfit] money NULL ,
[GrossProfitRate] money NULL ,
[ChangeCostNumber] money NULL ,
[ChangeCostMatter] money NULL ,
[ContributionMoney] money NULL ,
[ContributionRatio] money NULL ,
[AvgCoatUndue] money NULL ,
[AvgCoatCurrentdue] money NULL ,
[AvgCoatOverdue] money NULL ,
[CustomerContributionMoney] money NULL ,
[CustomerContributionRatio] money NULL ,
[Follow] bit NULL ,
[AddDate] datetime NULL ,
[AddUserId] int NULL ,
[LastDate] datetime NULL ,
[LastUserId] int NULL ,
[Display] int NULL DEFAULT ((1)) ,
[ProductSpecification] nvarchar(512) NULL ,
[Supplier] nvarchar(512) NULL ,
[SPU] nvarchar(256) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Finance_EveryDaySaleLog]', RESEED, 1040117)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'SaleDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'销售日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SaleDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'销售日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SaleDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'DepartmentName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'部门'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'DepartmentName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'部门'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'DepartmentName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'SaleName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'销售员'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SaleName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'销售员'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SaleName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'Customer')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'客户/店铺'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Customer'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'客户/店铺'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Customer'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ContractNumber')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'合同号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ContractNumber'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'合同号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ContractNumber'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ProductName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'产品名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProductName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'产品名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProductName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ProductSKU')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'产品SKU'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProductSKU'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'产品SKU'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProductSKU'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'SaleCount')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'销售数量'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SaleCount'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'销售数量'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SaleCount'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'SalePrice')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'销售单价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SalePrice'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'销售单价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SalePrice'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'Currency')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'货币种类'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Currency'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'货币种类'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Currency'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ExchangeRate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'汇率'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ExchangeRate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'汇率'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ExchangeRate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'SaleIncome')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'销售收入'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SaleIncome'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'销售收入'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'SaleIncome'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'MaterialUnitPrice')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'材料单价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'MaterialUnitPrice'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'材料单价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'MaterialUnitPrice'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ProcessUnitPrice')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'加工费单价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProcessUnitPrice'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'加工费单价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProcessUnitPrice'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'MaterialTotalPrice')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'材料总价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'MaterialTotalPrice'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'材料总价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'MaterialTotalPrice'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ProcessTotalPrice')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'加工费总价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProcessTotalPrice'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'加工费总价'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProcessTotalPrice'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'GrossProfit')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'毛利额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'GrossProfit'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'毛利额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'GrossProfit'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'GrossProfitRate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'毛利率'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'GrossProfitRate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'毛利率'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'GrossProfitRate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ChangeCostNumber')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'变动费用量'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ChangeCostNumber'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'变动费用量'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ChangeCostNumber'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ChangeCostMatter')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'变动费用事'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ChangeCostMatter'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'变动费用事'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ChangeCostMatter'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ContributionMoney')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'经营贡献金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ContributionMoney'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'经营贡献金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ContributionMoney'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ContributionRatio')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'贡献比例'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ContributionRatio'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'贡献比例'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ContributionRatio'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'AvgCoatUndue')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'应收账款平均资金成本_未到期 '
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'AvgCoatUndue'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'应收账款平均资金成本_未到期 '
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'AvgCoatUndue'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'AvgCoatCurrentdue')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'应收账款平均资金成本_当期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'AvgCoatCurrentdue'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'应收账款平均资金成本_当期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'AvgCoatCurrentdue'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'AvgCoatOverdue')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'应收账款平均资金成本_逾期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'AvgCoatOverdue'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'应收账款平均资金成本_逾期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'AvgCoatOverdue'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'CustomerContributionMoney')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'客户净贡献_金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'CustomerContributionMoney'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'客户净贡献_金额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'CustomerContributionMoney'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'CustomerContributionRatio')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'客户净贡献_比例'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'CustomerContributionRatio'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'客户净贡献_比例'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'CustomerContributionRatio'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'Follow')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否关注'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Follow'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否关注'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Follow'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'Display')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'1：显示，2：隐藏'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Display'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'1：显示，2：隐藏'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Display'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'ProductSpecification')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'产品规格'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProductSpecification'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'产品规格'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'ProductSpecification'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_EveryDaySaleLog', 
'COLUMN', N'Supplier')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'供应商'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Supplier'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'供应商'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_EveryDaySaleLog'
, @level2type = 'COLUMN', @level2name = N'Supplier'
GO

-- ----------------------------
-- Indexes structure for table Finance_EveryDaySaleLog
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Finance_EveryDaySaleLog
-- ----------------------------
ALTER TABLE [dbo].[Finance_EveryDaySaleLog] ADD PRIMARY KEY ([Id])
GO
