#update for 10279.ps1
Write-Host "Welcome to update 10729!"
Write-Host "Your application name"
$name = Read-Host
Write-Host "Your IIS Port No"
$port = Read-Host


Write-Host "Press [ENTER] to exit"
Read-Host

#download the the 10278.json to version
$version = Get-Content .\version.json -Raw | ConvertFrom-Json
$baseUri = "http://www.bespoke.com.my/download/10279";

Invoke-WebRequest -Uri "$baseUri/version.10279.json" -Method GET -OutFile .\version.json
Invoke-WebRequest -Uri "$baseUri/files.txt" -Method GET -OutFile .\files.10279.txt

$files  = Get-Content .\files.10279.txt | ? { $_.Trim().StartsWith("#") -eq $false }

#subscribers
foreach($f in $files){

    Write-Host "Downloading $f ..."
    Invoke-WebRequest -Uri "$baseUri/$f" -Method GET -OutFile .\$f	
}


