IF OBJECT_ID('Sph.DocumentTemplate', 'U') IS NOT NULL
  DROP TABLE Sph.DocumentTemplate
GO

CREATE TABLE Sph.DocumentTemplate
(
	 [DocumentTemplateId] INT PRIMARY KEY IDENTITY(1,1)
	,[Entity] VARCHAR(255) NOT NULL
	,[Json] VARCHAR(MAX) NOT NULL
	,[Name] VARCHAR(255) NOT NULL
	,[IsPublished] BIT NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 

ALTER TABLE Sph.DocumentTemplate
ADD [Json] VARCHAR(MAX) NULL