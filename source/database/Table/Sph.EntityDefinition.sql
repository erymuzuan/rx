USE [Sph]
IF OBJECT_ID('Sph.EntityDefinition', 'U') IS NOT NULL
  DROP TABLE Sph.EntityDefinition
GO

CREATE TABLE Sph.EntityDefinition
(
	 [EntityDefinitionId] INT PRIMARY KEY IDENTITY(1,1)
	,[Data] XML NOT NULL
	,[Name] VARCHAR(255) NOT NULL
	,[Plural] VARCHAR(255) NOT NULL
	,[IsPublished] BIT NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
