USE [master]
GO
ALTER DATABASE [DevV1] ADD FILEGROUP [MessageSla] CONTAINS MEMORY_OPTIMIZED_DATA 
GO
USE [master]
GO
ALTER DATABASE [DevV1] ADD FILE ( NAME = N'DevV1_MessageSla', FILENAME = N'E:\data\data\DevV1_MessageSla' ) TO FILEGROUP [MessageSla]
GO
ALTER DATABASE [DevV1] SET MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT=ON  

USE [DevV1]	
GO
CREATE TABLE [Sph].[MessageEvent]
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

CREATE PROCEDURE [Sph].[InsertMessageTrackingEvent]
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
	INSERT INTO [Sph].[MessageEvent]
	([DateTime], [Entity], [Event], [ItemId], [ProcessName], [ProcessingTimeSpan], [ProcessingTimeSpanInMiliseconds],
	[MachineName], [Worker], [RoutingKey], [MessageId]) 
	VALUES(@DateTime, @Entity, @Event, @ItemId, @ProcessName, @ProcessingTimeSpan, @ProcessingTimeSpanInMiliseconds,
	@MachineName, @Worker, @RoutingKey, @MessageId)

END


GO
CREATE TABLE [Sph].[CancelledMessage]
(
  [Id] uniqueidentifier NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT=1000000) DEFAULT (NEWID()),
  [Worker] NVARCHAR(255) NULL,
  [MessageId] NVARCHAR(255) NOT NULL
)
WITH (MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_AND_DATA)
GO


CREATE PROCEDURE [Sph].[CheckCancelledMessage]
  @Worker NVARCHAR(255) NULL,
  @MessageId NVARCHAR(255) NOT NULL

with native_compilation, schemabinding/*, execute as owner*/
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	SELECT COUNT(*) FROM [Sph].[CancelledMessage] 
	WHERE [Worker] = @Worker AND [MessageId]= @MessageId

END

GO
CREATE PROCEDURE [Sph].[RemoveCancelledMessage]
  @Worker NVARCHAR(255) NULL,
  @MessageId NVARCHAR(255) NOT NULL

with native_compilation, schemabinding/*, execute as owner*/
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	DELETE FROM [Sph].[CancelledMessage] 
	WHERE [Worker] = @Worker AND [MessageId]= @MessageId

END


GO
CREATE PROCEDURE [Sph].[PutCancelledMessage]
  @Worker NVARCHAR(255) NULL,
  @MessageId NVARCHAR(255) NOT NULL

with native_compilation, schemabinding/*, execute as owner*/
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	INSERT INTO [Sph].[CancelledMessage] ( [Worker] ,[MessageId])
	VALUES(@Worker , @MessageId)

END
GO