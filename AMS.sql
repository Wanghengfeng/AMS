USE [AMS]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--===============================================================
--新增用户表
--===============================================================
CREATE TABLE [dbo].[Account](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[UserName] [NVARCHAR](20) NOT NULL,
	[Password] [NVARCHAR](50) NOT NULL,
	[PasswordSalt] [NVARCHAR](16) NOT NULL,
	[Name] [NVARCHAR](10) NULL,
	[Email] [NVARCHAR](50) NULL,
	[LastLoginTime] [DATETIME] NOT NULL,
	[LastLoginIp] [NVARCHAR](50) NULL,
	[Isvalid] [BIT] NOT NULL,
	[CreatedBy] [INT] NULL,
	[CreatedTime] [DATETIME] NOT NULL,
	[ModifiedBy] [INT] NULL,
	[ModifiedTime] [DATETIME] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_LastLoginTime]  DEFAULT (GETDATE()) FOR [LastLoginTime]
GO

ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_CreatedTime]  DEFAULT (GETDATE()) FOR [CreatedTime]
GO

ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_ModifiedTime]  DEFAULT (GETDATE()) FOR [ModifiedTime]
GO

--===============================================================
--新增角色表
--===============================================================
CREATE TABLE [dbo].[Role](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Code] [NVARCHAR](50) NOT NULL,
	[Name] [NVARCHAR](20) NOT NULL,
	[Remark] [NVARCHAR](200) NULL,
	[Isvalid] [BIT] NOT NULL,
	[ModifiedTime] [DATETIME] NOT NULL,
	[CreatedTime] [DATETIME] NOT NULL,
	[ModifiedBy] [INT] NULL,
	[CreatedBy] [INT] NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_Isvalid]  DEFAULT ((1)) FOR [Isvalid]
GO

ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_ModifiedTime]  DEFAULT (GETDATE()) FOR [ModifiedTime]
GO

ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_CreatedTime]  DEFAULT (GETDATE()) FOR [CreatedTime]
GO

--===============================================================
--新增用户角色表
--===============================================================
CREATE TABLE [dbo].[AccountRole](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[AccountId] [INT] NOT NULL,
	[RoleId] [INT] NOT NULL,
 CONSTRAINT [PK_AccountRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AccountRole]  WITH CHECK ADD  CONSTRAINT [FK_AccountRole_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[AccountRole] CHECK CONSTRAINT [FK_AccountRole_Account]
GO

ALTER TABLE [dbo].[AccountRole]  WITH CHECK ADD  CONSTRAINT [FK_AccountRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO

ALTER TABLE [dbo].[AccountRole] CHECK CONSTRAINT [FK_AccountRole_Role]
GO

--===============================================================
--新增权限表
--===============================================================
CREATE TABLE [dbo].[Permission](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Code] [NVARCHAR](50) NOT NULL,
	[Name] [NVARCHAR](50) NOT NULL,
	[Category] [NVARCHAR](50) NOT NULL,
	[Isvalid] [BIT] NOT NULL,
	[ModifiedBy] [INT] NULL,
	[ModifiedTime] [DATETIME] NOT NULL,
	[CreatedTime] [DATETIME] NOT NULL,
	[CreatedBy] [INT] NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_Permission_Isvalid]  DEFAULT ((1)) FOR [Isvalid]
GO

ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_Permission_ModifiedTime]  DEFAULT (GETDATE()) FOR [ModifiedTime]
GO

ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [DF_Permission_CreatedTime]  DEFAULT (GETDATE()) FOR [CreatedTime]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permission', @level2type=N'COLUMN',@level2name=N'Category'
GO


--===============================================================
--新增角色权限表
--===============================================================
CREATE TABLE [dbo].[RolePermission](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[RoleId] [INT] NOT NULL,
	[PermissionId] [INT] NOT NULL,
 CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD  CONSTRAINT [FK_RolePermission_Permission] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permission] ([Id])
GO

ALTER TABLE [dbo].[RolePermission] CHECK CONSTRAINT [FK_RolePermission_Permission]
GO

ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD  CONSTRAINT [FK_RolePermission_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO

ALTER TABLE [dbo].[RolePermission] CHECK CONSTRAINT [FK_RolePermission_Role]
GO