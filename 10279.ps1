#update for 10279.ps1
Write-Host "Welcome to update 10729!"
Write-Host "Your application name"
$name = Read-Host
Write-Host "Your IIS Port No"
$port = Read-Host


Write-Host "Press [ENTER] to exit"
Read-Host

#download the the 10278.json to version
$version = Get-Content .\version.json | ConvertFrom-Json

Invoke-WebRequest -Uri "http://www.bespoke.com.my/download/version.10279.json" -Method GET -OutFile .\version.json


