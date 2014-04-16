IF OBJECT_ID('Sph.EntityChart', 'U') IS NOT NULL
  DROP TABLE Sph.EntityChart
GO

IF OBJECT_ID('Sph.EntityChart', 'U') IS NOT NULL
  DROP TABLE [Sph].[EntityChart]

CREATE TABLE Sph.EntityChart
(
	 [EntityChartId] INT PRIMARY KEY IDENTITY(1,1)
	,[EntityDefinitionId] INT NOT NULL
	,[Entity] VARCHAR(255) NOT NULL
	,[IsDashboardItem] BIT NOT NULL DEFAULT 0
	,[Data] XML NOT NULL
	,[EntityViewId] INT NOT NULL
	,[Name] VARCHAR(255) NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 

