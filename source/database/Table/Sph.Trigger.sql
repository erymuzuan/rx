 USE [Sph]
 GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sph].[Trigger]') AND type in (N'U'))
DROP TABLE [Sph].[Trigger]
GO



 CREATE TABLE [Sph].[Trigger]
(
	 [TriggerId] INT PRIMARY KEY IDENTITY(1,1)
	,[Title] VARCHAR(255) NOT NULL
	,[Entity] VARCHAR(255) NULL
	,[TypeOf] VARCHAR(255) NULL
	,[Data] XML NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)
