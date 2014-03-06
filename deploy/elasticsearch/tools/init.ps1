param($installPath, $toolsPath, $package)
Write-Host "Initializing folder..."
$parentFolder = (get-item $installPath)
do {
        $parentFolderFullName = $parentFolder.FullName

        $latest = Get-ChildItem -Path $parentFolderFullName -File -Filter client.app.sln | Select-Object -First 1
        if ($latest -ne $null) {
            $latestName = $latest.name
            #Write-Host "${latestName}"
        }

        if ($latest -eq $null) {
            $parentFolder = $parentFolder.parent    
        }
}
while ($parentFolder -ne $null -and $latest -eq $null)

#Write-Host "Solution Folder:" $parentFolder

if((Test-Path("elasticsearch")) -eq $false)
{
    mkdir "elasticsearch"
}
else
{
  Remove-Item -Path "elasticsearch\*" -force -recurse
}

if((Test-Path("elasticsearch\content")) -eq $false)
{
    mkdir "elasticsearch\content"
}

if((Test-Path("elasticsearch\bin")) -eq $false)
{
    mkdir "elasticsearch\bin"
}

if((Test-Path("elasticsearch\config")) -eq $false)
{
    mkdir "elasticsearch\config"
}

if((Test-Path("elasticsearch\lib")) -eq $false)
{
    mkdir "elasticsearch\lib"
}

Get-ChildItem -Path "$installPath\content" | Copy-Item -Destination "elasticsearch" -Force
Get-ChildItem -Path "$installPath\bin" | Copy-Item -Destination "elasticsearch\bin" -Force
Get-ChildItem -Path "$installPath\config" | Copy-Item -Destination "elasticsearch\config" -Force
Get-ChildItem -Path "$installPath\data" | Copy-Item -Destination "elasticsearch\data" -Force
Get-ChildItem -Path "$installPath\lib" | Copy-Item -Destination "elasticsearch\lib" -Force
Write-Host "Done"