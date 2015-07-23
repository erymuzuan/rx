Param(
       [Parameter(Position=0)]
       [int]$Build ,
       [Parameter(Position=1)]
       [ValidateSet('Production','Staging','Alpha')]
       [string]$Release = 'Alpha'
     )

function Create-FtpDirectory {
  param(
    [Parameter(Mandatory=$true)]
    [string]
    $sourceuri,
    [Parameter(Mandatory=$true)]
    [string]
    $username,
    [Parameter(Mandatory=$true)]
    [string]
    $password,
    [Parameter(Mandatory=$true)]
    [string]
    $Directory
  )
  
  $SecureString = ConvertTo-SecureString $password -AsPlainText -Force
  $PSCredential = New-Object System.Management.Automation.PSCredential ($username, $SecureString)

  Import-Module PSFTP 
  Set-FTPConnection -Credentials $PSCredential -Server ftp://www.reactivedeveloper.com -Session ss01 
  $Session = Get-FTPConnection -Session ss01

  New-FTPItem -Session $Session -Path "/Alpha/binaries" -Name $Directory
    
}


function Remove-FtpDirectory {
  param(
    [Parameter(Mandatory=$true)]
    [string]
    $sourceuri,
    [Parameter(Mandatory=$true)]
    [string]
    $username,
    [Parameter(Mandatory=$true)]
    [string]
    $password,
    [Parameter(Mandatory=$true)]
    [string]
    $Directory
  )
  
  $SecureString = ConvertTo-SecureString $password -AsPlainText -Force
  $PSCredential = New-Object System.Management.Automation.PSCredential ($username, $SecureString)

  Import-Module PSFTP 
  Set-FTPConnection -Credentials $PSCredential -Server ftp://www.reactivedeveloper.com -Session ss01 
  $Session = Get-FTPConnection -Session ss01

  Remove-FTPItem -Session $Session -Path $Directory -Recurse
    
}


function Upload-FtpFile {
  param(
    [Parameter(Mandatory=$true)]
    [string]
    $LocalFile,
    [Parameter(Mandatory=$true)]
    [string]
    $username,
    [Parameter(Mandatory=$true)]
    [string]
    $password,
    [Parameter(Mandatory=$true)]
    [string]
    $RemoteDirectory
  )
  
  Write-Host "Uploading $path ... please wait...."
  if ($LocalFile -match '\\$|\\\w+$') { throw 'sourceuri should end with a file name' }
  if((Test-Path($LocalFile)) -eq $false){
    Write-Warning "Cannot find $LocalFile"
    exit;
  }
    

  
  $SecureString = ConvertTo-SecureString $password -AsPlainText -Force
  $PSCredential = New-Object System.Management.Automation.PSCredential ($username, $SecureString)

  Import-Module PSFTP 
  Set-FTPConnection -Credentials $PSCredential -Server ftp://www.reactivedeveloper.com -Session ss01 
  $Session = Get-FTPConnection -Session ss01

  Add-FTPItem -Session $Session -Overwrite -Path $RemoteDirectory -LocalPath $LocalFile

}

Import-Module .\source\PSFTP\PSFTP.psm1


if($Build -eq 0)
{
    Write-Host "Please specify the Build Number"
    exit;
}


$ftpRoot = "ftp://www.reactivedeveloper.com/$Release/binaries"
$ftpUserName = "rxdeveloper"
$ftpPassword = "reH2TaXd"



Create-FtpDirectory -sourceuri "$ftpRoot/" -username $ftpUserName -password $ftpPassword -Directory $Build
Upload-FtpFile -RemoteDirectory "$ftpRoot/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\$previous.json
Upload-FtpFile -RemoteDirectory "$ftpRoot/$Build/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\version.$Build.json

if((Test-Path .\deployment\$Build.html) -eq $false)
{
    Write-Host -ForegroundColor Yellow "Cannot find $Build.html for your release note, please use the markdown files provided to generate the Release note"
    Read-Host
}

Upload-FtpFile -RemoteDirectory "$ftpRoot/$Build/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\$Build.html

Upload-FtpFile -RemoteDirectory "$ftpRoot/$Build/" -username $ftpUserName -password $ftpPassword -LocalFile .\$Build.7z
#$build.ps1 to root
Upload-FtpFile -RemoteDirectory "$ftpRoot/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\$Build.ps1
Upload-FtpFile -RemoteDirectory "$ftpRoot/$Build/" -username $ftpUserName -password $ftpPassword -LocalFile .\deployment\$Build.ps1
