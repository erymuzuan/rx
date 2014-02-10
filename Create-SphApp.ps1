Param(
       [string]$WorkingCopy = "..\wc",
       [string]$ApplicationName = "",
       [string]$Port = 0,
       [string]$SqlServer = "(localdb)\Projects",
	   [switch]$Help = $false
     )
if(($Help -eq $true) -or ($ApplicationName -eq ""))
{
	Write-Output "	A script to help copy the latest bin to your working copy, `
	You'll need to provide the path to your working copy and the application name`
	"	
	
	exit;
}
if($Port -eq 0)
{
	Write-Output "Please provide a port no for your web app"	
	
	exit;
}
# copy some dll into schedulers and subscribers
copy .\source\web\web.sph\bin\Common.Logging.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.Mvc.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.Razor.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.Razor.dll .\bin\subscribers


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
    mkdir $WorkingCopy
    Write-Host "Creating $WorkingCopy directory"
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

$webconfig = (gc "$WorkingCopy\web\web.config").replace("sph.0009","$ApplicationName").Replace("4436", "$Port").Replace("Initial Catalog=Sph","Initial Catalog=$ApplicationName").Replace("Initial Catalog=sph","Initial Catalog=$ApplicationName")
Set-Content  "$WorkingCopy\web\web.config" -Value $webconfig



#workers.console.runner.exe.config
$config1 = (gc "$WorkingCopy\subscribers.host\workers.console.runner.exe.config").replace("sph.0009","$ApplicationName").Replace("4436", "$Port").Replace("Initial Catalog=Sph","Initial Catalog=$ApplicationName").Replace("Initial Catalog=sph","Initial Catalog=$ApplicationName")
Set-Content  "$WorkingCopy\subscribers.host\workers.console.runner.exe.config" -Value $config1



#scheduler.delayactivity.exe.config
$config2 = (gc "$WorkingCopy\schedulers\scheduler.delayactivity.exe.config").replace("sph.0009","$ApplicationName").Replace("4436", "$Port").Replace("Initial Catalog=Sph","Initial Catalog=$ApplicationName").Replace("Initial Catalog=sph","Initial Catalog=$ApplicationName")
Set-Content  "$WorkingCopy\schedulers\scheduler.delayactivity.config" -Value $config2



#scheduler.workflow.trigger.exe.config
$config3 = (gc "$WorkingCopy\schedulers\scheduler.workflow.trigger.exe.config").replace("sph.0009","$ApplicationName").Replace("4436", "$Port").Replace("Initial Catalog=Sph","Initial Catalog=$ApplicationName").Replace("Initial Catalog=sph","Initial Catalog=$ApplicationName")
Set-Content  "$WorkingCopy\schedulers\scheduler.workflow.trigger.config" -Value $config3


#creates databases
Write-Host "Creating database $ApplicationName"
& sqlcmd -S "$SqlServer" -E -d master -Q "DROP DATABASE [$ApplicationName]"
& sqlcmd -S "$SqlServer" -E -d master -Q "CREATE DATABASE [$ApplicationName]"
Write-Host "Created database $ApplicationName"
#Start-Sleep -Seconds 10
& sqlcmd -S "$SqlServer" -E -d "$ApplicationName" -Q "CREATE SCHEMA [Sph] AUTHORIZATION [dbo]"
Write-Host "Created schema [SPH]"

Get-ChildItem -Filter *.sql -Path C:\project\work\sph\source\database\Table `
| %{
    Write-Host "Creating table $_"
    $sqlFileName = $_.FullName    
    & sqlcmd -S "$SqlServer" -E -d "$ApplicationName" -i "$sqlFileName"
}



#creates IIS express directory
Write-Host "Creating site"
& "C:\Program Files (x86)\IIS Express\appcmd.exe" add site /name:"web.$ApplicationName" /bindings:http/*:$Port /physicalPath:"$WorkingCopy\web"
Write-Host "Site created"
