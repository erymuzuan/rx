 IF OBJECT_ID('Sph.Role', 'U') IS NOT NULL
  DROP TABLE Sph.Role
GO

CREATE TABLE [Sph].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,
	[Json] VARCHAR(MAX) NOT NULL,
	[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(255) NULL,
	[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[ChangedBy] VARCHAR(255) NULL
	)
GO


ALTER TABLE  [Sph].[Role]
ADD [Json] VARCHAR(MAX) NULL

