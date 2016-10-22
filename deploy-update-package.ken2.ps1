param
(
    [Parameter(Mandatory=$true)]
    [int]$Build,
    [string]$WebServer = "S314"
    
)

$WebServerSession = New-PSSession -ComputerName $WebServer

Invoke-Command -ComputerName $WebServer -ScriptBlock `
{
    if((Test-Path("c:\apps\entt.rts\web\binaries\$using:Build\")) -eq $false)
    {        
        mkdir "c:\apps\entt.rts\web\binaries\$using:Build\"
    }
     
}
$current = $Build -1 ;

Copy-Item .\$Build.7z -Force -ToSession $WebServerSession c:\apps\entt.rts\web\binaries\$Build\
Copy-Item  .\deployment\$Build.* -Force -ToSession $WebServerSession c:\apps\entt.rts\web\binaries\$Build\
Copy-Item  .\deployment\version.$Build.* -Force -ToSession $WebServerSession c:\apps\entt.rts\web\binaries\$Build\
Copy-Item  .\deployment\$current.json -Force -ToSession $WebServerSession c:\apps\entt.rts\web\binaries\$current.json
