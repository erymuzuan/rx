
USE [His]

GO
-- DROP TABLE [Patient]
CREATE TABLE [Patient]
(
	 [Mrn] VARCHAR(255) NOT NULL PRIMARY KEY
	,[Name] VARCHAR(255) NOT NULL
	,[Gender] CHAR(1) NOT NULL
	,[Income] DECIMAL(18,0) NOT NULL
	,[Dob] SMALLDATETIME NOT NULL
	,[Nationality] VARCHAR(255) NULL
	,[Race] VARCHAR(50) NULL
	,[Religion] VARCHAR(50) NULL
	,[Age] TINYINT NULL
	,[Nrid] BIGINT NULL
	,[PassportNo] NVARCHAR(50) NULL
	,[BirthCert] NVARCHAR(50) NULL
	,[IdCardCopy] VARBINARY(MAX) NULL
	,[IdCardMimeType] VARCHAR(255) NULL
	,[Fee] MONEY NULL
	,[Weight] DECIMAL NULL
	,[Height] REAL NOT NULL
	,[AdditionalInfo] XML NULL
	,[Address] XML NOT NULL
	,[IsCivilServant] BIT NULL
	,[IsChildren] BIT NOT NULL
	,[RegisteredDate] DATETIME2 NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[ModifiedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Version] ROWVERSION NOT NULL
)
GO

CREATE TABLE [Department]
(
	 [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1)
	,[Name] VARCHAR(255) NOT NULL
	,[ModifiedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Version] ROWVERSION NOT NULL
)

GO

CREATE TABLE [PatientDepartment]
(
	 [PatientId] INT NOT NULL 
	,[DepartmentId] INT NOT NULL
	,[ModifiedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Version] ROWVERSION NOT NULL
)


GO
CREATE TABLE [dbo].[Ward] (
     [Id]       UNIQUEIDENTIFIER NOT NULL
	,[Name]     NVARCHAR (50)    NULL
	,[Block]    NVARCHAR (50)    NULL
	,[Capacity] SMALLINT         NOT NULL
	,[ModifiedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Version] ROWVERSION NOT NULL
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[Doctor]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NULL, 
    [Designation] NVARCHAR(50) NULL, 
    [Age] SMALLINT NULL
	,[Department] INT NOT NULL	
	,[ModifiedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Version] ROWVERSION NOT NULL
    CONSTRAINT [FK_Doctor_ToDepartment] FOREIGN KEY ([Department]) REFERENCES [Department]([Id])
)


GO
CREATE TABLE [dbo].[TransactionQueue] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [RowId]     sql_variant           NOT NULL,
    [Timestamp] DATETIME      DEFAULT (getdate()) NOT NULL,
    [Table]     NVARCHAR (50) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE TRIGGER [LogChangeTrigger]
	ON [dbo].[Patient]
	FOR INSERT, UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @id SQL_VARIANT
		SELECT @id = [Mrn] FROM inserted
		INSERT INTO dbo.TransactionQueue(RowId, [Table]) VALUES( @id, 'Patient')

	END

GO
CREATE PROCEDURE [dbo].[GetPatientsByGender]
	@Gender varchar(255)
AS
	SELECT Mrn, Name,Age, Income, Nrid, PassportNo, Nationality, Fee, IsChildren, IsCivilServant, AdditionalInfo, Address FROM [dbo].[Patient]
	WHERE [Gender] = @Gender
RETURN 0




