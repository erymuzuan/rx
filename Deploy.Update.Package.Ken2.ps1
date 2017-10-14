param
(
    [Parameter(Mandatory=$true)]
    [int]$Build,
    [string]$WebServer = "192.168.1.160",
    [pscredential]$Admin = (Get-Credential 'Administrator'),
    [string]$WebPath = "\\$WebServer\web\posentt"
    
)

$WebServerSession = New-PSSession -ComputerName $WebServer -Credential $Admin

Invoke-Command -Session $WebServerSession -ScriptBlock `
{
    if((Test-Path("$WebPath\binaries\$using:Build\")) -eq $false)
    {        
        mkdir "$WebPath\binaries\$using:Build\"
    }
     
}
$current = $Build -1 ;
    
Copy-Item .\$Build.7z -Force -Destination "$WebPath\binaries\$Build\"
Copy-Item  .\deployment\$Build.* -Force -Destination  "$WebPath\binaries\$Build\"
Copy-Item  .\deployment\version.$Build.* -Force -Destination  "$WebPath\binaries\$Build\"
Copy-Item  .\deployment\$current.json -Force -Destination  "$WebPath\binaries\$Build\"
Copy-Item  .\deployment\$current.ps1 -Force -Destination  "$WebPath\binaries\$Build\"
