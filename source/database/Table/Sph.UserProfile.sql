
IF OBJECT_ID('Sph.UserProfile', 'U') IS NOT NULL
  DROP TABLE [Sph].[UserProfile]

CREATE TABLE [Sph].[UserProfile](
	
	[Id] VARCHAR(255) PRIMARY KEY,
	[UserName] VARCHAR(50) NOT NULL,
	[FullName] VARCHAR(100) NULL,
	[Designation] VARCHAR(50) NULL,
	[Department] VARCHAR(50) NULL,
	[Email] VARCHAR(50) NULL,
	[Json] VARCHAR(MAX) NOT NULL,
	[ChangedDate] SMALLDATETIME NOT NULL,
	[ChangedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] SMALLDATETIME NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
)

