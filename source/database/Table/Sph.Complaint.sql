USE [Sph]
IF OBJECT_ID('Sph.Complaint', 'U') IS NOT NULL
  DROP TABLE Sph.[Complaint]
GO

CREATE TABLE [Sph].[Complaint]
(
	 [ComplaintId] INT PRIMARY KEY IDENTITY(1,1)	
	,[ReferenceNo] VARCHAR(255) NULL
	,[Type] VARCHAR (255) NULL
	,[Category] VARCHAR(255) NULL
	,[SubCategory] VARCHAR (255) NULL
	,[CommercialSpace] VARCHAR (255) NULL
	,[Status] VARCHAR(255) NULL
	,[TenantId] INT NULL
	,[Data] XML NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 