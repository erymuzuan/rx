CREATE DATABASE [rx_test_database]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'rx_test_database', FILENAME = N'E:\data\rx_test_database.mdf' , SIZE = 8192KB , FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'rx_test_database_log', FILENAME = N'E:\data\rx_test_database_log.ldf' , SIZE = 8192KB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [rx_test_database] SET COMPATIBILITY_LEVEL = 130
GO
ALTER DATABASE [rx_test_database] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [rx_test_database] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [rx_test_database] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [rx_test_database] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [rx_test_database] SET ARITHABORT OFF 
GO
ALTER DATABASE [rx_test_database] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [rx_test_database] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [rx_test_database] SET AUTO_CREATE_STATISTICS ON(INCREMENTAL = OFF)
GO
ALTER DATABASE [rx_test_database] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [rx_test_database] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [rx_test_database] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [rx_test_database] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [rx_test_database] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [rx_test_database] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [rx_test_database] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [rx_test_database] SET  DISABLE_BROKER 
GO
ALTER DATABASE [rx_test_database] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [rx_test_database] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [rx_test_database] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [rx_test_database] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [rx_test_database] SET  READ_WRITE 
GO
ALTER DATABASE [rx_test_database] SET RECOVERY FULL 
GO
ALTER DATABASE [rx_test_database] SET  MULTI_USER 
GO
ALTER DATABASE [rx_test_database] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [rx_test_database] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [rx_test_database] SET DELAYED_DURABILITY = DISABLED 
GO
USE [rx_test_database]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = Off;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = Primary;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = On;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = Primary;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = Off;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = Primary;
GO
USE [rx_test_database]
GO
IF NOT EXISTS (SELECT name FROM sys.filegroups WHERE is_default=1 AND name = N'PRIMARY') ALTER DATABASE [rx_test_database] MODIFY FILEGROUP [PRIMARY] DEFAULT
GO


CREATE SCHEMA [DevV1] AUTHORIZATION [dbo]

GO

DROP TABLE IF EXISTS [DevV1].[Patient]
GO
CREATE TABLE [DevV1].[Patient](
	[Id] [varchar](50) NOT NULL,
	[Mrn] AS CAST(JSON_VALUE([Json], 'strict $.Mrn') AS NVARCHAR(50)),
	[Dob] AS CAST(JSON_VALUE([Json], '$.Dob') AS DateTime2),
	[Age] AS CAST(JSON_VALUE([Json], '$.Age') AS int),
	[FullName] AS CAST(JSON_VALUE([Json], '$.FullName') AS NVARCHAR(50)),
	[Gender] AS CAST(JSON_VALUE([Json], '$.Gender') AS NVARCHAR(50)),
	[Religion] AS CAST(JSON_VALUE([Json], '$.Religion') AS NVARCHAR(50)),
	[Race] AS CAST(JSON_VALUE([Json], '$.Race') AS NVARCHAR(50)),
	[Status] AS CAST(JSON_VALUE([Json], '$.Status') AS NVARCHAR(50)),
	[MaritalStatus] AS CAST(JSON_VALUE([Json], '$.MaritalStatus') AS NVARCHAR(50)),
	[HomeAddress.State] AS CAST(JSON_VALUE([Json], '$.HomeAddress.State') AS VARCHAR(50)),
	[Json] [varchar](max)   CONSTRAINT Json
                CHECK (ISJSON(Json)=1),
	[CreatedDate] [DateTime2] NOT NULL,
	[CreatedBy] [varchar](255) NULL,
	[ChangedDate] [DateTime2] NOT NULL,
	[ChangedBy] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [DevV1].[Patient] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [DevV1].[Patient] ADD  DEFAULT (getdate()) FOR [ChangedDate]
GO

ALTER TABLE [DevV1].[Patient]
ADD [Dob] AS CAST(JSON_VALUE([Json], '$.Dob') AS DateTime2)

GO

ALTER TABLE [DevV1].[Patient]
ADD [Age] AS CAST(JSON_VALUE([Json], '$.Age') AS int)


GO

ALTER TABLE [DevV1].[Patient]
ADD [HomeAddress.State] AS CAST(JSON_VALUE([Json], '$.HomeAddress.State') AS NVARCHAR(50))


GO

ALTER TABLE [DevV1].[Patient]
ADD [Wife.Name] AS CAST(JSON_VALUE([Json], '$.Wife.Name') AS NVARCHAR(50))


GO

ALTER TABLE [DevV1].[Patient]
ADD [NextOfKin.FullName] AS CAST(JSON_VALUE([Json], '$.NextOfKin.FullName') AS NVARCHAR(50))
GO

ALTER TABLE [DevV1].[Patient]
ADD [IsMalaysian] AS ( CASE (CAST(JSON_VALUE([Json], '$.Race') AS NVARCHAR(50))) WHEN 'Others' THEN 0 ELSE 1 END)