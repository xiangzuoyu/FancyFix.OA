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

Date: 2018-03-16 17:53:07
*/


-- ----------------------------
-- Table structure for Supplier_PriceMapping
-- ----------------------------
DROP TABLE [dbo].[Supplier_PriceMapping]
GO
CREATE TABLE [dbo].[Supplier_PriceMapping] (
[Id] int NOT NULL IDENTITY(1,1) ,
[RawMaterialCode] int NULL ,
[VendorCode] int NULL ,
[AddDate] datetime NULL ,
[AddUserId] int NULL ,
[LastDate] datetime NULL ,
[LastUserId] int NULL ,
[Display] int NULL DEFAULT ((1)) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_PriceMapping', 
'COLUMN', N'RawMaterialCode')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'原材料ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_PriceMapping'
, @level2type = 'COLUMN', @level2name = N'RawMaterialCode'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'原材料ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_PriceMapping'
, @level2type = 'COLUMN', @level2name = N'RawMaterialCode'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_PriceMapping', 
'COLUMN', N'VendorCode')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'供应商ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_PriceMapping'
, @level2type = 'COLUMN', @level2name = N'VendorCode'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'供应商ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_PriceMapping'
, @level2type = 'COLUMN', @level2name = N'VendorCode'
GO

-- ----------------------------
-- Indexes structure for table Supplier_PriceMapping
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Supplier_PriceMapping
-- ----------------------------
ALTER TABLE [dbo].[Supplier_PriceMapping] ADD PRIMARY KEY ([Id])
GO
