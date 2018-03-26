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

Date: 2018-03-26 13:45:02
*/


-- ----------------------------
-- Table structure for Supplier_RawMaterialPrice
-- ----------------------------
DROP TABLE [dbo].[Supplier_RawMaterialPrice]
GO
CREATE TABLE [dbo].[Supplier_RawMaterialPrice] (
[Id] int NOT NULL IDENTITY(1,1) ,
[RawMaterialId] int NULL ,
[Years] int NULL ,
[Month1] money NULL ,
[Month2] money NULL ,
[Month3] money NULL ,
[Month4] money NULL ,
[Month5] money NULL ,
[Month6] money NULL ,
[Month7] money NULL ,
[Month8] money NULL ,
[Month9] money NULL ,
[Month10] money NULL ,
[Month11] money NULL ,
[Month12] money NULL ,
[AddDate] datetime NULL ,
[AddUserId] int NULL ,
[LastDate] datetime NULL ,
[LastUserId] int NULL ,
[Display] int NULL DEFAULT ((1)) ,
[VendorId] int NULL ,
[PriceFrequency] int NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Supplier_RawMaterialPrice]', RESEED, 33)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterialPrice', 
'COLUMN', N'RawMaterialId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'原材料ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterialPrice'
, @level2type = 'COLUMN', @level2name = N'RawMaterialId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'原材料ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterialPrice'
, @level2type = 'COLUMN', @level2name = N'RawMaterialId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterialPrice', 
'COLUMN', N'Years')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'年份'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterialPrice'
, @level2type = 'COLUMN', @level2name = N'Years'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'年份'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterialPrice'
, @level2type = 'COLUMN', @level2name = N'Years'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterialPrice', 
'COLUMN', N'VendorId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'供应商ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterialPrice'
, @level2type = 'COLUMN', @level2name = N'VendorId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'供应商ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterialPrice'
, @level2type = 'COLUMN', @level2name = N'VendorId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_RawMaterialPrice', 
'COLUMN', N'PriceFrequency')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'价格频次'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterialPrice'
, @level2type = 'COLUMN', @level2name = N'PriceFrequency'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'价格频次'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_RawMaterialPrice'
, @level2type = 'COLUMN', @level2name = N'PriceFrequency'
GO

-- ----------------------------
-- Indexes structure for table Supplier_RawMaterialPrice
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Supplier_RawMaterialPrice
-- ----------------------------
ALTER TABLE [dbo].[Supplier_RawMaterialPrice] ADD PRIMARY KEY ([Id])
GO
