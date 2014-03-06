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

if((Test-Path("rabbitmq")) -eq $false)
{
    mkdir "rabbitmq"
}

Get-ChildItem -Path "$installPath\content" | Copy-Item -Destination "rabbitmq" -Force
Write-Host "Done"