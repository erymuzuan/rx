$packagesFiles = ls -Recurse -Filter packages.config 
$packagesFiles | foreach{
    Write-Host "Getting content from "  $_.FullName
    [xml]$packagesXml = Get-Content $_.FullName
    $packagesXml.packages.package | foreach    {
        $folder = ".\packages\" + $_.id + "." + $_.version
       
        if((Test-Path $folder) -eq $false){
            Write-Host "Downloading " $folder
            $arg = "install " + $_.id + " -Version " + $_.version
           # Write-Host $arg
           Start-Process -FilePath ".\.nuget\NuGet.exe" -WorkingDirectory ".\packages" -ArgumentList $arg -Wait > output.txt
        }
    }    
}
