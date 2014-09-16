
IF OBJECT_ID('Sph.Watcher', 'U') IS NOT NULL
  DROP TABLE Sph.Watcher
GO

CREATE TABLE Sph.Watcher
(
	 [Id] VARCHAR(50) PRIMARY KEY NOT NULL
	,[Json] VARCHAR(MAX) NOT NULL
	,[DateTime] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[User] VARCHAR(255) NULL
	,[EntityName] VARCHAR(255) NOT NULL
	,[EntityId] VARCHAR(50) NOT NULL
	,[IsActive] BIT NOT NULL DEFAULT 0
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL
)
GO 
