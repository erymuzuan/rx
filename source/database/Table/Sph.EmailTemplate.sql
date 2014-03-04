IF OBJECT_ID('Sph.EmailTemplate', 'U') IS NOT NULL
  DROP TABLE Sph.EmailTemplate
GO

CREATE TABLE Sph.EmailTemplate
(
	 [EmailTemplateId] INT PRIMARY KEY IDENTITY(1,1)
	,[Entity] VARCHAR(255) NOT NULL
	,[Data] XML NOT NULL
	,[Name] VARCHAR(255) NOT NULL
	,[IsPublished] BIT NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
