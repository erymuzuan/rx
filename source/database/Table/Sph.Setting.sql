USE [Sph]
IF OBJECT_ID('Sph.Setting', 'U') IS NOT NULL
  DROP TABLE Sph.Setting
GO

CREATE TABLE Sph.Setting
(
	 [SettingId] INT PRIMARY KEY IDENTITY(1,1)
	,[Data] XML NOT NULL
	,[UserName] VARCHAR(255) NULL
	,[Key] VARCHAR(255) NOT NULL
	,[Value] VARCHAR(MAX) NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
