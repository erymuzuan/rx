USE [Sph]
GO
/*
DROP TABLE [Sph].[RentalApplication]
GO
*/
CREATE TABLE [Sph].[RentalApplication]
(
	 [RentalApplicationId] INT PRIMARY KEY IDENTITY(1,1)
	,[RegistrationNo] VARCHAR(255) NULL	
	,[ApplicationDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[Status] VARCHAR(255) NOT NULL 	
	,[CompanyName] VARCHAR(255) NULL
	,[CompanyRegistrationNo] VARCHAR(255) NULL	
	,[ContactName] VARCHAR(255) NULL	
	,[ContactIcNo] VARCHAR(50) NULL	
	,[CommercialSpaceId] INT  NULL
	,[Data] XML NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)
