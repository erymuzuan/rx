[CmdLetBinding()]
Param(
    [switch]$LoadRxPsModule = $false,
    [string]$Environment="dev.local"
  )


Write-Host "Importing posh-git" 
Import-Module posh-git

 if($LoadRxPsModule.IsPresent){
    Write-Host "Importing rx.ps.module" 
    Import-Module .\bin\utils\rx.ps.module.dll

    $test = Get-RxEntityDefinition -Name "Customer" -RxApplicationName "DevV1"
    Write-Verbose $test

    New-Alias -Name "rx-builder" -Value "Invoke-RxBuilder"
    New-Alias -Name "rx-deploy" -Value "Invoke-RxDeploy"
    New-Alias -Name "rx-diff" -Value "Invoke-RxDiff"
    New-Alias -Name "rx-ds" -Value "Invoke-RxDeploymentStatus"

    $GitPromptSettings.EnableWindowTitle = "DevV1.$Environment~~"
    $GitPromptSettings.BeforeText=" [RxPs "
    $GitPromptSettings.BeforeForegroundColor="Cyan"
 
 }

Write-Host "Setting RxApplicationName to DevV1"
Set-Variable -Name 'RxApplicationName' -Value 'DevV1'


Write-Host "Setting DevV1 website and IIS environment variables" 

$RxApplicationName="DevV1"
$RxHome = "$PWD\bin"
$machine = ($env:COMPUTERNAME).Replace("-","_")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqPassword","guest", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqBase","$RxHome\rabbitmq_base", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchIndexNumberOfShards","1", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_IisExpressExecutable","$RxHome\IIS Express\iisexpress.exe", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchHttpPort","9200", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticSearchJar","$RxHome\elasticsearch\lib\elasticsearch-1.7.5.jar", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchClusterName","cluster_$machine""_DevV1", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_HOME","$RxHome", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_WebPath","$PWD\source\web\web.sph", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqManagementPort","15672", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchIndexNumberOfReplicas","0", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_WebsitePort","4436", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqDirectory","$RxHome\rabbitmq_server", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_LoggerWebSocketPort","50238", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_SqlLocalDbName","ProjectsV13", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_SqlConnectionString", "Data Source=(localdb)\ProjectsV13;Initial Catalog=DevV1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqUserName","guest", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchNodeName","node_$machine" + "_DevV1", "Process")
[System.Environment]::SetEnvironmentVariable("RABBITMQ_BASE","$RxHome\rabbitmq_base", "Process")
[System.Environment]::SetEnvironmentVariable("PATH","$env:Path;$RxHome\rabbitmq_server\sbin", "Process")

[System.Environment]::SetEnvironmentVariable("RX_DEVV1_BromConnectionString", "Data Source=S301\DEV2016;Initial Catalog=PittisNonCore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_SnbWebNewAccount_BaseAddress", "http://eryken2.asuscomm.com:8086", "Process")


$computerName = $env:COMPUTERNAME
if((Test-Path("env.devv1.$computerName.ps1")) -eq $true){
    Write-Host "Loading env.devv1.$computerName.ps1"
    & ".\env.devv1.$computerName.ps1";
}else{
    Write-Host "Cannot find env.devv1.$computerName.ps1"
}
