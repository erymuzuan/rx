CREATE DATABASE [His]

GO
USE [His]

GO

CREATE TABLE [Patient]
(
	 [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1)
	,[Name] VARCHAR(255) NOT NULL
	,[Mrn] VARCHAR(255) NOT NULL
	,[Gender] VARCHAR(255) NOT NULL
	,[Income] DECIMAL(18,0) NOT NULL
	,[Dob] SMALLDATETIME NOT NULL
	,[Age] SMALLINT NULL
	,[Nrid] BIGINT NULL
)
GO

CREATE TABLE [Department]
(
	 [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1)
	,[Name] VARCHAR(255) NOT NULL
)

GO

CREATE TABLE [PatientDepartment]
(
	 [PatientId] INT NOT NULL 
	,[DepartmentId] INT NOT NULL
)

GO
CREATE TABLE [dbo].[TransactionQueue] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [RowId]     INT           NULL,
    [Timestamp] DATETIME      DEFAULT (getdate()) NOT NULL,
    [Table]     NVARCHAR (50) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE TRIGGER [Trigger]
	ON [dbo].[Patient]
	FOR INSERT, UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @id INT
		SELECT @id = [Id] FROM inserted
		INSERT INTO dbo.TransactionQueue(RowId, [Table]) VALUES( @id, 'Patient')

	END




