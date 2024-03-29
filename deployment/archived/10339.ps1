$build = 10339
$channel = 'Alpha'

#update for $build.ps1
Write-Verbose "Welcome to update $build!"

#download the the $build.json to version
$version = Get-Content .\version.json -Raw | ConvertFrom-Json
$baseUri = "http://$channel.rxdeveloper.com/binaries/$build";


mkdir $build


Invoke-WebRequest -Uri "$baseUri/version.$build.json" -Method GET -OutFile .\version.json

Write-Verbose "Downloading $baseUri/$build.7z, Please wait..."
Invoke-WebRequest -Uri "$baseUri/$build.7z" -Method GET -OutFile .\$build\$build.7z



cd .\$build
& ..\utils\7za.exe x ".\$build.7z"
cd ..

# now copy all the dll,pdb,js,css,html,cshtml to the current path

#web
ls -Path .\$build\web\bin -Filter *.dll | copy -Destination .\web\bin
ls -Path .\$build\web\bin -Filter *.pdb | copy -Destination .\web\bin
ls -Path .\web\bin -Filter *.xml | Remove-Item
ls -Path .\web\bin -Filter *.config | Remove-Item

ls -Path .\$build\database\Table -Filter *.* | copy -Destination .\database\Table


$xml = (Get-Content .\web\Web.config ) -as [xml]
$ApplicationName = $xml.SelectSingleNode('//appSettings/add[@key="sph:ApplicationName"]/@value').'#text'
$SqlConnectionString = [System.Environment]::GetEnvironmentVariable("RX_" + $ApplicationName + "_SqlConnectionString", "Process")
$LocalDb = [System.Environment]::GetEnvironmentVariable("RX_" + $ApplicationName + "_SqlLocalDbName", "Process")

Write-Host "Creating Sph.AccessToken table" -ForegroundColor Cyan
Import-Module .\utils\sqlcmd.dll
Invoke-SqlCmdRx -TrustedConnection -Server $LocalDb -Database $ApplicationName -InputFile .\database\Table\Sph.AccessToken.sql


#open the release note
Start-Process "$baseUri/$build.html"
