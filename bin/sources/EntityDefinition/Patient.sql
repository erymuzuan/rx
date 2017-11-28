CREATE TABLE [DevV1].[Patient](
  [Id] VARCHAR(50) PRIMARY KEY NOT NULL
,[Mrn] AS CAST(JSON_VALUE([Json], '$.Mrn') AS VARCHAR(255))
,[FullName] AS CAST(JSON_VALUE([Json], '$.FullName') AS VARCHAR(255))
,[Gender] AS CAST(JSON_VALUE([Json], '$.Gender') AS VARCHAR(255))
,[Religion] AS CAST(JSON_VALUE([Json], '$.Religion') AS VARCHAR(255))
,[Race] AS CAST(JSON_VALUE([Json], '$.Race') AS VARCHAR(255))
,[Status] AS CAST(JSON_VALUE([Json], '$.Status') AS VARCHAR(255))
,[MaritalStatus] AS CAST(JSON_VALUE([Json], '$.MaritalStatus') AS VARCHAR(255))
,[Json] VARCHAR(MAX)
,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[CreatedBy] VARCHAR(255) NULL
,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[ChangedBy] VARCHAR(255) NULL
)
