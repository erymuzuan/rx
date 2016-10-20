param
(
    [Parameter(Mandatory=$true)]
    [int]$Build,
    [string]$WebServer = "S302"
    
)

$WebServerSession = New-PSSession -ComputerName $WebServer

Invoke-Command -ComputerName $WebServer -ScriptBlock `
{
    if((Test-Path("c:\apps\rx\web\binaries\$using:Build\")) -eq $false)
    {        
        mkdir "c:\apps\rx\web\binaries\$using:Build\"
    }
     
}

Copy-Item .\$Build.7z -Force -ToSession $WebServerSession c:\apps\rx\web\binaries\$Build\
Copy-Item  .\deployment\10325.* -Force -ToSession $WebServerSession c:\apps\rx\web\binaries\$Build\
Copy-Item  .\deployment\version.$Build.* -Force -ToSession $WebServerSession c:\apps\rx\web\binaries\$Build\
