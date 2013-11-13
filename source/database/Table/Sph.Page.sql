 USE [Sph]
 GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sph].[Page]') AND type in (N'U'))
DROP TABLE [Sph].[Page]
GO



 CREATE TABLE Sph.Page
(
	 [PageId] INT PRIMARY KEY IDENTITY(1,1)
	,[Title] VARCHAR(255) NOT NULL
	,[Tag] VARCHAR(255) NULL
	,[Version] INT NULL
	,[IsPartial] BIT NOT NULL
	,[IsRazor] BIT NOT NULL
	,[VirtualPath] VARCHAR(4000) NULL
	,[Data] XML NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)
