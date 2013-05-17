CREATE TABLE Sph.RentalApplication
(
	 [RentalApplicationId] INT PRIMARY KEY IDENTITY(1,1)	
	,[CompanyName] VARCHAR(10) NULL
	,[CompanyRegistrationNo] VARCHAR(255) NULL	
	,[CommercialSpaceId] VARCHAR(255) NULL
	,[Data] XML NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)