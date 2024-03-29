$build = 10325
$channel = 'Alpha'

#update for $build.ps1
Write-Verbose "Welcome to update $build!"

#download the the $build.json to version
$version = Get-Content .\version.json -Raw | ConvertFrom-Json
$baseUri = "http://eryken2.asuscomm.com:8080/binaries/$build";


mkdir $build


Invoke-WebRequest -Uri "$baseUri/version.$build.json" -Method GET -OutFile .\version.json

Write-Verbose "Downloading $baseUri/$build.7z, Please wait..."
Invoke-WebRequest -Uri "$baseUri/$build.7z" -Method GET -OutFile .\$build\$build.7z



cd .\$build
& ..\utils\7za.exe x ".\$build.7z"
cd ..



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





#what if we need to update control.center
Write-Debug "Updating control center "
"copy .\$build\control.center\*.exe .\control.center\ `
copy .\$build\control.center\*.pdb .\control.center\ `
rmdir .\$build /s /q`
del .\$build.ps1`
del .\update.bat" | Out-File .\update.bat -Encoding ascii

#update control center.bat
'IF EXIST "Update.bat" ( 
    call Update.bat 
)
start control.center\controlcenter.exe /in-memory-broker'| Out-File .\ControlCenter.InMemory.bat -Encoding ascii


'IF EXIST "Update.bat" ( 
    call Update.bat 
)
start control.center\controlcenter.exe'| Out-File .\ControlCenter.bat -Encoding ascii

#open the release note
Start-Process "http://eryken2.asuscomm.com:8080/binaries/$build/$build.html"
