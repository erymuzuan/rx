
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sph].[Trigger]') AND type in (N'U'))
DROP TABLE [Sph].[Trigger]
GO



 CREATE TABLE [Sph].[Trigger]
(
	 [TriggerId] INT PRIMARY KEY IDENTITY(1,1)
	,[Name] VARCHAR(255) NOT NULL
	,[Entity] VARCHAR(255) NULL
	,[TypeOf] VARCHAR(255) NULL
	,[Json] NVARCHAR(MAX) NOT NULL
	,[IsActive] BIT NOT NULL DEFAULT 1
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)

GO 
ALTER TABLE [Sph].[Trigger]
ADD [IsActive] BIT NOT NULL DEFAULT 1