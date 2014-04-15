$build = 10279

#update for 10279.ps1
Write-Host "Welcome to update $build!" -ForegroundColor Cyan -BackgroundColor Black
Write-Host "What is your application name ? : " -NoNewline -ForegroundColor Yellow -BackgroundColor Black
$name = Read-Host
Write-Host "What is your IIS Port No ? : " -NoNewline -ForegroundColor Yellow -BackgroundColor Black
$port = Read-Host

Write-Host "You are now updating $name running on $port, Press [ENTER] to continue or q to exit"
$quit = Read-Host
if($quit -eq "q"){
    exit;
}


#download the the 10279.json to version
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
ls -Path .\$build\schedulers -Filter *.dll | copy -Destination .\schedulers
ls -Path .\$build\schedulers -Filter *.pdb | copy -Destination .\schedulers


ls -Path .\$build\subscribers -Filter *.dll | copy -Destination .\subscribers
ls -Path .\$build\subscribers -Filter *.pdb | copy -Destination .\subscribers


ls -Path .\$build\subscribers.host -Filter *.dll | copy -Destination .\subscribers.host
ls -Path .\$build\subscribers.host -Filter *.pdb | copy -Destination .\subscribers.host


ls -Path .\$build\tools -Filter *.dll | copy -Destination .\tools
ls -Path .\$build\tools -Filter *.pdb | copy -Destination .\tools


ls -Path .\$build\web\bin -Filter *.dll | copy -Destination .\web\bin
ls -Path .\$build\web\bin -Filter *.pdb | copy -Destination .\web\bin
ls -Path .\$build\web\Content -Filter *.* | copy -Destination .\web\Content
ls -Path .\$build\web\docs -Filter *.html | copy -Destination .\web\docs
ls -Path .\$build\web\docs\scripts -Filter *.js | copy -Destination .\web\docs\scripts
ls -Path .\$build\web\fonts -Filter *.* | copy -Destination .\web\fonts
ls -Path .\$build\web\App_Data -Filter *.* | copy -Destination .\web\App_Data

if((Test-Path .\web\App_Data) -eq $false)
{
    mkdir .\web\App_Data
    copy .\$build\web\App_Data\*.* -Destination .\web\App_Data
}



