Param(
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


if(!(Get-Command Invoke-WebRequest -ErrorAction SilentlyContinue))
{
    Write-Warning "You will need at least powershell version 3.0"
    exit;
}


if(!(Test-Path("C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regsql.exe")))
{
    Write-Warning "Cannot find aspnet_regsql in your path, you may not have .Net 4.5.1 SDK installed"
    Start-Process "http://www.bespoke.com.my/download"
    exit;
}


#verify the tools are in the path
if(!(Test-Path(".\rabbitmq_server\sbin\rabbitmqctl.bat")))
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
    Write-Warning "Cannot cannot to $SqlServer , Please make sure the server instance is correct and trusted connection can be used"
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




if((Test-Path("$WorkingCopy\StartAspnetAdminWeb.bat")) -eq $false)
{
    $c0 = (gc "$WorkingCopy\StartAspnetAdminWeb.bat").replace("WorkingCopyPath","$WorkingCopy\web")
    Set-Content "$WorkingCopy\StartAspnetAdminWeb.bat" -Value $c0
}



#creates databases
Write-Host "Creating database $ApplicationName"
& sqlcmd -S "(localdb)\$SqlServer" -E -d master -Q "DROP DATABASE [$ApplicationName]"
& sqlcmd -S "(localdb)\$SqlServer" -E -d master -Q "CREATE DATABASE [$ApplicationName]"
Write-Host "Created database $ApplicationName"
#Start-Sleep -Seconds 10
& sqlcmd -S "(localdb)\$SqlServer" -E -d "$ApplicationName" -Q "CREATE SCHEMA [Sph] AUTHORIZATION [dbo]"
& sqlcmd -S "(localdb)\$SqlServer" -E -d "$ApplicationName" -Q "CREATE SCHEMA [$ApplicationName] AUTHORIZATION [dbo]"
Write-Host "Created schema [SPH]"

Get-ChildItem -Filter *.sql -Path $WorkingCopy\database\Table `
| %{
    Write-Host "Creating table $_"
    $sqlFileName = $_.FullName    
    & sqlcmd -S "(localdb)\$SqlServer" -E -d "$ApplicationName" -i "$sqlFileName"
}


#Rabbitmqctl
& .\rabbitmq_server\sbin\rabbitmqctl.bat add_vhost "$ApplicationName"
& .\rabbitmq_server\sbin\rabbitmqctl.bat set_permissions -p "$ApplicationName" $RabbitMqUserName ".*" ".*" ".*"
& .\rabbitmq_server\sbin\rabbitmq-plugins.bat enable "rabbitmq_management"

#elastic search mappings
$esindex = $ElasticSearchHost + "/" + $ApplicationName.ToLowerInvariant() + "_sys"
Invoke-WebRequest -Method Put -Body "" -Uri $esindex  -ContentType "application/javascript"

Get-ChildItem -Filter *.json -Path .\database\mapping `
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
#set the IIS config
$apc = (Get-Content .\config\applicationhost.config) -as [xml]
$site = $apc.SelectSingleNode("//configuration/system.applicationHost/sites/site[@id=1]")
$site.SetAttribute("name", "web.$ApplicationName")
$site.SelectSingleNode("application/virtualDirectory").SetAttribute("physicalPath", "$WorkingCopy\web")
$bindingInformation = "*:" + $Port.ToString() + ":localhost"
$site.SelectSingleNode("bindings/binding").SetAttribute("bindingInformation","$bindingInformation")
$apc.Save("$WorkingCopy\config\applicationhost.config")

$startWebBat = Get-Content .\StartWeb.bat
$startWebBat.Replace("%USERPROFILE%\Documents\IISExpress", $WorkingCopy) > .\StartWeb.bat

#asp.net memberships
Write-Host "Executing Aspnet membership provider" -ForegroundColor Cyan
Start-Process -RedirectStandardOutput "v1.log" -Wait -WindowStyle Hidden -FilePath "C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regsql.exe" `
-ArgumentList  @("-E","-S",'"(localdb)\$SqlServer"',"-d " + $ApplicationName,"-A mr")


Write-Host "Aspnet membership has been added"
Write-Host "Please wait....."

#roles
& .\mru -r administrators -r developers -r can_edit_entity -r can_edit_workflow -c "$WorkingCopy\web\web.config"
& .\mru -u admin -p 123456 -e admin@$ApplicationName.com -r administrators -r developers -r can_edit_entity -r can_edit_workflow -c "$WorkingCopy\web\web.config"



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