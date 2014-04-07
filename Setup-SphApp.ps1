﻿Param(
       [string]$WorkingCopy = ".",
       [string]$ApplicationName = "",
       [string]$Port = 0,
       [string]$SqlServer = "Projects",
       [string]$RabbitMqUserName = "guest",
       [string]$RabbitMqPassword = "guest",
       [string]$ElasticSearchHost = "http://localhost:9200",
       [string]$ElasticSearchUserName = "",
       [string]$ElasticSearchPassword = "",
	   [switch]$Help = $false
     )
if(($Help -eq $true) -or ($ApplicationName -eq ""))
{
	Write-Output "	A script to help to create new SPH appp in your working copy, `
	You'll need to provide the path to your working copy and the application name`
	"	
	
	exit;
}
$WorkingCopy = pwd
Write-Host "Seting up Rx Developer- $ApplicationName project in $WorkingCopy"
if(!(Get-Command sqlcmd -ErrorAction SilentlyContinue))
{
    Write-Warning "Cannot find sqlcmd in your path"
    exit;
}

#verify the tools are in the path
if(!(Test-Path("$WorkingCopy\rabb")))
{
    Write-Warning "Cannot find rabbitmqctl"
    exit;
}
Try
{
   & sqlcmd -E -S "(localdb)\$SqlServer" -Q "SELECT COUNT(*) FROM sysdatabases"
}
Catch
{
    Write-Warning "Cannot cannot to (localdb)\$SqlServer , Please make sure the server instance is correct and trusted connection can be used"
    exit;
}


if($Port -eq 0)
{
	Write-Warning "Please provide a port no for your web app"	
	
	exit;
}
if($ApplicationName -eq "Dev")
{
	Write-Warning "Please provide a different name, Dev is a reserved keyword"	
	
	exit;
}

#remove all the configs from subscribers
ls -Path .\subscribers -Filter *.config | Remove-Item


#set file content for settings and bat
if((Test-Path("$WorkingCopy\StartWeb.bat")) -eq $false)
{
    copy .\StartWeb.bat -Destination $WorkingCopy
    $c2 = (gc "$WorkingCopy\StartWeb.bat").replace("web.sph","web.$ApplicationName")
    Set-Content "$WorkingCopy\StartWeb.bat" -Value $c2
}

if((Test-Path("$WorkingCopy\StartAspnetAdminWeb.bat")) -eq $false)
{
    copy .\StartAspnetAdminWeb.bat -Destination $WorkingCopy
    $c0 = (gc "$WorkingCopy\StartAspnetAdminWeb.bat").replace("c:\project\work\sph\source\web\web.sph","$WorkingCopy\web")
    Set-Content "$WorkingCopy\StartAspnetAdminWeb.bat" -Value $c0
}
if((Test-Path("$WorkingCopy\start-subscriber-console-runner.ps1")) -eq $false)
{
    copy .\StartSubscriber.bat -Destination $WorkingCopy
    $c1 = (gc "$WorkingCopy\StartSubscriber.bat").replace("sph.0009","$ApplicationName") 
    Set-Content  "$WorkingCopy\StartSubscriber.bat" -Value $c1
}

#web.config

$webconfig = (gc "$WorkingCopy\web\web.config")
Set-Content  "$WorkingCopy\web\web.config" -Value $webconfig



#workers.console.runner.exe.config
$config1 = (gc "$WorkingCopy\subscribers.host\workers.console.runner.exe.config")
Set-Content  "$WorkingCopy\subscribers.host\workers.console.runner.exe.config" -Value $config1



#scheduler.delayactivity.exe.config
$config2 = (gc "$WorkingCopy\schedulers\scheduler.delayactivity.exe.config")
Set-Content  "$WorkingCopy\schedulers\scheduler.delayactivity.config" -Value $config2

#scheduler.workflow.trigger.exe.config
$config3 = (gc "$WorkingCopy\schedulers\scheduler.workflow.trigger.exe.config")
Set-Content  "$WorkingCopy\schedulers\scheduler.workflow.trigger.config" -Value $config3


#creates databases
Write-Host "Creating database $ApplicationName"
& sqlcmd -S "(localdb)\$SqlServer" -E -d master -Q "DROP DATABASE [$ApplicationName]"
& sqlcmd -S "(localdb)\$SqlServer" -E -d master -Q "CREATE DATABASE [$ApplicationName]"
Write-Host "Created database $ApplicationName"
#Start-Sleep -Seconds 10
& sqlcmd -S "(localdb)\$SqlServer" -E -d "$ApplicationName" -Q "CREATE SCHEMA [Sph] AUTHORIZATION [dbo]"
& sqlcmd -S "(localdb)\$SqlServer" -E -d "$ApplicationName" -Q "CREATE SCHEMA [$ApplicationName] AUTHORIZATION [dbo]"
Write-Host "Created schema [SPH]"

Get-ChildItem -Filter *.sql -Path C:\project\work\sph\source\database\Table `
| %{
    Write-Host "Creating table $_"
    $sqlFileName = $_.FullName    
    & sqlcmd -S "(localdb)\$SqlServer" -E -d "$ApplicationName" -i "$sqlFileName"
}


#creates IIS express directory
Write-Host "Creating site"
$portBinding = $Port + ":localhost";
& "C:\Program Files (x86)\IIS Express\appcmd.exe" add site /name:"web.$ApplicationName" /bindings:http/*:$portBinding /physicalPath:"$WorkingCopy\web"
Write-Host "Site created"


#Rabbitmqctl
& rabbitmqctl add_vhost "$ApplicationName"
& rabbitmqctl set_permissions -p "$ApplicationName" $RabbitMqUserName ".*" ".*" ".*"


#elastic search mappings
$esindex = $ElasticSearchHost + "/" + $ApplicationName.ToLowerInvariant() + "_sys"
Invoke-WebRequest -Method Put -Body "" -Uri $esindex  -ContentType "application/javascript"

Get-ChildItem -Filter *.json -Path C:\project\work\sph\source\elasticsearch\mapping `
| %{
    $mappingUri = $esindex + "/" + $_.Name.ToLowerInvariant().Replace(".json", "") + "/_mapping"
    Write-Host "Creating elastic search mapping for $mappingUri"
    Invoke-WebRequest -Method PUT -Uri $mappingUri -InFile $_.FullName -ContentType "application/javascript"
}


#configs value
$allConfigs = @("$WorkingCopy\web\web.config"
, "$WorkingCopy\schedulers\scheduler.delayactivity.exe.config"
, "$WorkingCopy\schedulers\scheduler.workflow.trigger.exe.config"
, "$WorkingCopy\subscribers.host\workers.console.runner.exe.config"
, "$WorkingCopy\subscribers.host\workers.windowsservice.runner.exe.config"
, "$WorkingCopy\tools\sph.builder.exe.config"
)

foreach($configFile in $allConfigs){
    Write-Host "Processing $configFile"

    $xml = (Get-Content $configFile) -as [xml]
    $xml.SelectSingleNode('//appSettings/add[@key="sph:BaseUrl"]/@value').'#text' = 'http://localhost:' + $Port
    $xml.SelectSingleNode('//appSettings/add[@key="sph:BaseDirectory"]/@value').'#text' = $WorkingCopy
    $xml.SelectSingleNode('//appSettings/add[@key="sph:ApplicationName"]/@value').'#text' = $ApplicationName
    $xml.SelectSingleNode('//appSettings/add[@key="sph:ApplicationFullName"]/@value').'#text' = $ApplicationName

    $connectionString = 'Data Source=(localdb)\' + $SqlServer +';Initial Catalog='+ $ApplicationName +';Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False'

    $xml.SelectSingleNode('//connectionStrings/add[@name="Sph"]/@connectionString').'#text' = $connectionString
    $xml.SelectSingleNode('//spring/objects/object[@name="IPersistence"]/constructor-arg[@name="connectionString"]/@value').'#text' = $connectionString
    $xml.SelectSingleNode('//spring/objects/object[@name="IBrokerConnection"]/property[@name="VirtualHost"]/@value').'#text' = $ApplicationName
    $xml.SelectSingleNode('//spring/objects/object[@name="IBrokerConnection"]/property[@name="UserName"]/@value').'#text' = $RabbitMqUserName
    $xml.SelectSingleNode('//spring/objects/object[@name="IBrokerConnection"]/property[@name="Password"]/@value').'#text' = $RabbitMqPassword
    
    Try{
        $taskscheduler = $WorkingCopy   + "\schedulers\scheduler.delayactivity.exe"  
        $xml.SelectSingleNode('//spring/objects/object[@name="ITaskScheduler"]/constructor-arg[@name="executable"]/@value').'#text' = $taskscheduler
    }Catch{}
    $xml.Save($configFile)
}

#asp.net memberships
& aspnet_regsql.exe -E -S "(localdb)\$SqlServer" -d "$ApplicationName" -A mr
#roles
mru -r administrators -r developers -r can_edit_entity -r can_edit_workflow -c "$WorkingCopy\web\web.config"
mru -u admin -p 123456 -e admin@$ApplicationName.com -r administrators -r developers -c "$WorkingCopy\web\web.config"



#delete all accidentally added config
$rubbishConfigs = @("$WorkingCopy\subscribers\subscriber.workflow.dll.config"
,"$WorkingCopy\schedulers\scheduler.delayactivity.config"
,"$WorkingCopy\schedulers\razor.template.dll.config"
,"$WorkingCopy\schedulers\scheduler.workflow.trigger.config"
,"$WorkingCopy\schedulers\sql.repository.dll.config"
,"$WorkingCopy\subscribers\razor.template.dll.config"
,"$WorkingCopy\subscribers\sql.repository.dll.config"
)
foreach($ucon in $rubbishConfigs)
{
    if((Test-Path $ucon) -eq $true){
        Remove-Item $ucon
    }
}