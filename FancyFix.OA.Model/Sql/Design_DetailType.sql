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

Date: 2018-01-29 14:01:54
*/


-- ----------------------------
-- Table structure for DetailType
-- ----------------------------
DROP TABLE [dbo].[DetailType]
GO
CREATE TABLE [dbo].[DetailType] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ClassId] int NULL ,
[Display] int NULL DEFAULT ((1)) ,
[Name] nvarchar(256) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[DetailType]', RESEED, 4)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'DetailType', 
'COLUMN', N'Display')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否显示；1显示，2不显示'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'DetailType'
, @level2type = 'COLUMN', @level2name = N'Display'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否显示；1显示，2不显示'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'DetailType'
, @level2type = 'COLUMN', @level2name = N'Display'
GO

-- ----------------------------
-- Records of DetailType
-- ----------------------------
SET IDENTITY_INSERT [dbo].[DetailType] ON
GO
INSERT INTO [dbo].[DetailType] ([Id], [ClassId], [Display], [Name]) VALUES (N'1', N'1', N'1', N'主题')
GO
GO
INSERT INTO [dbo].[DetailType] ([Id], [ClassId], [Display], [Name]) VALUES (N'2', N'2', N'1', N'关键词')
GO
GO
INSERT INTO [dbo].[DetailType] ([Id], [ClassId], [Display], [Name]) VALUES (N'3', N'3', N'1', N'规格')
GO
GO
INSERT INTO [dbo].[DetailType] ([Id], [ClassId], [Display], [Name]) VALUES (N'4', N'4', N'1', N'材质')
GO
GO
SET IDENTITY_INSERT [dbo].[DetailType] OFF
GO

-- ----------------------------
-- Indexes structure for table DetailType
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table DetailType
-- ----------------------------
ALTER TABLE [dbo].[DetailType] ADD PRIMARY KEY ([Id])
GO
