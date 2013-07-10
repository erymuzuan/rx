USE [Sph]
IF OBJECT_ID('Sph.Maintenance', 'U') IS NOT NULL
  DROP TABLE Sph.[Maintenance]
GO

CREATE TABLE [Sph].[Maintenance]
(
	 [MaintenanceId] INT PRIMARY KEY IDENTITY(1,1)		 
	,[ComplaintId] VARCHAR (255) NULL
	,[WorkOrderNo] VARCHAR(255) NULL
	,[Status] VARCHAR (255) NOT NULL
	,[Resolution] VARCHAR(255) NOT NULL
	,[Officer] VARCHAR (255) NULL
	,[StartDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[EndDate] SMALLDATETIME NULL
	,[Data] XML NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 