$DebugPreference = "Continue"
Import-Module .\utils\sqlcmd.dll

$project = (Get-Content .\project.json) -join "`n" | ConvertFrom-Json 

$tables = @( 'Adapter',
'AuditTrail',
'BinaryStore',
'Designation',
'DocumentTemplate',
'EmailTemplate',
'EntityChart',
'EntityDefinition', 
'EntityForm',
'EntityView',
'Message',
'Organization',
'Page',
'ReportDefinition',
'ReportDelivery',
'ReportDelivery',
'Role',
'SearchDefinition',
'Setting',
'Tracker',
'TransformDefinition',
'Trigger',
'UserProfile',
'Workflow',
'WorkflowDefinition')

$applicationName = $project.applicationName
$localdb  = $project.sqlLocalDbName


Write-Debug "Application name is $applicationName"
foreach($table in $tables){
    Write-Debug "Now altering table $table..."
    $pk = Invoke-SqlCmdRx -E -S "(localdb)\$localdb" -d $applicationName -q "SELECT name FROM sys.key_constraints WHERE [type] = 'PK' AND [parent_object_id]= OBJECT_ID('[Sph].[$table]')"
    $pkname = $pk.name

    Write-Debug "PK for $table is $pkname"

    Invoke-SqlCmdRx -E -S "(localdb)\$localdb" -d $applicationName -q "ALTER TABLE [Sph].[$table] DROP CONSTRAINT [$pkname]"
    Invoke-SqlCmdRx -E -S "(localdb)\$localdb" -d $applicationName -q "ALTER TABLE [Sph].[$table] ALTER COLUMN [Id] VARCHAR(255) NOT NULL"
    Invoke-SqlCmdRx -E -S "(localdb)\$localdb" -d $applicationName -q "ALTER TABLE [Sph].[$table] ADD CONSTRAINT [PK_$table] PRIMARY KEY CLUSTERED ([Id] ASC)"

}


Invoke-SqlCmdRx -E -S "(localdb)\$localdb" -d $applicationName -q "ALTER TABLE [Sph].[ReportDelivery] ALTER COLUMN [ReportDefinitionId] VARCHAR(255) NOT NULL"
Invoke-SqlCmdRx -E -S "(localdb)\$localdb" -d $applicationName -q "ALTER TABLE [Sph].[Watcher] ALTER COLUMN [EntityId] VARCHAR(255) NOT NULL"

Invoke-SqlCmdRx -E -S "(localdb)\$localdb" -d $applicationName -q "ALTER TABLE [Sph].[EntityChart] ALTER COLUMN [EntityViewId] VARCHAR(255) NOT NULL"
Invoke-SqlCmdRx -E -S "(localdb)\$localdb" -d $applicationName -q "ALTER TABLE [Sph].[EntityChart] ALTER COLUMN [EntityDefinitionId] VARCHAR(255) NOT NULL"
