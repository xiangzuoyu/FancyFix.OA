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

Date: 2018-01-29 14:01:45
*/


-- ----------------------------
-- Table structure for DemandType
-- ----------------------------
DROP TABLE [dbo].[Design_DemandType]
GO
CREATE TABLE [dbo].[Design_DemandType] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Display] int NULL ,
[ClassId] int NULL ,
[Name] nvarchar(128) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Design_DemandType]', RESEED, 15)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'DemandType', 
'COLUMN', N'Display')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否显示'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'DemandType'
, @level2type = 'COLUMN', @level2name = N'Display'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否显示'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'DemandType'
, @level2type = 'COLUMN', @level2name = N'Display'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'DemandType', 
'COLUMN', N'Name')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'需求类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'DemandType'
, @level2type = 'COLUMN', @level2name = N'Name'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'需求类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'DemandType'
, @level2type = 'COLUMN', @level2name = N'Name'
GO

-- ----------------------------
-- Records of DemandType
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Design_DemandType] ON
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'1', N'1', N'1', N'产品设计')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'2', N'1', N'2', N'产品包装')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'3', N'1', N'3', N'拍照修图')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'4', N'1', N'4', N'场景搭配图')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'5', N'1', N'5', N'使用说明书')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'6', N'1', N'6', N'宝贝详情页')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'7', N'1', N'7', N'目录')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'8', N'1', N'8', N'PPT')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'9', N'1', N'9', N'视频拍摄')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'10', N'1', N'10', N'产品周边')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'11', N'1', N'11', N'品牌周边')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'12', N'1', N'12', N'品牌VI')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'13', N'1', N'13', N'活动策划')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'14', N'1', N'14', N'LOGO设计')
GO
GO
INSERT INTO [dbo].[Design_DemandType] ([Id], [Display], [ClassId], [Name]) VALUES (N'15', N'1', N'15', N'文化墙')
GO
GO
SET IDENTITY_INSERT [dbo].[Design_DemandType] OFF
GO

-- ----------------------------
-- Indexes structure for table DemandType
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DemandType
-- ----------------------------
ALTER TABLE [dbo].[Design_DemandType] ADD PRIMARY KEY ([Id])
GO
