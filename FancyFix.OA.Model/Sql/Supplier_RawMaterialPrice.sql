/*
Navicat SQL Server Data Transfer

Source Server         : local1
Source Server Version : 100000
Source Host           : (local):1433
Source Database       : OA
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 100000
File Encoding         : 65001

Date: 2018-05-05 13:56:51
*/


-- ----------------------------
-- Table structure for Supplier_RawMaterialPrice
-- ----------------------------
DROP TABLE [dbo].[Supplier_RawMaterialPrice]
GO
CREATE TABLE [dbo].[Supplier_RawMaterialPrice] (
[Id] int NOT NULL IDENTITY(1,1) ,
[RawMaterialId] nvarchar(128) NULL ,
[AddDate] datetime NULL ,
[AddUserId] int NULL ,
[LastDate] datetime NULL ,
[LastUserId] int NULL ,
[Display] int NULL DEFAULT ((1)) ,
[VendorId] nvarchar(128) NULL ,
[PriceFrequency] int NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Supplier_RawMaterialPrice]', RESEED, 48)
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
