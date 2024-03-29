$build = 10289

if(!(Get-Command 7za -ErrorAction SilentlyContinue))
{
    Write-Warning "Cannot find 7za in your path, Please download and install it in your path"
    exit;
}

Write-Host "PLEASE MAKE SURE YOU HAVE COMMIT ALL YOUR CHANGES" -ForegroundColor Yellow
Write-Host "Update will sometime replace your files, it's important that you commit all your changes, to allow you to compare what's the update have done to your files"
Write-Host "Press [Enter] to continue or q to exit"
$commit = Read-Host
if($commit -eq "q"){
    exit;
}




#update for $build.ps1
Write-Host "Welcome to update $build!" -ForegroundColor Cyan -BackgroundColor Black


#download the the $build.json to version
$version = Get-Content .\version.json -Raw | ConvertFrom-Json
$baseUri = "http://www.bespoke.com.my/download/$build";

mkdir $build
Invoke-WebRequest -Uri "$baseUri/version.$build.json" -Method GET -OutFile .\version.json
Invoke-WebRequest -Uri "$baseUri/$build.7z" -Method GET -OutFile .\$build\$build.7z

#unzip the package
if(!(Get-Command 7za -ErrorAction SilentlyContinue))
{
    Write-Warning "Cannot find 7za in your path to extract this output, please download them 7z command line and put it in your path"
    exit;
}
cd .\$build
& 7za x ".\$build.7z"
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


ls -Path .\$build\tools -Filter *.exe | copy -Destination .\tools
ls -Path .\$build\tools -Filter *.dll | copy -Destination .\tools
ls -Path .\$build\tools -Filter *.pdb | copy -Destination .\tools

#web
ls -Path .\$build\web\bin -Filter *.dll | copy -Destination .\web\bin
ls -Path .\$build\web\bin -Filter *.pdb | copy -Destination .\web\bin

ls -Path .\$build\web\Content -Filter *.* | copy -Destination .\web\Content

ls -Path .\$build\web\docs -Filter *.html | copy -Destination .\web\docs
ls -Path .\$build\web\docs\scripts -Filter *.js | copy -Destination .\web\docs\scripts
ls -Path .\$build\web\docs\scripts -Filter *.map | copy -Destination .\web\docs\scripts

ls -Path .\$build\web\fonts -Filter *.* | copy -Destination .\web\fonts

ls -Path .\$build\web\images -Filter *.png | copy -Destination .\web\scripts

ls -Path .\$build\web\scripts -Filter *.js | copy -Destination .\web\scripts
ls -Path .\$build\web\scripts -Filter *.map | copy -Destination .\web\scripts

ls -Path .\$build\web\SphApp\views -Filter *.html | copy -Destination .\web\SphApp\views
ls -Path .\$build\web\SphApp\viewmodels -Filter *.js | copy -Destination .\web\SphApp\viewmodels
ls -Path .\$build\web\SphApp\services -Filter *.js | copy -Destination .\web\SphApp\services

ls -Path .\$build\web\ -Filter *.js | copy -Destination .\web
ls -Path .\$build\web\ -Filter *.ico | copy -Destination .\web


if((Test-Path .\web\App_Data) -eq $false)
{
    mkdir .\web\App_Data
    copy .\$build\web\App_Data\*.* -Destination .\web\App_Data
}
ls -Path .\$build\web\App_Data -Filter *.* | copy -Destination .\web\App_Data


#open the release note
Start-Process "http://www.bespoke.com.my/download/$build/$build.html"
