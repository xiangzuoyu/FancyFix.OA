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

Date: 2018-03-16 17:53:18
*/


-- ----------------------------
-- Table structure for Supplier_RawMaterial
-- ----------------------------
DROP TABLE [dbo].[Supplier_RawMaterial]
GO
CREATE TABLE [dbo].[Supplier_RawMaterial] (
[Id] int NOT NULL IDENTITY(1,1) ,
[BU] nvarchar(128) NULL ,
[SAPCode] nvarchar(128) NULL ,
[Description] nvarchar(128) NULL ,
[Category] nvarchar(128) NULL ,
[LeadBuyer] nvarchar(128) NULL ,
[VendorId] int NULL ,
[PriceFrequency] int NULL ,
[Currency] nvarchar(128) NULL ,
[AddDate] datetime NULL ,
[AddUserId] int NULL ,
[LastDate] datetime NULL ,
[LastUserId] int NULL ,
[Display] int NULL DEFAULT ((1)) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterial', 
'COLUMN', N'BU')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'业务部门'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'BU'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'业务部门'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'BU'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterial', 
'COLUMN', N'SAPCode')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'原材料代码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'SAPCode'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'原材料代码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'SAPCode'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterial', 
'COLUMN', N'Description')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'采购产品名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'Description'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'采购产品名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'Description'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterial', 
'COLUMN', N'Category')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'品类'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'Category'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'品类'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'Category'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterial', 
'COLUMN', N'LeadBuyer')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'采购负责人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'LeadBuyer'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'采购负责人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'LeadBuyer'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterial', 
'COLUMN', N'VendorId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'供应商代码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'VendorId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'供应商代码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'VendorId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterial', 
'COLUMN', N'PriceFrequency')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'价格频次（月/季度/半年/年/单次）'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'PriceFrequency'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'价格频次（月/季度/半年/年/单次）'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'PriceFrequency'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterial', 
'COLUMN', N'Currency')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'价格单位'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'Currency'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'价格单位'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterial'
, @level2type = 'COLUMN', @level2name = N'Currency'
GO

-- ----------------------------
-- Indexes structure for table Supplier_RawMaterial
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Supplier_RawMaterial
-- ----------------------------
ALTER TABLE [dbo].[Supplier_RawMaterial] ADD PRIMARY KEY ([Id])
GO
