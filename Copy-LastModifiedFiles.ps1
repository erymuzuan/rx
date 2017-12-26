Param(
       [Parameter(Mandatory=$True,Position=0)]
       [string]$Path,
       [Parameter(Mandatory=$True,Position=1)]
       [string[]] $Destinations,
       [int]$First = 2,
       [Switch]$WhatIf,
       [string[]]$Excludes = @("*.config")
     )
$files = ls $Path -Exclude $Excludes | ? {$_.PSIsContainer -eq $false } | sort -Descending LastWriteTime | select -First $First

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