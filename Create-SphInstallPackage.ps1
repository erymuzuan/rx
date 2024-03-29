﻿Param(

       [switch]$UploadToOneDrive,
       [string]$Drop = '01',
       [switch]$PreRelease = $false,
       [string]$ToolsDirectory = "c:\project\tools"
     )

$LastBuild = ls -Path .\deployment -Filter '1*.ps1' | sort -Descending Name | select -First 1 | % Name | % {$_.Replace(".ps1", "") }
$Build = [System.Int32]::Parse($LastBuild) + 1

Write-Host "Have you compiled your solution and published web.sph ? (y/n) : " -ForegroundColor Yellow -NoNewline
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

$output = ".\bin\build"
#creates directory
if(Test-Path($output))
{
    Remove-Item $output -Force -Recurse
    Get-ChildItem -Path $output -Recurse |  Remove-Item $output -Force -Recurse
}

mkdir $output
Write-Host "Creating $output directory"

if((Test-Path("$output\sources")) -eq $false)
{
    mkdir "$output\sources"
}
if((Test-Path("$output\output")) -eq $false)
{
    mkdir "$output\output"
}

if((Test-Path("$output\web\SphApp\views")) -eq $false)
{
    mkdir "$output\web\SphApp\views"
}

if((Test-Path("$output\web\SphApp\viewmodels")) -eq $false)
{
    mkdir "$output\web\SphApp\viewmodels"
}


if((Test-Path("$output\schedulers")) -eq $false)
{
    mkdir "$output\schedulers"
}
if((Test-Path("$output\subscribers")) -eq $false)
{
    mkdir "$output\subscribers"
}
if((Test-Path("$output\subscribers.host")) -eq $false)
{
    mkdir "$output\subscribers.host"
}
if((Test-Path("$output\web")) -eq $false)
{
    mkdir "$output\web"
}

if((Test-Path("$output\tools")) -eq $false)
{
    mkdir "$output\tools"
}


if((Test-Path("$output\rabbitmq_base")) -eq $false)
{
    mkdir "$output\rabbitmq_base"
}


if((Test-Path("$output\utils")) -eq $false)
{
    mkdir "$output\utils"
}
copy "$ToolsDirectory\7za.exe" "$output\utils"
copy "$ToolsDirectory\mru.exe" "$output\utils"   
copy "$ToolsDirectory\n.exe" "$output\utils"     
copy "$ToolsDirectory\LINQPad.exe" "$output\utils"
copy .\bin\utils\sqlcmd.dll "$output\utils"

#setup
copy .\bin\Setup-SphApp.ps1 $output

#schedulers
Get-ChildItem -Filter *.* -Path ".\bin\schedulers" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.StartsWith("DevV1.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\schedulers" -Force


#subscribers
Get-ChildItem -Filter *.* -Path ".\bin\subscribers" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.StartsWith("DevV1.") -eq $false} `
| ? { $_.Name.StartsWith("ff") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\subscribers" -Force



#subscribers.host
Get-ChildItem -Filter *.* -Path ".\bin\subscribers.host" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.StartsWith("DevV1.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\subscribers.host" -Force

#tools
Get-ChildItem -Filter *.* -Path ".\bin\tools" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.StartsWith("DevV1.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\tools" -Force -Recurse

#web
Get-ChildItem -Filter *.* -Path ".\bin\web" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.StartsWith("DevV1.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| ? { $_.Name.EndsWith(".md") -eq $false} `
| Copy-Item -Destination "$output\web" -Force -Recurse
#App_data/empty.xsd
copy .\source\web\web.sph\App_Data -Destination $output\Web -Force -Recurse


#web.bin -- for dependencies
Get-ChildItem -Filter *.* -Path ".\source\web\web.sph\bin" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.StartsWith("DevV1.") -eq $false} `
| ? { $_.Name.EndsWith(".config") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\web\bin" -Force


if((Test-Path("$output\StartAspnetAdminWeb.bat")) -eq $false)
{
    copy .\StartAspnetAdminWeb.bat -Destination $output
    $c0 = (gc "$output\StartAspnetAdminWeb.bat").replace("$PWD\source\web\web.sph","workingcopy-web")
    Set-Content "$output\StartAspnetAdminWeb.bat" -Value $c0
}



#delete all accidentally added config
$rubbishConfigs = @("$output\subscribers\subscriber.workflow.dll.config"
,"$output\schedulers\scheduler.delayactivity.config"
,"$output\schedulers\razor.template.dll.config"
,"$output\schedulers\scheduler.workflow.trigger.config"
,"$output\schedulers\sql.repository.dll.config"
,"$output\subscribers\razor.template.dll.config"
,"$output\subscribers\sql.repository.dll.config"
)
foreach($ucon in $rubbishConfigs)
{
    if((Test-Path $ucon) -eq $true){
        Remove-Item $ucon
    }
}
#Nuget and packages
mkdir $output\.nuget
Get-ChildItem -Filter *.* -Path ".\bin\.nuget" `
| Copy-Item -Destination "$output\.nuget" -Force -Recurse
mkdir $output\packages
Get-ChildItem -Filter *.* -Path ".\bin\packages" `
| Copy-Item -Destination "$output\packages" -Force -Recurse


#iis express and config
mkdir $output\config
Get-ChildItem -Filter *.* -Path ".\bin\config" `
| Copy-Item -Destination "$output\config" -Force -Recurse

mkdir "$output\IIS Express"
Get-ChildItem -Filter *.* -Path ".\bin\IIS Express" `
| Copy-Item -Destination "$output\IIS Express" -Force -Recurse


#elastic search
mkdir $output\elasticsearch
Get-ChildItem -Filter *.* -Path ".\bin\elasticsearch" `
| Copy-Item -Destination "$output\elasticsearch" -Force -Recurse
#replace elasticsearch config
del $output\elasticsearch\config\elasticsearch.yml
copy .\bin\elasticsearch.template.yml $output\elasticsearch\config\elasticsearch.yml -Force


ls -Path "$output\elasticsearch\logs" -Filter *.* | Remove-Item

#rabbitmq_server
mkdir $output\rabbitmq_server
Get-ChildItem -Filter *.* -Path ".\bin\rabbitmq_server" `
| Copy-Item -Destination "$output\rabbitmq_server" -Force -Recurse



#control.center
mkdir "$output\control.center"
Get-ChildItem -Filter *.* -Path ".\bin\control.center" `
| Copy-Item -Destination "$output\control.center" -Force -Recurse


#databases and mapping
mkdir $output\database
Get-ChildItem -Filter *.* -Path ".\source\database" `
| Copy-Item -Destination "$output\database" -Force -Recurse
mkdir $output\database\mapping
Get-ChildItem -Filter *.* -Path ".\source\elasticsearch\mapping" `
| Copy-Item -Destination "$output\database\mapping" -Force -Recurse

copy .\bin\ControlCenter.bat $output

#remove the custom triggers
Get-Item -Path .\bin\build\subscribers\subscriber.trigger.* `
| ? { $_.Name.EndsWith("trigger.dll") -eq $false} `
| ? { $_.Name.EndsWith("trigger.pdb") -eq $false} `
| Remove-Item

Get-Item -Path .\bin\build\web\bin\subscriber.trigger.* `
| ? { $_.Name.EndsWith("trigger.dll") -eq $false} `
| ? { $_.Name.EndsWith("trigger.pdb") -eq $false} `
| Remove-Item

ls -Filter *.md -Path $output\web\docs | Remove-Item

#version
$versionJson = @"
{
"build": $Build
}
"@
$versionJson > $output\version.json
Write-Host ""

Write-Host ""

# remove unused and big files
ls -Path $output\control.center -Filter *.xml | Remove-Item
ls -Path $output\control.center -Filter *.cache | Remove-Item
ls -Path $output\control.center -Filter *.vshost.exe | Remove-Item

ls -Path $output\subscribers.host -Filter *.vshost.exe | Remove-Item
ls -Path $output\subscribers.host -Filter *.vshost.exe.config | Remove-Item
ls -Path $output\subscribers.host -Filter *.manifest | Remove-Item

ls -Path $output\tools -Filter *.vshost.exe | Remove-Item
ls -Path $output\tools -Filter *.manifest | Remove-Item
ls -Path $output\tools -Filter *.js | Remove-Item
ls -Path $output\tools -Filter *.html | Remove-Item


ls -Path $output -Recurse -Filter *.dll.config | Remove-Item
ls -Path $output -Recurse -Filter *.resources.dll | Remove-Item
ls -Path $output -Recurse -Filter *.dump | Remove-Item
ls -Path $output -Recurse -Filter *.lastcodeanalysissucceeded | Remove-Item
ls -Path $output -Recurse -Filter GalaSoft.*.pdb | Remove-Item
ls -Path $output -Recurse -Filter Microsoft.*.pdb | Remove-Item
ls -Path $output -Recurse -Filter sqlmembership.directoryservices.dll.config | Remove-Item
ls -Path $output -Recurse -Filter DiffPlex.pdb | Remove-Item
ls -Path $output -Recurse -Filter SQLSpatialTools.pdb | Remove-Item
ls -Path $output -Recurse -Filter Humanizer.pdb | Remove-Item
ls -Path $output -Recurse -Filter Common.Logging.pdb | Remove-Item
ls -Path $output -Recurse -Filter Humanizer.pdb | Remove-Item
ls -Path $output -Recurse -Filter Common.Logging.Core.pdb | Remove-Item
ls -Path $output -Recurse -Filter Spring.Core.pdb | Remove-Item
ls $output\tools | ? { $_.Mode.StartsWith('d----')} | Remove-Item -Force -Recurse
ls $output\control.center | ? { $_.Mode.StartsWith('d----')} | Remove-Item -Force -Recurse
ls $output\subscribers.host | ? { $_.Mode.StartsWith('d----')} | Remove-Item -Force -Recurse
ls $output\subscribers.host | ? { $_.Mode.StartsWith('d----')} | Remove-Item -Force -Recurse
ls $output\schedulers | ? { $_.Mode.StartsWith('d----')} | Remove-Item -Force -Recurse
ls $output\web\bin | ? { $_.Mode.StartsWith('d----')} | Remove-Item -Force -Recurse
Remove-Item $output\elasticsearch\data -Force -Recurse
ls -Path $output -Recurse -Filter DevV1.*.dll | Remove-Item
ls -Path $output -Recurse -Filter DevV1.*.pdb | Remove-Item
ls $output\web\App_Data\i18n | ? {$_.Name.StartsWith("options") -eq $false} | Remove-Item
ls $output\control.center\controlcenter.vshost.* | Remove-Item

ls -Path $output\web\bin\ -Recurse -Filter *.resources.dll
ls -Path $output\subscribers.host -Recurse -Filter *.resources.dll
ls -Path $output\tools -Recurse -Filter *.resources.dll
ls -Path $output\schedulers -Recurse -Filter *.resources.dll
ls -Path $output\control.center -Recurse -Filter *.resources.dll

ls $output\web\bin\ -r | ? {$_.PSIsContainer -eq $True} | ? {$_.GetFiles().Count -eq 0} | Remove-Item -Confirm
ls $output\subscribers.host -r | ? {$_.PSIsContainer -eq $True} | ? {$_.GetFiles().Count -eq 0} | Remove-Item -Confirm
ls $output\tools -r | ? {$_.PSIsContainer -eq $True} | ? {$_.GetFiles().Count -eq 0} | Remove-Item -Confirm
ls $output\schedulers -r | ? {$_.PSIsContainer -eq $True} | ? {$_.GetFiles().Count -eq 0} | Remove-Item -Confirm
ls $output\control.center -r | ? {$_.PSIsContainer -eq $True} | ? {$_.GetFiles().Count -eq 0} | Remove-Item -Confirm

if((Test-Path("$output\web\bin\roslyn")) -eq $false)
{
    mkdir $output\web\bin\roslyn
}
copy .\source\web\web.sph\bin\roslyn\* $output\web\bin\roslyn\

if((Test-Path("$output\tools\bin\roslyn")) -eq $false)
{
    mkdir $output\tools\bin\roslyn
}
copy .\source\web\web.sph\bin\roslyn\* $output\tools\bin\roslyn\


if((Test-Path("$output\subscribers.host\bin\roslyn")) -eq $false)
{
    mkdir $output\subscribers.host\bin\roslyn
}
copy .\source\web\web.sph\bin\roslyn\* $output\subscribers.host\bin\roslyn\


# TODO - remove humanizer localization folders
# TODO - remove erl_crashdump
# CODE ANALYSIS files
# vshost.exe
#manifest vshost.exe
# copy bin/roslyn to tools and web/bin

# clean up custom forms
[System.IO.File]::WriteAllText("$PWD\bin\build\web\App_Data\custom-dialog.json","[]")
[System.IO.File]::WriteAllText("$PWD\bin\build\web\App_Data\custom-partial-view.json","[]")
[System.IO.File]::WriteAllText("$PWD\bin\build\web\App_Data\custom-script.json","[]")
[System.IO.File]::WriteAllText("$PWD\bin\build\web\App_Data\routes.config.json","[]")



Write-Host "Please check for any errors, Press [Enter] to continue packaging into 7z or q to exit : " -ForegroundColor Yellow -NoNewline
$compressed = Read-Host
if($compressed -eq 'q')
{
    Write-Host "bye."
    exit;
}


#compress
& 7za a -t7z ".\sph.package.1.0.$Build-$Drop.7z" ".\bin\build\*"



Write-Host "Done compressing" -ForegroundColor Cyan
if($UploadToOneDrive.IsPresent)
{
    $Authentication = Get-ODAuthentication -ClientID "288ab228-361e-417f-bba2-eb2e6b17d618"
    $AuthToken=$Authentication.access_token
    Add-ODItem -AccessToken $AuthToken -Path "/Public/rx-drops" -LocalFile ".\sph.package.1.0.$Build-$Drop.7z"

    Write-Host "Done uploading to OneDrive"
}