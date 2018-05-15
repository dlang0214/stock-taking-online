--数据库建表脚本(SQL Server)

--2018/5/15
--日志表
CREATE TABLE [dbo].[Logs] (
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
    CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED ([Id] ASC)
      WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
  ) ON [PRIMARY]

--工作表
CREATE TABLE Jobs
(
    JobId int IDENTITY(1,1),
    JobName NVARCHAR(50) NOT NULL,
    JobDescription NVARCHAR(50),
    IsOpened BIT NOT NULL,
    CONSTRAINT [PK_dbo.Jobs] PRIMARY KEY CLUSTERED ([JobId] ASC)
)

--用户表
CREATE TABLE Users
(
    UserId int IDENTITY(1,1),
    UserName NVARCHAR(50) NOT NULL, --登录名称，例如手机号
    DisplayName NVARCHAR(50) NOT NULL, --显示名称，例如真实姓名
    PasswordHash NVARCHAR(50) NOT NULL, --加密后的密码
    CreatedTime DATETIME NOT NULL, --创建时间
    CurrentJobId int NULL, --当前所在Job
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
)


