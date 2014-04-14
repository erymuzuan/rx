Param(
       [int]$Build = 0,
       [switch]$PreRelease = $false
     )


Write-Host "Have you compiled your solution and published web.sph ? (y/n)"
$published = Read-Host
if($published -ne "y")
{
    Write-Warning "Too bad .. bye"
    exit;
}

if($Build -eq 0)
{
    Write-Warning "Please provide a build number"
    exit;
}

#remove all the configs from subscribers
ls -Path .\bin\subscribers -Filter *.config | Remove-Item


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

$WorkingCopy = ".\sph.packages\output"
#creates directory
if(Test-Path($WorkingCopy))
{
    Remove-Item $WorkingCopy -Force -Recurse
    Get-ChildItem -Path $WorkingCopy -Recurse |  Remove-Item $WorkingCopy -Force -Recurse
}

mkdir $WorkingCopy
Write-Host "Creating $WorkingCopy directory"

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
#setup
copy .\sph.packages\Setup-SphApp.ps1 $WorkingCopy

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
| ? { $_.Name.EndsWith(".md") -eq $false} `
| Copy-Item -Destination "$WorkingCopy\web" -Force -Recurse
#App_data/empty.xsd
copy .\source\web\web.sph\App_Data -Destination $WorkingCopy\Web -Force -Recurse


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
    $c0 = (gc "$WorkingCopy\StartAspnetAdminWeb.bat").replace("c:\project\work\sph\source\web\web.sph","workingcopy-web")
    Set-Content "$WorkingCopy\StartAspnetAdminWeb.bat" -Value $c0
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

#iis express and config
mkdir $WorkingCopy\config
Get-ChildItem -Filter *.* -Path ".\sph.packages\config" `
| Copy-Item -Destination "$WorkingCopy\config" -Force -Recurse

mkdir "$WorkingCopy\IIS Express"
Get-ChildItem -Filter *.* -Path ".\sph.packages\IIS Express" `
| Copy-Item -Destination "$WorkingCopy\IIS Express" -Force -Recurse


#elastic search
mkdir $WorkingCopy\elasticsearch
Get-ChildItem -Filter *.* -Path ".\sph.packages\elasticsearch" `
| Copy-Item -Destination "$WorkingCopy\elasticsearch" -Force -Recurse



#rabbitmq_server
mkdir $WorkingCopy\rabbitmq_server
Get-ChildItem -Filter *.* -Path ".\sph.packages\rabbitmq_server" `
| Copy-Item -Destination "$WorkingCopy\rabbitmq_server" -Force -Recurse



#control.center
mkdir "$WorkingCopy\control.center"
Get-ChildItem -Filter *.* -Path ".\sph.packages\control.center" `
| Copy-Item -Destination "$WorkingCopy\control.center" -Force -Recurse


#databases and mapping
mkdir $WorkingCopy\database
Get-ChildItem -Filter *.* -Path ".\source\database" `
| Copy-Item -Destination "$WorkingCopy\database" -Force -Recurse
mkdir $WorkingCopy\database\mapping
Get-ChildItem -Filter *.* -Path ".\source\elasticsearch\mapping" `
| Copy-Item -Destination "$WorkingCopy\database\mapping" -Force -Recurse

copy .\sph.packages\ControlCenter.bat $WorkingCopy
copy .\source\web\web.sph\index.html $WorkingCopy\getting.started.html
copy .\sph.packages\mru.exe $WorkingCopy

#remove the custom triggers
Get-Item -Path .\sph.packages\output\subscribers\subscriber.trigger.* `
| ? { $_.Name.EndsWith("trigger.dll") -eq $false} `
| ? { $_.Name.EndsWith("trigger.pdb") -eq $false} `
| Remove-Item

ls -Filter *.md -Path $WorkingCopy\web\docs | Remove-Item

#version
$versionJson = @"
{
"build": $Build
}
"@
$versionJson > $WorkingCopy\version.json
Write-Host ""

Write-Host ""

Write-Host "Please check for any errors, Press [Enter] to continue packaging into 7z or q to exit"
$compressed = Read-Host
if($compressed -eq 'q')
{
    exit;
}

#compress
& 7za a -t7z ".\sph.package.1.0.$Build.7z" ".\sph.packages\output\*"