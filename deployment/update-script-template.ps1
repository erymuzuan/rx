Param(
       [Parameter(Position=0)]
       [ValidateSet('Production','Staging','Alpha', 'PosEntt')]
       [string]$Channel = 'Alpha',
       [string]$ApplicationName,
       [string]$UpdateUrl
     )

$build = %build_number%
$channel = '%channel%'

#update for $build.ps1
Write-Verbose "Welcome to update $build!"

#download the the $build.json to version
$version = Get-Content .\version.json -Raw | ConvertFrom-Json
$baseUri = "http://$channel.rxdeveloper.com/binaries/$build";
if([System.Environment]::GetEnvironmentVariable("RX_$ApplicationName" + "_UpdateUrl").Length -gt 0){
    $baseUri = [System.Environment]::GetEnvironmentVariable("RX_$ApplicationName" + "_UpdateUrl") + "/binaries/$build"
}
if($UpdateUrl.Length -gt 0){
    $baseUri = "$UpdateUrl/binaries/$build"
}


mkdir $build


Invoke-WebRequest -Uri "$baseUri/version.$build.json" -Method GET -OutFile .\version.json

Write-Verbose "Downloading $baseUri/$build.7z, Please wait..."
Invoke-WebRequest -Uri "$baseUri/$build.7z" -Method GET -OutFile .\$build\$build.7z



cd .\$build
& ..\utils\7za.exe x ".\$build.7z"
cd ..

# now copy all the dll,pdb,js,css,html,cshtml to the current path
ls -Path .\$build\schedulers -Filter *.exe | copy -Destination .\schedulers
ls -Path .\$build\schedulers -Filter *.dll | copy -Destination .\schedulers
ls -Path .\$build\schedulers -Filter *.pdb | copy -Destination .\schedulers


ls -Path .\$build\subscribers -Filter *.dll | copy -Destination .\subscribers
ls -Path .\$build\subscribers -Filter *.pdb | copy -Destination .\subscribers


ls -Path .\$build\subscribers.host -Filter *.exe | copy -Destination .\subscribers.host
ls -Path .\$build\subscribers.host -Filter *.dll | copy -Destination .\subscribers.host
ls -Path .\$build\subscribers.host -Filter *.pdb | copy -Destination .\subscribers.host

#tools
ls -Path .\$build\tools -Filter *.exe | copy -Destination .\tools
ls -Path .\$build\tools -Filter *.dll | copy -Destination .\tools
ls -Path .\$build\tools -Filter *.pdb | copy -Destination .\tools

#utils
ls -Path .\$build\utils -Filter *.exe | copy -Destination .\utils
ls -Path .\$build\utils -Filter *.dll | copy -Destination .\utils
ls -Path .\$build\utils -Filter *.pdb | copy -Destination .\utils



#web
ls -Path .\$build\web\bin -Filter *.dll | copy -Destination .\web\bin
ls -Path .\$build\web\bin -Filter *.pdb | copy -Destination .\web\bin
ls -Path .\web\bin -Filter *.xml | Remove-Item
ls -Path .\web\bin -Filter *.config | Remove-Item

ls -Path .\$build\web\Content\external -Filter *.* | copy -Destination .\web\Content\external

ls -Path .\$build\web\docs -Filter *.html | copy -Destination .\web\docs
ls -Path .\$build\web\docs\scripts -Filter *.js | copy -Destination .\web\docs\scripts
ls -Path .\$build\web\docs\scripts -Filter *.map | copy -Destination .\web\docs\scripts

ls -Path .\$build\web\fonts -Filter *.* | copy -Destination .\web\fonts

ls -Path .\$build\web\images -Filter *.png | copy -Destination .\web\images

ls -Path .\$build\web\scripts -Filter *.js | copy -Destination .\web\scripts
ls -Path .\$build\web\scripts -Filter *.map | copy -Destination .\web\scripts


if((Test-Path .\web\App_Data) -eq $false)
{
    mkdir .\web\App_Data
    copy .\$build\web\App_Data\*.* -Destination .\web\App_Data -Recurse
}
# copy new snippets, but not the *.config.json in web\App_Data
ls -Path .\$build\web\App_Data\snippets -Filter *.* | copy -Destination .\web\App_Data\snippets -Recurse -Force



#open the release note
Start-Process "$baseUri/$build.html"
