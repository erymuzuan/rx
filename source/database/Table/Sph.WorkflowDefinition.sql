USE [Sph]
GO
IF OBJECT_ID('Sph.WorkflowDefinition', 'U') IS NOT NULL
  DROP TABLE Sph.WorkflowDefinition
GO

CREATE TABLE Sph.WorkflowDefinition
(
	 [WorkflowDefinitionId] INT PRIMARY KEY IDENTITY(1,1)	
	,[Name] VARCHAR(255) NULL
	,[Note] VARCHAR(255) NULL
	,[Data] XML NOT NULL
	,[IsActive] BIT NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
