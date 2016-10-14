param
(
    [int]$Build,
    [string]$WebServer = "S302"
    
)

$WebServerSession = New-PSSession -ComputerName $WebServer

Invoke-Command -ComputerName $WebServer -ScriptBlock { mkdir "c:\apps\rx\web\binaries\$using:Build\"}

Copy-Item .\$Build.7z -ToSession $WebServerSession c:\apps\rx\web\binaries\$Build\
Copy-Item  .\deployment\10325.* -ToSession $WebServerSession c:\apps\rx\web\binaries\$Build\
Copy-Item  .\deployment\version.$Build.* -ToSession $WebServerSession c:\apps\rx\web\binaries\$Build\
