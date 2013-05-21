USE [Sph]
IF OBJECT_ID('Sph.BinaryStore', 'U') IS NOT NULL
  DROP TABLE Sph.BinaryStore
GO

CREATE TABLE Sph.BinaryStore
(
	 [BinaryStoreId] INT PRIMARY KEY IDENTITY(1,1)
	,[StoreId] VARCHAR(50) NOT NULL
	,[Extension] VARCHAR(10) NULL
	,[Title] VARCHAR(255) NULL
	,[Tag] VARCHAR(255) NULL
	,[Content] VARBINARY(MAX)
)
GO 
