USE [Sph]
GO
 IF OBJECT_ID('Sph.Rent', 'U') IS NOT NULL
  DROP TABLE Sph.Rent
GO

CREATE TABLE [Sph].[Rent](
	[RentId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[TenantId] [int] NOT NULL,
	[ContractId] [int] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[ContractNo] VARCHAR(255) NOT NULL,
	[InvoiceNo] [nvarchar](50) NOT NULL,
	[Year] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[Half] VARCHAR(10) NULL,
	[Quarter] VARCHAR(10) NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[IsPaid] [bit] NOT NULL,
	[PaymentDateTime] [datetime] NULL,
	[Data] XML NOT NULL	,
	[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(255) NULL,
	[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[ChangedBy] VARCHAR(255) NULL
	)
GO



