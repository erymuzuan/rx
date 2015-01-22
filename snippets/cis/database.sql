﻿CREATE DATABASE [Cis]

GO
USE [Cis]

CREATE TABLE [Account]
(
	[AccountId] INT NOT NULL PRIMARY KEY IDENTITY(1,1)
	,[AccountNo] VARCHAR(255) NOT NULL
	,[FirstName] VARCHAR(255) NOT NULL
	,[LastName] VARCHAR(255) NOT NULL
	,[Dob] SMALLDATETIME NOT NULL
	,[Status] VARCHAR(50)

	
)

CREATE TABLE [Address]
(
	 [AddressId] INT NOT NULL PRIMARY KEY IDENTITY(1,1)
	,[Type] VARCHAR(50) NOT NULL
	,[AccountId] INT NOT NULL
	,[Street1] VARCHAR(255)
	,[Street2] VARCHAR(255)
	,[Postcode] VARCHAR(5)
	,[City] VARCHAR(50)
	,[State] VARCHAR(255)
	,[Country] VARCHAR(50)
)


CREATE TABLE [Invoice]
(
	 [InvoiceId] INT NOT NULL PRIMARY KEY IDENTITY(1,1)
	,[No] VARCHAR(255) NOT NULL
	,[Date] SMALLDATETIME NOT NULL
	,[AccountId] INT NOT NULL
	,[Amount] MONEY NOT NULL
)

CREATE TABLE [Payment]
(
	 [PaymentId] INT NOT NULL PRIMARY KEY IDENTITY(1,1)
	,[No] VARCHAR(255) NOT NULL
	,[Date] SMALLDATETIME NOT NULL
	,[AccountId] INT NOT NULL
	,[Amount] MONEY NOT NULL
	,[ReceiptNo] VARCHAR(255) NOT NULL
)
