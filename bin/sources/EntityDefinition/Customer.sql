
CREATE TABLE [DevV1].[Customer]
(
    [Id] VARCHAR(50) PRIMARY KEY NOT NULL
    ,[AccountNo] AS CAST(JSON_VALUE([Json], '$.AccountNo') AS VARCHAR(255))
    ,[FirstName] AS CAST(JSON_VALUE([Json], '$.FirstName') AS VARCHAR(255))
    ,[LastName] AS CAST(JSON_VALUE([Json], '$.LastName') AS VARCHAR(255))
    ,[Age] AS CAST(JSON_VALUE([Json], '$.Age') AS INT)
    ,[Gender] AS CAST(JSON_VALUE([Json], '$.Gender') AS VARCHAR(255))
    ,[IsPriority] AS CAST(JSON_VALUE([Json], '$.IsPriority') AS BIT)
    ,[RegisteredDate] DATETIME2 NOT NULL
    ,[Rating] AS CAST(JSON_VALUE([Json], '$.Rating') AS INT)
    ,[PrimaryContact] AS CAST(JSON_VALUE([Json], '$.PrimaryContact') AS VARCHAR(255))
    ,[Department] AS CAST(JSON_VALUE([Json], '$.Department') AS VARCHAR(255))
    ,[Address.State] AS CAST(JSON_VALUE([Json], '$.Address.State') AS VARCHAR(255))
    ,[Address.Locality] AS CAST(JSON_VALUE([Json], '$.Address.Locality') AS VARCHAR(255))
    ,[Contact.Name] AS CAST(JSON_VALUE([Json], '$.Contact.Name') AS VARCHAR(255))
    ,[Json] VARCHAR(MAX)
    ,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
    ,[CreatedBy] VARCHAR(255) NULL
    ,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
    ,[ChangedBy] VARCHAR(255) NULL
)
