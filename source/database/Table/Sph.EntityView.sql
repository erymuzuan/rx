IF OBJECT_ID('Sph.EntityView', 'U') IS NOT NULL
  DROP TABLE Sph.EntityView
GO

CREATE TABLE [Sph].[EntityView]
(
	[Id] VARCHAR(255) PRIMARY KEY NOT NULL
	,[EntityDefinitionId]  VARCHAR(255) NOT NULL
	,[Json] VARCHAR(MAX) NOT NULL
	,[Name] VARCHAR(255) NOT NULL
	,[IsPublished] BIT NOT NULL
	,[Route] VARCHAR(255) NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
