USE [Sph]
GO
 IF OBJECT_ID('Sph.ReportDefinition', 'U') IS NOT NULL
  DROP TABLE Sph.ReportDefinition
GO

CREATE TABLE [Sph].[ReportDefinition](
	[ReportDefinitionId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Title] VARCHAR(255) NOT NULL	,
	[Description] VARCHAR(2000) NULL,
	[IsActive] BIT NOT NULL	,
	[IsPrivate] BIT NOT NULL,
	[Data] XML NOT NULL	,
	[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(255) NULL,
	[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[ChangedBy] VARCHAR(255) NULL
	)
GO



