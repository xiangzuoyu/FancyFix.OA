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

Date: 2018-05-05 08:56:24
*/


-- ----------------------------
-- Table structure for Supplier_Price
-- ----------------------------
DROP TABLE [dbo].[Supplier_Price]
GO
CREATE TABLE [dbo].[Supplier_Price] (
[Id] int NOT NULL IDENTITY(1,1) ,
[RawMaterialPriceId] int NULL ,
[Years] int NULL ,
[Month] int NULL ,
[Price] money NULL ,
[AddDate] datetime NULL ,
[AddUserId] int NULL ,
[LastDate] datetime NULL ,
[LastUserId] int NULL ,
[Display] int NULL ,
[YearsMonth] datetime NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Supplier_Price]', RESEED, 244)
GO

-- ----------------------------
-- Indexes structure for table Supplier_Price
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Supplier_Price
-- ----------------------------
ALTER TABLE [dbo].[Supplier_Price] ADD PRIMARY KEY ([Id])
GO
