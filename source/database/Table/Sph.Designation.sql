 IF OBJECT_ID('Sph.Designation', 'U') IS NOT NULL
  DROP TABLE Sph.Designation
GO

CREATE TABLE [Sph].[Designation](
	[DesignationId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,
	[Data] XML NOT NULL	,
	[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(255) NULL,
	[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[ChangedBy] VARCHAR(255) NULL
	)
GO



