USE [Sph]
GO
 IF OBJECT_ID('Sph.Deposit', 'U') IS NOT NULL
  DROP TABLE Sph.Deposit
GO

CREATE TABLE [Sph].[Deposit](
	[DepositId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DateTime] [datetime] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[IDNumber] [nvarchar](50) NOT NULL,
	[RegistrationNo] [nvarchar](50) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[IsPaid] [bit] NOT NULL,
	[IsRefund] [bit] NOT NULL,
	[ReceiptNo] [nvarchar](50) NULL,
	[RefundedBy] [nvarchar](255) NULL,
	[IsVoid] [bit] NOT NULL,
	[PaymentDateTime] [datetime] NULL,
	[RefundDateTime] [datetime] NULL,
	[Data] XML NOT NULL	,
	[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(255) NULL,
	[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[ChangedBy] VARCHAR(255) NULL
	)
GO


