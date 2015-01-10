
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sph].[ScreenActivityForm]') AND type in (N'U'))
DROP TABLE [Sph].[ScreenActivityForm]
GO



 CREATE TABLE [Sph].[ScreenActivityForm]
(
	[Id] VARCHAR(50) PRIMARY KEY NOT NULL
	,[Title] VARCHAR(855) NOT NULL
	,[Json] VARCHAR(MAX) NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)
