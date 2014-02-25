Param(
       [string]$WorkingCopy = "..\wc", 
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