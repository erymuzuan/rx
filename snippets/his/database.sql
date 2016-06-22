
USE [His]

GO
/*
DROP TABLE [Religion.Code]
GO
DROP TABLE [GenderLookup]
GO
DROP TABLE [Country]
GO
DROP TABLE [Patient]

*/
-- 
CREATE TABLE [dbo].[Patient]
(
	 [Mrn] VARCHAR(255) NOT NULL
	,[FirstName] VARCHAR(50) NOT NULL
	,[LastName] VARCHAR(50) NOT NULL
	,[FullName] as [FirstName] + ' ' + [LastName] PERSISTED
	,[Gender] CHAR(1) NOT NULL
	,[Income] MONEY NOT NULL
	,[Dob] DATE NOT NULL
	,[Nationality.Code] TINYINT NOT NULL
	,[RaceCode] TINYINT NOT NULL
	,[ReligionCode] TINYINT NULL
	,[Age] TINYINT NULL
	,[Nrid] BIGINT NULL
	,[PassportNo] NVARCHAR(50) NULL
	,[BirthCert] NVARCHAR(50) NULL
	,[IdCardCopy] VARBINARY(MAX) NULL
	,[IdCardMimeType] VARCHAR(255) NULL
	,[Fee] SMALLMONEY NULL
	,[Weight] DECIMAL NULL
	,[Height] REAL NOT NULL
	,[AdditionalInfo] XML NULL
	,[Address] XML NOT NULL
	,[IsCivilServant] BIT NULL
	,[IsChildren] BIT NOT NULL
	,[RegisteredDate] DATETIME2 NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[ModifiedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Version] ROWVERSION NOT NULL
	,CONSTRAINT PK_Patient PRIMARY KEY CLUSTERED ([Mrn] ASC)
)

GO 
CREATE TABLE [dbo].[Religion.Code]
(
	[ReligionId] TINYINT NOT NULL PRIMARY KEY IDENTITY
	,[Religion] VARCHAR(50) NOT NULL
)
GO
INSERT INTO [Religion.Code] VALUES('Islam')
GO
INSERT INTO [Religion.Code] VALUES('Christianity')
GO
INSERT INTO [Religion.Code] VALUES('Hinduism')
GO
INSERT INTO [Religion.Code] VALUES('Bhudism')
GO
INSERT INTO [Religion.Code] VALUES('Aetheism')
GO
INSERT INTO [Religion.Code] VALUES('Judaism')
GO
INSERT INTO [Religion.Code] VALUES('Sihksm')
GO

ALTER TABLE [dbo].[Patient] WITH CHECK
ADD CONSTRAINT FK_Patient_ReligionCode
FOREIGN KEY ([ReligionCode])
REFERENCES [dbo].[Religion.Code]([ReligionId])

GO


CREATE TABLE [Country](
[Id] TINYINT NOT NULL PRIMARY KEY IDENTITY(1,1),
[Name] VARCHAR(255) NOT NULL,
[Code] CHAR(3) NOT NULL
)
GO
INSERT INTO [Country]([Name], [Code]) VALUES('Malaysia', 'MAS')
GO
INSERT INTO [Country]([Name], [Code]) VALUES('Indonesia', 'IDN')
GO

INSERT INTO [Country]([Name], [Code]) VALUES('Thailand', 'THA')
GO

INSERT INTO [Country]([Name], [Code]) VALUES('Singapore', 'SNG')
GO

INSERT INTO [Country]([Name], [Code]) VALUES('Vietnam', 'VTN')
GO



ALTER TABLE [dbo].[Patient] WITH CHECK
ADD CONSTRAINT FK_Patient_Country
FOREIGN KEY ([Country])
REFERENCES [dbo].[Country]([Id])
GO

CREATE TABLE [GenderLookup]
(
	[Key] CHAR(1) PRIMARY KEY NOT NULL,
	[Value] VARCHAR(255) NOT NULL
)
GO

ALTER TABLE [dbo].[Patient] WITH CHECK
ADD CONSTRAINT FK_Patient_Gender
FOREIGN KEY ([Gender])
REFERENCES [dbo].[GenderLookup]([Key])

GO 
INSERT INTO [GenderLookup]([Key], [Value]) VALUES('M', 'Male')

GO 
INSERT INTO [GenderLookup]([Key], [Value]) VALUES('F', 'Female')
GO 
INSERT INTO [GenderLookup]([Key], [Value]) VALUES('I', 'Inderteminate')

CREATE TABLE [dbo].[Department]
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
	SELECT Mrn, [FullName],Age, Income, Nrid, PassportNo, Fee, IsChildren, IsCivilServant, AdditionalInfo, Address FROM [dbo].[Patient]
	WHERE [Gender] = @Gender
RETURN 0



GO
-- =============================================
CREATE FUNCTION PatientsBornInYear 
(	
	@Year int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Mrn],[FullName], [Nrid] FROM [dbo].[Patient]
	WHERE YEAR([Dob]) = @Year
)
GO

CREATE FUNCTION GetFullName 
(
	-- Add the parameters for the function here
	@FirstName varchar(255),
	@LastName VARCHAR(50)
)
RETURNS varchar(255)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @FullName varchar(255)

	-- Add the T-SQL statements to compute the return value here
	SELECT @FullName = @FirstName + ' ' + @LastName

	-- Return the result of the function
	RETURN @FullName

END
GO