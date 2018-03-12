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

Date: 2018-03-10 17:30:35
*/


-- ----------------------------
-- Table structure for Supplier_Type
-- ----------------------------
DROP TABLE [dbo].[Supplier_Type]
GO
CREATE TABLE [dbo].[Supplier_Type] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ClassId] int NULL ,
[ClassName] varchar(100) NULL ,
[Display] int NULL DEFAULT ((1)) 
)


GO
