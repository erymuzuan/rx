 USE [Sph]
 GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sph].[Space]') AND type in (N'U'))
DROP TABLE [Sph].[Space]
GO



 CREATE TABLE Sph.Space
(
	 [SpaceId] INT PRIMARY KEY IDENTITY(1,1)
	,[TemplateId] INT NOT NULL
	,[TemplateName] VARCHAR(255) NOT NULL
	,[BuildingId] INT NULL
	,[BuildingName] VARCHAR(255) NULL
	,[FloorName] VARCHAR(255) NULL
	,[RegistrationNo] VARCHAR(255) NULL
	,[LotName] VARCHAR(255) NULL
	,[Status] VARCHAR(255)  NULL
	,[Category] VARCHAR(255) NULL
	,[IsOnline] BIT NOT NULL DEFAULT 0
	,[RentalType] VARCHAR(255) NULL
	,[Data] XML NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)

ALTER TABLE [Sph].[Space]
ADD [State] VARCHAR(255) NOT NULL DEFAULT ''

ALTER TABLE [Sph].[Space]
ADD [City] VARCHAR(255) NOT NULL DEFAULT ''

ALTER TABLE [Sph].[Space]
ADD [IsAvailable] BIT NOT NULL DEFAULT 1

ALTER TABLE [Sph].[Space]
ADD [RentalRate] MONEY NOT NULL DEFAULT 0


GO
ALTER TABLE [Sph].[Space]
ADD [Path] GEOGRAPHY NULL

GO
ALTER TABLE [Sph].[Space]
ADD [Wkt] VARCHAR(MAX) NULL

GO
ALTER TABLE [Sph].[Space]
ADD [EncodedWkt] VARCHAR(MAX) NULL