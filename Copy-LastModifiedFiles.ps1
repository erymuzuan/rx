Param(

       [string]$Path,
       [int]$First = 2,
       [Switch]$WhatIf,
       [string[]] $Destinations,
       [string[]]$Excludes = @("*.config")
     )
$files = ls $Path -Exclude $Excludes | sort -Descending LastWriteTime | select -First $First

if($WhatIf.IsPresent -eq $true){

    foreach($folder in $Destinations){
        Write-Host "Copying $files to $folder"
    }
    
    $files
    return;
}

foreach($folder in $Destinations){
    $files| Copy-Item -Destination $folder -Force -Verbose
}
$files