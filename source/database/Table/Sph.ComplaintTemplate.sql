USE [Sph]
IF OBJECT_ID('Sph.ComplaintTemplate', 'U') IS NOT NULL
  DROP TABLE Sph.ComplaintTemplate
GO

CREATE TABLE Sph.ComplaintTemplate
(
	 [ComplaintTemplateId] INT PRIMARY KEY IDENTITY(1,1)	
	,[Name] VARCHAR(255) NULL
	,[Data] XML NOT NULL
	,[IsActive] BIT NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 

 ALTER TABLE [Sph].[Sph].[ComplaintTemplate]
 ADD [Category] varchar(255) NULL
