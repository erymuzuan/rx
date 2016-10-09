CREATE TABLE [DevV1].[SurchargeAddOn](
  [Id] VARCHAR(50) PRIMARY KEY NOT NULL
,[Code] VARCHAR(255) NOT NULL
,[ServiceName] VARCHAR(255) NOT NULL
,[Name] VARCHAR(255) NOT NULL
,[SnbCode] VARCHAR(255) NOT NULL
,[Type] VARCHAR(255) NOT NULL
,[Json] VARCHAR(MAX)
,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[CreatedBy] VARCHAR(255) NULL
,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[ChangedBy] VARCHAR(255) NULL
)