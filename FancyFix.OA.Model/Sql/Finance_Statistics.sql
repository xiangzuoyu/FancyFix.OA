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

Date: 2018-08-18 11:55:48
*/


-- ----------------------------
-- Table structure for Finance_Statistics
-- ----------------------------
DROP TABLE [dbo].[Finance_Statistics]
GO
CREATE TABLE [dbo].[Finance_Statistics] (
[Id] int NOT NULL IDENTITY(1,1) ,
[SaleDate] datetime NULL ,
[Year] int NULL ,
[Month] int NULL ,
[Day] int NULL ,
[BusinessIncome] money NULL ,
[BudgetaryValue] money NULL ,
[BusinessIncomeRate] money NULL ,
[ActualReceipts] money NULL ,
[ActualDeliveryOrderNumber] money NULL ,
[PlanDeliveryOrderNumber] money NULL ,
[DeliveryPunctualityRate] money NULL ,
[AddUserId] int NULL ,
[AddDate] datetime NULL ,
[LastUserId] int NULL ,
[LastDate] datetime NULL ,
[Display] int NULL ,
[DepartmentName] nvarchar(128) NULL 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_Statistics', 
'COLUMN', N'BusinessIncome')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'营业收入'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'BusinessIncome'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'营业收入'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'BusinessIncome'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_Statistics', 
'COLUMN', N'BudgetaryValue')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'营业收入预算值'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'BudgetaryValue'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'营业收入预算值'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'BudgetaryValue'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_Statistics', 
'COLUMN', N'BusinessIncomeRate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'营业收入达成率'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'BusinessIncomeRate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'营业收入达成率'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'BusinessIncomeRate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_Statistics', 
'COLUMN', N'ActualReceipts')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'实际收款'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'ActualReceipts'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'实际收款'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'ActualReceipts'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_Statistics', 
'COLUMN', N'ActualDeliveryOrderNumber')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'实际发货订单数量'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'ActualDeliveryOrderNumber'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'实际发货订单数量'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'ActualDeliveryOrderNumber'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_Statistics', 
'COLUMN', N'PlanDeliveryOrderNumber')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'计划发货订单数量'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'PlanDeliveryOrderNumber'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'计划发货订单数量'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'PlanDeliveryOrderNumber'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_Statistics', 
'COLUMN', N'DeliveryPunctualityRate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'发货准时率'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'DeliveryPunctualityRate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'发货准时率'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'DeliveryPunctualityRate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Finance_Statistics', 
'COLUMN', N'DepartmentName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'部门'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'DepartmentName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'部门'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Finance_Statistics'
, @level2type = 'COLUMN', @level2name = N'DepartmentName'
GO

-- ----------------------------
-- Indexes structure for table Finance_Statistics
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Finance_Statistics
-- ----------------------------
ALTER TABLE [dbo].[Finance_Statistics] ADD PRIMARY KEY ([Id])
GO
