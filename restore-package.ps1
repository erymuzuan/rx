$packagesFiles = ls -Path .\source -Recurse -Filter packages.config 
$packagesFiles | foreach {
    Write-Host "Getting content from "  $_.FullName -ForegroundColor Cyan
    [xml]$packagesXml = Get-Content $_.FullName
    $packagesXml.packages.package | foreach    {
        $folder = ".\packages\" + $_.id + "." + $_.version
       
        if((Test-Path $folder) -eq $false){
            Write-Host ($folder.Replace(".\packages\","") + " ....")
            $arg = "install " + $_.id + " -Version " + $_.version
           # Write-Host $arg
           Start-Process -FilePath ".\.nuget\NuGet.exe" -WorkingDirectory ".\packages" -WindowStyle Hidden -ArgumentList $arg > output.txt
            
        }
    }    
}

$packagesFiles | foreach {
    Write-Host "Getting content from "  $_.FullName -ForegroundColor Cyan
    [xml]$packagesXml = Get-Content $_.FullName
    $packagesXml.packages.package | foreach    {
        $folder = ".\packages\" + $_.id + "." + $_.version
       
        if((Test-Path $folder) -eq $false){
            
            
        }
    }    
}

