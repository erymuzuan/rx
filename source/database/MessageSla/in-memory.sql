USE [master]
GO
ALTER DATABASE [<database_name, sysname, dbo>] ADD FILEGROUP [MessageSla] CONTAINS MEMORY_OPTIMIZED_DATA 
GO
USE [master]
GO
ALTER DATABASE [<database_name, sysname, dbo>] ADD FILE ( NAME = N'<database_name, sysname, dbo>_MessageSla', FILENAME = N'<file-group-path, ,>' ) TO FILEGROUP [MessageSla]
GO
ALTER DATABASE [<database_name, sysname, dbo>] SET MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT=ON  

USE [<database_name, sysname, dbo>]	
GO
CREATE TABLE [<schema_name, sysname, dbo>].[MessageEvent]
(
  [Id] uniqueidentifier NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT=1000000) DEFAULT (NEWID()), 
  [DateTime] SMALLDATETIME NOT NULL,
  [Entity]  NVARCHAR(255) NOT NULL,
  [Event] NVARCHAR(255) NOT NULL,
  [ItemId] NVARCHAR(255) NOT NULL,
  [ProcessName] NVARCHAR(255) NOT NULL,
  [ProcessingTimeSpan] NVARCHAR(255) NULL,
  [ProcessingTimeSpanInMiliseconds] FLOAT NULL,
  [MachineName] NVARCHAR(255) NULL,
  [Worker] NVARCHAR(255) NULL,
  [RoutingKey] NVARCHAR(255) NULL,
  [MessageId] NVARCHAR(255) NOT NULL
)
WITH (MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_AND_DATA)
GO

CREATE PROCEDURE [<schema_name, sysname, dbo>].[InsertMessageTrackingEvent]
  @DateTime SMALLDATETIME NOT NULL,
  @Entity  NVARCHAR(255) NOT NULL,
  @Event NVARCHAR(255) NOT NULL,
  @ItemId NVARCHAR(255) NOT NULL,
  @ProcessName NVARCHAR(255) NOT NULL,
  @ProcessingTimeSpan NVARCHAR(255) NULL,
  @ProcessingTimeSpanInMiliseconds FLOAT NULL,
  @MachineName NVARCHAR(255) NULL,
  @Worker NVARCHAR(255) NULL,
  @RoutingKey NVARCHAR(255) NULL,
  @MessageId NVARCHAR(255) NOT NULL

with native_compilation, schemabinding/*, execute as owner*/
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	INSERT INTO [<schema_name, sysname, dbo>].[MessageEvent]
	([DateTime], [Entity], [Event], [ItemId], [ProcessName], [ProcessingTimeSpan], [ProcessingTimeSpanInMiliseconds],
	[MachineName], [Worker], [RoutingKey], [MessageId]) 
	VALUES(@DateTime, @Entity, @Event, @ItemId, @ProcessName, @ProcessingTimeSpan, @ProcessingTimeSpanInMiliseconds,
	@MachineName, @Worker, @RoutingKey, @MessageId)

END


GO
CREATE TABLE [<schema_name, sysname, dbo>].[CancelledMessage]
(
  [Id] uniqueidentifier NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT=1000000) DEFAULT (NEWID()),
  [Worker] NVARCHAR(255) NULL,
  [MessageId] NVARCHAR(255) NOT NULL
)
WITH (MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_AND_DATA)
GO


CREATE PROCEDURE [<schema_name, sysname, dbo>].[CheckCancelledMessage]
  @Worker NVARCHAR(255) NULL,
  @MessageId NVARCHAR(255) NOT NULL

with native_compilation, schemabinding/*, execute as owner*/
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	SELECT COUNT(*) FROM [<schema_name, sysname, dbo>].[CancelledMessage] 
	WHERE [Worker] = @Worker AND [MessageId]= @MessageId

END

GO
CREATE PROCEDURE [<schema_name, sysname, dbo>].[RemoveCancelledMessage]
  @Worker NVARCHAR(255) NULL,
  @MessageId NVARCHAR(255) NOT NULL

with native_compilation, schemabinding/*, execute as owner*/
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	DELETE FROM [<schema_name, sysname, dbo>].[CancelledMessage] 
	WHERE [Worker] = @Worker AND [MessageId]= @MessageId

END


GO
CREATE PROCEDURE [<schema_name, sysname, dbo>].[PutCancelledMessage]
  @Worker NVARCHAR(255) NULL,
  @MessageId NVARCHAR(255) NOT NULL

with native_compilation, schemabinding/*, execute as owner*/
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	INSERT INTO [<schema_name, sysname, dbo>].[CancelledMessage] ( [Worker] ,[MessageId])
	VALUES(@Worker , @MessageId)

END
GO
/*GetMessageProcessingStatus*/

CREATE PROCEDURE [<schema_name, sysname, dbo>].[GetMessageProcessingStatus]
  @Worker NVARCHAR(255) NULL,
  @MessageId NVARCHAR(255) NOT NULL

with native_compilation, schemabinding/*, execute as owner*/
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	
	SELECT [MessageId], [Event] FROM [<schema_name, sysname, dbo>].[MessageEvent] 
	WHERE [Worker] = @Worker AND [MessageId]= @MessageId

END
GO