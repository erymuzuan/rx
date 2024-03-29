$build = 10308

#update for $build.ps1
Write-Verbose "Welcome to update $build!"

#download the the $build.json to version
$version = Get-Content .\version.json -Raw | ConvertFrom-Json
$baseUri = "http://staging.reactivedeveloper.com/binaries/$build";


mkdir $build


Invoke-WebRequest -Uri "$baseUri/version.$build.json" -Method GET -OutFile .\version.json

Write-Verbose "Downloading $baseUri/$build.7z, Please wait..."
Invoke-WebRequest -Uri "$baseUri/$build.7z" -Method GET -OutFile .\$build\$build.7z



cd .\$build
& ..\utils\7za.exe x ".\$build.7z"
cd ..



#what if we need to update control.center
Write-Debug "Updating control center "
"copy .\$build\control.center\*.exe .\control.center\ `
copy .\$build\control.center\*.pdb .\control.center\ `
del .\update.bat" | Out-File .\update.bat -Encoding ascii

#open the release note
Start-Process "http://staging.reactivedeveloper.com/binaries/$build/$build.html"
