USE [Sph]
IF OBJECT_ID('Sph.Building', 'U') IS NOT NULL
  DROP TABLE Sph.Building
GO

CREATE TABLE Sph.Building
(
	 [BuildingId] INT PRIMARY KEY IDENTITY(1,1)
	,[Data] XML NOT NULL
	,[LotNo] VARCHAR(255) NULL
	,[Name] VARCHAR(255) NULL
	,[Status] VARCHAR(255) NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)
GO 