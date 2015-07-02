Param(
       [int]$Build ,
       [switch]$Release = $false
     )

function Create-FtpDirectory {
  param(
    [Parameter(Mandatory=$true)]
    [string]
    $sourceuri,
    [Parameter(Mandatory=$true)]
    [string]
    $username,
    [Parameter(Mandatory=$true)]
    [string]
    $password,
    [Parameter(Mandatory=$true)]
    [string]
    $Directory
  )
  
  $SecureString = ConvertTo-SecureString $password -AsPlainText -Force
  $PSCredential = New-Object System.Management.Automation.PSCredential ($username, $SecureString)

  Import-Module PSFTP 
  Set-FTPConnection -Credentials $PSCredential -Server ftp://www.reactivedeveloper.com -Session ss01 
  $Session = Get-FTPConnection -Session ss01

  New-FTPItem -Session $Session -Path /staging/binaries -Name $Directory
    
}


function Remove-FtpDirectory {
  param(
    [Parameter(Mandatory=$true)]
    [string]
    $sourceuri,
    [Parameter(Mandatory=$true)]
    [string]
    $username,
    [Parameter(Mandatory=$true)]
    [string]
    $password,
    [Parameter(Mandatory=$true)]
    [string]
    $Directory
  )
  
  $SecureString = ConvertTo-SecureString $password -AsPlainText -Force
  $PSCredential = New-Object System.Management.Automation.PSCredential ($username, $SecureString)

  Import-Module PSFTP 
  Set-FTPConnection -Credentials $PSCredential -Server ftp://www.reactivedeveloper.com -Session ss01 
  $Session = Get-FTPConnection -Session ss01

  Remove-FTPItem -Session $Session -Path $Directory -Recurse
    
}


function Upload-FtpFile {
  param(
    [Parameter(Mandatory=$true)]
    [string]
    $LocalFile,
    [Parameter(Mandatory=$true)]
    [string]
    $username,
    [Parameter(Mandatory=$true)]
    [string]
    $password,
    [Parameter(Mandatory=$true)]
    [string]
    $RemoteDirectory
  )
  
  Write-Host "Uploading $path ... please wait...."
  if ($LocalFile -match '\\$|\\\w+$') { throw 'sourceuri should end with a file name' }
  if((Test-Path($LocalFile)) -eq $false){
    Write-Warning "Cannot find $LocalFile"
    exit;
  }
    

  
  $SecureString = ConvertTo-SecureString $password -AsPlainText -Force
  $PSCredential = New-Object System.Management.Automation.PSCredential ($username, $SecureString)

  Import-Module PSFTP 
  Set-FTPConnection -Credentials $PSCredential -Server ftp://www.reactivedeveloper.com -Session ss01 
  $Session = Get-FTPConnection -Session ss01

  Add-FTPItem -Session $Session -Overwrite -Path $RemoteDirectory -LocalPath $LocalFile

}


if($Build -eq 0)
{
    Write-Host "Please specify the Build Number"
    exit;
}

Write-Host "Have you compiled your solution and published web.sph ? (y/n) : " -NoNewline -ForegroundColor Yellow
$published = Read-Host
if($published -ne "y")
{
    Write-Warning "Too bad .. bye"
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


#schedulers
Get-ChildItem -Filter *.* -Path ".\bin\schedulers" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\schedulers" -Force


#subscribers
Get-ChildItem -Filter *.* -Path ".\bin\subscribers" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\subscribers" -Force



#subscribers.host
Get-ChildItem -Filter *.* -Path ".\bin\subscribers.host" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\subscribers.host" -Force



#tools
Get-ChildItem -Filter *.* -Path ".\bin\tools" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\tools" -Force -Recurse

#web
Get-ChildItem -Filter *.* -Path ".\bin\web" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\web" -Force -Recurse
#App_data/empty.xsd
copy .\source\web\web.sph\App_Data -Destination $output\Web -Force -Recurse


#web.bin -- for dependencies
Get-ChildItem -Filter *.* -Path ".\source\web\web.sph\bin" `
| ? { $_.Name.StartsWith("workflows.") -eq $false} `
| ? { $_.Name.StartsWith("Dev.") -eq $false} `
| ? { $_.Name.EndsWith(".config") -eq $false} `
| ? { $_.Name.EndsWith(".xml") -eq $false} `
| Copy-Item -Destination "$output\web\bin" -Force


#web.fonts
Get-ChildItem -Filter *.* -Path ".\source\web\core.sph\fonts" | Copy-Item -Destination "$output\web\fonts" -Force




#delete all accidentally added config
$rubbishConfigs = @("$output\subscribers\subscriber.workflow.dll.config"
,"$output\schedulers\scheduler.delayactivity.config"
,"$output\schedulers\razor.template.dll.config"
,"$output\schedulers\scheduler.workflow.trigger.config"
,"$output\schedulers\sql.repository.dll.config"
,"$output\subscribers\razor.template.dll.config"
,"$output\subscribers\sql.repository.dll.config"
,"$output\web\App_Data\custom-dialog.json"
,"$output\web\App_Data\custom-partial-view.json"
,"$output\web\App_Data\custom-script.json"
,"$output\web\App_Data\routes.config.json"
,"$output\web\App_Data\data-imports\*.json"
,"$output\control.center\metadata.cache"
,"$output\control.center\*.json"
,"$output\schedulers\*.dll.config"
,"$output\subscribers.host\*.vshost.exe"
,"$output\subscribers.host\*.vshost.exe.config"
,"$output\subscribers.host\*.vshost.exe.manifest"
,"$output\tools\*.dll.config"
,"$output\tools\*.js"
,"$output\tools\*.html"
)
foreach($ucon in $rubbishConfigs)
{
    if((Test-Path $ucon) -eq $true){
        Remove-Item $ucon
    }
}
ls -Filter *.7z -Path $output  -Recurse | Remove-Item
ls

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


#remove the custom triggers
Get-Item -Path .\bin\build\subscribers\subscriber.trigger.* `
| ? { $_.Name.EndsWith("trigger.dll") -eq $false} `
| ? { $_.Name.EndsWith("trigger.pdb") -eq $false} `
| Remove-Item

# remove workers service runner - just run the workser console runner
ls -Path $output\subscribers.host -Filter workers.windowsservice.runner.* | Remove-Item

Write-Host ""

Write-Host ""

# remove unused and big files
ls -Path "$output\control.center" -Filter *.xml | Remove-Item
ls -Path $output -Recurse -Filter Spring.Core.pdb | Remove-Item

$commonLibraries = @("Telerik", "Roslyn.Services", "Microsoft", "Oracle.ManagedDataAccess",
"SuperSocket",
"RabbitMQ.Client",
"RazorEngine",
"GalaSoft",
"Spring",
"System",
"WebGrease",
"NamedPipeWrapper",
"Newtonsoft.Json",
"websocket-sharp",
"Humanizer",
"log4net",
"Roslyn.Utilities",
"Roslyn.Compilers",
"Raygun.Diagnostics",
"Monads.NET",
"Mindscape.",
"MySql.Data",
"SQLSpatialTools",
"LinqToQuerystring",
"WebActivatorEx",
"ImageResizer",
"DiffPlex",
"WebActivator",
"Owin",
"Antlr3",
"Common.Logging",
"RazorGenerator.Mvc",
"ICSharpCode.AvalonEdit"
)

foreach($lib in $commonLibraries)
{
    
    Write-Host "Delete $lib dll ? [ENTER] or n [NO] : " -ForegroundColor Yellow -NoNewline
    $deleteLib = Read-Host
    if($deleteLib-eq "")
    {
        Write-Host "Deleting $lib.*"
        ls -Path $output -Recurse -Filter "$lib*.dll" | Remove-Item
        ls -Path $output -Recurse -Filter "$lib*.pdb" | Remove-Item
        ls -Path $output -Recurse -Filter "$lib*.xml" | Remove-Item
    }

}




# remove unused and big files
ls -Path $output\control.center -Filter *.xml | Remove-Item

ls -Path $output -Recurse -Filter assembly.test.* | Remove-Item
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
ls -Path $output -Recurse -Filter Invoke.Docx.* | Remove-Item
ls -Path $output -Recurse -Filter SQLSpatialTools.* | Remove-Item
ls $output\tools | ? { $_.Mode -eq 'd----'} | Remove-Item -Force -Recurse
ls $output\control.center | ? { $_.Mode -eq 'd----'} | Remove-Item -Force -Recurse
ls $output\subscribers | ? { $_.Mode -eq 'd----'} | Remove-Item -Force -Recurse
ls $output\subscribers.host | ? { $_.Mode -eq 'd----'} | Remove-Item -Force -Recurse
ls $output\schedulers | ? { $_.Mode -eq 'd----'} | Remove-Item -Force -Recurse
ls $output\web\bin | ? { $_.Mode -eq 'd----'} | Remove-Item -Force -Recurse
ls -Path $output -Recurse -Filter DevV1.*.dll | Remove-Item
ls -Path $output -Recurse -Filter DevV1.*.pdb | Remove-Item
ls $output\web\App_Data\i18n | ? {$_.Name.StartsWith("options") -eq $false} | Remove-Item
ls $output\control.center\controlcenter.vshost.* | Remove-Item
ls $output\control.center -Filter *.json | Remove-Item
ls $output\control.center -Filter *.manifest | Remove-Item




Write-Host "Please check for any errors, Press [Enter] to continue packaging into 7z or q to exit : " -ForegroundColor Yellow -NoNewline
$compressed = Read-Host
if($compressed -eq 'q')
{
    exit;
}

#compress
& 7za a -t7z ".\$Build.7z" ".\bin\build\*"

#creates the update manifest
$previous = $Build -1
if(Test-Path .\deployment\update-script-template.ps1)
{
    (Get-Content .\deployment\update-script-template.ps1).Replace("%build_number%", $Build.ToString()) > .\deployment\$Build.ps1
}

#create the update files
$today = (get-date).ToString("yyyy-MM-dd")
$updateJson = @"
{
    "build": $previous,
	"vnext" : $Build,
    "date" : "$today",
	"update-script" : "$Build.ps1"
}
"@
$updateJson > .\deployment\$previous.json

$versionBuildJson = @"
{
    
    "build": $Build,
    "date" : "$today"
}
"@
$versionBuildJson > .\deployment\version.$Build.json

#release note - copy from existing file, should maintains the UTF8 encoding
copy .\deployment\release-note-template.md .\deployment\$Build.md

$ftpRoot = "ftp://www.reactivedeveloper.com/staging/binaries"
if($Release -eq $true){
    $ftpRoot = "ftp://www.reactivedeveloper.com/production/binaries"
}
$ftpUserName = "rxdeveloper"
$ftpPassword = "reH2TaXd"

Write-Host -ForegroundColor Yellow "NOW edit the .\deployment\$Build.ps1 and the Release Note($Build.md) to reflect any custom scripts needed to be run"
Write-Host "Press [ENTER] to continue uploaded  to ftp " -NoNewline -ForegroundColor Yellow
Read-Host

if($Release -eq $false)
{
   ## Remove-FtpDirectory -username $ftpUserName -password $ftpPassword -Directory "/staging/binaries/$Build" -sourceuri $ftpRoot
   ## Remove-FtpDirectory -username $ftpUserName -password $ftpPassword -Directory "/staging/binaries/$Build.ps1" -sourceuri $ftpRoot
}

Create-FtpDirectory -sourceuri "$ftpRoot/" -username $ftpUserName -password $ftpPassword -Directory $Build
Upload-FtpFile -RemoteDirectory "$ftpRoot/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\$previous.json
Upload-FtpFile -RemoteDirectory "$ftpRoot/$Build/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\version.$Build.json

if((Test-Path .\deployment\$Build.html) -eq $false)
{
    Write-Host -ForegroundColor Yellow "Cannot find $Build.html for your release note, please use the markdown files provided to generate the Release note"
    Read-Host
}

Upload-FtpFile -RemoteDirectory "$ftpRoot/$Build/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\$Build.html

Upload-FtpFile -RemoteDirectory "$ftpRoot/$Build/" -username $ftpUserName -password $ftpPassword -LocalFile .\$Build.7z
#$build.ps1 to root
Upload-FtpFile -RemoteDirectory "$ftpRoot/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\$Build.ps1
Upload-FtpFile -RemoteDirectory "$ftpRoot/$Build/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\$Build.ps1
