$build = 10323
$channel = 'Alpha'

#update for $build.ps1
Write-Verbose "Welcome to update $build!"

#download the the $build.json to version
$version = Get-Content .\version.json -Raw | ConvertFrom-Json
$baseUri = "http://$channel.reactivedeveloper.com/binaries/$build";


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

ls -Path .\$build\web\sphapp\views -Filter *.html | copy -Destination .\web\sphapp\views
ls -Path .\$build\web\sphapp\viewmodels -Filter *.js | copy -Destination .\web\sphapp\viewmodels
ls -Path .\$build\web\sphapp\ko -Filter *.js | copy -Destination .\web\sphapp\ko
ls -Path .\$build\web\sphapp\services  -Filter *.js | copy -Destination .\web\sphapp\services


ls -Path .\web\SphApp\viewmodels -Filter _data.import.*.js | Remove-Item
ls -Path .\web\SphApp\views -Filter _data.import.*.html | Remove-Item



if((Test-Path .\web\App_Data) -eq $false)
{
    mkdir .\web\App_Data
    copy .\$build\web\App_Data\*.* -Destination .\web\App_Data -Recurse
}
# copy new snippets, but not the *.config.json in web\App_Data
ls -Path .\$build\web\App_Data\snippets -Filter *.* | copy -Destination .\web\App_Data\snippets -Recurse -Force



# elasticsearch index template
Get-ChildItem -Filter *.template -Path .\database\mapping `
| %{
    $templateName =  $_.Name.ToLowerInvariant().Replace(".template", "")
    $templateUri = "http://localhost:9200/_template/$templateName"
    $templateContent = Get-Content $_.FullName
    $templateJson = $templateContent.Replace("<<application_name>>", $esindex);

    Invoke-WebRequest -Method PUT -Uri $templateUri -ContentType "application/json" -Body $templateJson
    Write-Debug "Created elasticsearch index template for $templateName"
}


# elasticsearch pendingtask mapping for Workflow service
$project = Get-Content .\project.json | ConvertFrom-Json
$pendingTaskMappingUri = "http://localhost:9200/" + $project.applicationName.ToLower() + "/_mapping/pendingtask"
$pendingTaskMappingContent = Get-Content .\database\mapping\pendingtask.json
Invoke-WebRequest -Method DELETE -Uri $pendingTaskMappingUri
Invoke-WebRequest -Method PUT -Uri $pendingTaskMappingUri -ContentType "application/javascript" -Body $pendingTaskMappingContent


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
Start-Process "http://$channel.reactivedeveloper.com/binaries/$build/$build.html"
