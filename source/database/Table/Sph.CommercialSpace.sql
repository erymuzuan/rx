 USE [Sph]
 GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sph].[CommercialSpace]') AND type in (N'U'))
DROP TABLE [Sph].[CommercialSpace]
GO



 CREATE TABLE Sph.CommercialSpace
(
	 [CommercialSpaceId] INT PRIMARY KEY IDENTITY(1,1)
	,[BuildingId] INT NOT NULL
	,[FloorName] VARCHAR(255) NULL
	,[RegistrationNo] VARCHAR(255) NULL
	,[LotName] VARCHAR(255) NULL
	,[BuildingName] VARCHAR(255) NOT NULL
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

ALTER TABLE [Sph].[CommercialSpace]
ADD [State] VARCHAR(255) NOT NULL DEFAULT ''

ALTER TABLE [Sph].[CommercialSpace]
ADD [City] VARCHAR(255) NOT NULL DEFAULT ''

ALTER TABLE [Sph].[CommercialSpace]
ADD [IsAvailable] BIT NOT NULL DEFAULT 1

GO
SELECT * FROM [Sph].[CommercialSpace]