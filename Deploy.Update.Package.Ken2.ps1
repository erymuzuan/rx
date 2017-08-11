param
(
    [Parameter(Mandatory=$true)]
    [int]$Build,
    [string]$WebServer = "S314",
    [pscredential]$Admin = (Get-Credential 'local\Administrator')
    
)

$WebServerSession = New-PSSession -ComputerName $WebServer -Credential $Admin

Invoke-Command -Session $WebServerSession -ScriptBlock `
{
    if((Test-Path("\\prep5-pc\IIS\wwwroot\binaries\$using:Build\")) -eq $false)
    {        
        mkdir "\\prep5-pc\IIS\wwwroot\binaries\$using:Build\"
    }
     
}
$current = $Build -1 ;

Copy-Item .\$Build.7z -Force -ToSession $WebServerSession \\prep5-pc\IIS\wwwroot\binaries\$Build\
Copy-Item  .\deployment\$Build.* -Force -ToSession $WebServerSession \\prep5-pc\IIS\wwwroot\binaries\$Build\
Copy-Item  .\deployment\version.$Build.* -Force -ToSession $WebServerSession \\prep5-pc\IIS\wwwroot\binaries\$Build\
Copy-Item  .\deployment\$current.json -Force -ToSession $WebServerSession \\prep5-pc\IIS\wwwroot\binaries\$current.json
Copy-Item  .\deployment\$current.ps1 -Force -ToSession $WebServerSession \\prep5-pc\IIS\wwwroot\binaries\$current.ps1
