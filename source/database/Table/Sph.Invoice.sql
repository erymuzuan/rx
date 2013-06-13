USE [Sph]
GO
 IF OBJECT_ID('Sph.Invoice', 'U') IS NOT NULL
  DROP TABLE Sph.Invoice
GO

CREATE TABLE [Sph].[Invoice](
	[InvoiceId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[TenantIdSsmNo] VARCHAR(50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[ContractNo] VARCHAR(255) NOT NULL,
	[No] [nvarchar](50) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Data] XML NOT NULL	,
	[Date] SMALLDATETIME NULL,
	[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(255) NULL,
	[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[ChangedBy] VARCHAR(255) NULL
	)
GO



