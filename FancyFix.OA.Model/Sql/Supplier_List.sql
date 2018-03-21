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

Date: 2018-03-20 21:21:43
*/


-- ----------------------------
-- Table structure for Supplier_List
-- ----------------------------
DROP TABLE [dbo].[Supplier_List]
GO
CREATE TABLE [dbo].[Supplier_List] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Code] varchar(255) NULL ,
[Name] nvarchar(1024) NULL ,
[SupplierType] int NULL ,
[BusinessScope] nvarchar(2048) NULL ,
[Contact1] varchar(128) NULL ,
[Contact2] varchar(128) NULL ,
[Site] nvarchar(1024) NULL ,
[Address] nvarchar(2048) NULL ,
[StartDate] datetime NULL ,
[EndDate] datetime NULL ,
[LabelId] int NULL ,
[Note] nvarchar(2048) NULL ,
[SupplierAb] nvarchar(128) NULL ,
[AddDate] datetime SPARSE NULL ,
[AccountDate] nvarchar(126) NULL ,
[AddUserId] int NULL ,
[LastDate] datetime NULL ,
[LastUserId] int NULL ,
[Display] int NULL DEFAULT ((1)) 
)


GO
DBCC CHECKIDENT(N'[dbo].[Supplier_List]', RESEED, 352)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'Code')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'供应商代码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Code'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'供应商代码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Code'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'Name')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'供应商名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Name'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'供应商名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Name'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'SupplierType')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'供应商类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'SupplierType'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'供应商类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'SupplierType'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'BusinessScope')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'经营范围'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'BusinessScope'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'经营范围'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'BusinessScope'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'Contact1')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'联系人1'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Contact1'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'联系人1'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Contact1'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'Contact2')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'联系人2'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Contact2'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'联系人2'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Contact2'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'Site')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'网址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Site'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'网址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Site'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'Address')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Address'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Address'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'StartDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'开始时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'StartDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'开始时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'StartDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'EndDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'结束时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'EndDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'结束时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'EndDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'LabelId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'供应商标签'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'LabelId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'供应商标签'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'LabelId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'Note')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'备注'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Note'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'备注'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Note'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'SupplierAb')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'供应商名称缩写'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'SupplierAb'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'供应商名称缩写'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'SupplierAb'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'AddDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'添加时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'AddDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'添加时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'AddDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'AccountDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'账期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'AccountDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'账期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'AccountDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'AddUserId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'添加人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'AddUserId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'添加人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'AddUserId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'LastDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'最后修改时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'LastDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'最后修改时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'LastDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'LastUserId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'最后修改人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'LastUserId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'最后修改人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'LastUserId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_List', 
'COLUMN', N'Display')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'1：显示，2：隐藏'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Display'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'1：显示，2：隐藏'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_List'
, @level2type = 'COLUMN', @level2name = N'Display'
GO

-- ----------------------------
-- Indexes structure for table Supplier_List
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Supplier_List
-- ----------------------------
ALTER TABLE [dbo].[Supplier_List] ADD PRIMARY KEY ([Id])
GO
