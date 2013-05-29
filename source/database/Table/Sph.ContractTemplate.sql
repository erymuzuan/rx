USE [Sph]
IF OBJECT_ID('Sph.ContractTemplate', 'U') IS NOT NULL
  DROP TABLE Sph.ContractTemplate
GO

CREATE TABLE Sph.ContractTemplate
(
	 [ContractTemplateId] INT PRIMARY KEY IDENTITY(1,1)	
	,[Type] VARCHAR(255) NULL
	,[Data] XML NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
