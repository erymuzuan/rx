USE [Sph]
IF OBJECT_ID('Sph.ApplicationTemplate', 'U') IS NOT NULL
  DROP TABLE Sph.ApplicationTemplate
GO

CREATE TABLE Sph.ApplicationTemplate
(
	 [ApplicationTemplateId] INT PRIMARY KEY IDENTITY(1,1)	
	,[Name] VARCHAR(255) NULL
	,[Data] XML NOT NULL
	,[IsActive] BIT NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
