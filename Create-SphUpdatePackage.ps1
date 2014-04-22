Param(
       [int]$Build ,
       [switch]$PreRelease = $false
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
    $password
  )
  if ($sourceUri -match '\\$|\\\w+$') { throw 'sourceuri should end with a file name' }
  $ftprequest = [System.Net.FtpWebRequest]::Create($sourceuri);
  $ftprequest.Method = [System.Net.WebRequestMethods+Ftp]::MakeDirectory
  $ftprequest.UseBinary = $true

  $ftprequest.Credentials = New-Object System.Net.NetworkCredential($username,$password)

  $response = $ftprequest.GetResponse();

  Write-Host Create Folder Complete, status $response.StatusDescription

  $response.Close();
}


function Upload-FtpFile {
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
    $path
  )
  
  Write-Host "Uploadig $path ... please wait...."
  if ($sourceUri -match '\\$|\\\w+$') { throw 'sourceuri should end with a file name' }
  $ftprequest = [System.Net.FtpWebRequest]::Create($sourceuri);
  $ftprequest.Method = [System.Net.WebRequestMethods+Ftp]::UploadFile;
  $ftprequest.UseBinary = $true

  $ftprequest.Credentials = New-Object System.Net.NetworkCredential($username,$password)

  #$sourceStream = new-object IO.StreamReader $path
  #$fileContents = [Text.Encoding]::UTF8.GetBytes($sourceStream.ReadToEnd());
  #$sourceStream.Close();
  $fileContents = Get-Content $path -encoding byte
  $ftprequest.ContentLength = $fileContents.Length;

  $requestStream = $ftprequest.GetRequestStream();
  $requestStream.Write($fileContents, 0, $fileContents.Length);
  $requestStream.Close();

  $response = $ftprequest.GetResponse();

  Write-Host Upload File Complete, status $response.StatusDescription

  $response.Close();
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

$output = ".\sph.packages\output"
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


#control.center
mkdir "$output\control.center"
Get-ChildItem -Filter *.* -Path ".\sph.packages\control.center" `
| Copy-Item -Destination "$output\control.center" -Force -Recurse


#databases and mapping
mkdir $output\database
Get-ChildItem -Filter *.* -Path ".\source\database" `
| Copy-Item -Destination "$output\database" -Force -Recurse
mkdir $output\database\mapping
Get-ChildItem -Filter *.* -Path ".\source\elasticsearch\mapping" `
| Copy-Item -Destination "$output\database\mapping" -Force -Recurse


#remove the custom triggers
Get-Item -Path .\sph.packages\output\subscribers\subscriber.trigger.* `
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


Write-Host "Delete Roslyn dll ? [y/n] : " -ForegroundColor Yellow -NoNewline
$deleteRoslyn = Read-Host
if($deleteRoslyn -eq "y")
{
    ls -Path $output -Recurse -Filter Roslyn.Compilers.* | Remove-Item
    ls -Path $output -Recurse -Filter Roslyn.Services.* | Remove-Item
}

Write-Host "Please check for any errors, Press [Enter] to continue packaging into 7z or q to exit : " -ForegroundColor Yellow -NoNewline
$compressed = Read-Host
if($compressed -eq 'q')
{
    exit;
}

#compress
& 7za a -t7z ".\$Build.7z" ".\sph.packages\output\*"

#creates the update manifest
$previous = $Build -1
if(Test-Path .\deployment\$previous.ps1)
{
    (Get-Content .\deployment\$previous.ps1).Replace($previous, $Build) > .\deployment\$Build.ps1
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
#release note
"#Release Note for $Build" > .\deployment\$Build.md

$ftpRoot = "ftp://www.bespoke.com.my/website/download"
$ftpUserName = "bespoke"
$ftpPassword = "gsxr750wt"

Write-Host -ForegroundColor Yellow "NOW edit the .\deployment\$Build.ps1 and the Release Note($Build.md) to reflect any custom scripts needed to be run"
Write-Host "Press [ENTER] to continue uploaded  to ftp " -NoNewline -ForegroundColor Yellow
Read-Host

Create-FtpDirectory -sourceuri "$ftpRoot/$Build" -username $ftpUserName -password $ftpPassword
Upload-FtpFile -sourceuri "$ftpRoot/$previous.json" -username $ftpUserName -password $ftpPassword -path .\deployment\$previous.json
Upload-FtpFile -sourceuri "$ftpRoot/$Build/version.$Build.json" -username $ftpUserName -password $ftpPassword -path .\deployment\version.$Build.json
Upload-FtpFile -sourceuri "$ftpRoot/$Build/$Build.html" -username $ftpUserName -password $ftpPassword -path .\deployment\$Build.html

Upload-FtpFile -sourceuri "$ftpRoot/$Build/$Build.7z" -username $ftpUserName -password $ftpPassword -path .\$Build.7z
#$build.ps1 to root
Upload-FtpFile -sourceuri "$ftpRoot/$Build.ps1" -username $ftpUserName -password $ftpPassword -path .\deployment\$Build.ps1
Upload-FtpFile -sourceuri "$ftpRoot/$Build/$Build.ps1" -username $ftpUserName -password $ftpPassword -path .\deployment\$Build.ps1
