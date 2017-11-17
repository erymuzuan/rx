Param(
       [string[]] $Destinations,
       [string]$ProductName = "Rx Developer"
     )

     
function Get-ProductName {
  param(
    [Parameter(Mandatory=$true)]
    [System.IO.FileInfo]$File
  )
    $vi = $File.VersionInfo
    if($vi -eq $null){
        return;
    }
    $pn = $vi.ProductName
  
    return $pn
    
}
function Get-FileVersion{
  param(
    [Parameter(Mandatory=$true)]
    [System.IO.FileInfo]$File
  )
    $vi = $File.VersionInfo
    if($vi -eq $null){
        return;
    }
    $pv = $vi.ProductVersion
    $fv = $vi.FileVersion
  
    return $fv
    
}

function Get-FileRevision{
  param(
    [Parameter(Mandatory=$true)]
    [System.IO.FileInfo]$File
  )
    $fv = Get-FileVersion($File)
    $rev = ""
    if($fv -ne $null){
        $rev = $fv.Split(@("."), "RemoveEmptyEntries")[3];        
    }
    return $rev
    
}


function Get-FileCommitHash{
  param(
    [Parameter(Mandatory=$true)]
    [System.IO.FileInfo]$File
  )
    $vi = $File.VersionInfo
    if($vi -eq $null){
        continue;
    }
    $pv = $vi.ProductVersion
  
   return $fv
    
}



$latest = @{}

ls -Path .\source, .\bin -Recurse -Filter *.dll | % {
    $pn = Get-ProductName($_)
    if($pn -ne $ProductName){
        #Write-Host $_.Name $pn        
        return
    }

    $rev =Get-FileRevision($_)
    if($rev -eq $null){
        continue;
    }

    if($latest.ContainsKey($_.Name)){
        $last = $latest[$_.Name];
        if($rev -gt $last){
            $latest[$_.Name] = $rev
        }            
    }else{         
        $latest.Add($_.Name, $rev)  
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
    $dll = $_.Name
    $rev = $latest[$dll]

    # filter to dll which has the same revision number with the one in the files
    if($fv -ne $null -and $fv.StartsWith("2.1") -and $fv.EndsWith(".$rev")){
       # TODO : check the revision in the destination, if it's the same then ignore
      
       
       foreach($folder in $Destinations){            
            #Write-Host "Deploying to $folder" 
            $deployedRev = Get-FileRevision((ls "$folder\$dll"))
            if($deployedRev -lt $rev){
                Write-Host "Overriding $dll REV : $deployedRev with REV: $rev" 
                $_ | Copy-Item -Destination $folder -Force
            }
        }
        
       $latest.Remove($dll)
             
    }

}

Write-Output $latest 