CREATE TABLE [sph].[CancelledMessage]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MessageId] VARCHAR(50) NULL, 
    [Worker] VARCHAR(500) NULL
)
