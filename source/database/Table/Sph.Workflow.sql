USE [Sph]
GO
IF OBJECT_ID('Sph.Workflow', 'U') IS NOT NULL
  DROP TABLE Sph.Workflow
GO

CREATE TABLE Sph.Workflow
(
	 [WorkflowId] INT PRIMARY KEY IDENTITY(1,1)	
	,[WorkflowDefinitionId] VARCHAR(255) NULL
	,[Name] VARCHAR(255) NULL
	,[Version] INT NOT NULL
	,[State] VARCHAR(255) NULL
	,[IsActive] BIT NOT NULL
	,[Json] VARCHAR(MAX)
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
