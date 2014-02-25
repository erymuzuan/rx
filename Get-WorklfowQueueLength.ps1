Param(
       [string]$ApplicationName = "Dev",
       [string]$ManagementPort = 15672,
       [string]$ManagementHost = "http://localhost"
     )
     Write-Host "Getting the stalled queues"
$UserName = Read-Host "Enter User Name:" 
$Password = Read-Host -AsSecureString "Enter Your Password:" 
$Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $UserName , $Password 
$url = $ManagementHost + ":" + $ManagementPort + "/api/queues/Dev/workflow_execution"
$response = Invoke-WebRequest -Method GET -Uri $url  -Credential $Credential
$q = $response.Content | ConvertFrom-Json
$qlength = $q.messages
Write-Host "There are $qlength messages in workflow queue"