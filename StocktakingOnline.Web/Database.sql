--数据库建表脚本(SQL Server)

CREATE DATABASE [STO];
GO
USE [STO];
GO

--2018/5/15
--日志表
CREATE TABLE [Logs] (
      [Id] [int] IDENTITY(1,1) NOT NULL,
      [Application] [nvarchar](50) NOT NULL,
      [Logged] [datetime] NOT NULL,
      [Level] [nvarchar](50) NOT NULL,
      [Message] [nvarchar](max) NOT NULL,
      [UserName] [nvarchar](250) NULL,
      [ServerName] [nvarchar](max) NULL,
      [Port] [nvarchar](max) NULL,
      [Url] [nvarchar](max) NULL,
      [Https] [bit] NULL,
      [ServerAddress] [nvarchar](100) NULL,
      [RemoteAddress] [nvarchar](100) NULL,
      [Logger] [nvarchar](250) NULL,
      [Callsite] [nvarchar](max) NULL,
      [Exception] [nvarchar](max) NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
GO

--工作表
CREATE TABLE Jobs
(
    JobId int IDENTITY(1,1),
    JobName NVARCHAR(50) NOT NULL,
    JobDescription NVARCHAR(50),
    IsOpened BIT NOT NULL,
    CONSTRAINT [PK_dbo.Jobs] PRIMARY KEY CLUSTERED ([JobId] ASC)
);
GO

--用户表
CREATE TABLE Users
(
    UserId int IDENTITY(1,1),
    UserName NVARCHAR(50) NOT NULL, --登录名称，例如手机号
    DisplayName NVARCHAR(50) NOT NULL, --显示名称，例如真实姓名
    PasswordHash NVARCHAR(200) NOT NULL, --加密后的密码
    CreatedTime DATETIME NOT NULL, --创建时间
    CurrentJobId int NULL, --当前所在Job
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
)
GO
CREATE NONCLUSTERED INDEX [IX_Users_UserName] ON [Users] ([UserName])
GO

--角色表
CREATE TABLE [dbo].[Roles]
(
    [RoleId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [RoleName] NVARCHAR(50) NOT NULL,
)
GO
CREATE NONCLUSTERED INDEX [IX_Roles_RoleName] ON [Roles] ([RoleName])
GO

--用户-角色表
CREATE TABLE [dbo].[UserRoles]
(
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL
    PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_User] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]),
    CONSTRAINT [FK_UserRoles_Role] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([RoleId])
)
GO

--用户-角色视图
CREATE VIEW UserRolesView AS
SELECT ur.*, r.RoleName
FROM UserRoles ur
JOIN Roles r ON ur.RoleId = r.RoleId
GO


--盘点数据表
CREATE TABLE InventoryItems
(
    RecordId NVARCHAR(50) NOT NULL PRIMARY KEY,
    JobId int NOT NULL,
    UserId int NOT NULL,
    ProductId NVARCHAR(50),
    Quantity DECIMAL(18,5) NOT NULL,
    DepartmentId int NOT NULL,
    Brand NVARCHAR(50) NOT NULL,
    Model NVARCHAR(50),
    SerialNumber NVARCHAR(50),
    AssetNumber NVARCHAR(50),
    ImageFiles NVARCHAR(MAX),
    CreatedTime DATETIME NOT NULL
)
GO

--类别表
CREATE TABLE Departments
(
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(50) NOT NULL,
    DepartmentDescription NVARCHAR(200)
)
GO

--查询数据视图
CREATE VIEW InventoryItemsView AS
    SELECT ii.*, dpt.DepartmentName, u.DisplayName
    FROM InventoryItems ii
    LEFT JOIN Departments dpt ON ii.DepartmentId=dpt.DepartmentId
    LEFT JOIN Users u ON ii.UserId=u.UserId
GO
