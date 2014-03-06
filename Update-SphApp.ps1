Param(
       [string]$WorkingCopy = "", 
       [string]$ApplicationName = "",
       [string]$Port = 0,
       [string]$SqlServer = "(localdb)\Projects",
       [string]$RabbitMqUserName = "guest",
       [string]$RabbitMqPassword = "guest",
	   [switch]$Help = $false
     )
if(($Help -eq $true) -or ($ApplicationName -eq ""))
{
	Write-Output "	A script to help copy the latest bin to your working copy, `
	You'll need to provide the path to your working copy and the application name`
	"	
	
	exit;
}
# copy some dll into schedulers and subscribers
copy .\source\web\web.sph\bin\Common.Logging.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.Mvc.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.Razor.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.Razor.dll .\bin\subscribers

copy .\packages\Common.Logging.2.2.0\lib\net40\Common.Logging.dll .\bin\subscribers
copy .\packages\Common.Logging.2.2.0\lib\net40\Common.Logging.dll .\bin\subscribers.host
copy .\packages\Common.Logging.2.2.0\lib\net40\Common.Logging.dll .\bin\schedulers
copy .\packages\Common.Logging.2.2.0\lib\net40\Common.Logging.dll .\bin\tools
copy .\packages\Common.Logging.2.2.0\lib\net40\Common.Logging.dll .\bin\web\bin



copy .\source\web\web.sph\bin\Common.Logging.dll .\bin\schedulers
copy .\source\web\web.sph\bin\System.Web.Mvc.dll .\bin\schedulers
copy .\source\web\web.sph\bin\System.Web.Razor.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.Razor.dll .\bin\subscribers


copy source\web\web.sph\bin\System.Web.WebPages.Razor.dll bin\schedulers
copy source\web\web.sph\bin\System.Web.WebPages.dll bin\schedulers
copy source\web\web.sph\bin\System.Web.Mvc.dll bin\schedulers

copy source\web\web.sph\bin\System.Web.WebPages.Razor.dll bin\subscribers
copy source\web\web.sph\bin\System.Web.WebPages.dll bin\subscribers
copy source\web\web.sph\bin\System.Web.Mvc.dll bin\subscribers

#creates directory
if((Test-Path($WorkingCopy)) -eq $false)
{
    Write-Host "Nothing to update here"
    exit
}

if((Test-Path("$WorkingCopy\sources")) -eq $false)
{
    mkdir "$WorkingCopy\sources"
}
if((Test-Path("$WorkingCopy\output")) -eq $false)
{
    mkdir "$WorkingCopy\output"
}
if((Test-Path("$WorkingCopy\schedulers")) -eq $false)
{
    mkdir "$WorkingCopy\schedulers"
}
if((Test-Path("$WorkingCopy\subscribers")) -eq $false)
{
    mkdir "$WorkingCopy\subscribers"
}
if((Test-Path("$WorkingCopy\subscribers.host")) -eq $false)
{
    mkdir "$WorkingCopy\subscribers.host"
}
if((Test-Path("$WorkingCopy\web")) -eq $false)
{
    mkdir "$WorkingCopy\web"
}

if((Test-Path("$WorkingCopy\tools")) -eq $false)
{
    mkdir "$WorkingCopy\tools"
}

#schedulers
Get-ChildItem -Filter *.* -Path ".\bin\schedulers" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$WorkingCopy\schedulers" -Force


#subscribers
Get-ChildItem -Filter *.* -Path ".\bin\subscribers" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$WorkingCopy\subscribers" -Force



#subscribers.host
Get-ChildItem -Filter *.* -Path ".\bin\subscribers.host" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$WorkingCopy\subscribers.host" -Force

#tools
Get-ChildItem -Filter *.* -Path ".\bin\tools" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$WorkingCopy\tools" -Force -Recurse


#web
Get-ChildItem -Filter *.* -Path ".\bin\web" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$WorkingCopy\web" -Force -Recurse


#web.bin -- for dependencies
Get-ChildItem -Filter *.* -Path ".\source\web\web.sph\bin" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".config") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$WorkingCopy\web\bin" -Force


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


Write-Host "Do you want to re-create all the tables, this action will cause you to looose all the data"
$droptables = Read-Host "Type Yes to continue"

if($droptables -eq "Yes")
{
    #creates databases
    Get-ChildItem -Filter *.sql -Path C:\project\work\sph\source\database\Table `
    | %{
        Write-Host "Creating table $_"
        $sqlFileName = $_.FullName    
        & sqlcmd -S "$SqlServer" -E -d "$ApplicationName" -i "$sqlFileName"
    }
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

    $connectionString = 'Data Source=' + $SqlServer +';Initial Catalog='+ $ApplicationName +';Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False'

    $xml.SelectSingleNode('//connectionStrings/add[@name="Sph"]/@connectionString').'#text' = $connectionString
    $xml.SelectSingleNode('//spring/objects/object[@name="IPersistence"]/constructor-arg[@name="connectionString"]/@value').'#text' = $connectionString
    $xml.SelectSingleNode('//spring/objects/object[@name="IBrokerConnection"]/property[@name="VirtualHost"]/@value').'#text' = $ApplicationName
    $xml.SelectSingleNode('//spring/objects/object[@name="IBrokerConnection"]/property[@name="UserName"]/@value').'#text' = $RabbitMqUserName
    $xml.SelectSingleNode('//spring/objects/object[@name="IBrokerConnection"]/property[@name="Password"]/@value').'#text' = $RabbitMqPassword

    $xml.Save($configFile)
}


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