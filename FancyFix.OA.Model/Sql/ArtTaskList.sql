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

Date: 2018-01-29 14:01:33
*/


-- ----------------------------
-- Table structure for ArtTaskList
-- ----------------------------
DROP TABLE [dbo].[Design_ArtTaskList]
GO
CREATE TABLE [dbo].[Design_ArtTaskList] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Number] varchar(100) NULL ,
[Title] nvarchar(256) NULL ,
[Content] nvarchar(MAX) NULL ,
[Phone] varchar(15) NULL ,
[DueDate] datetime NULL ,
[SubmitterId] int NULL ,
[SubmittedDate] datetime NULL ,
[DesignerId] int NULL ,
[EstimatedStartDate] datetime NULL ,
[EstimatedEndDate] datetime NULL ,
[CompletionDate] datetime NULL ,
[AMPM] int NULL ,
[DepartmentId] int NULL ,
[Budget] money NULL ,
[DemandTypeId] int NULL ,
[Model] nvarchar(128) NULL ,
[StyleType] nvarchar(2048) NULL ,
[DetailTypeId] int SPARSE NULL ,
[Uri1] nvarchar(512) NULL ,
[Uri2] nvarchar(512) NULL ,
[Score] int NULL ,
[Display] int NULL DEFAULT ((1)) 
)


GO
DBCC CHECKIDENT(N'[dbo].[Design_ArtTaskList]', RESEED, 12)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'Number')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'编号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Number'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'编号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Number'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'Title')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'任务标题'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Title'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'任务标题'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Title'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'Content')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'任务详细'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Content'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'任务详细'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Content'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'Phone')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'事业部联系号码，用于沟通联系'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Phone'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'事业部联系号码，用于沟通联系'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Phone'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'DueDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'截止日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DueDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'截止日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DueDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'SubmitterId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'任务提交人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'SubmitterId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'任务提交人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'SubmitterId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'SubmittedDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'提交任务日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'SubmittedDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'提交任务日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'SubmittedDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'DesignerId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'设计师'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DesignerId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'设计师'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DesignerId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'EstimatedStartDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'预计开始日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'EstimatedStartDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'预计开始日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'EstimatedStartDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'EstimatedEndDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'预计结束日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'EstimatedEndDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'预计结束日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'EstimatedEndDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'CompletionDate')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'实际完成日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'CompletionDate'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'实际完成日期'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'CompletionDate'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'AMPM')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'上下午'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'AMPM'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'上下午'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'AMPM'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'DepartmentId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'需求部门'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DepartmentId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'需求部门'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DepartmentId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'Budget')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'项目预算'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Budget'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'项目预算'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Budget'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'DemandTypeId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'需求类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DemandTypeId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'需求类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DemandTypeId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'Model')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'型号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Model'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'型号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Model'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'StyleType')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'风格类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'StyleType'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'风格类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'StyleType'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'DetailTypeId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'细节类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DetailTypeId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'细节类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'DetailTypeId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'Uri1')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'参考地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Uri1'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'参考地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Uri1'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'Score')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'评分'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Score'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'评分'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Score'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'ArtTaskList', 
'COLUMN', N'Display')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'需求状态'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Display'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'需求状态'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'ArtTaskList'
, @level2type = 'COLUMN', @level2name = N'Display'
GO

-- ----------------------------
-- Indexes structure for table ArtTaskList
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table ArtTaskList
-- ----------------------------
ALTER TABLE [dbo].[Design_ArtTaskList] ADD PRIMARY KEY ([Id])
GO
