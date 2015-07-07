$build = 10313

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
ls -Path .\web\bin -Filter *.xml | Remove-Item
ls -Path .\web\bin -Filter *.config | Remove-Item



#open the release note
Start-Process "http://staging.reactivedeveloper.com/binaries/$build/$build.html"
