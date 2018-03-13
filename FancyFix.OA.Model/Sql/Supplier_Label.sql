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

Date: 2018-03-10 17:26:52
*/


-- ----------------------------
-- Table structure for Supplier_Label
-- ----------------------------
DROP TABLE [dbo].[Supplier_Label]
GO
CREATE TABLE [dbo].[Supplier_Label] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ClassId] int NULL ,
[ClassName] nvarchar(100) NULL ,
[Display] int NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Supplier_Label]', RESEED, 4)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'Supplier_Label', 
'COLUMN', N'Display')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'状态，1：可用；2：不可用'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_Label'
, @level2type = 'COLUMN', @level2name = N'Display'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'状态，1：可用；2：不可用'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'Supplier_Label'
, @level2type = 'COLUMN', @level2name = N'Display'
GO

-- ----------------------------
-- Records of Supplier_Label
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Supplier_Label] ON
GO
INSERT INTO [dbo].[Supplier_Label] ([Id], [ClassId], [ClassName], [Display]) VALUES (N'1', N'1', N'合格', N'1')
GO
GO
INSERT INTO [dbo].[Supplier_Label] ([Id], [ClassId], [ClassName], [Display]) VALUES (N'2', N'2', N'潜在', N'1')
GO
GO
INSERT INTO [dbo].[Supplier_Label] ([Id], [ClassId], [ClassName], [Display]) VALUES (N'3', N'3', N'黑名单', N'1')
GO
GO
INSERT INTO [dbo].[Supplier_Label] ([Id], [ClassId], [ClassName], [Display]) VALUES (N'4', N'4', N'其他', N'1')
GO
GO
SET IDENTITY_INSERT [dbo].[Supplier_Label] OFF
GO

-- ----------------------------
-- Indexes structure for table Supplier_Label
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Supplier_Label
-- ----------------------------
ALTER TABLE [dbo].[Supplier_Label] ADD PRIMARY KEY ([Id])
GO
