 USE [Sph]
 GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sph].[Inventory]') AND type in (N'U'))
DROP TABLE [Sph].[Inventory]
GO



 CREATE TABLE Sph.Inventory
(
	 [InventoryId] INT PRIMARY KEY IDENTITY(1,1)
	,[Name] VARCHAR(255) NOT NULL
	,[Category] VARCHAR(255) NULL
	,[Brand] VARCHAR(255) NULL
	,[Data] XML NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)
