CREATE TABLE [DevV1].[Customer](
  [Id] VARCHAR(50) PRIMARY KEY NOT NULL
,[AccountNo] VARCHAR(255)  NULL
,[FirstName] VARCHAR(255) NOT NULL
,[LastName] VARCHAR(255) NOT NULL
,[Age] INT  NULL
,[Gender] VARCHAR(255)  NULL
,[IsPriority] BIT NOT NULL
,[RegisteredDate] SMALLDATETIME NOT NULL
,[Rating] INT NOT NULL
,[PrimaryContact] VARCHAR(255)  NULL
,[Department] VARCHAR(255)  NULL
,[Address.State] VARCHAR(255) NOT NULL
,[Address.Locality] VARCHAR(255) NOT NULL
,[Contact.Name] VARCHAR(255) NOT NULL
,[Json] VARCHAR(MAX)
,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[CreatedBy] VARCHAR(255) NULL
,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[ChangedBy] VARCHAR(255) NULL
)