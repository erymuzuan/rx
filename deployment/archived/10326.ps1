$build = 10326
$channel = 'Alpha'

#update for $build.ps1
Write-Verbose "Welcome to update $build!"

#download the the $build.json to version
$version = Get-Content .\version.json -Raw | ConvertFrom-Json
$baseUri = "http://eryken2.asuscomm.com:8080/binaries/$build";


Invoke-WebRequest -Uri "$baseUri/version.$build.json" -Method GET -OutFile .\version.json

Write-Verbose "Downloading $baseUri/$build.7z, Please wait..."
Invoke-WebRequest -Uri "$baseUri/$build.7z" -Method GET -OutFile .\$build\$build.7z



cd .\$build
& ..\utils\7za.exe x ".\$build.7z"
cd ..


#schedulers
ls -Path .\$build\schedulers -Filter *.exe | copy -Destination .\schedulers
ls -Path .\$build\schedulers -Filter *.dll | copy -Destination .\schedulers
ls -Path .\$build\schedulers -Filter *.pdb | copy -Destination .\schedulers


#subscribers
ls -Path .\$build\subscribers -Filter *.dll | copy -Destination .\subscribers
ls -Path .\$build\subscribers -Filter *.pdb | copy -Destination .\subscribers


#host
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


#open the release note
Start-Process "http://eryken2.asuscomm.com:8080/binaries/$build/$build.html"
