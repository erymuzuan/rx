param
(
    [Parameter(Mandatory=$true)]
    [int]$Build,
    [string]$WebServer = "S314",
    [pscredential]$Admin = (Get-Credential 'ORG\Administrator')
    
)

$WebServerSession = New-PSSession -ComputerName $WebServer -Credential $Admin

Invoke-Command -Session $WebServerSession -ScriptBlock `
{
    if((Test-Path("c:\apps\rxwebsite\binaries\$using:Build\")) -eq $false)
    {        
        mkdir "c:\apps\rxwebsite\binaries\$using:Build\"
    }
     
}
$current = $Build -1 ;

Copy-Item .\$Build.7z -Force -ToSession $WebServerSession c:\apps\rxwebsite\binaries\$Build\
Copy-Item  .\deployment\$Build.* -Force -ToSession $WebServerSession c:\apps\rxwebsite\binaries\$Build\
Copy-Item  .\deployment\version.$Build.* -Force -ToSession $WebServerSession c:\apps\rxwebsite\binaries\$Build\
Copy-Item  .\deployment\$current.json -Force -ToSession $WebServerSession c:\apps\rxwebsite\binaries\$current.json
Copy-Item  .\deployment\$current.ps1 -Force -ToSession $WebServerSession c:\apps\rxwebsite\binaries\$current.ps1
