if((Test-Path .\packages) -eq $false){
        mkdir .\packages
}
$pwd = pwd
$packagesFiles = ls -Path .\source -Recurse -Filter packages.config 
$packagesFiles | foreach{
    Write-Host "Reading "  $_.FullName.Replace($pwd,"").Replace("packages.config","") -ForegroundColor Cyan
    [xml]$packagesXml = Get-Content $_.FullName
    $packagesXml.packages.package | foreach    {
        $folder = ".\packages\" + $_.id + "." + $_.version
       
        if((Test-Path $folder) -eq $false){
            Write-Host ($folder.Replace(".\packages\","") + "....")
            $arg = "install " + $_.id + " -Version " + $_.version
           # Write-Host $arg
           Start-Process -FilePath ".\.nuget\NuGet.exe" -WorkingDirectory ".\packages" -WindowStyle Hidden -ArgumentList $arg > output.txt
            
        }
    }    
}

$nuget = gps nuget* | measure
while($nuget.Count -gt 0){
    Start-Sleep -Milliseconds 500
    Write-Host "." -NoNewline    
    $nuget = gps nuget* | measure
}
Write-Host "Done downloading nuget. packages"
