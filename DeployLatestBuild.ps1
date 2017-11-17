Param(
       [string[]] $Destinations
     )
function Get-FileVersion{
  param(
    [Parameter(Mandatory=$true)]
    [string]$File
  )
    $vi = $File.VersionInfo
    if($vi -eq $null){
        continue;
    }
    $pv = $vi.ProductVersion
    $fv = $vi.FileVersion
  
    Write-Output $fv
    
}


$latest = @{}

ls -Path .\source, .\bin -Recurse -Filter *.dll | % {
    $vi = $_.VersionInfo
    if($vi -eq $null){
        continue;
    }
    $pv = $vi.ProductVersion
    $fv = $vi.FileVersion
    $rev = ""
    if($fv -ne $null -and $fv.StartsWith("2.1")){
        $rev = $fv.Split(@("."), "RemoveEmptyEntries")[3];
        
    }
    if($pv -ne $null -and $pv.StartsWith("2.1")){
         # Write-Host REV:$rev $_.Name  " " $_.VersionInfo.ProductVersion
         if($latest.ContainsKey($_.Name)){
            $last = $latest[$_.Name];
            if($rev -gt $last){
                $latest[$_.Name] = $rev
            }
            
         }else{
         
            $latest.Add($_.Name, $rev)  
         }
    }

}

Write-Output $latest

ls -Path .\source,.\bin -Recurse -Filter *.dll | % {
    $vi = $_.VersionInfo
    if($vi -eq $null){
        continue;
    }

    $pv = $vi.ProductVersion
    $fv = $vi.FileVersion
    $rev = $latest[$_.Name]

    # filter to dll which has the same revision number with the one in the files
    if($fv -ne $null -and $fv.StartsWith("2.1") -and $fv.EndsWith(".$rev")){
       # TODO : check the revision in the destination, if it's the same then ignore
       
       foreach($folder in $Destinations){
            Write-Host "Copying item $_ REV: $rev to $folder" 
            $_ | Copy-Item -Destination $folder -Force
        }
        
       $latest.Remove($_.Name)
             
    }

}

Write-Output $latest 