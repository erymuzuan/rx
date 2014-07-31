
IF OBJECT_ID('Sph.TransformDefinition', 'U') IS NOT NULL
  DROP TABLE Sph.TransformDefinition
GO

CREATE TABLE Sph.TransformDefinition
(
	 [TransformDefinitionId] INT PRIMARY KEY IDENTITY(1,1)
	,[Json] NVARCHAR(MAX) NOT NULL
	,[Name] VARCHAR(255) NULL
	,[InputTypeName] VARCHAR(255) NOT NULL
	,[OutputTypeName] VARCHAR(255) NOT NULL
	,[Description] VARCHAR(255) NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
